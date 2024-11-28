namespace ProjectPortfolio.Models
{
    public class ResponseModel<T>
    {
        public T? Data { get; set; }
        public List<string> ValidationMessages { get; set; } = [];
        public string Message { get; set; }
        public bool Status { get; set; }
        public bool IsError => ValidationMessages.Count != 0;
    }
}
