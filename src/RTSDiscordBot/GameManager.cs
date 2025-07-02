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
                return (false, "❌ Ya estás en una partida activa. Usa `/abandonar` para salir.");
            }

            // Verificar si ya está en cola
            if (_matchmakingQueue.Contains(userId))
            {
                return (false, "⏳ Ya estás en la cola de matchmaking.");
            }

            // Buscar oponente en la cola
            if (_matchmakingQueue.TryDequeue(out var opponentId) && opponentId != userId)
            {
                // Crear partida
                var gameId = (ulong)DateTime.UtcNow.Ticks;
                var game = new GameSession(gameId, userId, opponentId, channelId);
                _activeGames[gameId] = game;

                await NotifyGameStart(game);
                return (true, $"🎮 ¡Partida encontrada! ⚔️ **BATALLA RTS** vs <@{opponentId}>\n🚀 ¡Usen `/elegir-civ` para comenzar!");
            }
            else
            {
                // Agregar a la cola
                _matchmakingQueue.Enqueue(userId);
                return (true, "⏳ Buscando oponente para batalla RTS...");
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

            return (true, "❌ Has salido de la cola de matchmaking.");
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
            await NotifyGameEnd(game, winnerId, $"<@{userId}> abandonó la partida");
            
            _activeGames.TryRemove(game.GameId, out _);
            return true;
        }

        private async Task NotifyGameStart(GameSession game)
        {
            var channel = _client.GetChannel(game.ChannelId) as IMessageChannel;
            if (channel == null) return;

            var embed = new EmbedBuilder()
                .WithTitle("⚔️ ¡BATALLA RTS INICIADA!")
                .WithDescription($"**<@{game.Player1Id}> VS <@{game.Player2Id}>**")
                .WithColor(Color.Red)
                .AddField("🎯 Objetivo", "Destruye el Centro Cívico enemigo para ganar", false)
                .AddField("🚀 Empezar", "Ambos usen `/elegir-civ` para seleccionar civilización", false)
                .AddField("📊 Estado", "Usa `/estado-batalla` para ver el progreso", false)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await channel.SendMessageAsync(embed: embed);
        }

        private async Task NotifyGameEnd(GameSession game, ulong winnerId, string reason)
        {
            var channel = _client.GetChannel(game.ChannelId) as IMessageChannel;
            if (channel == null) return;

            var embed = new EmbedBuilder()
                .WithTitle("🏆 ¡BATALLA TERMINADA!")
                .WithDescription($"**Ganador: <@{winnerId}>**")
                .WithColor(Color.Gold)
                .AddField("📋 Resultado", reason, false)
                .AddField("⏱️ Duración", $"{DateTime.UtcNow - game.StartTime:mm\\:ss}", false)
                .WithTimestamp(DateTimeOffset.Now)
                .Build();

            await channel.SendMessageAsync(embed: embed);
        }

        public int GetQueueCount() => _matchmakingQueue.Count;
        public int GetActiveGamesCount() => _activeGames.Count;
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

        // Verificar si un jugador ha ganado (simplificado por ahora)
        public ulong? CheckWinner()
        {
            // Por ahora retornamos null - implementaremos la lógica de victoria después
            return null;
        }
    }

    public static class DisplayHelper
    {
        public static Embed CreateBattleStateEmbed(GameSession game)
        {
            var embed = new EmbedBuilder()
                .WithTitle("⚔️ Estado de la Batalla RTS")
                .WithColor(Color.Red)
                .WithTimestamp(DateTimeOffset.Now);

            embed.AddField("⏱️ Duración", $"{DateTime.UtcNow - game.StartTime:mm\\:ss}", true);
            embed.AddField("🎮 Modo", "**RTS TIEMPO REAL**", true);
            embed.AddField("📊 Estado", "EN PROGRESO", true);

            // Información básica de los jugadores
            embed.AddField($"🔵 <@{game.Player1Id}> {GetCivEmoji(game.Player1Civilization)}",
                "Estado: Jugando", false);

            embed.AddField($"🔴 <@{game.Player2Id}> {GetCivEmoji(game.Player2Civilization)}",
                "Estado: Jugando", false);

            return embed.Build();
        }

        private static string GetCivEmoji(string? civ)
        {
            return civ switch
            {
                "1" => "🗾",
                "2" => "🏛️",
                "3" => "⚡",
                _ => ""
            };
        }

        public static Embed CreateHelpEmbed()
        {
            var embed = new EmbedBuilder()
                .WithTitle("🎮 Comandos RTS Discord")
                .WithDescription("¡Batalla en tiempo real contra otros jugadores!")
                .WithColor(Color.Purple);

            embed.AddField("🔍 Matchmaking",
                "`/buscar-partida` - Buscar oponente\n" +
                "`/salir-cola` - Salir de la cola\n" +
                "`/abandonar` - Abandonar partida", false);

            embed.AddField("🏛️ Gestión",
                "`/elegir-civ <tipo>` - Elegir civilización\n" +
                "`/estado-batalla` - Ver progreso de batalla\n" +
                "`/mi-estado` - Ver tu estado personal", false);

            embed.AddField("⚔️ Acciones",
                "`/recolectar` - Recolectar recursos\n" +
                "`/construir <tipo> <x> <y>` - Construir edificio\n" +
                "`/entrenar <tipo>` - Entrenar unidad", false);

            embed.AddField("🏛️ Civilizaciones",
                "🗾 **Japoneses**: +25% ataque, +20% oro\n" +
                "🏛️ **Romanos**: +20% defensa, -10% costos\n" +
                "⚡ **Vikingos**: +20% construcción, +30% vida", false);

            return embed.Build();
        }
    }
}