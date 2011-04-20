<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListCompanies.aspx.cs" Inherits="BatchBookSample.ListCompanies" %>
<%@ Import Namespace="BatchBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Companies</h1>
    <ul>
        <asp:Repeater ID="CompanyRepeater" DataSource=<%# this.Companies %> runat="server">
            <ItemTemplate>
        <li>
            <a href="ViewCompany.aspx?companyId=<%# ((Company)Container.DataItem).Id %>"><%#  Server.HtmlEncode(((Company)Container.DataItem).Name) %></a>
        </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</asp:Content>
