using System;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Skybrud.Social.Instagram;
using Skybrud.Social.Json;
using umbraco.MacroEngines;

namespace Skybrud.Social.Umbraco.DataTypes.Instagram {

    public class InstagramOAuthDataValue {

        private InstagramService _service;

        public long Id { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public string AccessToken { get; set; }

        public bool HasData {
            get { return Id > 0 && !String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(AccessToken); }
        }

        public InstagramService Service {
            get { return _service ?? (_service = InstagramService.CreateFromAccessToken(AccessToken)); }
        }

        [Obsolete("Use property 'Service' instead")]
        public InstagramService GetService() {
            return _service ?? (_service = InstagramService.CreateFromAccessToken(AccessToken));
        }

        public string ToJson() {
            return new JavaScriptSerializer().Serialize(this);
        }

        public XElement ToXElement() {
            if (!HasData) return new XElement("InstagramOAuth");
            return new XElement(
                "InstagramOAuth",
                new XElement("Id", Id),
                new XElement("Name", Name),
                new XElement("ProfilePicture", ProfilePicture),
                new XElement("AccessToken", AccessToken)
            );
        }

        public static InstagramOAuthDataValue ParseJson(string xml) {
            if (xml != null && xml.StartsWith("<InstagramOAuth>") && xml.EndsWith("</InstagramOAuth>")) {
                return ParseXml(XElement.Parse(xml));
            }
            return new InstagramOAuthDataValue();
        }

        public static InstagramOAuthDataValue ParseJson(JsonObject obj) {
            return new InstagramOAuthDataValue {
                Id = obj.GetLong("Id"),
                Name = obj.GetString("Name"),
                ProfilePicture = obj.GetString("ProfilePicture"),
                AccessToken = obj.GetString("AccessToken")
            };
        }

        public static InstagramOAuthDataValue ParseXml(string xml) {
            return ParseXml(XElement.Parse(xml));
        }

        public static InstagramOAuthDataValue ParseXml(DynamicXml xml) {
            return ParseXml(xml.BaseElement);
        }

        public static InstagramOAuthDataValue ParseXml(XElement obj) {
            if (obj == null) return new InstagramOAuthDataValue();
            return new InstagramOAuthDataValue {
                Id = GetElementValue<long>(obj, "Id"),
                Name = GetElementValue<string>(obj, "Name"),
                ProfilePicture = GetElementValue<string>(obj, "ProfilePicture"),
                AccessToken = GetElementValue<string>(obj, "AccessToken")
            };
        }

        private static T GetElementValue<T>(XElement xElement, string name) {
            if (xElement == null) return default(T);
            XElement child = xElement.Element(name);
            return child == null ? default(T) : (T)Convert.ChangeType(child.Value, typeof(T));
        }

    }

}