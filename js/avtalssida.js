window.addEventListener('load', function () {
    var hdr = document.getElementById("diariecol");
    hdr.addEventListener('click', function () {
        alert('klickat');
    }, false);
    document.getElementById("clickdiv").addEventListener('click', function () {
        alert('annat klick');
    }, false);
}
    , false);