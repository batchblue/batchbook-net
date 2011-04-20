<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewCommunication.aspx.cs" Inherits="BatchBookSample.ViewCommunication" %>
<%@ Import Namespace="BatchBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1><%# Server.HtmlEncode(this.Communication.Subject) %></h1>
<p>
    <%# Server.HtmlEncode(this.Communication.Body) %>
</p>
<dl>
    <dt>
        Date:
    </dt>
    <dd>
        <%# this.Communication.Date %>
    </dd>
    <dt>
        Create At:
    </dt>
    <dd>
        <%# this.Communication.CreateAt %>
    </dd>    
    <dt>
        Updated At:
    </dt>
    <dd>
        <%# this.Communication.UpdatedAt %>
    </dd>
</dl>
<h2>Participants</h2>
<ul>
    <asp:Repeater DataSource="<%# this.Communication.Participants %>" runat="server">
        <ItemTemplate>
    <li>
       <a href="ViewPerson.aspx?personId=<%# ((Communication.Participant)Container.DataItem).ContactId %>"><%# Server.HtmlEncode(((Communication.Participant)Container.DataItem).Name) %></a>
       (<%# Server.HtmlEncode(((Communication.Participant)Container.DataItem).Role) %>)
    </li>
        </ItemTemplate>
    </asp:Repeater>
</ul>
<h2>Comments</h2>
<ul>
   <asp:Repeater DataSource="<%# this.Communication.Comments %>" runat="server">
        <ItemTemplate>
    <li>
        <dl>
            <dt>
                User:
            </dt>
            <dd>
                <%# Server.HtmlEncode(((MegaComment)Container.DataItem).User) %>
            </dd>
            <dt>
                Created At:
            </dt>
            <dd>
                <%# ((MegaComment)Container.DataItem).CreatedAt %>
            </dd>
            <dt>
                Comment:
            </dt>
            <dd>
                <%# Server.HtmlEncode(((MegaComment)Container.DataItem).Comment) %>
            </dd>
        </dl>
    </li>
        </ItemTemplate>
   </asp:Repeater> 
</ul>
<h2>Tags</h2>
<ul>
    <asp:Repeater DataSource="<%# this.Communication.Tags %>" runat="server">
        <ItemTemplate>
    <li>
        <%# Server.HtmlEncode(((Tag)Container.DataItem).Name) %>
    </li>
        </ItemTemplate>    
    </asp:Repeater>
</ul>
</asp:Content>