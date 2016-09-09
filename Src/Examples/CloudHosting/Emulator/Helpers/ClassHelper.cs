using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Tagolog.Examples.CloudHosting.Emulator.Helpers
{
    static class ClassHelper
    {
        [MethodImpl( MethodImplOptions.NoInlining )]
        public static string GetCurrentMethodName()
        {
            // 1 - means get upper method.
            return new StackTrace().GetFrame( 1 ).GetMethod().Name;
        }
    }
}
