using System.IO;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Layouts;
using Newtonsoft.Json;

namespace Tagolog.NLog
{
    /// <summary>
    /// An extension of <see cref="JsonLayout"/> that renders Tagolog tag collection from MDC.
    /// </summary>
    [Layout("TagologJsonLayout")]
    public class TagologJsonLayout : JsonLayout
    {
        public TagologJsonLayout()
        {
            BuiltInTagsEnabled = false;
            TagsKeyPrefix = string.Empty;
            // TagsContainerName = string.Empty;
        }

        /// <summary>
        /// Gets or sets a value indicating whether built-in Tagolog tags will be appended to Json.
        /// </summary>
        [DefaultParameter]
        public bool BuiltInTagsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value used as a prefix for each Tagolog tag key.
        /// </summary>
        [DefaultParameter]
        public string TagsKeyPrefix { get; set; }

        // Next release
        /// <summary>
        /// Gets or sets a value used as a name of Json container to append all Tagolog tags into.
        /// If no container name specified, tags appednded to the root container.
        /// </summary>
        /// <example>
        /// TagsContainerName = ""
        /// { ... , TagologTag1=Value1, TagologTag2=Value1, ... }
        /// 
        /// TagsContainerName = "TagologSeparateContainer"
        /// { ... TagologSeparateContainer={ TagologTag1=Value1, TagologTag2=Value1, ... } }
        /// </example>
        // [DefaultParameter]
        // public string TagsContainerName { get; set; }

        /// <summary>
        /// Formats the log event as a JSON document for writing.
        /// </summary>
        /// <param name="logEvent">The log event to be formatted.</param>
        /// <returns>A JSON string representation of the log event.</returns>
        protected override string GetFormattedMessage(LogEventInfo logEvent)
        {
            var jsonLayoutResult = "{}";

            if ( 0 != Attributes.Count )
            {
                // "{ attr1:value1, attr2:value2, ... attrN:valueN }"
                jsonLayoutResult = base.GetFormattedMessage( logEvent );
            }

            var tags = MdcStorage.GetTags( builtInTagsEnabled: BuiltInTagsEnabled, orderByAsc: null );

            if ( null == tags )
                return jsonLayoutResult;

            var stringBuilder = new StringBuilder( jsonLayoutResult );
            using ( var stringWriter = new StringWriter( stringBuilder ) )
            {
                using ( var jsonWriter = new JsonTextWriter( stringWriter ) )
                {
                    jsonWriter.Formatting = Formatting.None;

                    jsonWriter.WriteStartObject();

                    for ( int i = 0 ; i < tags.Length ; i++ )
                    {
                        var tagKey = tags[ i ].Key;
                        if ( ! string.IsNullOrEmpty( TagsKeyPrefix ) )
                            tagKey = TagsKeyPrefix + tagKey;
                        jsonWriter.WritePropertyName( tagKey );
                        jsonWriter.WriteValue( tags[ i ].Value );
                    }

                    jsonWriter.WriteEndObject();
                }
            }

            // At this point, string builder contains two Json fragments.
            // "{ attr1:value1, attr2:value2, ... attrN:valueN }{ tag1:value1, tag2:value2, ... tagN:valueN }"
            // Let's join them!
            stringBuilder.Remove( jsonLayoutResult.Length - 1, 2 );
            if ( 0 != Attributes.Count )
                stringBuilder.Insert( jsonLayoutResult.Length - 1, ( SuppressSpaces ) ? "," : ", " );

            return stringBuilder.ToString();
        }
    }
}
