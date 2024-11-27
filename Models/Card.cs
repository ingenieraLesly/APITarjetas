using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrimoAPITarjetas.Models
{
    public class Card
    {
        public int CardId { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        [JsonIgnore]
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public Card() { }

        public Card(string cardNumber)
        {
            CardNumber = cardNumber;
        }
    }
}
