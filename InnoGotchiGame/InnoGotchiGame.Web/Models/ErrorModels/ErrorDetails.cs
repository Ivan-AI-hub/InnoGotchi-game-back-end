using System.Text.Json;

namespace InnoGotchiGame.Web.Models.ErrorModels
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ErrorDetails(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ErrorDetails(int statusCode, IEnumerable<string> messages)
        {
            StatusCode = statusCode;
            Message = string.Join('\n', messages);
        }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
