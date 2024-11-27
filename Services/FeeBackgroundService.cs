using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PrimoAPITarjetas.Services
{
    public class FeeBackgroundService : BackgroundService
    {
        private readonly FeeService _feeService;
        private readonly ILogger<FeeBackgroundService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromHours(1); // Intervalo de actualización de tarifas

        public FeeBackgroundService(FeeService feeService, ILogger<FeeBackgroundService> logger)
        {
            _feeService = feeService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Lógica para actualizar las tarifas
                    _feeService.UpdateFee();
                    _logger.LogInformation("Tarifa actualizada exitosamente.");

                    // Esperar el intervalo especificado antes de la siguiente actualización
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // Manejar la excepción si la operación es cancelada
                    _logger.LogInformation("Actualización de tarifas cancelada.");
                }
                catch (Exception ex)
                {
                    // Manejar cualquier otra excepción que pueda ocurrir
                    _logger.LogError(ex, "Error al actualizar la tarifa.");
                }
            }
        }
    }
}
