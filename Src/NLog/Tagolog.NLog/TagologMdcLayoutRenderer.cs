using System.Linq;
using System.Text;
using System.Collections.Generic;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;

namespace Tagolog.NLog
{
    [LayoutRenderer("tagologmdc")]
    public class TagologMdcLayoutRenderer : LayoutRenderer
    {
        public TagologMdcLayoutRenderer()
        {
            OrderBy = false;
            BuiltInTags = false;
        }

        /// <summary>
        /// Gets or sets format string for the the tag.
        /// </summary>
        [RequiredParameter]
        [DefaultParameter]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the key of the tag inside format string.
        /// </summary>
        [RequiredParameter]
        [DefaultParameter]
        public string TagKey { get; set; }

        /// <summary>
        /// Gets or sets the value of the tag inside format string.
        /// </summary>
        [RequiredParameter]
        [DefaultParameter]
        public string TagValue { get; set; }

        public bool OrderBy { get; set; }

        public bool BuiltInTags { get; set; }

        public string Separator { get; set; }

        public string Prefix { get; set; }

        /// <summary>
        /// Renders the specified MDC item and appends it to the specified <see cref="StringBuilder" />.
        /// </summary>
        /// <param key="builder">The <see cref="StringBuilder"/> to append the rendered data to.</param>
        /// <param key="logEvent">Logging event.</param>
        protected override void Append( StringBuilder builder, LogEventInfo logEvent )
        {
            var tagologContext = MdcStorage.GetContext();

            if ( string.IsNullOrEmpty( tagologContext ) )
                return;

            IDictionary<string, string> tags;
            IDictionary<string, string> builtInTags;

            TagSerializer.TagsFromString( tagologContext, out tags, out builtInTags );

            var tagsAsList = tags.ToList();
            if ( BuiltInTags )
                tagsAsList.AddRange( builtInTags.ToList() );

            var orderByTags = ( OrderBy ) ?
                tagsAsList.OrderBy( _ => _.Key ).ToList() :
                tagsAsList.ToList();

            if ( 0 != orderByTags.Count )
                builder.Append( Prefix );

            for ( var i = 0 ; i < orderByTags.Count ; i ++ )
            {
                var tagPair = orderByTags[ i ];
                builder.Append( TagToString( tagPair.Key, tagPair.Value ) );

                if ( i < orderByTags.Count - 1 )
                {
                    if ( !string.IsNullOrEmpty( Separator ) )
                        builder.Append( Separator );
                }
            }
        }

        #region Helpers

        string TagToString( string key, string value )
        {
            return Format
                .Replace( @"\n", "\n" )
                .Replace( TagKey, key )
                .Replace( TagValue, value );
        }

        #endregion // Helpers
    }
}
