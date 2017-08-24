<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="avtal.aspx.cs" Inherits="avtalsgrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .padd
        {
            padding: 10px;
            font-weight: normal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="pull-right" style="margin-right: 25px">
        <asp:LinkButton ID="filterlb" runat="server" OnClick="filterlb_Click" Style="padding: 10px">Filtrera</asp:LinkButton>
        <a href="./excelexport.aspx" style="padding: 10px">Exportera synliga kolumner till Excel</a>
        <a href="./excelexport.aspx" style="padding: 10px">Exportera alla kolumner till Excel</a>
    </div>

    <div id="filterdiv" runat="server">
        <asp:RadioButtonList ID="avtalstyprbl" runat="server" Style="float: left">
            <asp:ListItem Text="Leverantörsavtal" class="padd"></asp:ListItem>
            <asp:ListItem Text="Kundavtal" class="padd"></asp:ListItem>
            <asp:ListItem Text="Samarbetsavtal" class="padd"></asp:ListItem>
            <asp:ListItem Text="Övrigt" class="padd"></asp:ListItem>
            <asp:ListItem Text="Alla typer" class="padd" Selected="True"></asp:ListItem>
        </asp:RadioButtonList>
        <div style="float: left" class="padd">
            <asp:RadioButtonList ID="statusrbl" runat="server" Style="float: left">
                <asp:ListItem Text="Aktivt" class="padd" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Inaktivt" class="padd"></asp:ListItem>
                <asp:ListItem Text="Alla" class="padd"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div style="float: left" class="padd">
            Ansvarig avdelning
            <asp:DropDownList ID="avdelningdd" runat="server">
            </asp:DropDownList>
        </div>
        <div style="float: left" class="padd">
            Ansvarig person SBK
            <asp:DropDownList ID="medarbetaredd" runat="server">
            </asp:DropDownList>
        </div>
        <div style="float: left" class="padd">
            <asp:Button ID="filterbtn" runat="server" Text="Applicera filter" 
                onclick="filterbtn_Click" />
        </div>
        
    </div>
    
    <asp:GridView ID="AvtalTable" runat="server" class="table table-striped" AllowSorting="True"
        GridLines="None" OnSorting="AvtalTable_Sorting" OnRowDataBound="AvtalTable_DataBound"
        AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" ShowFooter="False">
        <Columns>
            <%--<asp:BoundField DataField="ID" HeaderText="ID" />--%>
            <asp:BoundField DataField="diarienummer" HeaderText="Diarienummer" SortExpression="diarienummer" />
            <asp:BoundField DataField="avtalstyp" HeaderText="Avtalstyp" SortExpression="avtalstyp" />
            <asp:BoundField DataField="startdate" HeaderText="Börjar gälla" SortExpression="startdate"
                DataFormatString="{0:d}" />
            <asp:BoundField DataField="enddate" HeaderText="Upphör" SortExpression="enddate"
                DataFormatString="{0:d}" />
            <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
            <asp:BoundField DataField="motpartstyp" HeaderText="Motpartstyp" SortExpression="motpartstyp" />
            <asp:BoundField DataField="enligt_avtal" HeaderText="Enligt avtal" SortExpression="enligt_avtal" />
            <asp:BoundField DataField="avtalskontakt" HeaderText="Avtalskontakt" SortExpression="avtalskontakt" />
            <asp:BoundField DataField="ansvarig_sbk" HeaderText="Ansvarig SBK" SortExpression="ansvarig_sbk" />
            <asp:BoundField DataField="ansvarig_avd" HeaderText="Ansvarig avdelning" SortExpression="ansvarig_avd" />
            <asp:BoundField DataField="avtalsinnehall" HeaderText="Avtalsinnehåll" SortExpression="avtalsinnehall" />
        </Columns>
    </asp:GridView>
</asp:Content>
