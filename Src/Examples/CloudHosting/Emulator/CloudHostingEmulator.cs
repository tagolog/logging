using System;
using System.Threading;
using System.Collections.Generic;
using Tagolog.Examples.CloudHosting.Emulator.Helpers;

namespace Tagolog.Examples.CloudHosting.Emulator
{
    public class CloudHostingEmulator
    {
        public static void Emulate()
        {
            for ( var i = 0 ; i < 10 ; i++ )
                ThreadPool.QueueUserWorkItem( EmulateForSingleUser, null );
        }

        static void EmulateForSingleUser( object state )
        {
            var email = RegisterNewUser();
            for ( var i = 0 ; i < 10 ; i ++ )
            {
                var sessionId = WebInterface.Authenticate( email );
                if ( !string.IsNullOrEmpty( sessionId ) )
                {
                    var vmName = "VM-" + Guid.NewGuid().ToString( "D" );
                    if ( WebInterface.OrderNewVirtualMachine( sessionId, vmName, RandomHelper.GenerateInt( 10, 30 ) * 1.0M ) )
                    {
                        WebInterface.StartVirtualMachine( sessionId, vmName );

                        Thread.Sleep( RandomHelper.GenerateInt( 20, 30 ) * 1000 );

                        WebInterface.StopVirtualMachine( sessionId, vmName );
                    }
                }

                Thread.Sleep( RandomHelper.GenerateInt( 1, 10 ) * 1000 );
            }
        }

        static string RegisterNewUser()
        {
            var lastName = GetRandomName( EnglishNameHelper.LastNames );
            var firstName = GetRandomName( EnglishNameHelper.FirstNames );
            var email = string.Format( "{0}.{1}@tagolog.com", firstName, lastName );

            WebInterface.RegisterNewUser( email, firstName, lastName );

            return email;
        }

        static string GetRandomName( IReadOnlyList<string> names )
        {
            var randomIndex = RandomHelper.GenerateInt( 0, names.Count );
            return names[ randomIndex ];
        }
    }
}
