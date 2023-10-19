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
    Game.Template.init();
    Game.ComponentEvents.init();
    Game.Reversi.init();
    Game.ComponentEvents.addClick("btn-update-password", _openDialog, "update-user-password-dialog");
    Game.ComponentEvents.addClick("btn-update-2fa", _openDialog, "update-2fa-dialog");
    Game.ComponentEvents.addClick("btn-close-update-user-password-dialog", _closeDialog, "update-user-password-dialog"); // Game.ComponentEvents.addClick("twofa__login");

    Game.Model.listen("Redirect", _redirect);
    Game.Model.listen("OnMove", _onMove);
    Game.Model.listen("OnWrongMove", _wrongMoveMessage);
    Game.Model.listen("OnDisableMove", _disableMovePlacement);
    Game.Model.listen("OnFinish", _finish);
    Game.Model.listen("OnError", _onError);
    Game.Model.listen("OnPlayerOnline", _test);
    Game.Model.listen("OnCreateGame", _gameCreated);
    Game.Model.listen("OnDeletedUser", _onDeletedUserMessage);
    var location = window.location.pathname;
    var locationList = location.split('/');
    var locationListLength = locationList.length - 1;
    Game.LoginWith2faPage.init(locationList[locationListLength]);
    Game.RegisterPage.init(locationList[locationListLength]);
    Game.ComponentEvents.addClickOnClass("show__password__icon", showPassword);
    Game.ComponentEvents.addClickOnClass("hide__password__icon", hidePassword);
    Game.ComponentEvents.addOnFocus("register__password", showPasswordStrengthMeter);
    Game.ComponentEvents.addOnFocusOut("register__password", hidePasswordStrengthMeter);
    Game.ComponentEvents.addOnFocus("register__confirm__password", showPasswordStrengthMeter);
    Game.ComponentEvents.addOnFocusOut("register__confirm__password", hidePasswordStrengthMeter);
    Game.ComponentEvents.addOnInput("register__password", udpatePasswordMeter);
    Game.ComponentEvents.addOnInput("register__confirm__password", udpatePasswordMeter);
    Game.ComponentEvents.addClickOnClass("del-user", _showDeleteUserDialog);
    Game.ComponentEvents.addClick("close__dialog", _closeDeleteUserDialog);
    Game.ComponentEvents.addClick("confirm__delete__user__dialog", _submitDeleteUserDialog);
    Game.ComponentEvents.addClick("close__deleted__user__dialog", _closeDeletedUserMessageDialog);
    console.log(configMap.apiUrl);
    Game.STATS.init(); // _getCurrentGameState();

    afterInit();
  }; // ------- DELETE USER DIALOG -------


  var _showDeleteUserDialog = component => {
    var dialog = document.getElementById("delete-user-dialog");
    var userId = component.getAttribute("user-id");
    var userIdInput = document.getElementsByName("userId")[0];
    userIdInput.value = userId;
    dialog.showModal();
  };

  var _closeDeleteUserDialog = event => {
    event.preventDefault();
    var dialog = document.getElementById("delete-user-dialog");
    dialog.close();
  };

  var _closeDeletedUserMessageDialog = event => {
    var dialog = document.getElementById("account__deletion__dialog");
    dialog.close();
    location.reload(true);
  };

  var _submitDeleteUserDialog = event => {
    event.preventDefault();
    var dialog = document.getElementById("delete-user-dialog");
    var form = dialog.children[2];
    var userIdInput = document.getElementsByName("userId")[0];
    var reasonInput = document.getElementsByName("reason")[0];
    form.submit();
  }; // ------- PASSWORD CHECKER -------


  var _hasUpperCase = string => {
    return /[A-Z]/.test(string);
  };

  var _hasLowerCase = string => {
    return /[a-z]/.test(string);
  };

  var _hasNumber = string => {
    return /\d/.test(string);
  };

  var _hasNonAlphaNumeric = string => {
    return /[^a-zA-Z\d\s:]/g.test(string);
  };

  var _togglePasswordRequirement = (element, passwordCondition) => {
    if (passwordCondition) {
      element.classList.add("included");
      element.classList.remove("not-included");
      element.classList.remove("none");
      element.children[0].innerText = "task_alt";
      return;
    }

    element.classList.add("not-included");
    element.classList.remove("none");
    element.classList.remove("included");
    element.children[0].innerText = "close";
  };

  var udpatePasswordMeter = element => {
    for (var nextElement of element.parentNode.children) {
      if (nextElement.classList.contains("password__strength__checker")) {
        var requirements = nextElement.children[1].children;

        if (element.value.length === 0) {
          for (var el of requirements) {
            if (!el.classList.contains("none")) {
              el.classList.add("none");
              el.children[0].innerText = "close";
            }

            el.classList.remove("not-included", "included");
          }

          return;
        }

        _togglePasswordRequirement(requirements[0], element.value.length >= 12 && element.value.length <= 128);

        _togglePasswordRequirement(requirements[1], _hasUpperCase(element.value));

        _togglePasswordRequirement(requirements[2], _hasLowerCase(element.value));

        _togglePasswordRequirement(requirements[3], _hasNumber(element.value));

        _togglePasswordRequirement(requirements[4], _hasNonAlphaNumeric(element.value));
      }
    }
  };

  var hidePasswordStrengthMeter = componentId => {
    for (var nextElement of componentId.parentNode.children) {
      if (nextElement.classList.contains("password__strength__checker")) {
        nextElement.style.display = 'none';
      }
    }
  };

  var showPasswordStrengthMeter = componentId => {
    var strengthPasswordMeter = document.getElementsByClassName('password__strength__checker');

    for (var element of strengthPasswordMeter) {
      element.style.display = 'none';
    }

    for (var nextElement of componentId.parentNode.children) {
      if (nextElement.classList.contains("password__strength__checker")) {
        nextElement.style.display = 'flex';
      }
    }
  };

  var showPassword = element => {
    element.style.display = 'none';

    for (var nextElement of element.parentNode.children) {
      if (nextElement.classList.contains("hide__password__icon")) {
        nextElement.style.display = 'block';
      }

      if (nextElement.tagName.toLowerCase() === 'input') {
        nextElement.type = 'text';
      }
    }
  };

  var hidePassword = element => {
    element.style.display = 'none';

    for (var nextElement of element.parentNode.children) {
      if (nextElement.classList.contains("show__password__icon")) {
        nextElement.style.display = 'block';
      }

      if (nextElement.tagName.toLowerCase() === 'input') {
        nextElement.type = 'password';
      }
    }
  };

  var _test = message => {
    var counter = document.getElementById("online__player__counter");
    if (counter !== null) counter.innerText = "players online: ".concat(message);
  };

  var _closeDialog = dialogId => {
    var dialogComponent = document.getElementById(dialogId);
    if (dialogComponent == null) throw new Error("Dialog Id Not Found!");
    if (typeof dialogComponent.showModal === "function") dialogComponent.close();
  };

  var _openDialog = dialogId => {
    var dialogComponent = document.getElementById(dialogId);
    if (dialogComponent == null) throw new Error("Dialog Id Not Found!");
    if (typeof dialogComponent.showModal === "function") dialogComponent.showModal();
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
    Game.STATS.push(fichesToTurnAround, aanDeBeurt);

    _turnFiches(fichesToTurnAround, aanDeBeurt);

    var buttons = document.querySelectorAll(".fiche");
    buttons.forEach(button => {
      button.disabled = false;
      button.style = "pointer-events: auto;";
    }); // High light current user.

    var leftScoreOwner = document.querySelector(".game__ownedboardbalance__player-1");
    var rightScoreOwner = document.querySelector(".game__ownedboardbalance__player-2");
    rightScoreOwner.classList.remove("big");
    leftScoreOwner.classList.add("big");
    Game.Reversi.displayJoke();
  };

  var _wrongMoveMessage = /*#__PURE__*/function () {
    var _ref = _asyncToGenerator(function* (notExecutedMessage) {
      console.log(notExecutedMessage);
      var element = document.getElementById("wrong_move_info");
      console.log("Element:" + element);
      element.innerHTML = iconText("notification_important", notExecutedMessage);
      element.style.display = "flex";
      yield delay(4000);
      element.style.display = "none";
    });

    return function _wrongMoveMessage(_x) {
      return _ref.apply(this, arguments);
    };
  }();

  var _disableMovePlacement = (fichesToTurnAround, aanDeBeurt) => {
    Game.STATS.push(fichesToTurnAround, aanDeBeurt);

    _turnFiches(fichesToTurnAround, aanDeBeurt);

    var buttons = document.querySelectorAll(".fiche");
    buttons.forEach(button => {
      button.disabled = true;
      button.style = "pointer-events: none;";
    }); // Stop highlightin current user and highlight opponent

    var leftScoreOwner = document.querySelector(".game__ownedboardbalance__player-1");
    var rightScoreOwner = document.querySelector(".game__ownedboardbalance__player-2");
    rightScoreOwner.classList.add("big");
    leftScoreOwner.classList.remove("big");
  };

  var _finish = gameResult => {
    console.log("Game Finished", gameResult);
  };

  var _onDeletedUserMessage = reason => {
    var dialog = document.getElementById("account__deletion__dialog");
    var dialogMessage = document.getElementById("account__deletion__reason");
    dialogMessage.innerText = reason;
    dialog.showModal();
  };

  var _onError = message => {
    console.log('Error', message);
    console.log('TODO: ', 'create a proper popup error message');
  };

  var _gameCreated = () => {
    console.debug("Game Created");
    var location = window.location.pathname;
    var locationList = location.split('/');
    var pageName = locationList[locationList.length - 1];
    console.debug("Pagename:", pageName);
    if (pageName !== "AvailableGames") return;
    window.location.reload();
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

Game.API = (() => {
  var configMap = {
    imageCategories: ["nature", "city", "technology", "food", "still_life", "abstract", "wildlife"]
  };

  var _privateInit = () => {
    console.log("API: Private Init");
  };

  var getRandomDadJoke = /*#__PURE__*/function () {
    var _ref2 = _asyncToGenerator(function* () {
      return yield Game.Data.get("https://icanhazdadjoke.com/").catch(error => console.error(error));
    });

    return function getRandomDadJoke() {
      return _ref2.apply(this, arguments);
    };
  }();

  var getRandomImage = /*#__PURE__*/function () {
    var _ref3 = _asyncToGenerator(function* (imageElement) {
      var randomCategory = configMap.imageCategories[Math.floor(Math.random() * configMap.imageCategories.length)];
      console.log(randomCategory);
      var imageSrc = yield Game.Data.getImage("https://api.api-ninjas.com/v1/randomimage?category=".concat(randomCategory));
      var base64regex = /^([0-9a-zA-Z+/]{4})*(([0-9a-zA-Z+/]{2}==)|([0-9a-zA-Z+/]{3}=))?$/;
      var base64EncodedStr = imageSrc.toString('base64');
      console.log(base64regex.test(imageSrc));
      imageElement.src = "data:image/*;base64,".concat(base64EncodedStr);
    });

    return function getRandomImage(_x2) {
      return _ref3.apply(this, arguments);
    };
  }();

  return {
    init: _privateInit,
    getRandomDadJoke,
    getRandomImage
  };
})();

Game.ComponentEvents = (() => {
  var configMap = {};
  var stateMap = {};

  var privateInit = () => {};

  var privateAddClickListener = (id, callback, param) => {
    var componentId = document.getElementById(id);

    if (componentId == null) {
      console.error("Component Id: ".concat(id, " Not Found"));
      return;
    }

    componentId.addEventListener('click', event => {
      callback(event, param);
    });
  };

  var privateAddClickOnClassListener = (id, callback) => {
    var components = document.getElementsByClassName(id);

    if (components == null) {
      console.error("Component class: ".concat(id, " Not Found"));
      return;
    }

    var _loop = function _loop(component) {
      component.addEventListener('click', () => {
        callback(component);
      });
    };

    for (var component of components) {
      _loop(component);
    }
  };

  var privateAddOnFocusListener = (id, callback) => {
    var componentId = document.getElementById(id);

    if (componentId == null) {
      console.error("Component Id: ".concat(id, " Not Found"));
      return;
    }

    componentId.addEventListener('focus', () => {
      callback(componentId);
    });
  };

  var privateAddOnFocusOutListener = (id, callback) => {
    var componentId = document.getElementById(id);

    if (componentId == null) {
      console.error("Component Id: ".concat(id, " Not Found"));
      return;
    }

    componentId.addEventListener('focusout', () => {
      callback(componentId);
    });
  };

  var privateAddOnInputListener = (id, callback) => {
    var componentId = document.getElementById(id);

    if (componentId == null) {
      console.error("Component Id: ".concat(id, " Not Found"));
      return;
    }

    componentId.addEventListener('input', () => {
      callback(componentId);
    });
  };

  var _addOnChangeListener = (id, callback) => {
    var component = id.getElementsByTagName("select")[0];

    if (component == null) {
      console.error("Component: ".concat(id, " Not Found"));
      return;
    }

    if (component.getAttribute('listener') === 'true') return;
    component.setAttribute('listener', 'true');
    component.addEventListener('change', event => {
      callback(component, event);
    });
  };

  return {
    init: privateInit,
    addClick: privateAddClickListener,
    addClickOnClass: privateAddClickOnClassListener,
    addOnFocus: privateAddOnFocusListener,
    addOnFocusOut: privateAddOnFocusOutListener,
    addOnInput: privateAddOnInputListener,
    addOnChange: _addOnChangeListener
  };
})();

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
    environment: 'production'
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

  var getImage = url => {
    return $.ajax({
      url: url,
      headers: {
        'X-Api-Key': 'pXIbEgubhq8hsoBRXyfuRA==59cec1dOsdZ0R21p',
        'Accept': 'image/jpg'
      },
      success: function success(result) {
        console.log(result);
        return result;
      },
      error: function ajaxError(jqXHR) {
        console.error('Error: ', jqXHR.responseText);
      }
    });
  };

  var get = url => {
    return stateMap.environment == 'development' ? getMockData(url) : $.ajax({
      url: url,
      dataType: 'json',
      type: 'GET',
      contentType: 'application/json; charset=utf-8',
      error: function error(XMLHttpRequest, textStatus, errorThrown) {
        alert("Status: " + textStatus);
        alert("Error: " + errorThrown);
      }
    }).then(result => {
      console.log(result);
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
    $("#game-resign-button").on("click", event => {
      configMap.hubConnection.invoke("OnSurrender");
    });
    $("#game-pass-button").on("click", event => {
      var data = {
        x: -1,
        y: -1,
        hasPassed: true
      };
      configMap.hubConnection.invoke("OnMove", data);
    });
    $("#reversiboardform").on("submit", event => {
      event.preventDefault();
      clickAudio.play(); // let element = document.getElementById("extra-info");
      // console.log("Element: " + element);
      // element.textContent = null;
      // console.log(event);

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
    getImage,
    listen: listen
  };
})();

Game.Model = (() => {
  var configMap = {};

  var privateInit = () => {
    Game.Data.init('production');
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
  var configMap = {};
  var stateMap = {
    orignalRoles: {
      Speler: [],
      Moderator: [],
      Admin: []
    },
    changedRoles: []
  };

  var _assignEventListeners = () => {
    var currentLocation = window.location.pathname.split('/')[2];
    if (currentLocation !== "AsignRoles") return;
    var rows = document.getElementsByTagName("table")[0].rows; // var last = rows[rows.length - 1];
    // var cell = last.cells[0];
    // var value = cell.innerHTML

    var _loop2 = function _loop2(i) {
      var role = rows[i].cells[2].getElementsByTagName("select")[0].value;
      var guid = rows[i].cells[0].getElementsByTagName("input")[0].value;
      stateMap.orignalRoles[role].push(guid);
      Game.ComponentEvents.addOnChange(rows[i].cells[2], (component, event) => {
        var user = stateMap.changedRoles.find(el => el.id === guid);

        if (user === undefined) {
          stateMap.changedRoles.push({
            id: guid,
            role: component.value
          });
        } else {
          user.role = component.value;
        }

        var inputList = document.getElementById("changeListInput");
        inputList.value = JSON.stringify(stateMap.changedRoles);

        _showSaveButton();
      });
    };

    for (var i = 1; i < rows.length; i++) {
      _loop2(i);
    }
  };

  var _submitRoleChange = () => {
    console.log("Submit: ", stateMap.changedRoles);
  };

  var _showSaveButton = () => {
    var saveBtn = document.getElementById("savechangesbtn");
    saveBtn.style.display = "block";
  };

  var getRandomJokeTemplate = /*#__PURE__*/function () {
    var _ref5 = _asyncToGenerator(function* () {
      var jokeJson = yield Game.API.getRandomDadJoke();
      console.log(jokeJson);
      if (jokeJson === undefined) return;
      var dadjoke = jokeJson.joke;
      var jokeTemplate = Game.Template.parseTemplate("dadjoke.randomdadjoke", {
        joke: dadjoke
      });
      return jokeTemplate;
    });

    return function getRandomJokeTemplate() {
      return _ref5.apply(this, arguments);
    };
  }();

  var displayJoke = /*#__PURE__*/function () {
    var _ref6 = _asyncToGenerator(function* () {
      var template = yield getRandomJokeTemplate();
      var dadJokeHtml = $("#current-dad-joke");
      dadJokeHtml.html(template);
    });

    return function displayJoke() {
      return _ref6.apply(this, arguments);
    };
  }();

  var privateInit = () => {
    _assignEventListeners(); // $('form').submit(function(event){
    //     // cancel the default action
    //     event.preventDefault();
    //     // var body = escape(myEditor.get('element').value);
    //     // var theForm = $(this);
    //     // $.post(theForm.attr('action'), theForm.serialize() + '&body=' + body, function (data) {
    //         // do whatever with the result
    //     // });
    //     // $(this).submit();
    //     // $(this).submit();
    //     // event.submit();
    // });


    Game.ComponentEvents.addClick("savechangesbtn", _submitRoleChange);
    return true;
  };

  return {
    init: privateInit,
    displayJoke
  };
})();

Game.STATS = (() => {
  var configMap = {};
  var chart;
  var prevBeurt;
  var p1CapturedFiches = [];
  var p2CapturedFiches = [];

  var _privateInit = () => {
    update();
  };

  var update = () => {
    var context = $('#myChart'); // chart?.destory();

    chart = new Chart(context, {
      type: 'line',
      data: {
        labels: p1CapturedFiches.map((value, index) => index + 1),
        datasets: [{
          label: "Player 1",
          data: p1CapturedFiches,
          fill: false,
          borderColor: "#01A2E9",
          tension: 0.1
        }, {
          label: "Player 2",
          data: p2CapturedFiches,
          fill: false,
          borderColor: "#DA3D2D",
          tension: 0.1
        }]
      },
      options: {
        scales: {
          y: {
            beginAtZero: true
          }
        }
      }
    });
    console.log(context);
  };

  var pushData = (fiches, beurt) => {
    var turnedFiches = fiches.length; // console.log(turnedFiches);
    // console.log("Statsturn", beurt)
    // console.log(p1CapturedFiches, p2CapturedFiches);

    console.log("Toggle");

    if (beurt == 1 && prevBeurt != 1) {
      prevBeurt = 1;
      p1CapturedFiches.push(turnedFiches);
    }

    if (beurt == 2 && prevBeurt != 2) {
      prevBeurt = 2;
      p2CapturedFiches.push(turnedFiches);
    } // console.log(p1CapturedFiches, p2CapturedFiches);


    chart.data.labels = p1CapturedFiches.map((value, index) => index + 1);
    chart.data.datasets = [{
      label: "Player 1",
      data: p1CapturedFiches,
      fill: false,
      borderColor: "#01A2E9",
      tension: 0.1
    }, {
      label: "Player 2",
      data: p2CapturedFiches,
      fill: false,
      borderColor: "#DA3D2D",
      tension: 0.1
    }];
    chart.update(); // update();
  };

  return {
    init: _privateInit,
    push: pushData
  };
})();

Game.Template = (() => {
  var privateInit = () => {
    console.log("init template");
    Handlebars.registerHelper('ifCond', function (v1, v2, options) {
      if (v1 === v2) {
        return options.fn(this);
      }

      return options.inverse(this);
    });
  };

  var getTemplate = templateName => {
    var templates = spa_templates.templates;
    console.log(templates);

    for (var t of templateName.split(".")) {
      templates = templates[t];
    }

    return templates; // if (Object.hasOwn(templates, templateName)) {
    //     return templates[templateName];
    // }
  };

  var parseTemplate = (templateName, data) => {
    var template = getTemplate(templateName);
    return template(data);
  };

  return {
    init: privateInit,
    getTemplate,
    parseTemplate
  };
})();

Game.LoginWith2faPage = (() => {
  var configMap = {
    page: 'LoginWith2fa'
  };
  var stateMap = {
    currentPage: '',
    isLocated: false,
    pincode: []
  };

  var privateInit = page => {
    stateMap.currentPage = page;
    stateMap.isLocated = configMap.page === page;
    if (!stateMap.isLocated) return;
    maskInputFields();
  };

  var maskInputFields = () => {
    console.log('maskinput');
    var debug = false;
    var pincode1 = document.getElementsByName('Input.Pincode1');
    var form = $('#twofa__login__form');
    var group = form.find('#twofa__login__form__pincode');
    var inputs = group.find('input');
    var first = form.find('[name="Input.Pincode1"]');
    var second = form.find('[name="Input.Pincode2"]');
    var third = form.find('[name="Input.Pincode3"]');
    var fourth = form.find('[name="Input.Pincode4"]');
    var fifth = form.find('[name="Input.Pincode5"]');
    var sixth = form.find('[name="Input.Pincode6"]');
    inputs.on('keyup', event => {
      var code = event.keyCode || event.which;

      if (code === 9 && !event.shiftKey) {
        event.preventDefault();
        $('#twofa__login').focus();
      }
    }).inputmask({
      mask: '9',
      placeholder: '',
      showMaskOnHover: false,
      showMaskOnFocus: false,
      clearIncomplete: true,
      onincomplete: function onincomplete() {
        !debug || console.log('inputmask incomplete');
      },
      oncleared: function oncleared() {
        var index = inputs.index(this),
            prev = index - 1,
            next = index + 1;

        if (prev >= 0) {
          // clear field
          inputs.eq(prev).val(''); // focus field

          inputs.eq(prev).focus(); // remove last nubmer

          stateMap.pincode.splice(-1, 1);
        } else {
          return false;
        }

        !debug || console.log('[oncleared]', prev, index, next);
      },
      onKeyValidation: function onKeyValidation(key, result) {
        var index = inputs.index(this),
            prev = index - 1,
            next = index + 1; // focus to next field

        if (prev < 6) {
          inputs.eq(next).focus();
        }

        !debug || console.log('[onKeyValidation]', index, key, result, _pincode);
      },
      onBeforePaste: function onBeforePaste(data, opts) {
        $.each(data.split(''), function (index, value) {
          // set value
          inputs.eq(index).val(value);
          !debug || console.log('[onBeforePaste:each]', index, value);
        });
        return false;
      }
    });
    $('[name="Input.Pincode1"]').on('focus', function (event) {//   ! debug || console.log('[1:focus]', _pincode);
    }).inputmask({
      oncomplete: function oncomplete() {
        // add first character
        stateMap.pincode.push($(this).val()); // focus to second field

        $('[name="Input.Pincode2"]').focus(); // ! debug || console.log('[1:oncomplete]', _pincode);
      }
    });
    $('[name="Input.Pincode2"]').on('focus', function (event) {
      if (!(first.val().trim() !== '')) {
        // prevent default
        event.preventDefault(); // reset pincode

        stateMap.pincode = []; // handle each field

        inputs.each(function () {
          // clear each field
          $(this).val('');
        }); // focus to first field

        first.focus();
      }

      !debug || console.log('[2:focus]', _pincode);
    }).inputmask({
      oncomplete: function oncomplete() {
        // add second character
        stateMap.pincode.push($(this).val()); // focus to third field

        $('[name="Input.Pincode3"]').focus();
        !debug || console.log('[2:oncomplete]', _pincode);
      }
    });
    $('[name="Input.Pincode3"]').on('focus', function (event) {
      if (!(first.val().trim() !== '' && second.val().trim() !== '')) {
        // prevent default
        event.preventDefault(); // reset pincode

        stateMap.pincode = []; // handle each field

        inputs.each(function () {
          // clear each field
          $(this).val('');
        }); // focus to first field

        first.focus();
      }

      !debug || console.log('[3:focus]', _pincode);
    }).inputmask({
      oncomplete: function oncomplete() {
        // add third character
        stateMap.pincode.push($(this).val()); // focus to fourth field

        $('[name="Input.Pincode4"]').focus();
        !debug || console.log('[3:oncomplete]', _pincode);
      }
    });
    $('[name="Input.Pincode4"]').on('focus', function (event) {
      if (!(first.val().trim() !== '' && second.val().trim() !== '' && third.val().trim() !== '')) {
        // prevent default
        event.preventDefault(); // reset pincode

        stateMap.pincode = []; // handle each field

        inputs.each(function () {
          // clear each field
          $(this).val('');
        }); // focus to first field

        first.focus();
      }

      !debug || console.log('[4:focus]', _pincode);
    }).inputmask({
      oncomplete: function oncomplete() {
        // add fo fourth character
        stateMap.pincode.push($(this).val()); // focus to fifth field

        $('[name="Input.Pincode5"]').focus();
        !debug || console.log('[4:oncomplete]', _pincode);
      }
    });
    $('[name="Input.Pincode5"]').on('focus', function (event) {
      if (!(first.val().trim() !== '' && second.val().trim() !== '' && third.val().trim() !== '' && fourth.val().trim() !== '')) {
        // prevent default
        event.preventDefault(); // reset pincode

        stateMap.pincode = []; // handle each field

        inputs.each(function () {
          // clear each field
          $(this).val('');
        }); // focus to first field

        first.focus();
      }

      !debug || console.log('[5:focus]', stateMap.pincode);
    }).inputmask({
      oncomplete: function oncomplete() {
        // add fifth character
        stateMap.pincode.push($(this).val()); // focus to sixth field

        $('[name="Input.Pincode6"]').focus();
        !debug || console.log('[5:oncomplete]', stateMap.pincode);
      }
    });
    $('[name="Input.Pincode6"]').on('focus', function (event) {
      if (!(first.val().trim() !== '' && second.val().trim() !== '' && third.val().trim() !== '' && fourth.val().trim() !== '' && fifth.val().trim() !== '')) {
        // prevent default
        event.preventDefault(); // reset pincode

        stateMap.pincode = []; // handle each field

        inputs.each(function () {
          // clear each field
          $(this).val('');
        }); // focus to first field

        first.focus();
      }

      !debug || console.log('[6:focus]', stateMap.pincode);
    }).inputmask({
      oncomplete: function oncomplete() {
        // add sixth character
        stateMap.pincode.push($(this).val()); // pin length not equal to six characters

        if (stateMap.pincode.length !== 6) {
          // reset pin
          stateMap.pincode = []; // handle each field

          inputs.each(function () {
            // clear each field
            $(this).val('');
          }); // focus to first field

          $('[name="Input.Pincode1"]').focus();
        } else {
          // handle each field
          inputs.each(function () {
            // disable field
            $(this).prop('disabled', true);
          }); // send request
          //   _req = $.ajax({
          //     type: 'POST',
          //     url: '/api/tfa',
          //     data: {
          //       'code': _pincode.join(''),
          //       '_csrf': ''
          //     }
          //   })
          //   .done(function(data, textStatus, jqXHR) {
          //     try {
          //       ! debug || console.log('data', data);
          //       if (data.ok === true) {
          //         $group.addClass('form__group--success');
          //         $button.removeAttr('disabled');
          //       }
          //       if (data.ok === false) {
          //         $group.addClass('form__group--error');
          //       }
          //     } catch (err) {
          //     }
          //   })
          //   .fail(function(jqXHR, textStatus, errorThrown) {
          //     $group.removeClass('form__group--error');
          //   })
          //   .always(function(dataOrjqXHR, textStatus, jqXHRorErrorThrown) {
          //     // reset pin
          //     stateMap.pincode = [];
          //     // reset request
          //     _req = null;
          //     setTimeout(function() {
          //       // handle each field
          //       $inputs.each(function() {
          //         // clear all fields
          //         $(this).val('');
          //         // enable all fields
          //         $(this).prop('disabled', false);
          //       });
          //       // remove response status class
          //       $group.removeClass('form__group--success form__group--error');
          //       // disable submit button
          //       $button.attr('disabled', true);
          //       // focus to first field
          //       $first.focus();
          //     }, 2000);
          //   });
        }

        !debug || console.log('[6:oncomplete]', _pincode);
      }
    });

    pincode1.onfocus = event => {};

    console.log(pincode1);
  };

  return {
    init: privateInit
  };
})();

Game.RegisterPage = (() => {
  var configMap = {
    page: 'Register'
  };
  var stateMap = {
    currentPage: '',
    isLocated: false,
    pincode: []
  };

  var privateInit = page => {
    stateMap.currentPage = page;
    stateMap.isLocated = configMap.page === page;
    if (!stateMap.isLocated) return; // Game.ComponentEvents.addClickOnClass("show__password__icon", showPassword)
    // Game.ComponentEvents.addClickOnClass("hide__password__icon", hidePassword)
  };

  return {
    init: privateInit
  };
})();

var delay = ms => new Promise(res => setTimeout(res, ms));

var iconText = (icon, text) => "<i class='material-icons'>".concat(icon, "</i> ").concat(text);