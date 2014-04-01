using Skybrud.Social.Umbraco.DataTypes.Instagram;
using umbraco.MacroEngines;

namespace Skybrud.Social.Umbraco.DataTypes.Twitter {

    [RazorDataTypeModel("ee197f73-1e06-4985-8e42-33002813ce8f")]
    public class TwitterOAuthRazorDataTypeModel : IRazorDataTypeModel {
        
        public bool Init(int currentNodeId, string propertyData, out object instance) {

            if (propertyData.StartsWith("<TwitterOAuth>") || propertyData.StartsWith("</TwitterOAuth>")) {
                instance = InstagramOAuthDataValue.ParseXml(propertyData);
            } else {
                instance = new InstagramOAuthDataValue();
            }

            return true;

        }
    
    }

}