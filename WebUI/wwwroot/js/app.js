"use strict";

function asyncGeneratorStep(gen, resolve, reject, _next, _throw, key, arg) { try { var info = gen[key](arg); var value = info.value; } catch (error) { reject(error); return; } if (info.done) { resolve(value); } else { Promise.resolve(value).then(_next, _throw); } }

function _asyncToGenerator(fn) { return function () { var self = this, args = arguments; return new Promise(function (resolve, reject) { var gen = fn.apply(self, args); function _next(value) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "next", value); } function _throw(err) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "throw", err); } _next(undefined); }); }; }

class FeedbackWidget {
  constructor(elementId) {
    this._elementId = elementId;
    this._key = "feedback_widget";
  }

  get elementId() {
    return this._elementId;
  }

  show(message, type) {
    var element = document.getElementById(this._elementId);

    if (message instanceof String || typeof message === 'string') {
      $(element).find(".alert__title").text(message);
      $(element).find(".alert__message").text(null);
    } else if (message.hasOwnProperty('title') && message.hasOwnProperty('msg')) {
      $(element).find(".alert__title").text(message['title']);
      $(element).find(".alert__message").text(message['msg']);
    } // Alter alert based on type


    element.classList.add(type == "success" ? 'alert-success' : 'alert-danger');

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
    var element = document.getElementById(this._elementId);

    if (element.style.display === "block") {
      element.style.display = "none";
    }
  }

  log(message) {
    var localStorage = window.localStorage;

    if (localStorage.getItem(this._key) != null) {
      var temp = JSON.parse(localStorage.getItem(this._key));
      temp.push(message);

      if (temp.length > 10) {
        // Remove first item in JSON array
        temp.shift();
      }

      localStorage.setItem(this._key, JSON.stringify(temp));
    } else {
      console.log("This key is used for the first time");
      localStorage.setItem(this._key, "[" + JSON.stringify(message) + "]");
    }
  }

  removeLog() {
    localStorage.removeItem(this._key);
  }

  history() {
    var _localStorage$getItem;

    // TODO: Maby make it that the history is shown in order with the feedback widget.
    // assign the value off feedback_widget to logHistory variable if not null;
    var strLogHistory = (_localStorage$getItem = localStorage.getItem(this._key)) !== null && _localStorage$getItem !== void 0 ? _localStorage$getItem : [];
    var logHistory = JSON.parse(strLogHistory);

    for (var i = 0; i < logHistory.length; i++) {
      console.log("".concat(logHistory[i]['type'], " - ").concat(logHistory[i]['message']['title'], " : ").concat(logHistory[i]['message']['msg']));
    }
  }

}

var API_URL = 'https://localhost:44339/api/';

var Game = (url => {
  console.log('Hallo vanuit een module!');
  var configMap = {
    apiUrl: url
  };
  var stateMap = {}; // Private function init

  var privateInit = afterInit => {
    Game.Model.init();
    Game.Model.listen("Redirect", _redirect);
    Game.Model.listen("OnMove", _onMove);
    Game.Model.listen("OnWrongMove", _wrongMoveMessage);
    Game.Model.listen("OnDisableMove", _disableMovePlacement);
    Game.Model.listen("OnFinish", _finish);
    console.log(configMap.apiUrl);

    _getCurrentGameState();

    afterInit();
  };

  var _turnFiches = (fichesToTurnAround, turn) => {
    var cells = document.querySelectorAll(".fiche");
    cells.forEach(cell => {
      var x = parseInt(cell.getAttribute('x'));
      var y = parseInt(cell.getAttribute('y'));
      fichesToTurnAround.forEach(fiche => {
        if (fiche.x == x && fiche.y == y) {
          if (turn == 1) {
            cell.classList.remove("fiche-white", "fiche-empty");
            cell.classList.add("fiche-black");
          } else {
            cell.classList.remove("fiche-black", "fiche-empty");
            cell.classList.add("fiche-white");
          }
        }
      });
    });
  };

  var _redirect = url => {
    console.log(url);
    window.location.pathname = url;
  };

  var _onMove = (fichesToTurnAround, aanDeBeurt) => {
    console.log("Movement");
    console.log("Turn", aanDeBeurt);

    _turnFiches(fichesToTurnAround, aanDeBeurt);

    var buttons = document.querySelectorAll(".fiche");
    buttons.forEach(button => {
      button.disabled = false;
      button.style = "pointer-events: auto;";
    });
  };

  var _wrongMoveMessage = () => {
    var element = document.getElementById("extra-info");
    element.textContent = "Verkeerde zet, probeer het nog eens";
  };

  var _disableMovePlacement = (fichesToTurnAround, aanDeBeurt) => {
    _turnFiches(fichesToTurnAround, aanDeBeurt);

    var buttons = document.querySelectorAll(".fiche");
    buttons.forEach(button => {
      button.disabled = true;
      button.style = "pointer-events: none;";
    });
  };

  var _finish = gameResult => {
    console.log("Game Finished", gameResult);
  };

  var _getCurrentGameState = () => {
    setInterval(() => {
      Game.Model.getGameState().then(result => {
        stateMap.gameState = result.data;
        console.log(stateMap.gameState);
      });
    }, 2000);
  }; // Return value/object to the outer scope.


  return {
    init: privateInit
  };
})(API_URL);

