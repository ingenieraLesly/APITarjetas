using PrimoAPITarjetas.Data;
using PrimoAPITarjetas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace PrimoAPITarjetas.Services
{
    public class CardService
    {
        private readonly CardContext _context;
        private readonly ILogger<CardService> _logger;

        public CardService(CardContext context, ILogger<CardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Card> CreateCardAsync()
        {
            var cardNumber = GenerateCardNumber();

            var card = new Card
            {
                CardNumber = cardNumber,
                Balance = 1000000
            };

            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return card;
        }

        public async Task<Card?> GetCardAsync(int id)
        {
            return await _context.Cards.Include(c => c.Transactions).FirstOrDefaultAsync(c => c.CardId == id);
        }


        public async Task<bool> ProcessPaymentAsync(int id, decimal amount, decimal fee)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                _logger.LogWarning($"Card with ID {id} not found.");
                return false;
            }

            decimal totalAmount = amount + fee;
            if (card.Balance < totalAmount)
            {
                _logger.LogWarning($"Insufficient balance for card ID {id}. Current balance: {card.Balance}, required: {totalAmount}");
                return false;
            }

            card.Balance -= totalAmount;

            var transaction = new Transaction
            {
                CardId = id,
                Amount = amount,
                TransactionDate = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Payment processed successfully for card ID {id}. New balance: {card.Balance}");
            return true;
        }

        public async Task<decimal?> GetBalanceAsync(int id)
        {
            // Buscar la tarjeta en la base de datos
            var card = await _context.Cards.FindAsync(id);

            // Verificar si la tarjeta no fue encontrada
            if (card == null)
            {
                _logger.LogWarning($"Card with ID {id} not found.");
                return null; // Retornar null si la tarjeta no se encuentra
            }

            // Retornar el saldo de la tarjeta si se encuentra
            return card.Balance;
        }


        private string GenerateCardNumber()
        {
            var random = new Random();
            var cardNumber = new StringBuilder(15);

            for (int i = 0; i < 15; i++)
            {
                cardNumber.Append(random.Next(0, 10));
            }

            return cardNumber.ToString();
        }
    }
}
