Game.Data = (() => {
    
    let configMap = {
        apiKey: '',
        mock: [
            {
                url: 'api/Spel/Beurt/1',
                data: 1,
            }
        ],
    };

    let stateMap = { environment: 'development' };

    const get = (url) => {
        return (stateMap.environment == 'development') ? getMockData(url) : $.get(url)
        .then(result => {
            return result;
        }).catch(error => {
            console.error(error.message);
        });;
    };

    const getMockData = (url) => {
        const mockData = configMap.mock.filter(data => data.url == url);

        return new Promise((resolve, reject) => {
            resolve(mockData);
        });
    };

    const privateInit = (environment) => {
        console.log("Data: Init");

        if (environment != 'production' && environment != 'development')
            throw new Error('De environment welke gebruikt dient te worden bestaat niet.');
        
        stateMap.environment = environment;

        return true;
    };
    
    return {
        init: privateInit,
        get: get
    };
})();