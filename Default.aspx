<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
   

    <ul>
        <li>
            <a href="./avtal.aspx">Alla avtal</a>
        </li>
        <li>
            <a href="./avtal_detail.aspx?nytt_avtal=true">Lägg till nytt avtal</a>
        </li>
        <li>
            <a href="./person_detail.aspx?ny_person=true">Lägg till en person i databasen</a>
        </li>
        <li>
            <a href="./faktura_detail.aspx?ny_adress=true">Lägg till en fakturaadress</a>    
        </li>
        <li>
            <a href="./leveranser.aspx">Leveranser</a>
        </li>
        <%--<li>
            <a href="./avtalsgrid.aspx">Avtal som gridview</a>
        </li>--%>
    </ul>
   <asp:Label ID="userlabel" runat="server" Text="Label"></asp:Label>
   <asp:Label ID="auth" runat="server" Text="Label"></asp:Label>
</asp:Content>
