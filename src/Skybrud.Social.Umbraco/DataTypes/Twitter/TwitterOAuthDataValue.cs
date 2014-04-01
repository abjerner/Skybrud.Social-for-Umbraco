using System;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Skybrud.Social.Json;
using Skybrud.Social.Twitter;
using umbraco.MacroEngines;

namespace Skybrud.Social.Umbraco.DataTypes.Twitter {
    public enum TwitterDownloadMode {
        
        /// <summary>
        /// With the default download mode, data is fetched from the Twitter API on each user request.
        /// This value can be combined with macro caching.
        /// </summary>
        Default = 0,
        
        /// <summary>
        /// This data mode will only keep a local cache of the data, and only fetch new data from the
        /// Twitter API if the cache has expired.
        /// </summary>
        DownloadIfExpired = 1,

        /// <summary>
        /// When the property is set to this data mode, new data is fetched from a job running in the
        /// background on the server. This is the recommended way since user requests will not be
        /// affected by the time used to call the Twitter API.
        /// </summary>
        DownloadInBackground = 2
    
    }

    public class TwitterOAuthDataValue {

        private TwitterService _service;

        public long UserId { get; set; }
        public string ScreenName { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }

        public TwitterDataMode DataMode {
            get { return TwitterDataMode.Timeline; }
        }

        public TwitterDownloadMode DownloadMode {
            get { return TwitterDownloadMode.Default; }
        }

        public bool HasData {
            get { return UserId > 0 && !String.IsNullOrEmpty(ScreenName) && !String.IsNullOrEmpty(AccessToken); }
        }

        public TwitterService Service {
            get {
                if (String.IsNullOrEmpty(ConsumerKey)) throw new ArgumentNullException("ConsumerKey");
                if (String.IsNullOrEmpty(ConsumerSecret)) throw new ArgumentNullException("ConsumerSecret");
                if (String.IsNullOrEmpty(AccessToken)) throw new ArgumentNullException("AccessToken");
                if (String.IsNullOrEmpty(AccessTokenSecret)) throw new ArgumentNullException("AccessTokenSecret");
                return _service ?? (_service = TwitterService.CreateFromAccessInformation(new TwitterAccessInformation {
                    ConsumerKey = ConsumerKey,
                    ConsumerSecret = ConsumerSecret,
                    AccessToken = AccessToken,
                    AccessTokenSecret = AccessTokenSecret
                }));
            }
        }

        [Obsolete("Use property 'Service' instead")]
        public TwitterService GetService() {
            return Service;
        }

        public string ToJson() {
            return new JavaScriptSerializer().Serialize(new {
                UserId,
                ScreenName,
                Name,
                Avatar,
                ConsumerKey,
                ConsumerSecret,
                AccessToken,
                AccessTokenSecret,
                DataMode,
                DownloadMode
            });
        }

        public XElement ToXElement() {
            if (!HasData) return new XElement("TwitterOAuth");
            return new XElement(
                "TwitterOAuth",
                new XElement("UserId", UserId),
                new XElement("ScreenName", ScreenName),
                new XElement("Fullname", Name),
                new XElement("Avatar", Avatar),
                new XElement("ConsumerKey", ConsumerKey),
                new XElement("ConsumerSecret", ConsumerSecret),
                new XElement("AccessToken", AccessToken),
                new XElement("AccessTokenSecret", AccessTokenSecret)
                //new XElement("DataMode", DataMode),
                //new XElement("DownloadMode", DownloadMode)
            );
        }

        public static TwitterOAuthDataValue ParseJson(string str) {
            return JsonConverter.ParseObject(str, Parse);
        }

        public static TwitterOAuthDataValue Parse(JsonObject obj) {
            if (obj == null) return new TwitterOAuthDataValue();
            return new TwitterOAuthDataValue {
                UserId = obj.GetLong("UserId"),
                ScreenName = obj.GetString("ScreenName"),
                Name = obj.GetString("Fullname"),
                Avatar = obj.GetString("Avatar"),
                ConsumerKey = obj.GetString("ConsumerKey"),
                ConsumerSecret = obj.GetString("ConsumerSecret"),
                AccessToken = obj.GetString("AccessToken"),
                AccessTokenSecret = obj.GetString("AccessTokenSecret")
            };
        }

        public static TwitterOAuthDataValue ParseXml(string xml) {
            if (xml != null && xml.StartsWith("<TwitterOAuth>") && xml.EndsWith("</TwitterOAuth>")) {
                return ParseXml(XElement.Parse(xml));
            }
            // TODO: Add support for uTwit?
            return new TwitterOAuthDataValue();
        }

        public static TwitterOAuthDataValue ParseXml(DynamicXml xml) {
            return ParseXml(xml == null ? null : xml.BaseElement);
        }

        public static TwitterOAuthDataValue ParseXml(XElement obj) {
            if (obj == null) return new TwitterOAuthDataValue();
            return new TwitterOAuthDataValue {
                UserId = GetElementValue<long>(obj, "UserId"),
                ScreenName = GetElementValue<string>(obj, "ScreenName"),
                Name = GetElementValue<string>(obj, "Fullname"),
                Avatar = GetElementValue<string>(obj, "Avatar"),
                ConsumerKey = GetElementValue<string>(obj, "ConsumerKey"),
                ConsumerSecret = GetElementValue<string>(obj, "ConsumerSecret"),
                AccessToken = GetElementValue<string>(obj, "AccessToken"),
                AccessTokenSecret = GetElementValue<string>(obj, "AccessTokenSecret")
            };
        }

        private static T GetElementValue<T>(XElement xElement, string name) {
            if (xElement == null) return default(T);
            XElement child = xElement.Element(name);
            return child == null ? default(T) : (T)Convert.ChangeType(child.Value, typeof(T));
        }

    }

}