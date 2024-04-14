namespace Movement_be.Interfaces
{
    public interface IHttpService
    {
        Task<string> CallRestApi(HttpMethod httpMethod, string requestUri, string parameterString);
        
    }
}
