namespace Delta.Api.Wrapper
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int TotalRecords { get; set; }
        public List<string> FailedLists { get; set; }
        public ApiResponse(List<string> failedLists, int statusCode, string message, int totalRecords) 
        {
            FailedLists = failedLists;
            StatusCode = statusCode;
            Message = message;
            TotalRecords = totalRecords;
        }
    }
}
