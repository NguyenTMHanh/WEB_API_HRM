namespace WEB_API_HRM.RSP
{
    public class Response
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public List<string> Errors { get; set; }

        public Response(int code, string message, object data = null, List<string> errors = null)
        {
            Code = code;
            Message = message;
            Data = data;
            Errors = errors ?? new List<string>();
        }
    }
}
