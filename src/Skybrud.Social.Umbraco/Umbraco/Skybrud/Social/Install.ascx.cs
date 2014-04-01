using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Skybrud.Social.Umbraco.DataTypes;

namespace Skybrud.Social.Umbraco.Umbraco.Skybrud.Social {

    public partial class Install : System.Web.UI.UserControl {


        protected NameValueCollection BaseQueryString {
            get {
                return new NameValueCollection {
                    {"installing", Request.QueryString["installing"]},
                    {"dir", Request.QueryString["dir"]},
                    {"pId", Request.QueryString["pId"]},
                    {"customControl", Request.QueryString["customControl"]},
                    {"customUrl", Request.QueryString["customUrl"]}
                };
            }
        }

        public string BaseUrl {
            get { return "/umbraco/developer/packages/installer.aspx?" + SocialUtils.NameValueCollectionToQueryString(BaseQueryString); }
        }

        public string MapUrl(NameValueCollection nvc) {
            NameValueCollection all = BaseQueryString;
            if (nvc != null) {
                foreach (string key in nvc.Keys) {
                    all.Add(key, nvc[key]);
                }
            }
            return "/umbraco/developer/packages/installer.aspx?" + SocialUtils.NameValueCollectionToQueryString(all);
        }

        protected void Page_Load(object sender, EventArgs e) {
            
            // Register client files
            AddStyleSheet(GetCachableUrl("/umbraco/Skybrud/Social/install.min.css"));



            if (Request.QueryString["do"] == "addDataType") {
                var dt = DataTypeSummary.GetById(Request.QueryString["dt"]);
                if (dt != null) dt.Create();
                Response.Redirect(BaseUrl);
            }

            if (Request.QueryString["do"] == "removeDataType") {
                var dt = DataTypeSummary.GetById(Request.QueryString["dt"]);
                if (dt != null) dt.Delete();
                Response.Redirect(BaseUrl);
            }





            DataTypesTable.Text = "<table class=\"dataTypes\">\n";

            DataTypesTable.Text += "<tr>\n";
            DataTypesTable.Text += "<th>Name</td>\n";
            DataTypesTable.Text += "<th>Description</th>\n";
            DataTypesTable.Text += "<th>&nbsp;</th>\n";
            DataTypesTable.Text += "</tr>\n";

            foreach (var summary in DataTypeSummary.GetAll()) {

                string addUrl = MapUrl(new NameValueCollection {
                    {"do", "addDataType"},
                    {"dt", summary.UniqueId.ToString()}
                });

                string removeUrl = MapUrl(new NameValueCollection {
                    {"do", "removeDataType"},
                    {"dt", summary.UniqueId.ToString()}
                });

                DataTypesTable.Text += "<tr>\n";
                DataTypesTable.Text += "<td class=\"nw\"><b>" + summary.Name + "</b></td>\n";
                DataTypesTable.Text += "<td class=\"fw\">" + summary.Description + "</td>\n";
                DataTypesTable.Text += "<td class=\"nw\">" + (summary.Exists() ? "<a href=\"" + removeUrl + "\">Remove</a>" : "<a href=\"" + addUrl + "\">Add</a>") + "</td>\n";
                DataTypesTable.Text += "</tr>\n";

            }

            DataTypesTable.Text += "</table>\n";

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