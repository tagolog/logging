namespace Tagolog.Examples.CloudHosting.EmulatorInterface
{
    /// <summary>
    /// Generic logging interface that will be redirected
    /// to NLog, CommonLogging or something else
    /// in the implementation.
    /// </summary>
    public interface IEmulatorLogger
    {
        void Info( string message );
        void Error( string message );
        void InfoFormat( string format, params object[] args );
    }
}
