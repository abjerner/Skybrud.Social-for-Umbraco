using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using umbraco.interfaces;

namespace Skybrud.Social.Umbraco.DataTypes.Facebook {

    public class FacebookOAuthDataEditor : UpdatePanel, IDataEditor {

        protected readonly FacebookOAuthData Data;
        protected readonly FacebookOAuthDataType DataType;

        protected Panel Container;
        protected HtmlInputHidden UserIdTextBox;
        protected HtmlInputHidden UserNameTextBox;
        protected HtmlInputHidden AccessTokenTextBox;
        protected Literal InnerText;
        protected Literal InnerText2;

        public FacebookOAuthDataEditor(IData data, FacebookOAuthDataType dataType) {
            Data = (FacebookOAuthData) data;
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


            FacebookOAuthDataValue dataValue = new FacebookOAuthDataValue();

            if (Data.Value.ToString().StartsWith("<FacebookOAuth>")) {
                dataValue = FacebookOAuthDataValue.ParseXml(Data.Value.ToString());
            }


            // Register client files
            AddJavaScript(GetCachableUrl("/umbraco/Skybrud/Social/editors.min.js"));
            AddStyleSheet(GetCachableUrl("/umbraco/skybrud/Social/editors.min.css"));

            // Initialize a new panel for adding control (and construct
            // the ID based on the parent ID)
            Container = new Panel { ID = ID + "FacebookOAuth" };
            Container.Attributes["class"] = "SkybrudSocial FacebookOAuth";
            ContentTemplateContainer.Controls.Add(Container);


            InnerText = new Literal();
            InnerText2 = new Literal();
            UserIdTextBox = new HtmlInputHidden();
            UserNameTextBox = new HtmlInputHidden();
            AccessTokenTextBox = new HtmlInputHidden();

            UserIdTextBox.Attributes["class"] = "input user-id";
            UserNameTextBox.Attributes["class"] = "input user-name";
            AccessTokenTextBox.Attributes["class"] = "input access-token";

            Container.Controls.Add(UserIdTextBox);
            Container.Controls.Add(UserNameTextBox);
            Container.Controls.Add(AccessTokenTextBox);


            UserIdTextBox.Value = dataValue.Id > 0 ? dataValue.Id + "" : "";
            UserNameTextBox.Value = dataValue.Name;
            AccessTokenTextBox.Value = dataValue.AccessToken;

            Render(dataValue);
            
            Container.Controls.Add(InnerText);
            Container.Controls.Add(InnerText2);

        }

        private void Render(FacebookOAuthDataValue value) {
            if (value.HasData) {
                InnerText.Text = String.Format(
                    "<div class=\"avatar\" style=\"display: block; background-image: url(https://graph.facebook.com/" + value.Id + "/picture?type=small);\"></div>\n" +
                    "<div class=\"details\">\n" +
                    "<div class=\"name\" style=\"display: block;\">" + value.Name + "<span> (" + value.Id + ")</span></div>\n" +
                    "<a href=\"#\" class=\"authorize\" style=\"display: none;\" onclick=\"javascript:window.open('{0}', 'Facebook OAUth', 'scrollbars=no,resizable=yes,menubar=no,width=800,height=600'); return false;\">Authorize</a>\n" +
                    "<a href=\"#\" class=\"clear\" onclick=\"javascript:skybrud.social.clearFacebookOAuth('{1}');\">Clear</a>" +
                    "</div>",
                    "/umbraco/Skybrud/Social/FacebookAuth.aspx?dtdid=" + DataType.DataTypeDefinitionId + "&container=" + Container.ClientID,
                    Container.ClientID
                );
            } else {
                InnerText.Text = String.Format(
                    "<div class=\"avatar\"></div>\n" +
                    "<div class=\"details\">\n" +
                    "<div class=\"name\" style=\"display: none;\"></div>\n" +
                    "<a href=\"#\" class=\"authorize\" onclick=\"javascript:window.open('{0}', 'Facebook OAUth', 'scrollbars=no,resizable=yes,menubar=no,width=800,height=600'); return false;\">Authorize</a>\n" +
                    "<a href=\"#\" class=\"clear\" style=\"display: none;\" onclick=\"javascript:skybrud.social.clearFacebookOAuth('{1});\">Clear</a>" +
                    "</div>",
                    "/umbraco/Skybrud/Social/FacebookAuth.aspx?dtdid=" + DataType.DataTypeDefinitionId + "&container=" + Container.ClientID,
                    Container.ClientID
                );
            }
        }

        public void Save() {

            long userid;
            Int64.TryParse(UserIdTextBox.Value, out userid);

            // Initialize the data value object
            FacebookOAuthDataValue value = new FacebookOAuthDataValue {
                Id = userid,
                Name = UserNameTextBox.Value,
                AccessToken = AccessTokenTextBox.Value
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