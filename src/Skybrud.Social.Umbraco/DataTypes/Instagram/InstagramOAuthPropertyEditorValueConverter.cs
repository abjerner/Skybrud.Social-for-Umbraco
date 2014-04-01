using System;
using Umbraco.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace Skybrud.Social.Umbraco.DataTypes.Instagram {

    public class InstagramOAuthPropertyEditorValueConverter : IPropertyEditorValueConverter {
        
        public bool IsConverterFor(Guid propertyEditorId, string docTypeAlias, string propertyTypeAlias) {
            return DataTypeSummary.InstagramOAuth.DataTypeUniqueId.Equals(propertyEditorId);
        }

        public Attempt<object> ConvertPropertyValue(object value) {
            if (UmbracoContext.Current != null) {
                string data = value + "";
                if (data.StartsWith("<InstagramOAuth>") || data.StartsWith("</InstagramOAuth>")) {
                    return new Attempt<object>(true, InstagramOAuthDataValue.ParseXml(data));
                }
                return new Attempt<object>(true, new InstagramOAuthDataValue());
            }
            return Attempt<object>.False;
        }
    
    }

}