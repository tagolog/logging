using System;
using System.Threading;
using System.Collections.Generic;
using Tagolog.Examples.CloudHosting.Emulator.Helpers;
using Tagolog.Examples.CloudHosting.EmulatorInterface;

namespace Tagolog.Examples.CloudHosting.Emulator
{
    public class CloudHostingEmulator
    {
        public CloudHostingEmulator( IEmulatorLogger emulatorLogger )
        {
            _emulatorLogger = emulatorLogger;
            _webInterface = new WebInterface( emulatorLogger );
        }

        public void Emulate()
        {
            for ( var i = 0 ; i < 10 ; i++ )
                ThreadPool.QueueUserWorkItem( EmulateForSingleUser, null );
        }

        void EmulateForSingleUser( object state )
        {
            var email = RegisterNewUser();
            for ( var i = 0 ; i < 10 ; i ++ )
            {
                var sessionId = _webInterface.Authenticate( email );
                if ( !string.IsNullOrEmpty( sessionId ) )
                {
                    var vmName = "VM-" + Guid.NewGuid().ToString( "D" );
                    if ( _webInterface.OrderNewVirtualMachine( sessionId, vmName, RandomHelper.GenerateInt( 10, 30 ) * 1.0M ) )
                    {
                        _webInterface.StartVirtualMachine( sessionId, vmName );

                        Thread.Sleep( RandomHelper.GenerateInt( 20, 30 ) * 1000 );

                        _webInterface.StopVirtualMachine( sessionId, vmName );
                    }
                }

                Thread.Sleep( RandomHelper.GenerateInt( 1, 10 ) * 1000 );
            }
        }

        string RegisterNewUser()
        {
            var lastName = GetRandomName( EnglishNameHelper.LastNames );
            var firstName = GetRandomName( EnglishNameHelper.FirstNames );
            var email = string.Format( "{0}.{1}@tagolog.com", firstName, lastName );

            _webInterface.RegisterNewUser( email, firstName, lastName );

            return email;
        }

        static string GetRandomName( IReadOnlyList<string> names )
        {
            var randomIndex = RandomHelper.GenerateInt( 0, names.Count );
            return names[ randomIndex ];
        }

        readonly IEmulatorLogger _emulatorLogger;
        readonly WebInterface _webInterface;
    }
}
