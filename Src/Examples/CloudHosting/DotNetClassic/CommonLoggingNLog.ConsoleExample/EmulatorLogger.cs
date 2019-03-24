using Common.Logging;
using Tagolog.Examples.CloudHosting.EmulatorInterface;

namespace CommonLoggingNLog.ConsoleExample
{
    class EmulatorLogger : IEmulatorLogger
    {
        public void Info( string message )
        {
            _logger.Info( message );
        }

        public void Error( string message )
        {
            _logger.Error( message );
        }

        public void InfoFormat( string format, params object[] args )
        {
            _logger.InfoFormat( format, args );
        }

        readonly ILog _logger = LogManager.GetLogger<EmulatorLogger>();
    }
}
