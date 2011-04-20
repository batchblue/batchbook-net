<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewPerson.aspx.cs" Inherits="BatchBookSample.ViewPerson" %>
<%@ Import Namespace="BatchBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1><%# Server.HtmlEncode(this.Person.FirstName) %> <%# Server.HtmlEncode(this.Person.LastName) %></h1>
<p>
    <%# Server.HtmlEncode(this.Person.Notes) %>
</p>
<dl>
    <dt>
        Created At:
    </dt>
    <dd>
        <%# this.Person.CreatedAt %>
    </dd>
    <dt>
        Updated At:
    </dt>
    <dd>
        <%# this.Person.UpdatedAt %>
    </dd>
    <dt>
        Title:
    </dt>
    <dd>
        <%# Server.HtmlEncode(this.Person.Title) %>
    </dd>
    <dt>
        Company:
    </dt>
    <dd>
        <a href="ViewCompany.aspx?companyId=<%# this.Person.CompanyId %>"><%# Server.HtmlEncode(this.Person.Company) %></a>
    </dd>
    <dt>
        Links
    </dt>
    <dd>
        <ul>
            <li>
                <a href="ListPersonCommuications.aspx?personId=<%# this.Person.Id %>">List Communications</a>    
            </li>
            <li>
                <a href="ListPersonTodos.aspx?personId=<%# this.Person.Id %>">List Todos</a>
            </li>
        </ul>
    </dd>
</dl>
<h2>Comments</h2>
<ul>
   <asp:Repeater DataSource="<%# this.Person.MegaComments %>" runat="server">
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
</asp:Content>
