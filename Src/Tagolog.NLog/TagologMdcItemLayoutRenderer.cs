using System.Text;
using System.Collections.Generic;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.LayoutRenderers;

namespace Tagolog.NLog
{
    [LayoutRenderer( "tagologmdcitem" )]
    public class TagologMdcItemLayoutRenderer : LayoutRenderer
    {
        /// <summary>
        /// Gets or sets the name of the item from Tagolog MDC context.
        /// </summary>
        [RequiredParameter]
        [DefaultParameter]
        public string Item { get; set; }

        /// <summary>
        /// Renders the specified MDC item and appends it to the specified <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to append the rendered data to.</param>
        /// <param name="logEvent">Logging event.</param>
        protected override void Append( StringBuilder builder, LogEventInfo logEvent )
        {
            var tagologContext = MdcStorage.GetContext();

            if ( string.IsNullOrEmpty( tagologContext ) )
            {
                InternalLogger.Trace( "TagologMdcItemLayoutRenderer: tagolog context is null or empty." );
                return;
            }

            IDictionary<string, string> tags;
            IDictionary<string, string> builtInTags;

            TagSerializer.TagsFromString( tagologContext, out tags, out builtInTags );

            InternalLogger.Trace( "TagologMdcItemLayoutRenderer: [Tags.Count={0}] [BuiltInTags.Count={1}]", tags.Count, builtInTags.Count );

            string itemValue;
            if ( tags.ContainsKey( Item ) )
                itemValue = tags[ Item ];
            else if ( builtInTags.ContainsKey( Item ) )
                itemValue = builtInTags[ Item ];
            else
                return;

            builder.Append( itemValue );
        }
    }
}
