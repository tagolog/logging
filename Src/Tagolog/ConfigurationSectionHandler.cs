using System;
using System.Xml;
using System.Configuration;
using Tagolog.Private;
using Tagolog.Adapters;

namespace Tagolog
{
    public class ConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public const string ConfigurationSectionName = "tagolog";

        internal static TagologSetting Load()
        {
            var config = ConfigurationManager.GetSection( ConfigurationSectionHandler.ConfigurationSectionName );

            if ( null == config )
                return new TagologSetting( typeof( NoOpLogAdapter ) );

            if ( ! ( config is TagologSetting ) )
                throw new ConfigurationErrorsException( string.Format(
                    "Configuration section is not of expected type \"{0}\"", typeof( TagologSetting ).FullName ) );

            return ( TagologSetting ) config;
        }

        #region "IConfigurationSectionHandler" interface implementation

        object IConfigurationSectionHandler.Create( object parent, object configContext, XmlNode section )
        {
            if ( parent != null )
                throw new ConfigurationErrorsException( "Nested configuration sections are not supported." );

            var logAdapterNodes = section.SelectNodes( XmlName.LogAdapterSection );

            if ( null == logAdapterNodes )
                throw new ConfigurationErrorsException( string.Format( "No configuration section <{0}> found.", XmlName.LogAdapterSection ) );

            if ( logAdapterNodes.Count > 1 )
                throw new ConfigurationErrorsException( "Only one <logAdapter> section is allowed." );

            if ( 0 == logAdapterNodes.Count )
                return null;

            // 1 == sectionNodes.Count
            XmlNode logAdapterNode = logAdapterNodes[ 0 ];

            string adapterTypeName = string.Empty;

            // Get log adapter type attribute value.
            var typeAttribute = logAdapterNode.Attributes[ XmlName.TypeAttribute ];
            if ( null != typeAttribute )
                adapterTypeName = typeAttribute.Value;

            if ( string.IsNullOrEmpty( adapterTypeName ) )
            {
                throw new ConfigurationErrorsException(
                    string.Format(
                        "Required attribute \"{0}\" is not found in configuration section <{1}>.",
                        XmlName.TypeAttribute, XmlName.LogAdapterSection ) );
            }

            return new TagologSetting( Type.GetType( adapterTypeName, true, false ) );
        }

        #endregion // "IConfigurationSectionHandler" interface implementation

        #region Data

        static class XmlName
        {
            public static readonly string LogAdapterSection = "logAdapter";
            public static readonly string TypeAttribute = "type";
        }

        #endregion // Data
    }
}
