namespace IoT.Domain.Interfaces
{
    public interface ILogger<TModel> where TModel :class
    {
        void Logger(TModel model);
    }
}
