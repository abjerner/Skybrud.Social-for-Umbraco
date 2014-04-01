using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.editorControls;
using DBTypes = umbraco.cms.businesslogic.datatype.DBTypes;

namespace Skybrud.Social.Umbraco.DataTypes.Instagram {

    public class InstagramOAuthPrevalueEditor : AbstractJsonPrevalueEditor {
        
        //private readonly FacebookTimelineDataType DataType;
        protected TextBox ClientIdTextBox;
        protected TextBox ClientSecretTextBox;
        protected TextBox RedirectUriTextBox;

        public InstagramOAuthPrevalueEditor(InstagramOAuthDataType dataType) : base(dataType, DBTypes.Ntext) {
            // DataType = dataType;
        }

        protected override void CreateChildControls() {
            ClientIdTextBox = new TextBox { ID = "instagramClientId", CssClass = "umbEditorTextField" };
            ClientSecretTextBox = new TextBox { ID = "instagramClientSecret", CssClass = "umbEditorTextField" };
            RedirectUriTextBox = new TextBox { ID = "instagramReturnUri", CssClass = "umbEditorTextField" };
            Controls.Add(ClientIdTextBox);
            Controls.Add(ClientSecretTextBox);
            Controls.Add(RedirectUriTextBox);
        }

        /// <summary>
        /// A non-generic overload returning the correct type of options.
        /// </summary>
        public InstagramOAuthPrevalueOptions GetPreValueOptions() {
            return GetPreValueOptions<InstagramOAuthPrevalueOptions>() ?? new InstagramOAuthPrevalueOptions();
        }

        protected override void OnLoad(EventArgs e) {
            // This item is obfuscated and can not be translated.
            base.OnLoad(e);
            InstagramOAuthPrevalueOptions options = GetPreValueOptions();
            if (options == null) return;
            ClientIdTextBox.Text = options.ClientId;
            ClientSecretTextBox.Text = options.ClientSecret;
            RedirectUriTextBox.Text = options.ReturnUri;
        }

        public override void Save() {
            SaveAsJson(new InstagramOAuthPrevalueOptions {
                ClientId = ClientIdTextBox.Text,
                ClientSecret = ClientSecretTextBox.Text,
                ReturnUri = RedirectUriTextBox.Text
            });
        }

        protected override void Render(HtmlTextWriter writer) {

            writer.WriteLine(
                "<div style=\"margin-bottom: 15px; color: #666; font-style: italic; max-width: 640px; font-size: 12px; line-height: 16px;\">\n" +
                "Instagram uses OAuth for authentication, which requires a public key (client id) and a private key (client secret). " +
                "You can find these at your <a href=\"http://instagram.com/developer/clients/manage/\" target=\"_blank\">Instagram apps overview page</a>." +
                "</div>"
            );

            writer.WriteLine(
                "<div style=\"margin-bottom: 15px; color: #666;font-style: italic; max-width: 640px; font-size: 12px; line-height: 16px;\">\n" +
                "The redirect URI of your application should be <span style=\"text-decoration: underline;\">http://yourdomain.com/umbraco/Skybrud/Social/InstagramAuth.aspx</span>. " +
                "It is only possible to use a single domain with Instagram per application, so your users should access Umbraco through this domain." +
                "</div>"
            );

            writer.WriteLine("<div class=\"propertyItem\">");
            writer.WriteLine("<div class=\"propertyItemheader\">Client ID<br /><small>The public ID/key of the application.</small></div>");
            writer.WriteLine("<div class=\"propertyItemContent\">");
            ClientIdTextBox.RenderControl(writer);
            writer.WriteLine("</div>");
            writer.WriteLine("</div>");

            writer.WriteLine("<div class=\"propertyItem\">");
            writer.WriteLine("<div class=\"propertyItemheader\">Client Secret<br /><small>The private key of the application.</small></div>");
            writer.WriteLine("<div class=\"propertyItemContent\">");
            ClientSecretTextBox.RenderControl(writer);
            writer.WriteLine("</div>");
            writer.WriteLine("</div>");

            writer.WriteLine("<div class=\"propertyItem\">");
            writer.WriteLine("<div class=\"propertyItemheader\">Redirect URI<br /><small>The redirect URI as specified for your application at Instagram.com.</small></div>");
            writer.WriteLine("<div class=\"propertyItemContent\">");
            RedirectUriTextBox.RenderControl(writer);
            writer.WriteLine("</div>");
            writer.WriteLine("</div>");

        }

    }

}