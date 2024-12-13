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




//const noNotificationsMessagesEl = document.getElementById('no-notifications-messsage').classList.remove('hidden');

// Hubs notifications
function displayNotification(message) {
    //const container = document.getElementsByClassName("ns")[0];
    const container = document.getElementsByClassName("notes-cont")[0];



    // REMOVE PLACEHOLDER TEXT IF EXISTS
    //console.log(noNotificationsMessagesEl);
    //noNotificationsMessagesEl.classList.remove('hidden');
    document.getElementById('no-notifications-messsage').classList.add('hidden');



    //let template1 = `
    //    <div class="n cont">
    //        <div class="con">
    //            <label>${message}</label>
    //        </div>
    //        <div style="border:1px solid magenta;" class="c" onclick="removeNotification(this)">
    //            x
    //        </div>
    //    </div>
    //`;
    
    //let template0 = `
    //<div class="n cont">
    //    <div class="time">
    //        <label>${new Date().toISOString()}</label>
    //    </div>
    //    <div class="cc">
    //        <div class="con">
    //            <label>${message}</label>
    //        </div>
    //        <div style="border:1px solid magenta;" class="c" onclick="removeNotification(this)">
    //            x
    //        </div>
    //    </div>
    //</div>
    //`;

    let template = `
        <div class="n cont">
            <div class="time">
                <label>${new Date().toISOString()}</label>
            </div>
            <hr style="margin:1px 0px; padding:0px;" />
            <div class="cc">
                <div class="con">
                    <label>${message}</label>
                </div>
                <div class="c" onclick="removeNotification(this)">
                    <button class="b-c">
                        &#10006;
                    </button>
                </div>
            </div>
        </div>
    `;
    
    container.innerHTML += template;

    document.getElementsByClassName('notifications-sidebar')[0].classList.add('notifications-collapsible-toggler');
}

function removeNotification(e) {
    let p = findAncestor(e, "cont");
    p.remove();

    console.log(document.getElementsByClassName('cont').length == 0);

    if (document.getElementsByClassName('cont').length == 0) {
        console.log('elements exists');
        document.getElementById('no-notifications-messsage').classList.remove('hidden');
        //console.log(noNotificationsMessagesEl);
        //noNotificationsMessagesEl.classList.add('hidden');
    }
}

function findAncestor(el, cls) {
    while ((el = el.parentElement) && !el.classList.contains(cls));
    return el;
}


// ON FOCUS REMOVE NOTIFICATIONS LIGHT
function onToggleNotificationsSidebarClicked(e) {

    var i = e.getElementsByTagName('i')[0];
    var notes = document.getElementById('notes');

    if (i.classList.contains('fa-chevron-left')) {
        i.classList.remove('fa-chevron-left');
        i.classList.add('fa-chevron-right');
        e.style.left = '0px';
        notes.style.left = '-300px';
    } else {
        i.classList.remove('fa-chevron-right');
        i.classList.add('fa-chevron-left');
        e.style.left = '300px';
        notes.style.left = '0px';
    }

    document.getElementsByClassName('notifications-sidebar')[0].classList.remove('notifications-collapsible-toggler');

    //i.classList.toggle('fa-chevron-left', 'fa-chevron-right');
    //console.log(i);

    //e.replaceChild('<i class="fa-solid fa-chevron-left xl"></i>', '<i class="fa-solid fa-chevron-right xl"></i>');

    //$('#notes-toggler').on('click', function (event) {
    //    //console.log(e);
    //    //console.log(event.children());

    //    var el = $(this).children('i').eq(0);
    //    console.log(el);
    //    $('.fa-chevron-right, .fa-chevron-left').toggle();
    //    //$('.fa-chevron-right, .fa-chevron-left').toggle();
    //});
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