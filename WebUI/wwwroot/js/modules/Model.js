Game.Model = (() => {

    let configMap = {
        
    };

    const privateInit = () => {
        return true;
    };

    const _getGameState = () => {
        return Game.Data.get('api/Spel/Beurt/1').then(result => {
            if (result != null && result[0].data != null && result[0].data >= 0 && result[0].data <= 2) {
                return result[0];
            }
            throw new Error('Er ging iets mis met het opvragen van de huidige gamestate.');
        });
    };

    return {
        init: privateInit,
        getGameState: _getGameState,
    };
})();