<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="avtalsgrid.aspx.cs" Inherits="avtalsgrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <a href="./excelexport.aspx" class="pull-right" style="margin-right: 25px">Exportera till Excel</a>
    <asp:GridView ID="AvtalTable" 
        runat="server" 
        class="table table-striped" 
        AllowSorting="True" 
        GridLines="None" 
        OnSorting="AvtalTable_Sorting" 
        OnRowDataBound="AvtalTable_DataBound"
        AutoGenerateColumns="False">

        <Columns>
            <%--<asp:BoundField DataField="ID" HeaderText="ID" />--%>
            <asp:BoundField DataField="enligt_avtal" HeaderText="Enligt avtal" SortExpression="enligt_avtal" />
            <asp:BoundField DataField="diarienummer" HeaderText="Diarienummer" SortExpression="diarienummer" />
            <asp:BoundField DataField="startdate" HeaderText="Börjar gälla" SortExpression="startdate" dataformatstring="{0:d}"/>
            <asp:BoundField DataField="enddate" HeaderText="Upphör" SortExpression="enddate" dataformatstring="{0:d}"/>
            
        </Columns>
    </asp:GridView>
   
</asp:Content>


