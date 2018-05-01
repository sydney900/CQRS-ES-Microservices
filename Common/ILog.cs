namespace Common
{
    public interface ILog
    {
        void LogError(string error);
        void LogEvent(string evt);
    }
}
