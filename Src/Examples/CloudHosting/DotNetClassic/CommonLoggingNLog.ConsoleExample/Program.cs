using System;
using Tagolog;
using Tagolog.Examples.CloudHosting.Emulator;

namespace CommonLoggingNLog.ConsoleExample
{
    class Program
    {
        static void Main( string[] args )
        {
            TagScopeManager.ConfigureLogAdapter( "Tagolog.NLog.LogAdapter,Tagolog.NLog" );

            var emulatorLogger = new EmulatorLogger();
            var cloudHostingEmulator = new CloudHostingEmulator( emulatorLogger );
            cloudHostingEmulator.Emulate();

            Console.WriteLine( "Press any key..." );
            Console.ReadKey();
        }
    }
}
