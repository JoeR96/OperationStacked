using System.Net;
using System.Net.Http.Headers;

namespace OperationStacked.Response
{
    public class Response
    {
        public Response(HttpResponseMessage responseMessage)
        {
            this.Message = responseMessage;
            this.StatusCode = responseMessage.StatusCode;
            this.Headers = responseMessage.Headers;
        }

        public HttpResponseHeaders Headers { get; private set; }
        public HttpResponseMessage Message { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
    }

    public class Response<TResponse> : Response
    {
        public Response(TResponse data, HttpResponseMessage responseMessage) : base(responseMessage)
        {
            this.Data = data;
        }

        public TResponse? Data { get; private set; }
    }
}
