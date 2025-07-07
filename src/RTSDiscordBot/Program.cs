using Discord;
using Discord.Net;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RTSDiscordBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var config = new DiscordSocketConfig()
                    {
                        GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages,
                        AlwaysDownloadUsers = false,
                        MessageCacheSize = 100,
                        LogLevel = LogSeverity.Info
                    };

                    services.AddSingleton(config);
                    services.AddSingleton<DiscordSocketClient>();
                    services.AddSingleton<GameManager>();
                    services.AddSingleton<CommandHandler>();
                    services.AddHostedService<DiscordBotService>();
                    services.AddHostedService<GameCleanupService>();
                });
    }

    public class DiscordBotService : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandHandler _commandHandler;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DiscordBotService> _logger;

        public DiscordBotService(
            DiscordSocketClient client, 
            CommandHandler commandHandler, 
            IConfiguration configuration,
            ILogger<DiscordBotService> logger)
        {
            _client = client;
            _commandHandler = commandHandler;
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;

            var token = _configuration["Discord:Token"];
            
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("TOKEN NO ENCONTRADO!");
                _logger.LogError("Crea el archivo appsettings.json con tu token");
                return;
            }

            await _commandHandler.InitializeAsync();

            try
            {
                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();
                
                _logger.LogInformation("Bot iniciado exitosamente");
                
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error conectando el bot");
            }
        }

        private async Task ReadyAsync()
        {
            _logger.LogInformation($"Bot conectado como {_client.CurrentUser}!");
            _logger.LogInformation($"Conectado a {_client.Guilds.Count} servidores");
            
            // Configurar estado del bot
            await _client.SetGameAsync("RTS Discord | /ayuda", type: ActivityType.Playing);
            
            try
            {
                await _commandHandler.RegisterCommandsAsync();
                _logger.LogInformation("Comandos RTS registrados exitosamente");
                _logger.LogInformation("Los jugadores ya pueden usar /buscar-partida!");
            }
            catch (HttpException ex)
            {
                _logger.LogError(ex, "Error registrando comandos");
                _logger.LogWarning("Los comandos pueden tardar hasta 1 hora en aparecer");
            }
        }

        private Task LogAsync(LogMessage log)
        {
            var emoji = log.Severity switch
            {
                LogSeverity.Critical => "[CRIT]",
                LogSeverity.Error => "[ERROR]",
                LogSeverity.Warning => "[WARN]",
                LogSeverity.Info => "[INFO]",
                LogSeverity.Verbose => "[VERB]",
                LogSeverity.Debug => "[DEBUG]",
                _ => "[LOG]"
            };

            var logLevel = log.Severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Verbose => LogLevel.Debug,
                LogSeverity.Debug => LogLevel.Trace,
                _ => LogLevel.Information
            };

            _logger.Log(logLevel, "{Emoji} {Source}: {Message}", emoji, log.Source, log.Message);
            
            if (log.Exception != null)
            {
                _logger.LogError(log.Exception, "Excepcion adicional:");
            }

            return Task.CompletedTask;
        }
    }
}