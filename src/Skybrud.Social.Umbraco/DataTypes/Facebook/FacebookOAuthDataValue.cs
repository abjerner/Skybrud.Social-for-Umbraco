using System;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Skybrud.Social.Facebook;
using Skybrud.Social.Json;
using umbraco.MacroEngines;

namespace Skybrud.Social.Umbraco.DataTypes.Facebook {
    
    public class FacebookOAuthDataValue {

        private FacebookService _service;
        
        public long Id;
        public string Name;
        public string AccessToken;

        public bool HasData {
            get { return Id > 0 && !String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(AccessToken); }
        }

        public FacebookService Service {
            get { return _service ?? (_service = FacebookService.CreateFromAccessToken(AccessToken)); }
        }

        [Obsolete("Use property 'Service' instead")]
        public FacebookService GetService() {
            return _service ?? (_service = FacebookService.CreateFromAccessToken(AccessToken));
        }

        public string ToJson() {
            return new JavaScriptSerializer().Serialize(this);
        }

        public XElement ToXElement() {
            if (!HasData) return new XElement("FacebookOAuth");
            return new XElement(
                "FacebookOAuth",
                new XElement("Id", Id),
                new XElement("Name", Name),
                new XElement("AccessToken", AccessToken)
            );
        }

        public static FacebookOAuthDataValue ParseJson(string str) {
            return ParseJson(JsonConverter.ParseObject(str));
        }

        public static FacebookOAuthDataValue ParseJson(JsonObject obj) {
            if (obj == null) return new FacebookOAuthDataValue();
            return new FacebookOAuthDataValue {
                Id = obj.GetLong("Id"),
                Name = obj.GetString("Name"),
                AccessToken = obj.GetString("AccessToken")
            };
        }

        public static FacebookOAuthDataValue ParseXml(string xml) {
            if (xml != null && xml.StartsWith("<FacebookOAuth>") && xml.EndsWith("</FacebookOAuth>")) {
                return ParseXml(XElement.Parse(xml));
            }
            return new FacebookOAuthDataValue();
        }

        public static FacebookOAuthDataValue ParseXml(DynamicXml xml) {
            return ParseXml(xml == null ? null : xml.BaseElement);
        }

        public static FacebookOAuthDataValue ParseXml(XElement obj) {
            if (obj == null) return new FacebookOAuthDataValue();
            return new FacebookOAuthDataValue {
                Id = GetElementValue<long>(obj, "Id"),
                Name = GetElementValue<string>(obj, "Name"),
                AccessToken = GetElementValue<string>(obj, "AccessToken")
            };
        }

        private static T GetElementValue<T>(XElement xElement, string name) {
            if (xElement == null) return default(T);
            XElement child = xElement.Element(name);
            return child == null ? default(T) : (T) Convert.ChangeType(child.Value, typeof (T));
        }
    
    }

}