using NLog;
using Tagolog.Examples.CloudHosting.EmulatorInterface;

namespace Tagolog.Examples.CloudHosting.DotNetCore.NLog.ConsoleExample
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
            _logger.Info( format, args );
        }

        readonly ILogger _logger = LogManager.GetLogger( typeof( EmulatorLogger ).Name );
    }
}
