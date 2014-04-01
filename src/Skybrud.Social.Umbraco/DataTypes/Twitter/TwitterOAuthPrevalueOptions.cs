using System;

namespace Skybrud.Social.Umbraco.DataTypes.Twitter {

    public class TwitterOAuthPrevalueOptions {

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }

        public bool IsValid {
            get {
                if (String.IsNullOrEmpty(ConsumerKey)) return false;
                if (String.IsNullOrEmpty(ConsumerSecret)) return false;
                return true;
            }
        }

    }

}