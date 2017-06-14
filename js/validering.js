function validOrgNr() {
    // alert("Funktionen körs");
    var tb = document.getElementById("orgnrtb");
//    var tbname = '<%= orgnrtb.ClientID %>';
//    alert(tbname);
//    var tb = document.getElementById(tbname);
    tb.classList.add("has-warning");

    var errorlabel = document.getElementById("orgnrerror");
    // errorlabel.style.display = "block";
    return false;
}