"use strict";

const connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Trace) // None to disable
    .withUrl("https://localhost:7006/hubs/itemUpdatedNotifier", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        accessTokenFactory: async () => {
            //return new Promise((resolve) => {
            //    $.get('/Account/AccessToken?handler=Token').done((token) => resolve(token));
            //});
            return await getAccessToken();
        }
    })
    .withAutomaticReconnect()
    .build();


//async function start() {
//    try {
//        await connection.start();
//        console.log("SignalR Connected.");
//    } catch (err) {
//        console.log(err);
//        setTimeout(start, 5000);
//    }
//};


connection.on("ItemUpdatedNotifierSender", function (message) {
    console.log(message);
    displayNotification(message);
});

connection.start().then(function () {
    console.log('Connected!');
}).catch(function (err) {
    return console.error(err);
    //return console.error(err.toString());
});

connection.onreconnecting(error => {
    console.assert(connection.state === signalR.HubConnectionState.Reconnecting);
    console.log("Reconnecting... ", error);
});

connection.onreconnected(connectionId => {
    console.assert(connection.state === signalR.HubConnectionState.Connected);
    console.log("Reconnected with connectionId: ", connectionId);
});

connection.onclose(error => {
    console.assert(connection.state === signalR.HubConnectionState.Disconnected);
    console.log("Connection closed due to unknown reason. Please refresh your page to restore functionality.");
    //await start();
});

window.addEventListener("beforeunload", () => {
    connection.stop().then(() => {
        console.log("SignalR connection stopped.");
    }).catch(err => {
        console.error("Error stopping SignalR connection: ", err);
    });
});

//await
//start();




async function getAccessToken() {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: '/Account/AccessToken/?handler=Token',
            type: 'GET',
            timeout: (30 * 1000),
            success: (response) => {
                console.log("Got access_token: " + response);
                resolve(response);
            },
            error: (response) => {
                console.error('Unable to retrieve access_token');
                console.error(response);
                reject(response);
            }
        })
    })
}