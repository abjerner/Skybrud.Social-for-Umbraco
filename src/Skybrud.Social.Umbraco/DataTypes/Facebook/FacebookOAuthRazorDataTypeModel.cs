using umbraco.MacroEngines;

namespace Skybrud.Social.Umbraco.DataTypes.Facebook {

    [RazorDataTypeModel("95A0C417-7690-4AD8-A7D6-ED853B021E51")]
    public class FacebookOAuthRazorDataTypeModel : IRazorDataTypeModel {
        
        public bool Init(int currentNodeId, string propertyData, out object instance) {

            if (propertyData.StartsWith("<FacebookOAuth>") || propertyData.StartsWith("</FacebookOAuth>")) {
                instance = FacebookOAuthDataValue.ParseXml(propertyData);
            } else {
                instance = new FacebookOAuthDataValue();
            }

            return true;

        }
    
    }

}