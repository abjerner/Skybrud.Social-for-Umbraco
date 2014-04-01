using System.Xml;
using umbraco.cms.businesslogic.datatype;

namespace Skybrud.Social.Umbraco.DataTypes.Facebook {
    
    public class FacebookOAuthData : DefaultData {

        public FacebookOAuthData(BaseDataType dataType) : base(dataType) { }

        public override XmlNode ToXMl(XmlDocument data) {
            if (Value != null && Value.ToString() != "") {
                var xd = new XmlDocument();
                xd.LoadXml(Value.ToString());
                return data.ImportNode(xd.DocumentElement, true);
            }
            return base.ToXMl(data);
        }

    }

}