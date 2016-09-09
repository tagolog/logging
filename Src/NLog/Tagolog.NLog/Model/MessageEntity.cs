COMPILE NONE

using System;
using System.Linq;
using System.Collections.Generic;

namespace Tagolog.NLog.Model
{
    class MessageEntity
    {
        public Guid Id { get; set; }

        public Guid ApplicationId { get; set; }
        public Guid CustomerId { get; set; }

        public DbLogLevel LogLevel { get; set; }
        public DateTime Created { get; set; }
        public DateTime Logged { get; set; }
        public string UserIdentity { get; set; }
        public string LogIPAddress { get; set; }
        public string RequestIPAddress { get; set; }

        public string MsgText { get; set; }
        public string ExceptionText { get; set; }

        public string CorrelationId { get; set; }

        public int PacketLength
        {
            get
            {
                var messageLength = MsgText.Length;
                var exceptionText = ( ! string.IsNullOrWhiteSpace( ExceptionText ) ) ? ExceptionText.Length : 0;
                var tagLength = Tags.Select( _ => _.Code.Length + _.Value.Length ).Sum();

                return messageLength + exceptionText + tagLength;
            }
        }

        public string SphinxTagsText { get; set; }

        public IList<TagEntity> Tags { get; set; }
    }
}
