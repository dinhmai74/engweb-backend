namespace HollypocketBackend.Models
{
    interface Response
    {
        public bool Error { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }

    public class APIResponse : Response
    {
        public bool Error { get; set; } = false;
        public object Data { get; set; }
        public string Message { get; set; }

    }

    public class ErrorResponse : Response
    {
        public bool Error { get; set; } = true;
        public object Data { get; set; }
        public string Message { get; set; }

    }

}