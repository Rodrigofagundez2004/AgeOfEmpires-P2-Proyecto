using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Library;
using System.Linq;

namespace RTSDiscordBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly GameManager _gameManager;

        public CommandHandler(DiscordSocketClient client, GameManager gameManager)
        {
            _client = client;
            _gameManager = gameManager;
        }

        public Task InitializeAsync()
        {
            _client.SlashCommandExecuted += HandleSlashCommandAsync;
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            var commands = new[]
            {
                // Comandos básicos que funcionan
                new SlashCommandBuilder()
                    .WithName("test")
                    .WithDescription("🧪 Probar que el bot funciona"),

                new SlashCommandBuilder()
                    .WithName("ayuda")
                    .WithDescription("❓ Ver todos los comandos disponibles"),

                // Matchmaking
                new SlashCommandBuilder()
                    .WithName("buscar-partida")
                    .WithDescription("🔍 Buscar oponente para batalla RTS"),

                new SlashCommandBuilder()
                    .WithName("salir-cola")
                    .WithDescription("❌ Salir de la cola de matchmaking"),

                new SlashCommandBuilder()
                    .WithName("abandonar")
                    .WithDescription("🏳️ Abandonar la partida actual"),

                // Estado del juego
                new SlashCommandBuilder()
                    .WithName("estado-batalla")
                    .WithDescription("⚔️ Ver estado completo de la batalla"),

                new SlashCommandBuilder()
                    .WithName("mi-estado")
                    .WithDescription("📊 Ver tu estado personal"),

                // Civilización
                new SlashCommandBuilder()
                    .WithName("elegir-civ")
                    .WithDescription("🏛️ Elegir tu civilización")
                    .AddOption("tipo", ApplicationCommandOptionType.String, "Civilización a elegir", true,
                        choices: new[]
                        {
                            new ApplicationCommandOptionChoiceProperties { Name = "🗾 Japoneses", Value = "1" },
                            new ApplicationCommandOptionChoiceProperties { Name = "🏛️ Romanos", Value = "2" },
                            new ApplicationCommandOptionChoiceProperties { Name = "⚡ Vikingos", Value = "3" }
                        }),

                // Comandos informativos
                new SlashCommandBuilder()
                    .WithName("cola-info")
                    .WithDescription("📊 Ver información del matchmaking")
            };

            foreach (var command in commands)
            {
                await _client.CreateGlobalApplicationCommandAsync(command.Build());
            }
        }

        private async Task HandleSlashCommandAsync(SocketSlashCommand command)
        {
            try
            {
                await command.DeferAsync();

                switch (command.Data.Name)
                {
                    case "test":
                        await HandleTestCommand(command);
                        break;
                    case "ayuda":
                        await HandleAyudaCommand(command);
                        break;
                    case "buscar-partida":
                        await HandleBuscarPartidaCommand(command);
                        break;
                    case "salir-cola":
                        await HandleSalirColaCommand(command);
                        break;
                    case "abandonar":
                        await HandleAbandonarCommand(command);
                        break;
                    case "estado-batalla":
                        await HandleEstadoBatallaCommand(command);
                        break;
                    case "mi-estado":
                        await HandleMiEstadoCommand(command);
                        break;
                    case "elegir-civ":
                        await HandleElegirCivCommand(command);
                        break;
                    case "cola-info":
                        await HandleColaInfoCommand(command);
                        break;
                    default:
                        await command.FollowupAsync("❌ Comando no reconocido.", ephemeral: true);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ejecutando comando {command.Data.Name}: {ex.Message}");
                await command.FollowupAsync("❌ Ocurrió un error ejecutando el comando.", ephemeral: true);
            }
        }

        private async Task HandleTestCommand(SocketSlashCommand command)
        {
            var embed = new EmbedBuilder()
                .WithTitle("🎮 ¡Bot RTS Funcionando!")
                .WithDescription("El bot está conectado y listo para batallas")
                .WithColor(Color.Green)
                .AddField("✅ Estado", "Conectado", true)
                .AddField("🎯 Listo para", "Batallas RTS en Discord", true)
                .AddField("🚀 Siguiente", "Usa `/buscar-partida` para empezar", true)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await command.FollowupAsync(embed: embed);
        }

        private async Task HandleBuscarPartidaCommand(SocketSlashCommand command)
        {
            var (success, message) = await _gameManager.JoinMatchmaking(command.User.Id, command.Channel.Id);
            
            if (success)
            {
                await command.FollowupAsync(message);
            }
            else
            {
                await command.FollowupAsync(message, ephemeral: true);
            }
        }

        private async Task HandleSalirColaCommand(SocketSlashCommand command)
        {
            var (success, message) = await _gameManager.LeaveMatchmaking(command.User.Id);
            await command.FollowupAsync(message, ephemeral: true);
        }

        private async Task HandleAbandonarCommand(SocketSlashCommand command)
        {
            var success = await _gameManager.AbandonGame(command.User.Id);
            
            if (success)
            {
                await command.FollowupAsync("🏳️ Has abandonado la partida.");
            }
            else
            {
                await command.FollowupAsync("❌ No estás en una partida activa.", ephemeral: true);
            }
        }

        private async Task HandleEstadoBatallaCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("❌ No estás en una partida activa.", ephemeral: true);
                return;
            }

            var embed = DisplayHelper.CreateBattleStateEmbed(game);
            await command.FollowupAsync(embed: embed);
        }

        private async Task HandleMiEstadoCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("❌ No estás en una partida activa.", ephemeral: true);
                return;
            }

            var isPlayer1 = command.User.Id == game.Player1Id;
            var civilization = isPlayer1 ? game.Player1Civilization : game.Player2Civilization;

            var embed = new EmbedBuilder()
                .WithTitle("📊 Tu Estado Personal")
                .WithColor(Color.Blue)
                .AddField("🎮 Estado", "En partida", true)
                .AddField("🏛️ Civilización", civilization ?? "No elegida", true)
                .AddField("⚔️ Oponente", $"<@{game.GetOpponentId(command.User.Id)}>", true)
                .AddField("⏱️ Duración", $"{DateTime.UtcNow - game.StartTime:mm\\:ss}", true)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await command.FollowupAsync(embed: embed, ephemeral: true);
        }

        private async Task HandleAyudaCommand(SocketSlashCommand command)
        {
            var embed = new EmbedBuilder()
                .WithTitle("🎮 Bot RTS Discord - Comandos")
                .WithDescription("¡Bot de batallas RTS en Discord!")
                .WithColor(Color.Purple);

            embed.AddField("🧪 Comandos Básicos",
                "`/test` - Probar que el bot funciona\n" +
                "`/ayuda` - Ver esta ayuda\n" +
                "`/cola-info` - Ver estado del matchmaking", false);

            embed.AddField("🔍 Matchmaking",
                "`/buscar-partida` - Buscar oponente\n" +
                "`/salir-cola` - Salir de la cola\n" +
                "`/abandonar` - Abandonar partida", false);

            embed.AddField("🏛️ Partida",
                "`/elegir-civ <tipo>` - Elegir civilización\n" +
                "`/estado-batalla` - Ver progreso de batalla\n" +
                "`/mi-estado` - Ver tu estado personal", false);

            embed.AddField("🏛️ Civilizaciones Disponibles",
                "🗾 **Japoneses**: Samurai especial\n" +
                "🏛️ **Romanos**: Legionario especial\n" +
                "⚡ **Vikingos**: Berserker especial", false);

            embed.AddField("🚧 En Desarrollo",
                "Más comandos de juego próximamente", false);

            await command.FollowupAsync(embed: embed.Build(), ephemeral: true);
        }

        private async Task HandleElegirCivCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("❌ No estás en una partida activa.", ephemeral: true);
                return;
            }

            var tipo = command.Data.Options.First(x => x.Name == "tipo").Value.ToString();
            var isPlayer1 = command.User.Id == game.Player1Id;
            
            // Guardar la civilización elegida
            if (isPlayer1)
            {
                game.Player1Civilization = tipo;
            }
            else
            {
                game.Player2Civilization = tipo;
            }

            var civName = tipo switch
            {
                "1" => "🗾 Japoneses",
                "2" => "🏛️ Romanos",
                "3" => "⚡ Vikingos",
                _ => "❓ Desconocida"
            };

            await command.FollowupAsync($"⚡ Has elegido: **{civName}**\n🎖️ ¡Preparándote para la batalla!");
            
            // Notificar al oponente
            await NotifyOpponent(game, command.User.Id, $"🏛️ <@{command.User.Id}> eligió {civName}. ¡La batalla se intensifica!");
        }

        private async Task HandleColaInfoCommand(SocketSlashCommand command)
        {
            var queueCount = _gameManager.GetQueueCount();
            var activeGames = _gameManager.GetActiveGamesCount();

            var embed = new EmbedBuilder()
                .WithTitle("📊 Estado del Matchmaking RTS")
                .WithColor(Color.Blue)
                .AddField("⏳ Jugadores en Cola", queueCount.ToString(), true)
                .AddField("⚔️ Batallas Activas", activeGames.ToString(), true)
                .AddField("🎮 Modo", "**RTS DISCORD**", true)
                .AddField("🚀 Estado del Bot", "✅ Funcionando", true)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await command.FollowupAsync(embed: embed, ephemeral: true);
        }

        private async Task NotifyOpponent(GameSession game, ulong currentPlayerId, string message)
        {
            try
            {
                var opponentId = game.GetOpponentId(currentPlayerId);
                var channel = _client.GetChannel(game.ChannelId) as IMessageChannel;
                
                if (channel != null)
                {
                    await channel.SendMessageAsync($"<@{opponentId}> {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error notificando oponente: {ex.Message}");
            }
        }
    }
}