using System;
using Umbraco.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web;

namespace Skybrud.Social.Umbraco.DataTypes.Facebook {

    public class FacebookOAuthPropertyEditorValueConverter : IPropertyEditorValueConverter {
        
        public bool IsConverterFor(Guid propertyEditorId, string docTypeAlias, string propertyTypeAlias) {
            return DataTypeSummary.FacebookOAuth.DataTypeUniqueId.Equals(propertyEditorId);
        }

        public Attempt<object> ConvertPropertyValue(object value) {
            return new Attempt<object>(true, FacebookOAuthDataValue.ParseXml(value + ""));
        }
    
    }

}