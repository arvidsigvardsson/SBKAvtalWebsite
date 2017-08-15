<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="leveranser.aspx.cs" Inherits="leveranser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:GridView ID="LeveranserGrid" 
        runat="server" 
        class="table table-striped" 
        AllowSorting="False" 
        GridLines="None" 
       
        OnRowDataBound="LeveranserGrid_DataBound"
        AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="datum" HeaderText="Datum för leverans" dataformatstring="{0:d}"/>
            <asp:BoundField DataField="Enligt avtal" HeaderText="Enligt avtal" />
        </Columns>
    </asp:GridView>
</asp:Content>

