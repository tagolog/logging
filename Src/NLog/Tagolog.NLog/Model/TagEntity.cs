using System;

namespace Tagolog.NLog.Model
{
    class TagEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public long RowId { get; set; }

        //public Guid CustomerId { get; protected set; }
        //public string Code { get; protected set; }
        //public string Key { get; set; }
        //public string Description { get; set; }
        //public Guid Id { get; protected set; }
        //public long RowId { get; protected set; }

    }
}
