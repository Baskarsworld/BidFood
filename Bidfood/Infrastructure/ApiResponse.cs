using System.Net;

namespace Bidfood.Infrastructure
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public Error? Error { get; set; }
    }

    public class Error
    {
        public string? Code { get; set; }
        public string? Message { get; set; }
        public string? Target { get; set; }
    }

    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public Error? Error { get; set; }
    }
}
