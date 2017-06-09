<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="avtal.aspx.cs" Inherits="avtal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h4>Alla avtal</h4>
    <asp:Table ID="avtalstabell" runat="server" CssClass="table">
        <asp:TableRow>
            <asp:TableHeaderCell></asp:TableHeaderCell>
            <asp:TableHeaderCell>Diarienummer</asp:TableHeaderCell>
            <asp:TableHeaderCell>Börjar gälla</asp:TableHeaderCell>
            <asp:TableHeaderCell>Upphör gälla</asp:TableHeaderCell>
            <asp:TableHeaderCell>Status</asp:TableHeaderCell>
            <asp:TableHeaderCell>Typ av motpart</asp:TableHeaderCell>
            <asp:TableHeaderCell>Avtals-id</asp:TableHeaderCell>
            <asp:TableHeaderCell>Länk till avtal</asp:TableHeaderCell>
            <asp:TableHeaderCell>Org/personnummer</asp:TableHeaderCell>
            <asp:TableHeaderCell>Enlig avtal</asp:TableHeaderCell>
            <asp:TableHeaderCell>Internt alias</asp:TableHeaderCell>
            <asp:TableHeaderCell>Kommentar</asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
&nbsp;
</asp:Content>

