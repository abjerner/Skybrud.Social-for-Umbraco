using System;

namespace Skybrud.Social.Umbraco.DataTypes.Facebook {
    
    public class FacebookOAuthPrevalueOptions {
    
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string ReturnUri { get; set; }

        public bool IsValid {
            get {
                if (String.IsNullOrEmpty(AppKey)) return false;
                if (String.IsNullOrEmpty(AppSecret)) return false;
                if (String.IsNullOrEmpty(ReturnUri)) return false;
                return true;
            }
        }
    
    }

}