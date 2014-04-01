using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Skybrud.Social.Umbraco.DataTypes.Facebook;
using umbraco.interfaces;

namespace Skybrud.Social.Umbraco.DataTypes.Instagram {

    public class InstagramOAuthDataEditor : UpdatePanel, IDataEditor {

        protected readonly InstagramOAuthData Data;
        protected readonly InstagramOAuthDataType DataType;

        protected Panel Container;
        protected HtmlInputHidden UserIdTextBox;
        protected HtmlInputHidden UserNameTextBox;
        protected HtmlInputHidden ProfilePictureTextBox;
        protected HtmlInputHidden AccessTokenTextBox;
        protected Literal InnerText;

        public InstagramOAuthDataEditor(IData data, InstagramOAuthDataType dataType) {
            Data = (InstagramOAuthData) data;
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

            InstagramOAuthDataValue dataValue = new InstagramOAuthDataValue();

            if (Data.Value.ToString().StartsWith("<InstagramOAuth>")) {
                dataValue = InstagramOAuthDataValue.ParseXml(Data.Value.ToString());
            }

            // Register client files
            AddJavaScript(GetCachableUrl("/umbraco/Skybrud/Social/editors.min.js"));
            AddStyleSheet(GetCachableUrl("/umbraco/skybrud/Social/editors.min.css"));

            // Initialize a new panel for adding control (and construct
            // the ID based on the parent ID)
            Container = new Panel { ID = ID + "InstagramOAuth" };
            Container.Attributes["class"] = "SkybrudSocial InstagramOAuth";
            ContentTemplateContainer.Controls.Add(Container);

            UserIdTextBox = new HtmlInputHidden();
            UserNameTextBox = new HtmlInputHidden();
            ProfilePictureTextBox = new HtmlInputHidden();
            AccessTokenTextBox = new HtmlInputHidden();
            InnerText = new Literal();

            UserIdTextBox.Attributes["class"] = "input user-id";
            UserNameTextBox.Attributes["class"] = "input user-name";
            ProfilePictureTextBox.Attributes["class"] = "input profile-picture";
            AccessTokenTextBox.Attributes["class"] = "input access-token";

            Container.Controls.Add(UserIdTextBox);
            Container.Controls.Add(UserNameTextBox);
            Container.Controls.Add(ProfilePictureTextBox);
            Container.Controls.Add(AccessTokenTextBox);

            if (dataValue != null) {
                UserIdTextBox.Value = dataValue.Id > 0 ? dataValue.Id + "" : "";
                UserNameTextBox.Value = dataValue.Name;
                ProfilePictureTextBox.Value = dataValue.ProfilePicture;
                AccessTokenTextBox.Value = dataValue.AccessToken;
            }

            Render(dataValue);

            Container.Controls.Add(InnerText);

        }

        private void Render(InstagramOAuthDataValue value) {
            if (value.HasData) {
                InnerText.Text = String.Format(
                    "<div class=\"avatar\" style=\"display: block; background-image: url(" + value.ProfilePicture + ");\"></div>\n" +
                    "<div class=\"details\">\n" +
                    "<div class=\"name\" style=\"display: block;\">" + value.Name + "<span> (" + value.Id + ")</span></div>\n" +
                    "<a href=\"#\" class=\"authorize\" style=\"display: none;\" onclick=\"javascript:window.open('{0}', 'Facebook OAUth', 'scrollbars=no,resizable=yes,menubar=no,width=800,height=600'); return false;\">Authorize</a>\n" +
                    "<a href=\"#\" class=\"clear\" onclick=\"javascript:skybrud.social.instagram.clear('{1}');\">Clear</a>" +
                    "</div>",
                    "/umbraco/Skybrud/Social/InstagramAuth.aspx?dtdid=" + DataType.DataTypeDefinitionId + "&container=" + Container.ClientID,
                    Container.ClientID
                );
            } else {
                InnerText.Text = String.Format(
                    "<div class=\"avatar\"></div>\n" +
                    "<div class=\"details\">\n" +
                    "<div class=\"name\" style=\"display: none;\"></div>\n" +
                    "<a href=\"#\" class=\"authorize\" onclick=\"javascript:window.open('{0}', 'Facebook OAUth', 'scrollbars=no,resizable=yes,menubar=no,width=800,height=600'); return false;\">Authorize</a>\n" +
                    "<a href=\"#\" class=\"clear\" style=\"display: none;\" onclick=\"javascript:skybrud.social.instagram.clear('{1});\">Clear</a>" +
                    "</div>",
                    "/umbraco/Skybrud/Social/InstagramAuth.aspx?dtdid=" + DataType.DataTypeDefinitionId + "&container=" + Container.ClientID,
                    Container.ClientID
                );
            }
        }

        public void Save() {

            long userid;
            Int64.TryParse(UserIdTextBox.Value, out userid);

            // Initialize the data value object
            InstagramOAuthDataValue value = new InstagramOAuthDataValue {
                Id = userid,
                Name = UserNameTextBox.Value,
                ProfilePicture = ProfilePictureTextBox.Value,
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