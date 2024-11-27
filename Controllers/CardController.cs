using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PrimoAPITarjetas.Models;
using PrimoAPITarjetas.Services;
using System.Threading.Tasks;

namespace PrimoAPITarjetas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly CardService _cardService;
        private readonly FeeService _feeService;
        private readonly ILogger<CardController> _logger;

        public CardController(CardService cardService, FeeService feeService, ILogger<CardController> logger)
        {
            _cardService = cardService;
            _feeService = feeService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Card>> CreateCard()
        {
            var card = await _cardService.CreateCardAsync();
            return CreatedAtAction(nameof(GetCard), new { id = card.CardId }, card);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
            var card = await _cardService.GetCardAsync(id);

            if (card == null)
            {
                return NotFound();
            }

            return card;
        }

        [HttpPost("{id}/pay")]
        public async Task<IActionResult> Pay(int id, [FromQuery] decimal amount)
        {
            var card = await _cardService.GetCardAsync(id);
            if (card == null)
            {
                return NotFound("Card not found.");
            }

            var fee = _feeService.CalculateFee(amount);
            var success = await _cardService.ProcessPaymentAsync(id, amount, fee);

            if (!success)
            {
                return BadRequest("Insufficient balance or card not found.");
            }

            var newBalance = await _cardService.GetBalanceAsync(id);
            return Ok(new
            {
                Card = card, // Añadir la información de la tarjeta
                Amount = amount,
                Fee = fee,
                TotalAmount = amount + fee,
                NewBalance = newBalance
            });
        }

        [HttpGet("{id}/balance")]
        public async Task<ActionResult<decimal>> GetBalance(int id)
        {
            var balance = await _cardService.GetBalanceAsync(id);

            if (balance == null)
            {
                return NotFound();
            }

            return balance.Value;
        }
    }
}
