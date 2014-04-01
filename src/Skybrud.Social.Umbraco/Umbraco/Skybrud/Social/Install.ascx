<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Install.ascx.cs" Inherits="Skybrud.Social.Umbraco.Umbraco.Skybrud.Social.Install" %>
<%@ Import Namespace="Skybrud.Social.Umbraco" %>

<table cellspacing="0" cellpadding="0">
    <tr>
        <td><img width="80" height="80" title="" alt="" src="<%=Resources.LogoDark %>" /></td>
        <td style="font-size: 20px; padding-left: 20px;">Skybrud.Social for Umbraco</td>
    </tr>
</table>

<p>This package comes with a few data types for getting started. Select the ones you need below. Also remember that you can always consult the documentation <a href="http://social.skybrud.dk/umbraco-package.aspx" target="_blank">HERE</a>.</p>

<p><strong>Data Types</strong></p>
<asp:Literal runat="server" ID="DataTypesTable" />