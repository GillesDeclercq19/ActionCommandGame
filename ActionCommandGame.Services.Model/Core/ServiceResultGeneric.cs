namespace ActionCommandGame.Services.Model.Core
{
    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }
    }
}
