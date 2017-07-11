//window.addEventListener('load', function () {
//    var tbs = Array.prototype.slice.call(document.getElementsByTagName('input'));
//    tbs.forEach(function (element) {
//        element.addEventListener('onkeyup', function () {
//            alert("Ändring");
//        });
//    });
//});

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

function tbchange(sender) {
    var submitbtn = document.getElementById("submitbtn");
    if (submitbtn.value === 'Sparat' || submitbtn.value === 'Uppdaterat') {
        submitbtn.disabled = false;
        submitbtn.value = 'Uppdatera';
        submitbtn.className = "btn btn-primary";
    }
}
