using System;

namespace Tagolog.Examples.CloudHosting.Emulator.Model
{
    class User
    {
        public User( string email )
        {
            UserId = Guid
                .NewGuid()
                .ToString( "D" )
                .ToUpper();
        }

        public string UserId { get; private set; }
        public string Email { get; private set; }
    }
}
