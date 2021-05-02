var COINS_CURRENCY_CODE = "SC";
///////////////////////// Cloud Script Handler Functions /////////////////////////
function SaveProgress(args) {
    // set up Coins VC
    var addVcRequest = {
        PlayFabId: currentPlayerId,
        VirtualCurrency: COINS_CURRENCY_CODE,
        Amount: args.CoinsCollected
    };
    return server.AddUserVirtualCurrency(addVcRequest);
}

///////////////////////// Define the handlers /////////////////////////
handlers.SaveProgress = SaveProgress;