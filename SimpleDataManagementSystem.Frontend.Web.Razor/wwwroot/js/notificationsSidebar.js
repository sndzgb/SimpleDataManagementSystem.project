//const noNotificationsMessagesEl = document.getElementById('no-notifications-messsage').classList.remove('hidden');

// Hubs notifications
function displayNotification(message) {
    console.log("displayNotification called");
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
