// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showSnackbar(message) {
    // Get the snackbar DIV
    var x = document.getElementById("snackbar");

    x.innerHTML = message;

    // Add the "show" class to DIV
    x.className = "show";

    // After 3 seconds, remove the show class from DIV
    setTimeout(function () {
        x.className = x.className.replace("show", "");

        // Replace with default placeholder
        var element = document.createElement("em");
        element.appendChild(document.createTextNode('Placeholder Test'));
        document.getElementById('snackbar').appendChild(element);
    }, 3000);
}