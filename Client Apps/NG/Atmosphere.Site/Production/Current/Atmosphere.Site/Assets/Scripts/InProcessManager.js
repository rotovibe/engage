function enableLoading() {
    var busy = document.getElementById("divLoading")
    if (busy != null)
        busy.className = "divLoading show";
}

function disableLoading() {
    var busy = document.getElementById("divLoading")
    if (busy != null)
        busy.className = "divLoading hide";
}