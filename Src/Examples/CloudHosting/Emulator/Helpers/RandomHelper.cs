using System;

namespace Tagolog.Examples.CloudHosting.Emulator.Helpers
{
    static class RandomHelper
    {
        public static int GenerateInt( int min, int max )
        {
            return Random.Next( min, max );
        }

        public static bool GenerateBoolean()
        {
            return Random.Next( 0, 100 ) > 50;
        }

        readonly static Random Random = new Random();
    }
}
