using System;
using NLog;
using NLog.Config;
using Tagolog.Examples.CloudHosting.Emulator;

namespace Tagolog.Examples.CloudHosting.DotNetCore.NLog.ConsoleExample
{
    class Program
    {
        static void Main( string[] args )
        {
            InitializeLoggingConfiguration();

            var emulatorLogger = new EmulatorLogger();
            var cloudHostingEmulator = new CloudHostingEmulator( emulatorLogger );
            cloudHostingEmulator.Emulate();

            Console.WriteLine( "Press any key..." );
            Console.ReadKey();
        }

        protected static void InitializeLoggingConfiguration()
        {
            TagScopeManager.ConfigureLogAdapter( "Tagolog.NLog.LogAdapter,Tagolog.NLog" );

            LogManager.Configuration = new XmlLoggingConfiguration( "NLog.config" );
            LogManager.ThrowExceptions = true;
            LogManager.Configuration.Reload();
        }
    }
}
