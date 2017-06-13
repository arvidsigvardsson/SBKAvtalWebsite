<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="avtal_detail.aspx.cs" Inherits="avtal_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Label ID="debugl" runat="server" Text="debug"></asp:Label>

    <asp:Label ID="debugl2" runat="server" Text="debug"></asp:Label>

    <form class="form-horizontal" method="post" action="avtal_detail.aspx">
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Diarienummer" class="control-label col-sm-2"></asp:Label>
                <div class="col-sm-10">
            <asp:TextBox ID="diarietb" runat="server" class="form-control"></asp:TextBox>
            </div>
        </div>
    
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Avtal börjar gälla"  class="control-label col-sm-2"></asp:Label>
            <div class="col-sm-10">
                <asp:TextBox ID="startdatetb" type="date" runat="server" class="form-control"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Avtal upphör gälla"  class="control-label col-sm-2"></asp:Label>
            <div class="col-sm-10">
                <asp:TextBox ID="enddate" type="date" runat="server" class="form-control"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label4" runat="server" Text="Status" class="control-label col-sm-2"></asp:Label>
            <div class="col-sm-10">
                <asp:DropDownList ID="statusdd" runat="server">
                    <asp:ListItem>Aktivt</asp:ListItem>
                    <asp:ListItem>Inaktivt</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        
        <div class="form-group">
            <asp:Label ID="Label5" runat="server" Text="Typ av motpart" class="control-label col-sm-2"></asp:Label>
            <div class="col-sm-10">
                <asp:DropDownList ID="motpartsdd" runat="server">
                    <asp:ListItem>Extern</asp:ListItem>
                    <asp:ListItem>Förvaltning</asp:ListItem>
                    <asp:ListItem>Kommunalt bolag</asp:ListItem>
                    <asp:ListItem>Uppgift saknas</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label6" runat="server" Text="Avtals-id" class="control-label col-sm-2"></asp:Label>
            <div class="col-sm-10">
                <asp:TextBox ID="sbkidtb" runat="server" class="form-control"></asp:TextBox>
            </div>
        </div>

        <asp:Button ID="submitbtn" runat="server" Text="Skicka" onclick="Button1_Click" class="btn btn-primary"/>
        
        <%--<input type="submit" value="Skicka" runat="server" name="submitbtn"/>--%>
        
    </form>
</asp:Content>
