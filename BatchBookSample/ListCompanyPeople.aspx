<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListCompanyPeople.aspx.cs" Inherits="BatchBookSample.ListCompanyPeople" %>
<%@ Import Namespace="BatchBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1><%# Server.HtmlEncode(this.Company.Name) %> People</h1>
<ul>
    <asp:Repeater ID="PeopleRepeater" DataSource="<%# this.Company.People %>" runat="server" >
        <ItemTemplate>
    <li>
        <a href="ViewPerson.aspx?personId=<%# ((Person)Container.DataItem).Id %>">
            <%#  Server.HtmlEncode(((Person)Container.DataItem).FirstName) %> 
            <%#  Server.HtmlEncode(((Person)Container.DataItem).LastName) %>
        </a>            
    </li>
        </ItemTemplate>
    </asp:Repeater>
</ul>
</asp:Content>
