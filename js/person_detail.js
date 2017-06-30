function tbchange() {
    var submitbtn = document.getElementById("submitbtn");
    if (submitbtn.value === 'Sparat') {
        submitbtn.disabled = false;
        submitbtn.value = 'Uppdatera';
    }
}
