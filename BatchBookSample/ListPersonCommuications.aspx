<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListPersonCommuications.aspx.cs" Inherits="BatchBookSample.ListPersonCommuications" %>
<%@ Import Namespace="BatchBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1><%# Server.HtmlEncode(this.Person.FirstName) %> <%# Server.HtmlEncode(this.Person.LastName) %> Communications</h1>
<ul>
    <asp:Repeater ID="CommunicationsRepeater" DataSource="<%# this.Person.Communications %>" runat="server" >
        <ItemTemplate>
    <li>
        <dl>
            <dt>
                Date:
            </dt>
            <dd>
                <%# ((Communication)Container.DataItem).Date %>
            </dd>
            <dt>
                Subject:
            </dt>
            <dd>
                <a href="ViewCommunication.aspx?communicationId=<%# ((Communication)Container.DataItem).Id %>">
                    <%# Server.HtmlEncode(((Communication)Container.DataItem).Subject) %>
                </a>
            </dd>
        </dl>             
    </li>
        </ItemTemplate>
    </asp:Repeater>
</ul>

</asp:Content>
