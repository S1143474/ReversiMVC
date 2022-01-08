const API_URL = 'https://localhost:44339/api/';

const Game = ((url) => {
    console.log('Hallo vanuit een module!');

    let configMap = {
        apiUrl: url
    };

    let stateMap = {

    };

    // Private function init
    const privateInit = (afterInit) => {
        console.log(configMap.apiUrl);
        _getCurrentGameState();
        afterInit();
    };

    const _getCurrentGameState = () => {
        setInterval(() => {
            Game.Model.getGameState().then(result => {
                stateMap.gameState = result.data;
                console.log(stateMap.gameState);
            });
        }, 2000);
    };

    // Return value/object to the outer scope.
    return {
        init: privateInit
    };
})(API_URL);