using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using umbraco.interfaces;

namespace Skybrud.Social.Umbraco.DataTypes.Twitter {

    public class TwitterOAuthDataEditor : UpdatePanel, IDataEditor {

        protected readonly TwitterOAuthData Data;
        protected readonly TwitterOAuthDataType DataType;

        protected Panel Container;
        protected HtmlInputHidden UserIdTextBox;
        protected HtmlInputHidden ScreenNameTextBox;
        protected HtmlInputHidden NameTextBox;
        protected HtmlInputHidden AvatarTextBox;
        protected HtmlInputHidden ConsumerKeyTextBox;
        protected HtmlInputHidden ConsumerSecretTextBox;
        protected HtmlInputHidden AccessTokenTextBox;
        protected HtmlInputHidden AccessTokenSecretTextBox;
        protected Literal InnerText;

        public TwitterOAuthDataEditor(IData data, TwitterOAuthDataType dataType) {
            Data = (TwitterOAuthData) data;
            DataType = dataType;
        }

        public virtual bool TreatAsRichTextEditor {
            get { return false; }
        }

        public bool ShowLabel {
            get { return true; }
        }

        public Control Editor {
            get { return this; }
        }

        protected override void OnInit(EventArgs e) {

            base.OnInit(e);

            TwitterOAuthDataValue dataValue = new TwitterOAuthDataValue();

            if (Data.Value.ToString().StartsWith("<TwitterOAuth>")) {
                dataValue = TwitterOAuthDataValue.ParseXml(Data.Value.ToString());
            }

            // Register client files
            AddJavaScript(GetCachableUrl("/umbraco/Skybrud/Social/editors.min.js"));
            AddStyleSheet(GetCachableUrl("/umbraco/skybrud/Social/editors.min.css"));

            // Initialize a new panel for adding control (and construct
            // the ID based on the parent ID)
            Container = new Panel { ID = ID + "TwitterOAuth" };
            Container.Attributes["class"] = "SkybrudSocial TwitterOAuth";
            ContentTemplateContainer.Controls.Add(Container);

            UserIdTextBox = new HtmlInputHidden();
            ScreenNameTextBox = new HtmlInputHidden();
            NameTextBox = new HtmlInputHidden();
            AvatarTextBox = new HtmlInputHidden();
            ConsumerKeyTextBox = new HtmlInputHidden();
            ConsumerSecretTextBox = new HtmlInputHidden();
            AccessTokenTextBox = new HtmlInputHidden();
            AccessTokenSecretTextBox = new HtmlInputHidden();
            InnerText = new Literal();

            UserIdTextBox.Attributes["class"] = "input userid";
            ScreenNameTextBox.Attributes["class"] = "input screenname";
            NameTextBox.Attributes["class"] = "input name";
            AvatarTextBox.Attributes["class"] = "input avatar";
            ConsumerKeyTextBox.Attributes["class"] = "input consumerkey";
            ConsumerSecretTextBox.Attributes["class"] = "input consumersecret";
            AccessTokenTextBox.Attributes["class"] = "input accesstoken";
            AccessTokenSecretTextBox.Attributes["class"] = "input accesstokensecret";

            Container.Controls.Add(UserIdTextBox);
            Container.Controls.Add(ScreenNameTextBox);
            Container.Controls.Add(NameTextBox);
            Container.Controls.Add(AvatarTextBox);
            Container.Controls.Add(ConsumerKeyTextBox);
            Container.Controls.Add(ConsumerSecretTextBox);
            Container.Controls.Add(AccessTokenTextBox);
            Container.Controls.Add(AccessTokenSecretTextBox);

            if (dataValue != null) {
                UserIdTextBox.Value = dataValue.UserId > 0 ? dataValue.UserId + "" : "";
                ScreenNameTextBox.Value = dataValue.ScreenName;
                NameTextBox.Value = dataValue.Name;
                ConsumerKeyTextBox.Value = dataValue.ConsumerKey;
                ConsumerSecretTextBox.Value = dataValue.ConsumerSecret;
                AccessTokenTextBox.Value = dataValue.AccessToken;
                AccessTokenTextBox.Value = dataValue.AccessTokenSecret;
            }

            Render(dataValue);

            Container.Controls.Add(InnerText);

        }

