<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="avtalsgrid.aspx.cs" Inherits="avtalsgrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:GridView ID="AvtalTable" runat="server" class="table table-striped" AllowSorting="True" GridLines="None" OnSorting="AvtalTable_Sorting">
    </asp:GridView>
   
</asp:Content>


