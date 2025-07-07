using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Library;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace RTSDiscordBot
{
    public class GameManager
    {
        private readonly ConcurrentDictionary<ulong, GameSession> _activeGames = new();
        private readonly ConcurrentQueue<ulong> _matchmakingQueue = new();
        private readonly DiscordSocketClient _client;

        public GameManager(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task<(bool success, string message)> JoinMatchmaking(ulong userId, ulong channelId)
        {
            // Verificar si ya está en una partida
            if (_activeGames.Values.Any(g => g.Player1Id == userId || g.Player2Id == userId))
            {
                return (false, "Ya estas en una partida activa. Usa `/abandonar` para salir.");
            }

            // Verificar si ya está en cola
            if (_matchmakingQueue.Contains(userId))
            {
                return (false, "Ya estas en la cola de matchmaking.");
            }

            // Buscar oponente en la cola
            if (_matchmakingQueue.TryDequeue(out var opponentId) && opponentId != userId)
            {
                // Crear partida
                var gameId = (ulong)DateTime.UtcNow.Ticks;
                var game = new GameSession(gameId, userId, opponentId, channelId);
                _activeGames[gameId] = game;

                await NotifyGameStart(game);
                return (true, $"Partida encontrada! **BATALLA RTS** vs <@{opponentId}>\nUsen `/elegir-civ` para comenzar!");
            }
            else
            {
                // Agregar a la cola
                _matchmakingQueue.Enqueue(userId);
                return (true, "Buscando oponente para batalla RTS...");
            }
        }

        public async Task<(bool success, string message)> LeaveMatchmaking(ulong userId)
        {
            var newQueue = new ConcurrentQueue<ulong>();
            while (_matchmakingQueue.TryDequeue(out var id))
            {
                if (id != userId)
                    newQueue.Enqueue(id);
            }

            while (_matchmakingQueue.TryDequeue(out _)) { }
            while (newQueue.TryDequeue(out var id))
                _matchmakingQueue.Enqueue(id);

            return (true, "Has salido de la cola de matchmaking.");
        }

        public GameSession? GetGameByPlayer(ulong userId)
        {
            return _activeGames.Values.FirstOrDefault(g => g.Player1Id == userId || g.Player2Id == userId);
        }

        public async Task<bool> AbandonGame(ulong userId)
        {
            var game = GetGameByPlayer(userId);
            if (game == null) return false;

            var winnerId = game.Player1Id == userId ? game.Player2Id : game.Player1Id;
            await NotifyGameEnd(game, winnerId, $"<@{userId}> abandono la partida");
            
            _activeGames.TryRemove(game.GameId, out _);
            return true;
        }

        public async Task CheckGameEnd(GameSession game, IMessageChannel? channel)
        {
            try
            {
                // Verificar si algún Centro Cívico fue destruido
                var player1CentroCivico = game.Player1Game.Jugador1.Edificios
                    .OfType<CentroCivico>()
                    .FirstOrDefault();

                var player2CentroCivico = game.Player2Game.Jugador2.Edificios
                    .OfType<CentroCivico>()
                    .FirstOrDefault();

                ulong? winnerId = null;
                string winReason = "";

                // Verificar si el Centro Cívico del Player1 fue destruido
                if (player1CentroCivico == null || player1CentroCivico.VidaActual <= 0)
                {
                    winnerId = game.Player2Id;
                    winReason = "Centro Civico enemigo destruido";
                }
                // Verificar si el Centro Cívico del Player2 fue destruido
                else if (player2CentroCivico == null || player2CentroCivico.VidaActual <= 0)
                {
                    winnerId = game.Player1Id;
                    winReason = "Centro Civico enemigo destruido";
                }
                // Verificar condición de derrota usando la utilidad del juego
                else if (UtilidadesJuego.EstaDerrotada(game.Player1Game.Jugador1.Edificios))
                {
                    winnerId = game.Player2Id;
                    winReason = "Todos los edificios principales destruidos";
                }
                else if (UtilidadesJuego.EstaDerrotada(game.Player2Game.Jugador2.Edificios))
                {
                    winnerId = game.Player1Id;
                    winReason = "Todos los edificios principales destruidos";
                }

                // Si hay un ganador, terminar la partida
                if (winnerId.HasValue && channel != null)
                {
                    await NotifyGameEnd(game, winnerId.Value, winReason);
                    _activeGames.TryRemove(game.GameId, out _);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verificando fin de juego: {ex.Message}");
            }
        }

        private async Task NotifyGameStart(GameSession game)
        {
            var channel = _client.GetChannel(game.ChannelId) as IMessageChannel;
            if (channel == null) return;

            var embed = new EmbedBuilder()
                .WithTitle("BATALLA RTS INICIADA!")
                .WithDescription($"**<@{game.Player1Id}> VS <@{game.Player2Id}>**")
                .WithColor(Color.Red)
                .AddField("Objetivo", "Destruye el Centro Civico enemigo para ganar", false)
                .AddField("Posiciones", $"<@{game.Player1Id}> inicia en (0,0)\n<@{game.Player2Id}> inicia en (99,99)", false)
                .AddField("Empezar", "Ambos usen `/elegir-civ` para seleccionar civilizacion", false)
                .AddField("Estado", "Usa `/estado-batalla` para ver el progreso", false)
                .AddField("Recursos", "Cada jugador inicia con recursos basicos y 3 aldeanos", false)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await channel.SendMessageAsync(embed: embed);
        }

        private async Task NotifyGameEnd(GameSession game, ulong winnerId, string reason)
        {
            var channel = _client.GetChannel(game.ChannelId) as IMessageChannel;
            if (channel == null) return;

            var embed = new EmbedBuilder()
                .WithTitle("BATALLA TERMINADA!")
                .WithDescription($"**Ganador: <@{winnerId}>**")
                .WithColor(Color.Gold)
                .AddField("Resultado", reason, false)
                .AddField("Duracion", $"{DateTime.UtcNow - game.StartTime:mm\\:ss}", false)
                .AddField("Victoria", "Excelente estrategia!", false)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await channel.SendMessageAsync($"<@{winnerId}> <@{game.GetOpponentId(winnerId)}> VICTORIA!", embed: embed);
        }

        public int GetQueueCount() => _matchmakingQueue.Count;
        public int GetActiveGamesCount() => _activeGames.Count;

        // Método para limpiar partidas inactivas
        public async Task CleanupInactiveGames()
        {
            var cutoffTime = DateTime.UtcNow.AddHours(-2);
            var inactiveGames = _activeGames.Values.Where(g => g.LastActivity < cutoffTime).ToList();

            foreach (var game in inactiveGames)
            {
                var channel = _client.GetChannel(game.ChannelId) as IMessageChannel;
                if (channel != null)
                {
                    await channel.SendMessageAsync("Partida terminada por inactividad.");
                }
                _activeGames.TryRemove(game.GameId, out _);
            }
        }
    }

    public class GameSession
    {
        public ulong GameId { get; }
        public ulong Player1Id { get; }
        public ulong Player2Id { get; }
        public ulong ChannelId { get; }
        public DateTime StartTime { get; }
        public DateTime LastActivity { get; set; }

        public JuegoFacade Player1Game { get; }
        public JuegoFacade Player2Game { get; }
        
        public string? Player1Civilization { get; set; }
        public string? Player2Civilization { get; set; }

        public GameSession(ulong gameId, ulong player1Id, ulong player2Id, ulong channelId)
        {
            GameId = gameId;
            Player1Id = player1Id;
            Player2Id = player2Id;
            ChannelId = channelId;
            StartTime = DateTime.UtcNow;
            LastActivity = DateTime.UtcNow;

            Player1Game = new JuegoFacade();
            Player2Game = new JuegoFacade();
        }

        public JuegoFacade GetPlayerGame(ulong userId)
        {
            UpdateActivity();
            return userId == Player1Id ? Player1Game : Player2Game;
        }

        public ulong GetOpponentId(ulong userId)
        {
            return userId == Player1Id ? Player2Id : Player1Id;
        }

        public void UpdateActivity()
        {
            LastActivity = DateTime.UtcNow;
        }

        // Verificar si un jugador ha ganado
        public ulong? CheckWinner()
        {
            // Verificar Centro Cívico Player1
            var player1Centro = Player1Game.Jugador1.Edificios.OfType<CentroCivico>().FirstOrDefault();
            if (player1Centro == null || player1Centro.VidaActual <= 0)
            {
                return Player2Id;
            }

            // Verificar Centro Cívico Player2
            var player2Centro = Player2Game.Jugador2.Edificios.OfType<CentroCivico>().FirstOrDefault();
            if (player2Centro == null || player2Centro.VidaActual <= 0)
            {
                return Player1Id;
            }

            // Verificar usando la lógica de derrota del juego
            if (UtilidadesJuego.EstaDerrotada(Player1Game.Jugador1.Edificios))
            {
                return Player2Id;
            }

            if (UtilidadesJuego.EstaDerrotada(Player2Game.Jugador2.Edificios))
            {
                return Player1Id;
            }

            return null;
        }

        public bool IsPlayerTurn(ulong userId)
        {
            return true;
        }
    }

    public static class DisplayHelper
    {
        public static Embed CreateBattleStateEmbed(GameSession game)
        {
            var embed = new EmbedBuilder()
                .WithTitle("Estado de la Batalla RTS")
                .WithColor(Color.Red)
                .WithTimestamp(DateTimeOffset.Now);

            embed.AddField("Duracion", $"{DateTime.UtcNow - game.StartTime:mm\\:ss}", true);
            embed.AddField("Modo", "**RTS TIEMPO REAL**", true);
            
            // Verificar si hay ganador
            var winner = game.CheckWinner();
            if (winner.HasValue)
            {
                embed.AddField("Estado", $"GANADOR: <@{winner.Value}>", true);
                embed.WithColor(Color.Gold);
            }
            else
            {
                embed.AddField("Estado", "EN PROGRESO", true);
            }

            // Información de Player1
            var player1Centro = game.Player1Game.Jugador1.Edificios.OfType<CentroCivico>().FirstOrDefault();
            var player1Vida = player1Centro?.VidaActual ?? 0;
            var player1MaxVida = player1Centro?.VidaMaxima ?? 1500;
            
            embed.AddField($"Jugador 1: <@{game.Player1Id}> {GetCivEmoji(game.Player1Civilization)}",
                $"Centro Civico: {player1Vida}/{player1MaxVida} HP\n" +
                $"Unidades: {game.Player1Game.Jugador1.Unidades.Count}\n" +
                $"Edificios: {game.Player1Game.Jugador1.Edificios.Count}\n" +
                $"Poblacion: {game.Player1Game.Jugador1.PoblacionActual}/{game.Player1Game.Jugador1.CapacidadPoblacionMaxima}", true);

            // Información de Player2
            var player2Centro = game.Player2Game.Jugador2.Edificios.OfType<CentroCivico>().FirstOrDefault();
            var player2Vida = player2Centro?.VidaActual ?? 0;
            var player2MaxVida = player2Centro?.VidaMaxima ?? 1500;
            
            embed.AddField($"Jugador 2: <@{game.Player2Id}> {GetCivEmoji(game.Player2Civilization)}",
                $"Centro Civico: {player2Vida}/{player2MaxVida} HP\n" +
                $"Unidades: {game.Player2Game.Jugador2.Unidades.Count}\n" +
                $"Edificios: {game.Player2Game.Jugador2.Edificios.Count}\n" +
                $"Poblacion: {game.Player2Game.Jugador2.PoblacionActual}/{game.Player2Game.Jugador2.CapacidadPoblacionMaxima}", true);

            return embed.Build();
        }

        private static string GetCivEmoji(string? civ)
        {
            return civ switch
            {
                "1" => "[JAP]",
                "2" => "[ROM]",
                "3" => "[VIK]",
                _ => ""
            };
        }

        public static Embed CreateHelpEmbed()
        {
            var embed = new EmbedBuilder()
                .WithTitle("Comandos RTS Discord")
                .WithDescription("Batalla en tiempo real contra otros jugadores!")
                .WithColor(Color.Purple);

            embed.AddField("Matchmaking",
                "`/buscar-partida` - Buscar oponente\n" +
                "`/salir-cola` - Salir de la cola\n" +
                "`/abandonar` - Abandonar partida", false);

            embed.AddField("Gestion",
                "`/elegir-civ <tipo>` - Elegir civilizacion\n" +
                "`/estado-batalla` - Ver progreso de batalla\n" +
                "`/mi-estado` - Ver tu estado personal\n" +
                "`/estado-recursos` - Ver recursos actuales", false);

            embed.AddField("Acciones",
                "`/recolectar` - Recolectar recursos\n" +
                "`/construir <tipo> <x> <y>` - Construir edificio\n" +
                "`/entrenar <tipo>` - Entrenar unidad\n" +
                "`/sacar-unidad <x> <y>` - Deployar unidad\n" +
                "`/atacar <x> <y>` - Atacar objetivo", false);

            embed.AddField("Informacion",
                "`/ver-mapa` - Ver estado del mapa\n" +
                "`/ver-aldeanos` - Ver aldeanos en Centro Civico\n" +
                "`/ver-edificios` - Ver todos tus edificios", false);

            embed.AddField("Civilizaciones",
                "**Japoneses**: +25% ataque, +20% oro\n" +
                "**Romanos**: +20% defensa, -10% costos\n" +
                "**Vikingos**: +20% construccion, +30% vida", false);

            embed.AddField("Objetivo",
                "Destruye el Centro Civico enemigo para ganar la partida", false);

            return embed.Build();
        }

        public static Embed CreatePlayerStatsEmbed(GameSession game, ulong playerId)
        {
            var isPlayer1 = playerId == game.Player1Id;
            var playerGame = game.GetPlayerGame(playerId);
            var jugador = isPlayer1 ? playerGame.Jugador1 : playerGame.Jugador2;
            var civilization = isPlayer1 ? game.Player1Civilization : game.Player2Civilization;

            var embed = new EmbedBuilder()
                .WithTitle("Estadisticas Detalladas")
                .WithColor(Color.Blue)
                .WithTimestamp(DateTimeOffset.Now);

            // Información básica
            embed.AddField("Civilizacion", GetCivName(civilization), true);
            embed.AddField("Poblacion", $"{jugador.PoblacionActual}/{jugador.CapacidadPoblacionMaxima}", true);
            embed.AddField("Tiempo en Partida", $"{DateTime.UtcNow - game.StartTime:mm\\:ss}", true);

            // Recursos
            var recursos = jugador.Inventario.Recursos;
            embed.AddField("Recursos",
                $"Madera: {recursos[TipoRecurso.Madera].CantidadDisponible}\n" +
                $"Piedra: {recursos[TipoRecurso.Piedra].CantidadDisponible}\n" +
                $"Oro: {recursos[TipoRecurso.Oro].CantidadDisponible}\n" +
                $"Alimento: {recursos[TipoRecurso.Alimento].CantidadDisponible}", true);

            // Edificios
            var edificiosPorTipo = jugador.Edificios.GroupBy(e => e.GetType().Name);
            var edificiosInfo = string.Join("\n", edificiosPorTipo.Select(g => $"{g.Key}: {g.Count()}"));
            embed.AddField("Edificios", edificiosInfo.Length > 0 ? edificiosInfo : "Ninguno", true);

            // Unidades
            var unidadesPorTipo = jugador.Unidades.GroupBy(u => u.GetType().Name);
            var unidadesInfo = string.Join("\n", unidadesPorTipo.Select(g => $"{g.Key}: {g.Count()}"));
            embed.AddField("Unidades", unidadesInfo.Length > 0 ? unidadesInfo : "Ninguna", true);

            return embed.Build();
        }

        private static string GetCivName(string? tipo)
        {
            return tipo switch
            {
                "1" => "Japoneses",
                "2" => "Romanos",
                "3" => "Vikingos",
                _ => "No elegida"
            };
        }
    }
}