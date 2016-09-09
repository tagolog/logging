using System;

namespace Tagolog.Private.Correlation
{
    class ThreadGuidCorrelationId : ThreadCorrelationId
    {
        public ThreadGuidCorrelationId()
        {
        }

        public ThreadGuidCorrelationId( ThreadCorrelationId parent )
            : base( parent )
        {
        }

        #region Overrides of CorrelationId

        protected override string GenerateNewBaseId()
        {
            return Guid.NewGuid().ToString();
        }

        #endregion
    }
}
