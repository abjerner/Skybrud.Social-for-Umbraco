using System;
using Umbraco.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace Skybrud.Social.Umbraco.DataTypes.Twitter {

    public class TwitterOAuthPropertyEditorValueConverter : IPropertyEditorValueConverter {
        
        public bool IsConverterFor(Guid propertyEditorId, string docTypeAlias, string propertyTypeAlias) {
            return DataTypeSummary.TwitterOAuth.DataTypeUniqueId.Equals(propertyEditorId);
        }

        public Attempt<object> ConvertPropertyValue(object value) {
            if (UmbracoContext.Current != null) {
                string data = value + "";
                if (data.StartsWith("<TwitterOAuth>") || data.StartsWith("</TwitterOAuth>")) {
                    return new Attempt<object>(true, TwitterOAuthDataValue.ParseXml(data));
                }
                return new Attempt<object>(true, new TwitterOAuthDataValue());
            }
            return Attempt<object>.False;
        }
    
    }

}