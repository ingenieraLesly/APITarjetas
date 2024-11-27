using Newtonsoft.Json;

namespace PrimoAPITarjetas.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty; // Inicializar con un valor vacío

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
