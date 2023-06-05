using System.Text.Json;

namespace API.Models
{
    // TODO: Replace this class with a record struct/class.
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
