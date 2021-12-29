class FeedbackWidget {

    constructor(elementId) {
        this._elementId = elementId;
        this._key = "feedback_widget";
    }

    get elementId() {
        return this._elementId;
    }

    show(message, type) {
        let element = document.getElementById(this._elementId);

        if (message instanceof String || typeof message === 'string') {
            $(element).find(".alert__title").text(message);
            $(element).find(".alert__message").text(null);
        } else if (message.hasOwnProperty('title') && message.hasOwnProperty('msg')) {
            $(element).find(".alert__title").text(message['title']);
            $(element).find(".alert__message").text(message['msg']);
            
        }
        
        // Alter alert based on type
        debugger;
        element.classList.add((type == "success") ? 'alert-success' : 'alert-danger');
        if (type == "success") {
            element.classList.add("alert-hover");
            $(element).find(".alert__close").hide();
        }

        if (element.style.display === "none") {
            element.style.display = "block";
        }

        this.log({
            "message": message,
            "type": type
        });
    }

    hide() {
        let element = document.getElementById(this._elementId);

        if (element.style.display === "block") {
            element.style.display = "none"
        }
    }

    log(message) {
        let localStorage = window.localStorage;

        if (localStorage.getItem(this._key) != null) {
            let temp = JSON.parse(localStorage.getItem(this._key));

            temp.push(message);

            if (temp.length > 10) {
                // Remove first item in JSON array
                temp.shift();
            }

            localStorage.setItem(this._key, JSON.stringify(temp));
        } else {
            console.log("This key is used for the first time")
            localStorage.setItem(this._key, "[" + JSON.stringify(message) + "]");
        }
    }

    removeLog() {
        localStorage.removeItem(this._key);
    }

    history() {
        // TODO: Maby make it that the history is shown in order with the feedback widget.
        // assign the value off feedback_widget to logHistory variable if not null;
        let strLogHistory = localStorage.getItem(this._key) ?? [];
        let logHistory = JSON.parse(strLogHistory);

        for (let i = 0; i < logHistory.length; i++) {
            console.log(`${logHistory[i]['type']} - ${logHistory[i]['message']['title']} : ${logHistory[i]['message']['msg']}`);
        }        
    }
}