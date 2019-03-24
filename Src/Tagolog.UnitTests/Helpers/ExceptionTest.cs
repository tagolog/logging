using System;

namespace Tagolog.UnitTests.Helpers
{
    static class ExceptionTest
    {
        public static bool IsThrown<TException>( Action actionThatThrowsException )
            where TException : Exception
        {
            try
            {
                actionThatThrowsException.Invoke();
            }
            catch ( TException )
            {
                return true;
            }

            return false;
        }
    }
}
