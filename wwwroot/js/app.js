"use strict";

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var FeedbackWidget = /*#__PURE__*/function () {
  function FeedbackWidget(elementId) {
    _classCallCheck(this, FeedbackWidget);

    this._elementId = elementId;
    this._key = "feedback_widget";
  }

  _createClass(FeedbackWidget, [{
    key: "elementId",
    get: function get() {
      return this._elementId;
    }
  }, {
    key: "show",
    value: function show(message, type) {
      var element = document.getElementById(this._elementId);

      if (message instanceof String || typeof message === 'string') {
        $(element).find(".alert__title").text(message);
        $(element).find(".alert__message").text(null);
      } else if (message.hasOwnProperty('title') && message.hasOwnProperty('msg')) {
        $(element).find(".alert__title").text(message['title']);
        $(element).find(".alert__message").text(message['msg']);
      } // Alter alert based on type


      debugger;
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
  }, {
    key: "hide",
    value: function hide() {
      var element = document.getElementById(this._elementId);

      if (element.style.display === "block") {
        element.style.display = "none";
      }
    }
  }, {
    key: "log",
    value: function log(message) {
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
  }, {
    key: "removeLog",
    value: function removeLog() {
      localStorage.removeItem(this._key);
    }
  }, {
    key: "history",
    value: function history() {
      var _localStorage$getItem;

      // TODO: Maby make it that the history is shown in order with the feedback widget.
      // assign the value off feedback_widget to logHistory variable if not null;
      var strLogHistory = (_localStorage$getItem = localStorage.getItem(this._key)) !== null && _localStorage$getItem !== void 0 ? _localStorage$getItem : [];
      var logHistory = JSON.parse(strLogHistory);

      for (var i = 0; i < logHistory.length; i++) {
        console.log("".concat(logHistory[i]['type'], " - ").concat(logHistory[i]['message']['title'], " : ").concat(logHistory[i]['message']['msg']));
      }
    }
  }]);

  return FeedbackWidget;
}();

var API_URL = 'https://localhost:44339/api/';

var Game = function (url) {
  console.log('Hallo vanuit een module!');
  var configMap = {
    apiUrl: url
  };
  var stateMap = {}; // Private function init

  var privateInit = function privateInit(afterInit) {
    console.log(configMap.apiUrl);

    _getCurrentGameState();

    afterInit();
  };

  var _getCurrentGameState = function _getCurrentGameState() {
    setInterval(function () {
      Game.Model.getGameState().then(function (result) {
        stateMap.gameState = result.data;
        console.log(stateMap.gameState);
      });
    }, 2000);
  }; // Return value/object to the outer scope.


  return {
    init: privateInit
  };
}(API_URL);

Game.Data = function () {
  var configMap = {
    apiKey: '',
    mock: [{
      url: 'api/Spel/Beurt/1',
      data: 1
    }]
  };
  var stateMap = {
    environment: 'development'
  };

  var get = function get(url) {
    return stateMap.environment == 'development' ? getMockData(url) : $.get(url).then(function (result) {
      return result;
    })["catch"](function (error) {
      console.error(error.message);
    });
    ;
  };

  var getMockData = function getMockData(url) {
    var mockData = configMap.mock.filter(function (data) {
      return data.url == url;
    });
    return new Promise(function (resolve, reject) {
      resolve(mockData);
    });
  };

  var privateInit = function privateInit(environment) {
    console.log("Data: Init");
    if (environment != 'production' && environment != 'development') throw new Error('De environment welke gebruikt dient te worden bestaat niet.');
    stateMap.environment = environment;
    return true;
  };

  return {
    init: privateInit,
    get: get
  };
}();

Game.Model = function () {
  var configMap = {};

  var privateInit = function privateInit() {
    return true;
  };

  var _getGameState = function _getGameState() {
    return Game.Data.get('api/Spel/Beurt/1').then(function (result) {
      if (result != null && result[0].data != null && result[0].data >= 0 && result[0].data <= 2) {
        return result[0];
      }

      throw new Error('Er ging iets mis met het opvragen van de huidige gamestate.');
    });
  };

  return {
    init: privateInit,
    getGameState: _getGameState
  };
}();

Game.Reversi = function () {
  console.log("hallo, vanuit module Reversi.");
  var configMap = {};

  var privateInit = function privateInit() {
    return true;
  };

  return {
    init: privateInit
  };
}();