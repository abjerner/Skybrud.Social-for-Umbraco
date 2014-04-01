using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Skybrud.Social.Facebook;
using Skybrud.Social.Facebook.OAuth;
using Skybrud.Social.Facebook.Responses;
using Skybrud.Social.Umbraco.DataTypes;
using Skybrud.Social.Umbraco.DataTypes.Facebook;
using Umbraco.Core;
using umbraco.cms.businesslogic.datatype;

namespace Skybrud.Social.Umbraco.Umbraco.Skybrud.Social {

    public partial class FacebookAuth : System.Web.UI.Page {
        
        /// <summary>
        /// Gets the authorizing code from the query string (if specified).
        /// </summary>
        public string AuthCode {
            get { return Request.QueryString["code"]; }
        }

        /// <summary>
        /// Gets the definition id of the data type.
        /// </summary>
        public int DataTypeDefinitionId { get; private set; }

        public string ContainerId { get; private set; }

        public string AuthState {
            get { return Request.QueryString["state"]; }
        }

        public string AuthErrorReason {
            get { return Request.QueryString["error_reason"]; }
        }

        public string AuthError {
            get { return Request.QueryString["error"]; }
        }

        public string AuthErrorDescription {
            get { return Request.QueryString["error_description"]; }
        }

        private void ReadInput() {

            // At first try to get the dtdid from the "state" parameter
            if (Request.QueryString["state"] != null) {

                string[] pieces = Request.QueryString["state"].Split('|');
                
                int value;
                DataTypeDefinitionId = Int32.TryParse(pieces[0], out value) ? value : 0;

                ContainerId = pieces.Length > 1 ? pieces[1] : null;

            }

            // Then try to get the dtdid from the "dtdid" parameter (overwrites "state")
            if (Request.QueryString["dtdid"] != null) {
                int value;
                DataTypeDefinitionId = Int32.TryParse(Request.QueryString["dtdid"], out value) ? value : 0;
            }

            if (Request.QueryString["container"] != null) {
                ContainerId = Request.QueryString["container"];
            }

        }
    
        protected void Page_Load(object sender, EventArgs e) {

            ReadInput();

            if (DataTypeDefinitionId == 0) {
                Content.Text = "Hold on now! A matching data type could not be found.";
                return;
            }

            // Get and validate the datatype
            FacebookOAuthDataType dataType = new DataTypeDefinition(DataTypeDefinitionId).DataType as FacebookOAuthDataType;
            if (dataType == null) {
                Content.Text = "Hold on now! A matching data type could not be found.";
                return;
            }

            FacebookOAuthPrevalueOptions options = dataType.PreValueOptions;
            if (!options.IsValid) {
                Content.Text = "Hold on now! The options of the underlying prevalue editor isn't valid.";
                return;
            }

            // Configure the OAuth client based on the options of the prevalue options
            FacebookOAuthClient client = new FacebookOAuthClient {
                AppId = options.AppKey,
                AppSecret = options.AppSecret,
                ReturnUri = options.ReturnUri
            };

            // Check whether an error response was received from Facebook
            if (AuthError != null) {
                Content.Text = "<div class=\"error\">Error: " + AuthErrorDescription + "</div>";
                return;
            }

            // Redirect the user to the Facebook login dialog
            if (AuthCode == null) {
                Response.Redirect(client.GetAuthorizationUrl(
                    DataTypeDefinitionId + "|" + ContainerId,
                    "read_stream", "user_status", "user_about_me", "user_photos"
                ));
                return;
            }
            
            // Exchange the authorization code for a user access token
            string userAccessToken;
            try {
                userAccessToken = client.GetAccessTokenFromAuthCode(AuthCode);
            } catch (Exception ex) {
                Content.Text = "<div class=\"error\"><b>Unable to acquire access token</b><br />" + ex.Message + "</div>";
                return;
            }
            
            try {
                
                // Initialize the Facebook service (no calls are made here)
                FacebookService service = FacebookService.CreateFromAccessToken(userAccessToken);

                // Make a call to the Facebook API to get information about the user
                FacebookMeResponse me = service.Methods.Me();

                // Update the UI and close the popup window
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "callback", String.Format(
                    "self.opener.skybrud.social.facebookOAuthCallback('{0}', {1}); window.close();",
                    ContainerId,
                    new JavaScriptSerializer().Serialize(new {
                        id = me.Id,
                        name = me.Name ?? me.UserName,
                        access_token = userAccessToken
                    })
                ), true);

            } catch (Exception ex) {
                Content.Text = "<div class=\"error\"><b>Unable to get user information</b><br />" + ex.Message + "</div>";
                return;
            }


        }

    
    }

}