using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RTSDiscordBot
{
    public class GameCleanupService : BackgroundService
    {
        private readonly GameManager _gameManager;
        private readonly ILogger<GameCleanupService> _logger;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(30); // Limpiar cada 30 minutos

        public GameCleanupService(GameManager gameManager, ILogger<GameCleanupService> logger)
        {
            _gameManager = gameManager;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de limpieza de partidas iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _gameManager.CleanupInactiveGames();
                    _logger.LogInformation("Limpieza de partidas completada");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error durante la limpieza de partidas");
                }

                await Task.Delay(_cleanupInterval, stoppingToken);
            }
        }
    }
}