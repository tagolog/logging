using System;

namespace Tagolog.Internal
{
    internal interface IApplicationRepository
    {
        ApplicationEntity GetByCode( Guid code );
    }
}
