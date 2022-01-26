"use strict";

/*var connection = new signalR.HubConnectionBuilder().withUrl("/reversiHub").build();*/

console.log("Permission", Notification.permission);
if (Notification.permission === "granted") {

} else if (Notification.permission !== "denied") {
    Notification.requestPermission().then(permission => {
        console.log(permission);
    });
}
/*
connection.on("StartGame", (token, spelerToken) => {
    *//*connection.invoke("StartGame", "", "");*//*
    console.log("Game started");
    if (Notification.permission === "granted") {
        var notification = new Notification("New message!",
            {
                body: text,
                icon: '/img/reversi.png'
            });
        *//*notification.onclick = (e) => {
            window.location.href = "https://";
        };*//*
    }
});*/

/*connection.on("Redirect", (url) => {
    console.log("Redirect", url);

    window.location.pathname = url;
});
*/
/*connection.on("OnMove", (aanDeBeurt) => {
    console.log("My turn", aanDeBeurt);

    let buttons = document.querySelectorAll(".fiche");
     
    buttons.forEach(button => {
        button.disabled = false;
        button.style = "pointer-events: auto;";
    });
});*/

/*connection.on("OnDisableMove", () => {
    let buttons = document.querySelectorAll(".fiche");

    buttons.forEach(button => {
        button.disabled = true;
        button.style = "pointer-events: none;";
    });
});*/


/*async function start() {
    try {
        await connection.start();
        console.debug("SignalR");
    } catch (err) {
        console.debug(err);
        setTimeout(start, 3000);
    }
}

connection.onclose(async () => {
    await start();
});

start();*/
