<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="BatchBookSample._Default" %>
<%@ Import Namespace="BatchBook" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<div style="padding:10px;">
    <h2>
        Welcome to the BatchBook .Net Sample.
    </h2>
    <p>
        <a href="ListCompanies.aspx">Here</a> is a page that list companies.    
    </p>
    <dl>
        <dt style="font-weight:bold; font-size:14pt;">
            Recent BatchBook Activity
        </dt>
        <dd>
            <ul style="list-style-type:none; padding-left: 0px;">
                <asp:Repeater ID="recentActivityRepeater" runat="server">
                    <ItemTemplate>
                <li style="width:350px;">
                    <h3><%# Server.HtmlEncode(((Activity)Container.DataItem).Name) %></h3>
                    <div>
                        <span style="float:left">User: <%# Server.HtmlEncode(((Activity)Container.DataItem).UserName) %></span>
                        <span style="float:right"><%# ((Activity)Container.DataItem).Date %></span>
                    </div>
                    <div style="clear:both">
                            <%# Server.HtmlEncode(((Activity)Container.DataItem).Description) %>
                    </div>
                </li>                                
                    </ItemTemplate>
                </asp:Repeater>
            </ul>                    
        </dd>
    </dl>
</div>
</asp:Content>
