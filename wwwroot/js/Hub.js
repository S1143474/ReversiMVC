"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/reversiHub").build();

console.log("Permission", Notification.permission);
if (Notification.permission === "granted") {

} else if (Notification.permission !== "denied") {
    Notification.requestPermission().then(permission => {
        console.log(permission);
    });
}

connection.on("Start", (token, text) => {
    console.log(token, text);

    if (Notification.permission === "granted") {
        var notification = new Notification("New message!",
            {
                body: text,
                icon: '/img/reversi.png'
            });
        /*notification.onclick = (e) => {
            window.location.href = "https://";
        };*/
    }
});

connection.start().then(function () {
    //document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    console.error(err.toString());
});

$(() => {

});


