Game.Reversi = (() => {
    console.log("hallo, vanuit module Reversi.");

    let configMap = {

    };

    const privateInit = () => {
        return true;
    };

    return {
        init: privateInit
    }
})();