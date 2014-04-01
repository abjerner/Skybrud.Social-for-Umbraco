using System;
using System.Web.Script.Serialization;
using Skybrud.Social.Instagram;
using Skybrud.Social.Instagram.OAuth;
using Skybrud.Social.Instagram.Responses;
using Skybrud.Social.Umbraco.DataTypes.Instagram;
using Umbraco.Web.UI.Pages;
using umbraco.cms.businesslogic.datatype;

namespace Skybrud.Social.Umbraco.Umbraco.Skybrud.Social {

    public partial class InstagramAuth : UmbracoEnsuredPage {
        
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

            Page.Title = "Skybrud.Social";

            ReadInput();

            if (DataTypeDefinitionId == 0) {
                Content.Text = "<div class=\"error\">Hold on now! A matching data type could not be found.</div>";
                return;
            }

            // Get and validate the datatype
            InstagramOAuthDataType dataType = new DataTypeDefinition(DataTypeDefinitionId).DataType as InstagramOAuthDataType;
            if (dataType == null) {
                Content.Text = "<div class=\"error\">Hold on now! A matching data type could not be found.</div>";
                return;
            }

            InstagramOAuthPrevalueOptions options = dataType.PreValueOptions;
            if (!options.IsValid) {
                Content.Text = "<div class=\"error\">Hold on now! The options of the underlying prevalue editor isn't valid.</div>";
                return;
            }

            // Configure the OAuth client based on the options of the prevalue options
            InstagramOAuthClient client = new InstagramOAuthClient {
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                ReturnUri = options.ReturnUri
            };

            // Check whether an error response was received from Instagram
            if (AuthError != null) {
                Content.Text = "<div class=\"error\">Error: " + AuthErrorDescription + "</div>";
                return;
            }

            // Redirect the user to the Instagram login dialog
            if (AuthCode == null) {
                Response.Redirect(client.GetAuthorizationUrl(
                    DataTypeDefinitionId + "|" + ContainerId,
                    InstagramScope.Basic
                ));
                return;
            }

            try {

                // Exchange the authorization code for an access token (as well as information about the user)
                InstagramAccessTokenResponse response = client.GetAccessTokenFromAuthCode(AuthCode);
                
                // Update the UI and close the popup window
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "callback", String.Format(
                    "self.opener.skybrud.social.instagram.callback('{0}', {1}); window.close();",
                    ContainerId,
                    new JavaScriptSerializer().Serialize(new {
                        id = response.User.Id,
                        name = response.User.FullName ?? response.User.Username,
                        avatar = response.User.ProfilePicture,
                        access_token = response.AccessToken
                    })
                ), true);
            
            } catch (Exception ex) {
                Content.Text = "<div class=\"error\"><b>Unable to acquire access token</b><br />" + ex.Message + "</div>";
            }

        }
    
    }

}