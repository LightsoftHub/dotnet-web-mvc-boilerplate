"use strict";

const url = "http://localhost:3001/signalr-hub";

function getToken() {
    const xhr = new XMLHttpRequest();
    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {
            if (this.readyState !== 4) return;
            if (this.status == 200) {
                resolve(this.responseText);
            } else {
                reject(this.statusText);
            }
        };
        xhr.open("GET", "/Auth/GetToken");
        xhr.send();
    });
}

const options = {
    transport: signalR.HttpTransportType.LongPolling,
    xdomain: true,
    useDefaultPath: false,
    logging: signalR.LogLevel.Trace,
    accessTokenFactory: getToken
};

let connection = new signalR.HubConnectionBuilder()
    .withUrl(url, options)
    //.configureLogging(signalR.LogLevel.Debug)
    .build();

Object.defineProperty(WebSocket, 'OPEN', { value: 1, });

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        var json = JSON.stringify(err);
        console.log(json);
        setTimeout(start, 10000);
    }
};

connection.onclose(async () => {
    await start();
});

async function loadUserNotifications() {
    const host = location.protocol + '//' + location.host;
    try {
        $.ajax({
            type: 'GET',
            url: host + '/Notifications/UserEntries',
            success: function (res) {
                if (res.records != null) {
                    var notifications = '';
                    res.records.forEach(el => {
                        var url = host + '/Notifications/Details/' + el.id;
                        var iconCss = el.markAsRead ? 'text-muted' : 'text-warning';
                        var contentClass = el.markAsRead ? 'text-muted' : '';

                        //header
                        var entry = '<a href="' + url + '">';
                        entry += '<div class="card mb-2 notification-entry">';
                        entry += '<div class="card-body">';
                        entry += '<div class="d-flex">';
                        entry += '<div class="flex-shrink-0">';
                        //entry += '<svg class="pc-icon ' + iconClass + '">';
                        entry += '<i class="far fa-newspaper fa-2x ' + iconCss + '"></i>';
                        //entry += '</svg>';
                        entry += '</div>';

                        //content
                        entry += '<div class="flex-grow-1 ms-3">';
                        let dArr = el.createdOn.substr(0, 10).split('-');
                        let year = dArr[0];
                        let month = dArr[1];
                        let day = dArr[2];
                        let time = el.createdOn.substr(11, 8);
                        var viewDate = year + '/' + month + '/' + day + ' ' + time;
                        entry += '<span class="float-end text-sm text-muted">' + viewDate + '</span>';

                        entry += '<h5 class="text-body mb-2 ' + contentClass + '">' + el.title.substr(0, 25) + '</h5>';

                        entry += '<p class="mb-0 ' + contentClass + '">';
                        entry += el.message;
                        entry += '</p>';

                        entry += '</div>';
                        entry += '</div>';
                        entry += '</div>';
                        entry += '</div>';
                        entry += '</a>';

                        notifications += entry;
                    });

                    $("#notification-entries").prepend(notifications);

                    var unreadCount = res.unread;

                    if (unreadCount) {
                        const notifyIcon = document.getElementById('notification-icon');
                        notifyIcon.classList.add('animated', 'swing', 'infinite');

                        $("#notification-count").html('<span class="badge bg-danger pc-h-badge">' + unreadCount + '</span>');
                    }

                    console.log("User notifications loaded.");
                }
            },
            error: function (jqXHR, exception) {
                onAjaxError(jqXHR, exception);
            }
        });
    }
    catch (err) {
        //ErrorInSweetAlert(err.message); //in Common.js
        console.log(err.message);
    }
}

// Start the connection.
start();
loadUserNotifications();

connection.on("server-notification", async () => {
    await loadUserNotifications();
});