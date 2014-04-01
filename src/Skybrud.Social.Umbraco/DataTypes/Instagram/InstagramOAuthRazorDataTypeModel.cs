using umbraco.MacroEngines;

namespace Skybrud.Social.Umbraco.DataTypes.Instagram {

    [RazorDataTypeModel("64c76f4a-59f6-4b58-9833-7d2c09fe3b43")]
    public class InstagramOAuthRazorDataTypeModel : IRazorDataTypeModel {
        
        public bool Init(int currentNodeId, string propertyData, out object instance) {

            if (propertyData.StartsWith("<InstagramOAuth>") || propertyData.StartsWith("</InstagramOAuth>")) {
                instance = InstagramOAuthDataValue.ParseXml(propertyData);
            } else {
                instance = new InstagramOAuthDataValue();
            }

            return true;

        }
    
    }

}