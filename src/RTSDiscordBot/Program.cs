using Discord;
using Discord.Net;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                        MessageCacheSize = 100
                    };

                    services.AddSingleton(config);
                    services.AddSingleton<DiscordSocketClient>();
                    services.AddSingleton<GameManager>();
                    services.AddSingleton<CommandHandler>();
                    services.AddHostedService<DiscordBotService>();
                });
    }

    public class DiscordBotService : BackgroundService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandHandler _commandHandler;
        private readonly IConfiguration _configuration;

        public DiscordBotService(DiscordSocketClient client, CommandHandler commandHandler, IConfiguration configuration)
        {
            _client = client;
            _commandHandler = commandHandler;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;

            var token = _configuration["Discord:Token"];
            
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("❌ TOKEN NO ENCONTRADO!");
                Console.WriteLine("🔧 Crea el archivo appsettings.json con tu token");
                return;
            }

            await _commandHandler.InitializeAsync();

            try
            {
                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error conectando: {ex.Message}");
            }
        }

        private async Task ReadyAsync()
        {
            Console.WriteLine($"🤖 ¡Bot conectado como {_client.CurrentUser}!");
            Console.WriteLine($"📊 Conectado a {_client.Guilds.Count} servidores");
            
            try
            {
                await _commandHandler.RegisterCommandsAsync();
                Console.WriteLine("✅ Comandos RTS registrados exitosamente");
                Console.WriteLine("🎮 ¡Los jugadores ya pueden usar /buscar-partida!");
            }
            catch (HttpException ex)
            {
                Console.WriteLine($"❌ Error registrando comandos: {ex.Message}");
                Console.WriteLine("⏳ Los comandos pueden tardar hasta 1 hora en aparecer");
            }
        }

        private Task LogAsync(LogMessage log)
        {
            var emoji = log.Severity switch
            {
                LogSeverity.Error => "❌",
                LogSeverity.Warning => "⚠️",
                LogSeverity.Info => "ℹ️",
                _ => "📝"
            };

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {emoji} {log.Source}: {log.Message}");
            return Task.CompletedTask;
        }
    }
}