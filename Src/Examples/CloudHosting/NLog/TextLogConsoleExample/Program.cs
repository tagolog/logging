using System;
using Tagolog.Examples.CloudHosting.Emulator;

namespace Tagolog.Examples.CloudHosting.NLog.TextLogConsoleExample
{
    class Program
    {
        static void Main( string[] args )
        {
            CloudHostingEmulator.Emulate();

            Console.WriteLine( "Press any key..." );
            Console.ReadKey();
        }
    }
}
