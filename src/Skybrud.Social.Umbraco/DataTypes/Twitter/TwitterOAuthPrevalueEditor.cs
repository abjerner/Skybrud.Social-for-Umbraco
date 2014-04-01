using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.editorControls;
using DBTypes = umbraco.cms.businesslogic.datatype.DBTypes;

namespace Skybrud.Social.Umbraco.DataTypes.Twitter {

    public class TwitterOAuthPrevalueEditor : AbstractJsonPrevalueEditor {
        
        protected TextBox ConsumerKeyTextBox;
        protected TextBox ConsumerSecretTextBox;

        public TwitterOAuthPrevalueEditor(TwitterOAuthDataType dataType) : base(dataType, DBTypes.Ntext) {
            // Call parent constructor
        }

        protected override void CreateChildControls() {
            ConsumerKeyTextBox = new TextBox { ID = "twitterConsumerKey", CssClass = "umbEditorTextField" };
            ConsumerSecretTextBox = new TextBox { ID = "twitterConsumerSecret", CssClass = "umbEditorTextField" };
            Controls.Add(ConsumerKeyTextBox);
            Controls.Add(ConsumerSecretTextBox);
        }

        /// <summary>
        /// A non-generic overload returning the correct type of options.
        /// </summary>
        public TwitterOAuthPrevalueOptions GetPreValueOptions() {
            return GetPreValueOptions<TwitterOAuthPrevalueOptions>() ?? new TwitterOAuthPrevalueOptions();
        }

        protected override void OnLoad(EventArgs e) {
            // This item is obfuscated and can not be translated.
            base.OnLoad(e);
            TwitterOAuthPrevalueOptions options = GetPreValueOptions();
            if (options == null) return;
            ConsumerKeyTextBox.Text = options.ConsumerKey;
            ConsumerSecretTextBox.Text = options.ConsumerSecret;
        }

        public override void Save() {
            SaveAsJson(new TwitterOAuthPrevalueOptions {
                ConsumerKey = ConsumerKeyTextBox.Text,
                ConsumerSecret = ConsumerSecretTextBox.Text
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

            writer.WriteLine(
                "<div style=\"margin-bottom: 15px; color: #666;font-style: italic; max-width: 640px; font-size: 12px; line-height: 16px;\">\n" +
                "The redirect URI of your application should be <span style=\"text-decoration: underline;\">http://yourdomain.com/umbraco/Skybrud/Social/InstagramAuth.aspx</span>. " +
                "It is only possible to use a single domain with Instagram per application, so your users should access Umbraco through this domain." +
                "</div>"
            );

            writer.WriteLine("<div class=\"propertyItem\">");
            writer.WriteLine("<div class=\"propertyItemheader\">Consumer Key<br /><small>The public ID/key of the application.</small></div>");
            writer.WriteLine("<div class=\"propertyItemContent\">");
            ConsumerKeyTextBox.RenderControl(writer);
            writer.WriteLine("</div>");
            writer.WriteLine("</div>");

            writer.WriteLine("<div class=\"propertyItem\">");
            writer.WriteLine("<div class=\"propertyItemheader\">Consumer Secret<br /><small>The private key of the application.</small></div>");
            writer.WriteLine("<div class=\"propertyItemContent\">");
            ConsumerSecretTextBox.RenderControl(writer);
            writer.WriteLine("</div>");
            writer.WriteLine("</div>");

        }

    }

}