<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewCompany.aspx.cs" Inherits="BatchBookSample.ViewCompany" %>
<%@ Import Namespace="BatchBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1><%#  Server.HtmlEncode(this.Company.Name) %></h1>
<p>
    <%#  Server.HtmlEncode(this.Company.Notes) %>
</p>
<dl>
    <dt>
        Created At:
    </dt>
    <dd>
        <%#  this.Company.CreatedAt %>
    </dd>
    <dt>
        Updated At:
    </dt>
    <dd>
        <%#  this.Company.UpdatedAt %>
    </dd>
    <dt>
        Links
    </dt>
    <dd>
        <ul>
            <li>
                <a href="ListCompanyPeople.aspx?companyId=<%# this.Company.Id %>">View People</a>
            </li>
            <li>
                <a href="ListCompanyTodos.aspx?companyId=<%# this.Company.Id %>">View ToDos</a>
            </li>
            <li>
                <a href="ListCompanyCommunications.aspx?companyId=<%# this.Company.Id %>">View Communications</a>
            </li>
        </ul>
    </dd>
</dl>
<h2>Locations</h2>
<ul>
    <asp:Repeater ID="LocationsRepeater" DataSource="<%# this.Company.Locations %>" runat="server">
        <ItemTemplate>
    <li>
        <dl>
            <dt>
                Primary?
            </dt>
            <dd>
                <%#  ((Location)Container.DataItem).IsPrimary %>
            </dd>
            <dt>
                Street 1:
            </dt>
            <dd>
                <%# Server.HtmlEncode(((Location)Container.DataItem).Street1) %>
            </dd>
            <dt>
                Street 2:
            </dt>
            <dd>
                <%# Server.HtmlEncode(((Location)Container.DataItem).Street2) %>
            </dd>
            <dt>
                City, State, Zip:
            </dt>
            <dd>
                <%# Server.HtmlEncode(((Location)Container.DataItem).City) %>,
                <%# Server.HtmlEncode(((Location)Container.DataItem).State) %>,
                <%# Server.HtmlEncode(((Location)Container.DataItem).PostalCode) %>
            </dd>
        </dl>
    </li>
        </ItemTemplate>
    </asp:Repeater>
</ul>
<h2>Comments</h2>
<ul>
   <asp:Repeater DataSource="<%# this.Company.Comments %>" runat="server">
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
    <asp:Repeater DataSource="<%# this.Company.Tags %>" runat="server">
        <ItemTemplate>
    <li>
        <%# Server.HtmlEncode(((Tag)Container.DataItem).Name) %>
    </li>
        </ItemTemplate>    
    </asp:Repeater>
</ul>
</asp:Content>
