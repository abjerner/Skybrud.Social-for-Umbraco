using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using umbraco.cms.businesslogic.datatype;

namespace Skybrud.Social.Umbraco.DataTypes.Instagram {

    public class InstagramOAuthData : DefaultData {

        public InstagramOAuthData(BaseDataType dataType) : base(dataType) { }

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