Game.Data = (() => {
  var configMap = {
    apiKey: '',
    hubUrl: '/reversiHub',
    hubConnection: '',
    mock: [{
      url: 'api/Spel/Beurt/1',
      data: 1
    }]
  };
  var stateMap = {
    environment: 'development'
  };

  var listen = function listen(hubMethodName, callback) {
    console.log("Listening to: ", hubMethodName);
    configMap.hubConnection.on(hubMethodName, function () {
      for (var _len = arguments.length, others = new Array(_len), _key = 0; _key < _len; _key++) {
        others[_key] = arguments[_key];
      }

      console.log(others);
      callback(...others);
    });
  };

  var start = () => {
    try {
      configMap.hubConnection.start();
    } catch (error) {
      console.debug(error);
      setTimeout(start, 3000);
    }
  };

  var get = url => {
    return stateMap.environment == 'development' ? getMockData(url) : $.get(url).then(result => {
      return result;
    }).catch(error => {
      console.error(error.message);
    });
    ;
  };

  var getMockData = url => {
    var mockData = configMap.mock.filter(data => data.url == url);
    return new Promise((resolve, reject) => {
      resolve(mockData);
    });
  };

  var privateInit = environment => {
    console.log("Data: Init");
    $("#reversiboardform").on("submit", event => {
      event.preventDefault();
      clickAudio.play();
      var element = document.getElementById("extra-info");
      element.textContent = null; // console.log(event);

      var x = parseInt(event.originalEvent.submitter.getAttribute('x'));
      var y = parseInt(event.originalEvent.submitter.getAttribute('y'));
      var data = {
        x: x,
        y: y,
        hasPassed: false
      }; // console.log("X:", x);
      // console.log("Y:", y);

      configMap.hubConnection.invoke("OnMove", data);
      /*return false;*/
    });
    configMap.hubConnection = new signalR.HubConnectionBuilder().withUrl(configMap.hubUrl).build();
    configMap.hubConnection.onclose( /*#__PURE__*/_asyncToGenerator(function* () {
      yield start();
    }));
    start();
    if (environment != 'production' && environment != 'development') throw new Error('De environment welke gebruikt dient te worden bestaat niet.');
    stateMap.environment = environment;
    return true;
  };

  return {
    init: privateInit,
    get: get,
    listen: listen
  };
})();

Game.Model = (() => {
  var configMap = {};

  var privateInit = () => {
    Game.Data.init('development');
    Game.Data.listen("StartGame", () => {});
    Game.Data.listen("Redirect", () => {}, "url"); // Game.Data.listen("OnMove", (aanDeBeurt) => { console.log(aanDeBeurt); }, "aanDeBeurt");

    return true;
  };

  var _listen = (on, callback) => {
    // TODO: Checking possibilities or something.
    Game.Data.listen(on, callback);
  };

  var _getGameState = () => {
    return Game.Data.get('api/Spel/Beurt/1').then(result => {
      if (result != null && result[0].data != null && result[0].data >= 0 && result[0].data <= 2) {
        return result[0];
      }

      throw new Error('Er ging iets mis met het opvragen van de huidige gamestate.');
    });
  };

  return {
    init: privateInit,
    listen: _listen,
    getGameState: _getGameState
  };
})();

Game.Reversi = (() => {
  console.log("hallo, vanuit module Reversi.");
  var configMap = {};

  var privateInit = () => {
    return true;
  };

  return {
    init: privateInit
  };
})();