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
                // Comandos básicos
                new SlashCommandBuilder()
                    .WithName("test")
                    .WithDescription("Probar que el bot funciona"),

                new SlashCommandBuilder()
                    .WithName("ayuda")
                    .WithDescription("Ver todos los comandos disponibles"),

                // Matchmaking
                new SlashCommandBuilder()
                    .WithName("buscar-partida")
                    .WithDescription("Buscar oponente para batalla RTS"),

                new SlashCommandBuilder()
                    .WithName("salir-cola")
                    .WithDescription("Salir de la cola de matchmaking"),

                new SlashCommandBuilder()
                    .WithName("abandonar")
                    .WithDescription("Abandonar la partida actual"),

                // Estado del juego
                new SlashCommandBuilder()
                    .WithName("estado-batalla")
                    .WithDescription("Ver estado completo de la batalla"),

                new SlashCommandBuilder()
                    .WithName("mi-estado")
                    .WithDescription("Ver tu estado personal"),

                new SlashCommandBuilder()
                    .WithName("estado-recursos")
                    .WithDescription("Ver tus recursos actuales"),

                // Civilización
                new SlashCommandBuilder()
                    .WithName("elegir-civ")
                    .WithDescription("Elegir tu civilización")
                    .AddOption("tipo", ApplicationCommandOptionType.String, "Civilización a elegir", true,
                        choices: new[]
                        {
                            new ApplicationCommandOptionChoiceProperties { Name = "Japoneses", Value = "1" },
                            new ApplicationCommandOptionChoiceProperties { Name = "Romanos", Value = "2" },
                            new ApplicationCommandOptionChoiceProperties { Name = "Vikingos", Value = "3" }
                        }),

                // Comandos de gameplay
                new SlashCommandBuilder()
                    .WithName("recolectar")
                    .WithDescription("Enviar aldeano a recolectar recursos"),

                new SlashCommandBuilder()
                    .WithName("construir")
                    .WithDescription("Construir un edificio")
                    .AddOption("tipo", ApplicationCommandOptionType.String, "Tipo de edificio", true,
                        choices: new[]
                        {
                            new ApplicationCommandOptionChoiceProperties { Name = "Casa", Value = "casa" },
                            new ApplicationCommandOptionChoiceProperties { Name = "Cuartel", Value = "cuartel" },
                            new ApplicationCommandOptionChoiceProperties { Name = "Almacén Oro", Value = "almacen_oro" },
                            new ApplicationCommandOptionChoiceProperties { Name = "Almacén Piedra", Value = "almacen_piedra" },
                            new ApplicationCommandOptionChoiceProperties { Name = "Almacén Madera", Value = "almacen_madera" },
                            new ApplicationCommandOptionChoiceProperties { Name = "Almacén Alimento", Value = "almacen_alimento" }
                        })
                    .AddOption("x", ApplicationCommandOptionType.Integer, "Coordenada X (0-99)", true)
                    .AddOption("y", ApplicationCommandOptionType.Integer, "Coordenada Y (0-99)", true),

                new SlashCommandBuilder()
                    .WithName("entrenar")
                    .WithDescription("Entrenar unidad en cuartel")
                    .AddOption("tipo", ApplicationCommandOptionType.String, "Tipo de unidad", true,
                        choices: new[]
                        {
                            new ApplicationCommandOptionChoiceProperties { Name = "Aldeano", Value = "aldeano" },
                            new ApplicationCommandOptionChoiceProperties { Name = "Infantería", Value = "infanteria" },
                            new ApplicationCommandOptionChoiceProperties { Name = "Arquero", Value = "arquero" },
                            new ApplicationCommandOptionChoiceProperties { Name = "Caballería", Value = "caballeria" }
                        }),

                new SlashCommandBuilder()
                    .WithName("sacar-unidad")
                    .WithDescription("Sacar unidad del cuartel")
                    .AddOption("x", ApplicationCommandOptionType.Integer, "Coordenada X destino (0-99)", true)
                    .AddOption("y", ApplicationCommandOptionType.Integer, "Coordenada Y destino (0-99)", true),

                new SlashCommandBuilder()
                    .WithName("atacar")
                    .WithDescription("Atacar enemigo")
                    .AddOption("x", ApplicationCommandOptionType.Integer, "Coordenada X objetivo (0-99)", true)
                    .AddOption("y", ApplicationCommandOptionType.Integer, "Coordenada Y objetivo (0-99)", true),

                new SlashCommandBuilder()
                    .WithName("ver-mapa")
                    .WithDescription("Ver una sección del mapa")
                    .AddOption("x", ApplicationCommandOptionType.Integer, "Centro X (0-99)", false)
                    .AddOption("y", ApplicationCommandOptionType.Integer, "Centro Y (0-99)", false),

                // Comandos informativos
                new SlashCommandBuilder()
                    .WithName("cola-info")
                    .WithDescription("Ver información del matchmaking"),

                new SlashCommandBuilder()
                    .WithName("ver-aldeanos")
                    .WithDescription("Ver aldeanos en el Centro Cívico"),

                new SlashCommandBuilder()
                    .WithName("ver-edificios")
                    .WithDescription("Ver todos tus edificios")
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
                    case "estado-recursos":
                        await HandleEstadoRecursosCommand(command);
                        break;
                    case "elegir-civ":
                        await HandleElegirCivCommand(command);
                        break;
                    case "recolectar":
                        await HandleRecolectarCommand(command);
                        break;
                    case "construir":
                        await HandleConstruirCommand(command);
                        break;
                    case "entrenar":
                        await HandleEntrenarCommand(command);
                        break;
                    case "sacar-unidad":
                        await HandleSacarUnidadCommand(command);
                        break;
                    case "atacar":
                        await HandleAtacarCommand(command);
                        break;
                    case "ver-mapa":
                        await HandleVerMapaCommand(command);
                        break;
                    case "cola-info":
                        await HandleColaInfoCommand(command);
                        break;
                    case "ver-aldeanos":
                        await HandleVerAldeanosCommand(command);
                        break;
                    case "ver-edificios":
                        await HandleVerEdificiosCommand(command);
                        break;
                    default:
                        await command.FollowupAsync("Comando no reconocido.", ephemeral: true);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ejecutando comando {command.Data.Name}: {ex.Message}");
                await command.FollowupAsync("Ocurrió un error ejecutando el comando.", ephemeral: true);
            }
        }

        private async Task HandleTestCommand(SocketSlashCommand command)
        {
            var embed = new EmbedBuilder()
                .WithTitle("Bot RTS Funcionando!")
                .WithDescription("El bot está conectado y listo para batallas")
                .WithColor(Color.Green)
                .AddField("Estado", "Conectado", true)
                .AddField("Listo para", "Batallas RTS en Discord", true)
                .AddField("Siguiente", "Usa `/buscar-partida` para empezar", true)
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
                await command.FollowupAsync("Has abandonado la partida.");
            }
            else
            {
                await command.FollowupAsync("No estás en una partida activa.", ephemeral: true);
            }
        }

        private async Task HandleEstadoBatallaCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("No estás en una partida activa.", ephemeral: true);
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
                await command.FollowupAsync("No estás en una partida activa.", ephemeral: true);
                return;
            }

            var isPlayer1 = command.User.Id == game.Player1Id;
            var civilization = isPlayer1 ? game.Player1Civilization : game.Player2Civilization;
            var playerGame = game.GetPlayerGame(command.User.Id);
            var jugador = isPlayer1 ? playerGame.Jugador1 : playerGame.Jugador2;

            var embed = new EmbedBuilder()
                .WithTitle("Tu Estado Personal")
                .WithColor(Color.Blue)
                .AddField("Estado", "En partida", true)
                .AddField("Civilización", GetCivName(civilization), true)
                .AddField("Oponente", $"<@{game.GetOpponentId(command.User.Id)}>", true)
                .AddField("Población", $"{jugador.PoblacionActual}/{jugador.CapacidadPoblacionMaxima}", true)
                .AddField("Unidades", jugador.Unidades.Count.ToString(), true)
                .AddField("Edificios", jugador.Edificios.Count.ToString(), true)
                .AddField("Duración", $"{DateTime.UtcNow - game.StartTime:mm\\:ss}", true)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await command.FollowupAsync(embed: embed, ephemeral: true);
        }

        private async Task HandleEstadoRecursosCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("No estás en una partida activa.", ephemeral: true);
                return;
            }

            var isPlayer1 = command.User.Id == game.Player1Id;
            var playerGame = game.GetPlayerGame(command.User.Id);
            var jugador = isPlayer1 ? playerGame.Jugador1 : playerGame.Jugador2;

            var recursos = jugador.Inventario.Recursos;

            var embed = new EmbedBuilder()
                .WithTitle("Tus Recursos")
                .WithColor(Color.Gold)
                .AddField("Madera", recursos[TipoRecurso.Madera].CantidadDisponible.ToString(), true)
                .AddField("Piedra", recursos[TipoRecurso.Piedra].CantidadDisponible.ToString(), true)
                .AddField("Oro", recursos[TipoRecurso.Oro].CantidadDisponible.ToString(), true)
                .AddField("Alimento", recursos[TipoRecurso.Alimento].CantidadDisponible.ToString(), true)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await command.FollowupAsync(embed: embed, ephemeral: true);
        }

        private async Task HandleRecolectarCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("No estás en una partida activa.", ephemeral: true);
                return;
            }

            var isPlayer1 = command.User.Id == game.Player1Id;
            var playerGame = game.GetPlayerGame(command.User.Id);
            var jugador = isPlayer1 ? playerGame.Jugador1 : playerGame.Jugador2;

            try
            {
                await playerGame.AldeanoRecolecta(jugador);
                await command.FollowupAsync("Aldeano enviado a recolectar recursos!");
                
                // Verificar victoria después de la acción
                await _gameManager.CheckGameEnd(game, command.Channel as IMessageChannel);
                
                // Notificar al oponente
                await NotifyOpponent(game, command.User.Id, "Tu oponente está recolectando recursos...");
            }
            catch (Exception ex)
            {
                await command.FollowupAsync($"Error al recolectar: {ex.Message}", ephemeral: true);
            }
        }

        private async Task HandleConstruirCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("No estás en una partida activa.", ephemeral: true);
                return;
            }

            var tipo = command.Data.Options.First(x => x.Name == "tipo").Value.ToString();
            var x = Convert.ToInt32(command.Data.Options.First(x => x.Name == "x").Value);
            var y = Convert.ToInt32(command.Data.Options.First(x => x.Name == "y").Value);

            var isPlayer1 = command.User.Id == game.Player1Id;
            var playerGame = game.GetPlayerGame(command.User.Id);
            var jugador = isPlayer1 ? playerGame.Jugador1 : playerGame.Jugador2;

            try
            {
                Edificio edificio = tipo switch
                {
                    "casa" => new Casa(x, y),
                    "cuartel" => new Cuartel(x, y),
                    "almacen_oro" => new AlmacenOro(x, y),
                    "almacen_piedra" => new AlmacenPiedra(x, y),
                    "almacen_madera" => new AlmacenMadera(x, y),
                    "almacen_alimento" => new AlmacenAlimento(x, y),
                    _ => throw new ArgumentException("Tipo de edificio no válido")
                };

                await playerGame.SacarAldeanoYConstruirEdificio(jugador, edificio, x, y);
                await command.FollowupAsync($"{edificio.Name} en construcción en ({x}, {y})!");
                
                // Verificar victoria después de la acción
                await _gameManager.CheckGameEnd(game, command.Channel as IMessageChannel);
                
                // Notificar al oponente
                await NotifyOpponent(game, command.User.Id, $"Tu oponente está construyendo un {edificio.Name} en ({x}, {y})");
            }
            catch (Exception ex)
            {
                await command.FollowupAsync($"Error al construir: {ex.Message}", ephemeral: true);
            }
        }

        private async Task HandleEntrenarCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("No estás en una partida activa.", ephemeral: true);
                return;
            }

            var tipo = command.Data.Options.First(x => x.Name == "tipo").Value.ToString();

            var isPlayer1 = command.User.Id == game.Player1Id;
            var playerGame = game.GetPlayerGame(command.User.Id);
            var jugador = isPlayer1 ? playerGame.Jugador1 : playerGame.Jugador2;

            try
            {
                // Buscar cuartel del jugador
                var cuartel = jugador.Edificios.OfType<Cuartel>().FirstOrDefault();
                if (cuartel == null)
                {
                    await command.FollowupAsync("No tienes un cuartel construido.", ephemeral: true);
                    return;
                }

                var tipoUnidad = tipo switch
                {
                    "aldeano" => TipoUnidad.Aldeano,
                    "infanteria" => TipoUnidad.Infanteria,
                    "arquero" => TipoUnidad.Arquero,
                    "caballeria" => TipoUnidad.Caballeria,
                    _ => throw new ArgumentException("Tipo de unidad no válido")
                };

                var costo = cuartel.ObtenerCostoPorTipo(tipoUnidad);
                
                if (!jugador.IntentarPagar(costo))
                {
                    await command.FollowupAsync($"No tienes suficientes recursos. Necesitas: {costo}", ephemeral: true);
                    return;
                }

                var nuevaUnidad = cuartel.EntrenarUnidad(tipoUnidad);
                cuartel.AgregarUnidad(nuevaUnidad);
                jugador.Unidades.Add(nuevaUnidad);

                await command.FollowupAsync($"{nuevaUnidad.Nombre} entrenado exitosamente! Usa `/sacar-unidad` para deployarlo.");
                
                // Verificar victoria después de la acción
                await _gameManager.CheckGameEnd(game, command.Channel as IMessageChannel);
                
                // Notificar al oponente
                await NotifyOpponent(game, command.User.Id, $"Tu oponente entrenó un {nuevaUnidad.Nombre}");
            }
            catch (Exception ex)
            {
                await command.FollowupAsync($"Error al entrenar: {ex.Message}", ephemeral: true);
            }
        }

        private async Task HandleSacarUnidadCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("No estás en una partida activa.", ephemeral: true);
                return;
            }

            var x = Convert.ToInt32(command.Data.Options.First(x => x.Name == "x").Value);
            var y = Convert.ToInt32(command.Data.Options.First(x => x.Name == "y").Value);

            var isPlayer1 = command.User.Id == game.Player1Id;
            var playerGame = game.GetPlayerGame(command.User.Id);
            var jugador = isPlayer1 ? playerGame.Jugador1 : playerGame.Jugador2;

            try
            {
                var cuartel = jugador.Edificios.OfType<Cuartel>().FirstOrDefault();
                if (cuartel == null)
                {
                    await command.FollowupAsync("No tienes un cuartel.", ephemeral: true);
                    return;
                }

                playerGame.SacarUnidadDeCuartel(cuartel, x, y);
                await command.FollowupAsync($"Unidad deployada en ({x}, {y})!");
                
                // Verificar victoria después de la acción
                await _gameManager.CheckGameEnd(game, command.Channel as IMessageChannel);
                
                // Notificar al oponente
                await NotifyOpponent(game, command.User.Id, $"Tu oponente deployó una unidad en ({x}, {y})");
            }
            catch (Exception ex)
            {
                await command.FollowupAsync($"Error al sacar unidad: {ex.Message}", ephemeral: true);
            }
        }

        private async Task HandleAtacarCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("No estás en una partida activa.", ephemeral: true);
                return;
            }

            var x = Convert.ToInt32(command.Data.Options.First(x => x.Name == "x").Value);
            var y = Convert.ToInt32(command.Data.Options.First(x => x.Name == "y").Value);

            var isPlayer1 = command.User.Id == game.Player1Id;
            var playerGame = game.GetPlayerGame(command.User.Id);
            var jugador = isPlayer1 ? playerGame.Jugador1 : playerGame.Jugador2;

            try
            {
                await playerGame.AtacarUnidadSimple(jugador);
                await command.FollowupAsync($"Ataque realizado hacia ({x}, {y})!");
                
                // Verificar victoria después de la acción
                await _gameManager.CheckGameEnd(game, command.Channel as IMessageChannel);
                
                // Notificar al oponente
                await NotifyOpponent(game, command.User.Id, $"Tu oponente realizó un ataque en ({x}, {y})!");
            }
            catch (Exception ex)
            {
                await command.FollowupAsync($"Error al atacar: {ex.Message}", ephemeral: true);
            }
        }

        private async Task HandleVerMapaCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("No estás en una partida activa.", ephemeral: true);
                return;
            }

            var isPlayer1 = command.User.Id == game.Player1Id;
            var playerGame = game.GetPlayerGame(command.User.Id);

            // Para simplificar, mostramos solo información básica del mapa
            var embed = new EmbedBuilder()
                .WithTitle("Vista del Mapa")
                .WithDescription("Mapa 100x100 con recursos distribuidos")
                .WithColor(Color.Blue)
                .AddField("Bosques", "Distribuidos aleatoriamente", true)
                .AddField("Minas de Oro", "Distribuidas aleatoriamente", true)
                .AddField("Minas de Piedra", "Distribuidas aleatoriamente", true)
                .AddField("Tu Centro Cívico", isPlayer1 ? "(0, 0)" : "(99, 99)", true)
                .AddField("Centro Cívico Enemigo", isPlayer1 ? "(99, 99)" : "(0, 0)", true)
                .Build();

            await command.FollowupAsync(embed: embed, ephemeral: true);
        }

        private async Task HandleVerAldeanosCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("No estás en una partida activa.", ephemeral: true);
                return;
            }

            var isPlayer1 = command.User.Id == game.Player1Id;
            var playerGame = game.GetPlayerGame(command.User.Id);
            var jugador = isPlayer1 ? playerGame.Jugador1 : playerGame.Jugador2;

            var centroCivico = jugador.Edificios.OfType<CentroCivico>().FirstOrDefault();
            if (centroCivico == null)
            {
                await command.FollowupAsync("No se encontró tu Centro Cívico.", ephemeral: true);
                return;
            }

            var aldeanos = centroCivico.ObtenerAldeanos();
            var embed = new EmbedBuilder()
                .WithTitle("Aldeanos en Centro Cívico")
                .WithColor(Color.Orange)
                .AddField("Cantidad", aldeanos.Count.ToString(), true);

            if (aldeanos.Count > 0)
            {
                embed.AddField("Lista de Aldeanos", string.Join("\n", aldeanos.Select(a => $"• {a.Nombre} (Vida: {a.VidaActual})")), false);
            }
            else
            {
                embed.AddField("Estado", "No hay aldeanos en el Centro Cívico", false);
            }

            await command.FollowupAsync(embed: embed.Build(), ephemeral: true);
        }

        private async Task HandleVerEdificiosCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("No estás en una partida activa.", ephemeral: true);
                return;
            }

            var isPlayer1 = command.User.Id == game.Player1Id;
            var playerGame = game.GetPlayerGame(command.User.Id);
            var jugador = isPlayer1 ? playerGame.Jugador1 : playerGame.Jugador2;

            var embed = new EmbedBuilder()
                .WithTitle("Tus Edificios")
                .WithColor(Color.Orange)
                .AddField("Total", jugador.Edificios.Count.ToString(), true);

            var edificiosPorTipo = jugador.Edificios.GroupBy(e => e.GetType().Name);
            foreach (var grupo in edificiosPorTipo)
            {
                embed.AddField(grupo.Key, grupo.Count().ToString(), true);
            }

            await command.FollowupAsync(embed: embed.Build(), ephemeral: true);
        }

        private async Task HandleAyudaCommand(SocketSlashCommand command)
        {
            var embed = new EmbedBuilder()
                .WithTitle("Bot RTS Discord - Comandos")
                .WithDescription("Bot de batallas RTS en Discord!")
                .WithColor(Color.Purple);

            embed.AddField("Comandos Básicos",
                "`/test` - Probar que el bot funciona\n" +
                "`/ayuda` - Ver esta ayuda\n" +
                "`/cola-info` - Ver estado del matchmaking", false);

            embed.AddField("Matchmaking",
                "`/buscar-partida` - Buscar oponente\n" +
                "`/salir-cola` - Salir de la cola\n" +
                "`/abandonar` - Abandonar partida", false);

            embed.AddField("Partida",
                "`/elegir-civ <tipo>` - Elegir civilización\n" +
                "`/estado-batalla` - Ver progreso de batalla\n" +
                "`/mi-estado` - Ver tu estado personal\n" +
                "`/estado-recursos` - Ver tus recursos", false);

            embed.AddField("Gameplay",
                "`/recolectar` - Enviar aldeano a recolectar\n" +
                "`/construir <tipo> <x> <y>` - Construir edificio\n" +
                "`/entrenar <tipo>` - Entrenar unidad\n" +
                "`/sacar-unidad <x> <y>` - Deployar unidad\n" +
                "`/atacar <x> <y>` - Atacar objetivo", false);

            embed.AddField("Civilizaciones Disponibles",
                "**Japoneses**: Samurai especial\n" +
                "**Romanos**: Legionario especial\n" +
                "**Vikingos**: Berserker especial", false);

            await command.FollowupAsync(embed: embed.Build(), ephemeral: true);
        }

        private async Task HandleElegirCivCommand(SocketSlashCommand command)
        {
            var game = _gameManager.GetGameByPlayer(command.User.Id);
            if (game == null)
            {
                await command.FollowupAsync("No estás en una partida activa.", ephemeral: true);
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

            var civName = GetCivName(tipo);
            var playerGame = game.GetPlayerGame(command.User.Id);

            // Agregar unidad especial
            playerGame.AgregarUnidadPorCivilizacion(isPlayer1 ? playerGame.Jugador1 : playerGame.Jugador2, tipo);

            await command.FollowupAsync($"Has elegido: **{civName}**\nUnidad especial agregada! Preparándote para la batalla!");
            
            // Notificar al oponente
            await NotifyOpponent(game, command.User.Id, $"<@{command.User.Id}> eligió {civName}. La batalla se intensifica!");
        }

        private async Task HandleColaInfoCommand(SocketSlashCommand command)
        {
            var queueCount = _gameManager.GetQueueCount();
            var activeGames = _gameManager.GetActiveGamesCount();

            var embed = new EmbedBuilder()
                .WithTitle("Estado del Matchmaking RTS")
                .WithColor(Color.Blue)
                .AddField("Jugadores en Cola", queueCount.ToString(), true)
                .AddField("Batallas Activas", activeGames.ToString(), true)
                .AddField("Modo", "**RTS DISCORD**", true)
                .AddField("Estado del Bot", "Funcionando", true)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await command.FollowupAsync(embed: embed, ephemeral: true);
        }

        private string GetCivName(string? tipo)
        {
            return tipo switch
            {
                "1" => "Japoneses",
                "2" => "Romanos", 
                "3" => "Vikingos",
                _ => "No elegida"
            };
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