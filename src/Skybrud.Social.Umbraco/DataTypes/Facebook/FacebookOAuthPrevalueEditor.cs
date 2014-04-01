using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.editorControls;
using DBTypes = umbraco.cms.businesslogic.datatype.DBTypes;

namespace Skybrud.Social.Umbraco.DataTypes.Facebook {
    
    public class FacebookOAuthPrevalueEditor : AbstractJsonPrevalueEditor {
        
        protected TextBox AppKeyTextBox;
        protected TextBox AppSecretTextBox;
        protected TextBox ReturnUriTextBox;

        public FacebookOAuthPrevalueEditor(FacebookOAuthDataType dataType) : base(dataType, DBTypes.Ntext) {
            // just call the parent constructor
        }

        protected override void CreateChildControls() {
            AppKeyTextBox = new TextBox { ID = "facebookAppKey", CssClass = "umbEditorTextField" };
            AppSecretTextBox = new TextBox { ID = "facebookAppSecret", CssClass = "umbEditorTextField" };
            ReturnUriTextBox = new TextBox { ID = "facebookReturnUri", CssClass = "umbEditorTextField" };
            Controls.Add(AppKeyTextBox);
            Controls.Add(AppSecretTextBox);
            Controls.Add(ReturnUriTextBox);
        }

        /// <summary>
        /// A non-generic overload returning the correct type of options.
        /// </summary>
        public FacebookOAuthPrevalueOptions GetPreValueOptions() {
            return GetPreValueOptions<FacebookOAuthPrevalueOptions>() ?? new FacebookOAuthPrevalueOptions();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            FacebookOAuthPrevalueOptions options = GetPreValueOptions();
            if (options == null) return;
            AppKeyTextBox.Text = options.AppKey;
            AppSecretTextBox.Text = options.AppSecret;
            ReturnUriTextBox.Text = options.ReturnUri;
        }

        public override void Save() {
            SaveAsJson(new FacebookOAuthPrevalueOptions {
                AppKey = AppKeyTextBox.Text,
                AppSecret = AppSecretTextBox.Text,
                ReturnUri = ReturnUriTextBox.Text
            });
        }

        protected override void Render(HtmlTextWriter writer) {

            writer.WriteLine(
                "<div style=\"margin-bottom: 15px; color: #666; font-style: italic; max-width: 640px; font-size: 12px; line-height: 16px;\">\n" +
                "Facebook uses OAuth for authentication, which requires a public key (app key) and a private key (app secret). " +
                "You can find these at your <a href=\"https://developers.facebook.com/apps\" target=\"_blank\">Facebook apps overview page</a>." +
                "</div>"
            );

            writer.WriteLine(
                "<div style=\"margin-bottom: 15px; color: #666;font-style: italic; max-width: 640px; font-size: 12px; line-height: 16px;\">\n" +
                "The return URI of your application should be <span style=\"text-decoration: underline;\">http://yourdomain.com/umbraco/Skybrud/Social/FacebookAuth.aspx</span>. " +
                "It is only possible to use a single domain with Facebook per application, so your users should access Umbraco through this domain." +
                "</div>"
            );

            writer.WriteLine("<div class=\"propertyItem\">");
            writer.WriteLine("<div class=\"propertyItemheader\">App Key<br /><small>The public ID/key of the application.</small></div>");
            writer.WriteLine("<div class=\"propertyItemContent\">");
            AppKeyTextBox.RenderControl(writer);
            writer.WriteLine("</div>");
            writer.WriteLine("</div>");

            writer.WriteLine("<div class=\"propertyItem\">");
            writer.WriteLine("<div class=\"propertyItemheader\">App Secret<br /><small>The private key of the application.</small></div>");
            writer.WriteLine("<div class=\"propertyItemContent\">");
            AppSecretTextBox.RenderControl(writer);
            writer.WriteLine("</div>");
            writer.WriteLine("</div>");

            writer.WriteLine("<div class=\"propertyItem\">");
            writer.WriteLine("<div class=\"propertyItemheader\">Return URI<br /><small>The return URI as specified for your application at Facebook.com.</small></div>");
            writer.WriteLine("<div class=\"propertyItemContent\">");
            ReturnUriTextBox.RenderControl(writer);
            writer.WriteLine("</div>");
            writer.WriteLine("</div>");

        }
    
    }

}