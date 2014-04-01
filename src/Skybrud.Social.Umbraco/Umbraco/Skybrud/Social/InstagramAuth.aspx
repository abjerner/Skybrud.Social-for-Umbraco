<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/umbraco/masterpages/umbracoPage.Master" CodeBehind="InstagramAuth.aspx.cs" Inherits="Skybrud.Social.Umbraco.Umbraco.Skybrud.Social.InstagramAuth" %>

<%@ Import Namespace="Skybrud.Social.Umbraco" %>
<%@ Register TagPrefix="Umbraco" Namespace="umbraco.uicontrols" Assembly="controls" %>

<asp:Content ContentPlaceHolderID="body" runat="server">
    <img width="295" height="100" title="" alt="" src="<%=Resources.Logo %>" />
    <div style="height: 2px; background: #efefef; margin-bottom: 20px;"></div>
    <asp:Literal runat="server" ID="Content" />
</asp:Content>