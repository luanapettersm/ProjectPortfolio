namespace ProjectPortfolio.Models
{
    public class ResponseModel<T>
    {
        public T? Data { get; set; }
        public List<string> ValidaationMessages { get; set; } = [];
        public string Message { get; set; }
        public bool Status { get; set; }
        public bool IsError => ValidaationMessages.Any();
    }
}
