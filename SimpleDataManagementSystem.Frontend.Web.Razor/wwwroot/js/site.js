// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const snackbarType = {
    INFO: 0,
    WARNING: 1,
    DANGER: 2
}

function showSnackbar(message, snackbarType) {
    
    var x = document.getElementById("snackbar");

    switch (snackbarType) {
        case 0:
            console.log("info");
            x.classList.add("info");
            break;
        case 1:
            console.log("warning");
            x.classList.add("warning");
            break;
        case 2:
            console.log("danger");
            x.classList.add("danger");
            break;
        default:
            //x.classList.add("info");
            break;
    }
    
    x.innerHTML = message;
    x.classList.add("show");

    // Add the "show" class to DIV
    //x.className = "show";

    // After 3 seconds, remove the show class from DIV
    setTimeout(function () {
        x.className = x.className.replace("show", "");
        x.className = x.className.replace("info", "");
        x.className = x.className.replace("warning", "");
        x.className = x.className.replace("danger", "");
        x.innerHTML = "";

        // Replace with default placeholder
        var element = document.createElement("em");
        element.appendChild(document.createTextNode('Placeholder Test'));
        document.getElementById('snackbar').appendChild(element);
    }, 3 * 1000);
}

function onHambToggle(event) {
    document.getElementById('navBarLinks').classList.toggle('collapsed');
}

function onLogoutClicked(event) {
    event.preventDefault();
    document.getElementsByName('form-logout')[0].submit();
}


// used mostly for "GET" forms
const e = Array.from(document.getElementsByTagName('form')).filter(x => x.hasAttribute('validatable'))[0];
console.log(e);
//document.getElementsByTagName('form')[1].addEventListener("submit", function (event) {
if (e) {
    e.addEventListener("submit", function (event) {
        event.preventDefault();
        
        // if the form does not contain 'validatable' proceed with default logic
        if (!e) {
            event.submit();
            return;
        }

        var obj = {};
        var elements = e.querySelectorAll("input, select, textarea, checkbox");
    
        var reqs = document.getElementsByClassName('form-element-required');

        for (let item of reqs) {
            item.remove();
        }
    
        var required = [];

        for (var i = 0; i < elements.length; ++i) {
            var element = elements[i];
            var name = element.name;
        
            var value = element.value;
        
            var parentDiv = element.closest("div");

            //required
            if (element.hasAttribute('form-field-required') && !element.value.trim()) {
            
                let container = document.createElement("div");
                container.classList.add("form-element-required");
                let small = document.createElement("small");

                let err = element.getAttribute('data-validation-required-error-message');
                
                let txt = "";
                if (!err) {
                    txt = document.createTextNode("This field is required.");
                } else {
                    txt = document.createTextNode(err);
                }
                
                small.appendChild(txt);
                small.style.color = "red";
            
                container.appendChild(small);
            
                parentDiv.after(container);
                required.push(container);
            }

            if (name) {
                obj[name] = value;
            }
        }

        if (!required || required.length == 0) {
            e.submit();
        }
        //var json = JSON.stringify(obj);
    });
}

