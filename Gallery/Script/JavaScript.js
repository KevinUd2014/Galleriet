window.onload = function () {
    var aTags = document.getElementsByTagName("a");

    for (var i = 0; i < aTags.length; i++) {

        if (aTags[i] == document.URL) {
            aTags[i].focus();
        }
    }
}
