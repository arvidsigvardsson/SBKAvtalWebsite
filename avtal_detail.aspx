<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="avtal_detail.aspx.cs" Inherits="avtal_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/validering.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <form class="form-horizontal" method="post" action="avtal_detail.aspx">
    
     <div class="form-group row">
        <asp:Label ID="Label25" runat="server" Text="Typ av avtal" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:DropDownList ID="avtalstyptb" runat="server" onchange="tbchange()" 
                AutoPostBack="True" onselectedindexchanged="avtalstyptb_SelectedIndexChanged">
                <asp:ListItem>Kundavtal</asp:ListItem>
                <asp:ListItem>Leverantörsavtal</asp:ListItem>
                <asp:ListItem>Samarbetsavtal</asp:ListItem>
                <asp:ListItem>Övrigt</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    
    <div class="form-group row">
        <asp:Label ID="Label1" runat="server" Text="Diarienummer" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            
            <asp:TextBox ID="diarietb" runat="server" class="form-control" tag='input' ClientIDMode="Static"
                onkeyup="tbchange()"></asp:TextBox>

                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="diarietb" ErrorMessage="Fält krävs" 
                SetFocusOnError="True" ForeColor="#FF3300"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="Label2" runat="server" Text="Avtal börjar gälla" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            
            <asp:TextBox ID="startdatetb" type="date" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>

            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Använd formatet YYYY-MM-DD" 
                Operator="DataTypeCheck" Type="Date" ControlToValidate="startdatetb" 
                ForeColor="#FF3300"></asp:CompareValidator>

            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ErrorMessage="Fält krävs" ControlToValidate="startdatetb" ForeColor="#FF3300"></asp:RequiredFieldValidator>
        </div>
    </div>


    <div class="form-group row">
        <asp:Label ID="Label3" runat="server" Text="Avtal upphör gälla" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="enddate" type="date" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>

             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ErrorMessage="Fält krävs" ControlToValidate="enddate" ForeColor="#FF3300"></asp:RequiredFieldValidator>

             <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Använd formatet YYYY-MM-DD" 
                Operator="DataTypeCheck" Type="Date" ControlToValidate="enddate" 
                ForeColor="#FF3300"></asp:CompareValidator>

        </div>
    </div>


    <div class="form-group row">
        <asp:Label ID="Label4" runat="server" Text="Status" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:DropDownList ID="statusdd" runat="server" onchange="tbchange()">
                <asp:ListItem>Aktivt</asp:ListItem>
                <asp:ListItem>Inaktivt</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="Label5" runat="server" Text="Typ av motpart" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-10">
            <asp:DropDownList ID="motpartsdd" runat="server" onchange="tbchange()">
                <asp:ListItem>Extern</asp:ListItem>
                <asp:ListItem>Förvaltning</asp:ListItem>
                <asp:ListItem>Kommunalt bolag</asp:ListItem>
                <asp:ListItem>Uppgift saknas</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="Label6" runat="server" Text="Avtals-id" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="sbkidtb" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="Label7" runat="server" Text="Organisationsnummer" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">

            <asp:TextBox ID="orgnrtb" runat="server" class="form-control"
                ClientIDMode="Static" onkeyup="tbchange()" CausesValidation="True"></asp:TextBox>

                <asp:CustomValidator ID="OrgNummerValidator" runat="server" 
                ErrorMessage="CustomValidator" 
                onservervalidate="OrgNummerValidator_ServerValidate" 
                ControlToValidate="orgnrtb" ForeColor="#FF3300" ValidateEmptyText="True"></asp:CustomValidator>

            <%--<div class="text-danger" id="orgnrerror" style="display: none">
                fel form</div>--%>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="Label10" runat="server" Text="Enligt avtal" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="enlavttb" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="Label8" runat="server" Text="Internt alias" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="intidtb" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="Label9" runat="server" Text="Kommentar" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="kommentartb" runat="server" TextMode="MultiLine" class="form-control"
                onkeyup="tbchange()"></asp:TextBox>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="Label11" runat="server" Text="Avtalstecknare" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:DropDownList ID="avtalstecknaredd" runat="server" OnSelectedIndexChanged="Dropdowns_SelectedIndexChanged"
                AutoPostBack="True" ViewStateMode="Enabled">
            </asp:DropDownList>
        </div>
        <%--<div class="col-sm-8">
                <a href="./person_detail.aspx?ny_person=true">Lägg till ny person</a>
            </div>--%>
    </div>
    <div class="form-group row">
        <asp:Label ID="Label12" runat="server" Text="Avtalskontakt" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:DropDownList ID="kontaktdd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Dropdowns_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <%--<div class="col-sm-8">
                <a href="./faktura_detail.aspx?ny_person=true">Lägg till ny fakturaadress</a>
            </div>--%>
    </div>
    <div class="form-group row">
        <asp:Label ID="Label13" runat="server" Text="Upphandlat av" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:DropDownList ID="upphandlatdd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Dropdowns_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </div>
    
    <div class="form-group row">
        <asp:Label ID="Label16" runat="server" Text="Ansvarig SBK" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:DropDownList ID="ansvarig_sbkdd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Dropdowns_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </div>

   

    <div class="form-group row">
        <asp:Label ID="Label14" runat="server" Text="Ansvarig avdelning" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:DropDownList ID="ansvarig_avddd" runat="server" onchange="tbchange()">
            </asp:DropDownList>
        </div>
    </div>

    <div class="form-group row">
        <asp:Label ID="Label15" runat="server" Text="Ansvarig enhet" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <%--<asp:TextBox ID="ansvenhtb" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>--%>
            <asp:DropDownList ID="ansvarig_enhetdd" runat="server" onchange="tbchange()">
            </asp:DropDownList>
        </div>
    </div>

      <div class="form-group row">
       <asp:Label ID="Label1999" runat="server" Text="Avtalsinnehåll" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="checkbox checkboxlist col-sm-4">
            <asp:CheckBoxList ID="innehallcbl" runat="server" RepeatLayout="UnorderedList">
                
            </asp:CheckBoxList>
        </div>
        </div>


    
    
    <div class="form-group row">
        <asp:Label ID="Label18" runat="server" Text="Sökväg till avtal" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="pathtoavtaltb" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>
        </div>
    </div>
    

    <div id="kundavtalscontrols" runat="server">
     <div class="form-group row">
        <asp:Label ID="Label17" runat="server" Text="Datakontakt" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:DropDownList ID="datakontaktdd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Dropdowns_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </div>

    <div class="form-group row">
        <div class="col-sm-2"></div>
        <h3 class="col-sm-6 h3">Ekonomi</h3>
    </div>

    <div class="form-group row">
        <asp:Label ID="Label19" runat="server" Text="Konto" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="kontotb" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>
        </div>
    </div>

    <div class="form-group row">
        <asp:Label ID="Label20" runat="server" Text="Kstl" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="kstltb" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>
        </div>
    </div>

    <div class="form-group row">
        <asp:Label ID="Label21" runat="server" Text="Vht" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="vhttb" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>
        </div>
    </div>

    <div class="form-group row">
        <asp:Label ID="Label22" runat="server" Text="MTP" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="mtptb" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>
        </div>
    </div>

    <div class="form-group row">
        <asp:Label ID="Label23" runat="server" Text="Aktivitet" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="aktivitettb" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>
        </div>
    </div>

    <div class="form-group row">
        <asp:Label ID="Label24" runat="server" Text="Objekt" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="objekttb" runat="server" class="form-control" onkeyup="tbchange()"></asp:TextBox>
        </div>
    </div>

    <div class="form-group row">
        <div class="col-sm-2"></div>
        <h3 class="col-sm-6 h3">Leveranser</h3>
    </div>

    <div class="form-group row">
        <asp:Label ID="Label28" runat="server" Text="Manuella leveransdatum" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="manuellevtb" runat="server" TextMode="MultiLine" Rows="10" onkeyup="tbchange()"></asp:TextBox>
        </div>
        
    </div>
    </div>

    <div class="form-group row">
        <asp:Label ID="Label26" runat="server" Text="Lösenord" class="control-label col-sm-2 text-right"></asp:Label>
        <div class="col-sm-6">
            <asp:TextBox ID="passwordtb" runat="server" class="form-control" onkeyup="tbchange()" TextMode="Password"></asp:TextBox>
            <asp:CustomValidator ID="passwordvalidator" runat="server" 
                ErrorMessage="CustomValidator" ControlToValidate="passwordtb" 
                ForeColor="Red" onservervalidate="passwordvalidator_ServerValidate" 
                SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>
        </div>
    </div>

    <div class="form-group row">
        <div class="control-label col-sm-2 text-right">
            <asp:Button ID="submitbtn" runat="server" Text="Skicka" class="btn btn-primary"
            onclick="submitbtn_Click" ClientIDMode="Static" />
        
        </div>
    </div>

    
    </form>
   
</asp:Content>
