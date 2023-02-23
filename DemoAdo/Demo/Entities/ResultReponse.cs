namespace Demo.Entities
{
    public class ResultReponse<T>
    {
        public string Message { get; set; } 
        public T Data { get; set; }
        public ResultReponse(string message, T data)
        {
            this.Data = data;
            this.Message = message; 
        }
    }
    public class ResultReponse
    {
        public string Message { get; set; }
        public ResultReponse(string message)
        {
            this.Message = message;
        }
    }
}
