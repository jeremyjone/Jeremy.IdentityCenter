namespace Jeremy.IdentityCenter.Business.Common.Models
{
    public enum Code
    {
        Success = 200,
        Failed = 400,
        UnAuthorization = 401
    }

    public abstract class ResponseMessage
    {
        public Code Code { get; set; } = Code.Success;
        public string Status { get; set; }

        public static ResponseMessage<T> Success<T>(T response)
        {
            return new ResponseMessage<T>(Code.Success, response);
        }

        public static ResponseMessage<T> Failed<T>(T response)
        {
            return new ResponseMessage<T>(Code.Failed, response);
        }
    }

    public class ResponseMessage<T> : ResponseMessage
    {
        public ResponseMessage(Code code, string status, T response)
        {
            Code = code;
            Status = status;
            Response = response;
        }

        public ResponseMessage(Code code, T response)
        {
            Code = code;
            Response = response;
            Status = code.ToString();
        }

        public T Response { get; set; }
    }
}
