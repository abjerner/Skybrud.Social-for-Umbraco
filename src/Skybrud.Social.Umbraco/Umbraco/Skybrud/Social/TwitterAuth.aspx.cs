using System;
using System.Web.Script.Serialization;
using Skybrud.Social.OAuth;
using Skybrud.Social.Twitter;
using Skybrud.Social.Twitter.OAuth;
using Skybrud.Social.Twitter.Objects;
using Skybrud.Social.Umbraco.DataTypes.Twitter;
using umbraco.BasePages;
using umbraco.cms.businesslogic.datatype;

namespace Skybrud.Social.Umbraco.Umbraco.Skybrud.Social {

    public partial class TwitterAuth : UmbracoEnsuredPage {

        #region To make sure Umbraco can recognize us after the callback

        /// <summary>
        /// Gets the definition id of the data type.
        /// </summary>
        public int DataTypeDefinitionId { get; private set; }

        public string ContainerId { get; private set; }

        #endregion

        #region OAuth stuff

        /// <summary>
        /// Gets the authorizing code from the query string (if specified).
        /// </summary>
        public string OAuthToken {
            get { return Request.QueryString["oauth_token"]; }
        }

        public string OAuthVerifier {
            get { return Request.QueryString["oauth_verifier"]; }
        }

        public string DeniedToken {
            get { return Request.QueryString["denied"]; }
        }

        public bool HasUserDenied {
            get { return !String.IsNullOrEmpty(DeniedToken); }
        }

        #endregion

        public string BaseUrl {
            get {
                return String.Format(
                    "http://{0}/umbraco/skybrud/social/TwitterAuth.aspx?dtdid={1}&container={2}",
                    Request.ServerVariables["HTTP_HOST"],
                    DataTypeDefinitionId,
                    ContainerId
                );
           }
        }

        private void ReadInput() {
            
            // Try to get the dtdid from the "dtdid" parameter
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
            TwitterOAuthDataType dataType = new DataTypeDefinition(DataTypeDefinitionId).DataType as TwitterOAuthDataType;
            if (dataType == null) {
                Content.Text = "<div class=\"error\">Hold on now! A matching data type could not be found.</div>";
                return;
            }

            TwitterOAuthPrevalueOptions options = dataType.PreValueOptions;
            if (!options.IsValid) {
                Content.Text = "<div class=\"error\">Hold on now! The options of the underlying prevalue editor isn't valid.</div>";
                return;
            }




            // Check whether the user has denied the app
            if (HasUserDenied) {
                Session.Remove(DeniedToken);
                Content.Text = "<div class=\"error\">Error: The app was denied access to your account.</div>";
                return;
            }


            TwitterOAuthClient client = new TwitterOAuthClient {
                ConsumerKey = options.ConsumerKey,
                ConsumerSecret = options.ConsumerSecret,
                Callback = BaseUrl
            };




            if (OAuthToken != null) {

                // Grab the request token from the session
                OAuthRequestToken token = Session[OAuthToken] as OAuthRequestToken;

                // Check whether the requets token was found in the current session
                if (token == null) {
                    Content.Text = "<div class=\"error\">An error occured. Timeout?</div>";
                    return;
                }

                // Blah
                client.Token = token.Token;
                client.TokenSecret = token.TokenSecret;

                // Now get the access token
                try {
                    OAuthAccessToken accessToken = client.GetAccessToken(OAuthVerifier);
                    client.Token = accessToken.Token;
                    client.TokenSecret = accessToken.TokenSecret;
                } catch (Exception) {
                    Content.Text = "<div class=\"error\">Unable to retrieve access token from <b>Twitter.com</b>.</div>";
                    return;
                }

                try {

                    TwitterService service = TwitterService.CreateFromOAuthClient(client);

                    TwitterUser user = service.Account.VerifyCredentials();

                    Content.Text += "<pre><b>Id</b> " + user.Id + "</pre>";
                    Content.Text += "<pre><b>ScreenName</b> " + user.ScreenName + "</pre>";
                    Content.Text += "<pre><b>Name</b> " + user.Name + "</pre>";
                    Content.Text += "<pre><b>Avatar</b> " + user.ProfileImageUrlHttps + "</pre>";
                    Content.Text += "<pre><b>Url</b> " + user.Url + "</pre>";
                    Content.Text += "<pre><b>Description</b> " + user.Description + "</pre>";

                    Content.Text += "<img src=\"" + user.ProfileImageUrlHttps + "\" alt=\"\" />\n";

                    // Update the UI and close the popup window
                    Page.ClientScript.RegisterClientScriptBlock(GetType(), "callback", String.Format(
                        "self.opener.skybrud.social.twitter.callback('{0}', {1}); window.close();",
                        ContainerId,
                        new JavaScriptSerializer().Serialize(new {
                            UserId = user.Id,
                            user.ScreenName,
                            Name = String.IsNullOrEmpty(user.Name) ? "" : user.Name,
                            Avatar = user.ProfileImageUrlHttps,
                            client.ConsumerKey,
                            client.ConsumerSecret,
                            AccessToken = client.Token,
                            AccessTokenSecret = client.TokenSecret
                        })
                    ), true);

                } catch (TwitterException ex) {
                    Content.Text = "<div class=\"error\">Error in the communication with Twitter.com<br /><br />" + ex.Message + " (Code: " + ex.Code + ")</div>";
                } catch (Exception) {
                    Content.Text = "<div class=\"error\">Error in the communication with Twitter.com</div>";
                }

                return;

            }

            #region OAuth 1.0a - Step 1 

            // Get a request token from the Twitter API
            OAuthRequestToken requestToken = client.GetRequestToken();

            // Save the token information to the session so we can grab it later
            Session[requestToken.Token] = requestToken;

            // Redirect the user to the authentication page at Twitter.com
            Response.Redirect(requestToken.AuthorizeUrl);

            #endregion
            
        }
    
    }

}