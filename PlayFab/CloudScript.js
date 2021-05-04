var COINS_CURRENCY_CODE = "CO";
var LIVES_CURRENCY_CODE = "LV";

///////////////////////// Cloud Script Handler Functions /////////////////////////
function SaveProgress(args) {

    if (args.IsGameOver){
        SubtractLife();
    }
    // set up Coins VC
    var addVcRequest = {
        PlayFabId: currentPlayerId,
        VirtualCurrency: COINS_CURRENCY_CODE,
        Amount: args.CoinsCollected
    };
    return server.AddUserVirtualCurrency(addVcRequest);
}

function SubtractLife() {
    var subtractVcRequest = {
        PlayFabId: currentPlayerId,
        VirtualCurrency: LIVES_CURRENCY_CODE,
        Amount: 1
    };
    return server.SubtractUserVirtualCurrency(subtractVcRequest);
}

///////////////////////// Define the handlers /////////////////////////
handlers.SaveProgress = SaveProgress;
