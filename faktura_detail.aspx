<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="faktura_detail.aspx.cs" Inherits="faktura_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="js/person_detail.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <form class="form-horizontal" method="post" action="avtal_detail.aspx">
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Förnamn" class="control-label col-sm-2"></asp:Label>
                <div class="col-sm-10">
            <asp:TextBox ID="firstnametb" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>
            </div>
        </div>
    
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Efternamn" class="control-label col-sm-2"></asp:Label>
                <div class="col-sm-10">
            <asp:TextBox ID="lastnametb" runat="server" class="form-control" ></asp:TextBox>
            </div>
        </div>
       
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Belägenhetsadress" class="control-label col-sm-2"></asp:Label>
                <div class="col-sm-10">
            <asp:TextBox ID="belagentb" runat="server" class="form-control" ></asp:TextBox>
            </div>
        </div>

         <div class="form-group">
            <asp:Label ID="Label4" runat="server" Text="Postnummer" class="control-label col-sm-2"></asp:Label>
                <div class="col-sm-10">
            <asp:TextBox ID="postnummertb" runat="server" class="form-control" ></asp:TextBox>
            </div>
        </div>

         <div class="form-group">
            <asp:Label ID="Label5" runat="server" Text="Postort" class="control-label col-sm-2"></asp:Label>
                <div class="col-sm-10">
            <asp:TextBox ID="postorttb" runat="server" class="form-control" ></asp:TextBox>
            </div>
        </div>

        <asp:Button ID="submitbtn" runat="server" Text="Spara" class="btn btn-primary" ClientIDMode="Static"
            onclick="submitbtn_Click"/>
        
        <%--<input type="submit" value="Skicka" runat="server" name="submitbtn"/>--%>
        
    </form>
</asp:Content>

