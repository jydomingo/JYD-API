using System.Net;

namespace JYD.Helper
{
    public class ResponseParameter
    {
        public HttpStatusCode Code { get; set; }
        public string Data { get; set; }
        public string ErrorMessage { get; set; }

    }
}
