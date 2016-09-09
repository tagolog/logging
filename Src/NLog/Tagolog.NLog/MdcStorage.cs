using NLog;

namespace Tagolog.NLog
{
    public static class MdcStorage
    {
        public static void Save( string context )
        {
            MappedDiagnosticsContext.Set( TagologContextParamName, context );
        }

        public static void Clear()
        {
            MappedDiagnosticsContext.Remove( TagologContextParamName );
        }

        public static string GetContext()
        {
            if ( !MappedDiagnosticsContext.Contains( TagologContextParamName ) )
                return string.Empty;

            return MappedDiagnosticsContext.Get( TagologContextParamName );
        }

        const string TagologContextParamName = "tagolog-NLog-MDC-2C4FE90F-07F5-4E3C-AA4F-350312A353C7";
    }
}