        private void Render(TwitterOAuthDataValue value) {
            if (value.HasData) {

                string name;
                if (String.IsNullOrEmpty(value.Name)) {
                    name = value.ScreenName + "<span> (" + value.UserId + ")</span>";
                } else {
                    name = value.Name + "<span> (" + value.ScreenName + " / " + value.UserId + ")</span>";
                }

                InnerText.Text = String.Format(
                    "<div class=\"avatar\" style=\"display: block; background-image: url(" + value.Avatar + ");\"></div>\n" +
                    "<div class=\"details\">\n" +
                    "<div class=\"name\" style=\"display: block;\">" + name + "</div>\n" +
                    "<a href=\"#\" class=\"authorize\" style=\"display: none;\" onclick=\"javascript:window.open('{0}', 'Twitter OAUth', 'scrollbars=no,resizable=yes,menubar=no,width=800,height=600'); return false;\">Authorize</a>\n" +
                    "<a href=\"#\" class=\"clear\" onclick=\"javascript:skybrud.social.twitter.clear('{1}');\">Clear</a>" +
                    "</div>",
                    "/umbraco/Skybrud/Social/TwitterAuth.aspx?dtdid=" + DataType.DataTypeDefinitionId + "&container=" + Container.ClientID,
                    Container.ClientID
                );
            } else {
                InnerText.Text = String.Format(
                    "<div class=\"avatar\"></div>\n" +
                    "<div class=\"details\">\n" +
                    "<div class=\"name\" style=\"display: none;\"></div>\n" +
                    "<a href=\"#\" class=\"authorize\" onclick=\"javascript:window.open('{0}', 'Twitter OAUth', 'scrollbars=no,resizable=yes,menubar=no,width=800,height=600'); return false;\">Authorize</a>\n" +
                    "<a href=\"#\" class=\"clear\" style=\"display: none;\" onclick=\"javascript:skybrud.social.twitter.clear('{1});\">Clear</a>" +
                    "</div>",
                    "/umbraco/Skybrud/Social/TwitterAuth.aspx?dtdid=" + DataType.DataTypeDefinitionId + "&container=" + Container.ClientID,
                    Container.ClientID
                );
            }
        }

        public void Save() {

            long userid;
            Int64.TryParse(UserIdTextBox.Value, out userid);

            // Initialize the data value object
            TwitterOAuthDataValue value = new TwitterOAuthDataValue {
                UserId = userid,
                ScreenName = ScreenNameTextBox.Value,
                Name = NameTextBox.Value,
                Avatar = AvatarTextBox.Value,
                ConsumerKey = ConsumerKeyTextBox.Value,
                ConsumerSecret = ConsumerSecretTextBox.Value,
                AccessToken = AccessTokenTextBox.Value,
                AccessTokenSecret = AccessTokenSecretTextBox.Value
            };
            
            Render(value);

            // Save the value as XML
            Data.Value = value.ToXElement().ToString(SaveOptions.DisableFormatting);

        }

        private string GetCachableUrl(string url) {
            if (url.StartsWith("/")) {
                string path = Page.Server.MapPath(url);
                return File.Exists(path) ? url + "?" + File.GetLastWriteTime(path).Ticks : url;
            }
            return url;
        }

        private void AddJavaScript(string url) {

            if (Page.Header.Controls.OfType<HtmlControl>().Any(control => control.TagName == "script" && control.Attributes["src"] == url)) {
                return;
            }

            HtmlGenericControl script = new HtmlGenericControl("script");
            script.Attributes["type"] = "text/javascript";
            script.Attributes["src"] = url;
            Page.Header.Controls.Add(script);

        }

        private void AddStyleSheet(string url) {

            if (Page.Header.Controls.OfType<HtmlControl>().Any(control => control.TagName == "link" && control.Attributes["type"] == "text/css" && control.Attributes["href"] == url)) {
                return;
            }

            HtmlLink link = new HtmlLink { Href = url };
            link.Attributes.Add("rel", "stylesheet");
            link.Attributes.Add("type", "text/css");
            Page.Header.Controls.Add(link);

        }

    }

}