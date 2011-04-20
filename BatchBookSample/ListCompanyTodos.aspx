<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListCompanyTodos.aspx.cs" Inherits="BatchBookSample.ListCompanyTodos" %>
<%@ Import Namespace="BatchBook" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1><%# Server.HtmlEncode(this.Company.Name) %> Todos</h1>
<ul>
    <asp:Repeater ID="TodosRepeater" DataSource="<%# this.Company.Todos %>" runat="server" >
        <ItemTemplate>
    <li>
        <dl>
            <dt>
                Created At:
            </dt>
            <dd>
                <%# ((Todo)Container.DataItem).CreatedAt %>
            </dd>
            <dt>
                Updated At:
            </dt>
            <dd>
                <%# ((Todo)Container.DataItem).UpdatedAt %>
            </dd>
            <dt>
                Assigned By:
            </dt>
            <dd>
                <%# Server.HtmlEncode(((Todo)Container.DataItem).AssignedBy) %>
            </dd>
            <dt>
                Assigned To:
            </dt>
            <dd>
                <%# Server.HtmlEncode(((Todo)Container.DataItem).AssignedTo) %>
            </dd>
            <dt>
                Due Date:
            </dt>
            <dd>
                <%# ((Todo)Container.DataItem).DueDate %>
            </dd>
            <dt>
                Complete?
            </dt>
            <dd>
                <%# ((Todo)Container.DataItem).Complete %>
            </dd>
            <dt>
                Description:
            </dt>
            <dd>
                <%# Server.HtmlEncode(((Todo)Container.DataItem).Description) %>
            </dd> 
            <dt>
                Comments:
            </dt>
            <dd>
                <ul>
                    <asp:Repeater DataSource="<%# ((Todo)Container.DataItem).Comments %>" runat="server">
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
            </dd>            
        </dl>
    </li>
        </ItemTemplate>
    </asp:Repeater>
</ul>
</asp:Content>
