// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//function onHambToggle(event) {
//    document.getElementById('navBarLinks').classList.toggle('collapsed');
//}

function onLogoutClicked(event) {
    event.preventDefault();
    document.getElementsByName('form-logout')[0].submit();
}


// used mostly for "GET" forms
const form = Array.from(document.getElementsByTagName('form')).filter(x => x.hasAttribute('validatable'))[0];
//console.log(form);
//document.getElementsByTagName('form')[1].addEventListener("submit", function (event) {
if (form) {
    form.addEventListener("submit", function (event) {
        event.preventDefault();
        
        // if the form does not contain 'validatable' proceed with default logic
        if (!form) {
            event.submit();
            return;
        }

        var obj = {};
        var elements = form.querySelectorAll("input, select, textarea, checkbox");
    
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
            form.submit();
        }
        //var json = JSON.stringify(obj);
    });
}




function onExpandClicked(e) {
    console.log(e);
    let optionsToggle = e.closest('.link').querySelector('.dropdown-options');
    let el = e.querySelectorAll('.fa-solid')[0].classList; // NEW

    if (optionsToggle.classList.contains('active-anchor')) {
        optionsToggle.classList.remove('active-anchor');
        optionsToggle.classList.add('collapsed-items-dropdown-menu');
        
        el.remove('fa-caret-down'); // NEW
        el.add('fa-caret-up'); // NEW
        return;
    } else {
        let allOptions = document.getElementsByClassName('dropdown-options');
        for (let o of allOptions) {
            o.classList.remove('active-anchor');
        }

        optionsToggle.classList.add('active-anchor');
        el.remove('fa-caret-up'); // NEW
        el.add('fa-caret-down'); // NEW
    }

    const options = document.getElementsByClassName('dropdown-options');

    for (let o of options) {
        if (!o.classList.contains('.collapsed-items-dropdown-menu')) {
            o.classList.add('collapsed-items-dropdown-menu');
        }
    }

    let toggle = e.closest('.link').querySelector('.dropdown-options');
    toggle.classList.remove('collapsed-items-dropdown-menu');
}


function onHambClicked(e) {
    let links = document.getElementsByClassName('toggleable');

    for (let l of links) {
        l.classList.toggle('collapsed');
    }
}


function updateLocalization(e, culture) {
    // Create a new paragraph element
    //let input = document.createElement(`<input type='text' value='${culture}' hidden />`);
    let input = document.createElement("input");
    input.type = "text";
    input.setAttribute("hidden", "");
    input.setAttribute("value", culture);
    input.setAttribute("name", "culture");
    
    // Append the new paragraph as a child of the parent
    e.appendChild(input);

    //return;

    //var token = $('input[name="__RequestVerificationToken"]').val();
    //let xhr = null;
    //xhr = $.ajax({
    //    type: "POST",
    //    url: "/Localization",
    //    data: {
    //        __RequestVerificationToken: token,
    //    },
    //    success: function (response) {
    //        console.log("culture changed");
    //    },
    //    error: function (xhr, status, error) {
    //        // xhr, status, error
    //        let response = JSON.parse(xhr.responseText);
    //        showSnackbar(response.message, snackbarType.DANGER);
    //    }
    //});
}

function toggleFlagDropDown(e) {
    //let s = document.getElementsByClassName('lang-selected')[0].classList;
    let o = document.getElementsByClassName('lang-options')[0].classList;
    let s = document.getElementsByClassName('lang-selected')[0].classList;

    if (o.contains('hidden')) {
        o.remove('hidden');
        s.add('border-bottom-no-radius');
    } else {
        o.add('hidden');
        s.remove('border-bottom-no-radius');
    }

    //let s = document.getElementsByClassName('lang-selected')[0].classList;
    //let o = document.getElementsByClassName('lang-options')[0].classList;

    //if (s.contains('collapsed')) {
    //    s.remove('collapsed');
    //    s.add('bottom-border-no-radius');
    //    o.remove('hidden');
    //} else {
    //    s.add('collapsed');
    //    s.remove('bottom-border-no-radius');
    //    o.add('hidden');
    //}
}