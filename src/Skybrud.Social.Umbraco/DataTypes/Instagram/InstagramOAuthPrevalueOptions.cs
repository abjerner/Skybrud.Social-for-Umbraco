using System;

namespace Skybrud.Social.Umbraco.DataTypes.Instagram {
    
    public class InstagramOAuthPrevalueOptions {

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ReturnUri { get; set; }

        public bool IsValid {
            get {
                if (String.IsNullOrEmpty(ClientId)) return false;
                if (String.IsNullOrEmpty(ClientSecret)) return false;
                if (String.IsNullOrEmpty(ReturnUri)) return false;
                return true;
            }
        }

    }

}