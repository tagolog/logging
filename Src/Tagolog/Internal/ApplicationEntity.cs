using System;

namespace Tagolog.Internal
{
    internal class ApplicationEntity
    {
        public Guid Id { get; set; }
        public long RowId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public string EncryptionKey { get; set; }

    }
}
