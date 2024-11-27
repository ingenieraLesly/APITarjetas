using System;
using System.Threading;

namespace PrimoAPITarjetas.Services
{
    public class FeeService
    {
        private decimal _currentFee;
        private Timer _timer;
        private static readonly Random _random = new Random();
        private static readonly object _lock = new object();

        public FeeService()
        {
            _currentFee = 1.0m;
            _timer = new Timer(UpdateFee, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        }

        public decimal CalculateFee(decimal amount)
        {
            return amount * _currentFee;
        }

        public decimal GetCurrentFee()
        {
            lock (_lock)
            {
                return _currentFee;
            }
        }

        public void UpdateFee(object? state = null)
        {
            lock (_lock)
            {
                try
                {
                    decimal randomDecimal = (decimal)_random.NextDouble() * 2;
                    _currentFee *= randomDecimal;

                    if (_currentFee < 0.01m)
                    {
                        _currentFee = 0.01m;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al actualizar la tarifa: {ex.Message}");
                }
            }
        }
    }
}
