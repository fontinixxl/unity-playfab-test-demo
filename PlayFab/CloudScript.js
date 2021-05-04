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

UpdateStatistic = function (args, context) {
  var event = context.playStreamEvent;

  if(event != null) {
    var request = {
      PlayFabId: currentPlayerId,
      Statistics: [{
        StatisticName: event.stat_name,
        Value: event.value
      }]
    };

    server.UpdatePlayerStatistics(request);
  }
};

UpdateStatistics = function (args, context) {
  var event = context.playStreamEvent;

  if(event != null) {
    var stats = event.stats;
    var statArray = [];

    for(var key in stats) {
        var value = stats[key];
        statArray.push({StatisticName: key, Value: value});
	}

    var request = {
      PlayFabId: currentPlayerId,
      Statistics: statArray
    };

    server.UpdatePlayerStatistics(request);
  }
};

///////////////////////// Define the handlers /////////////////////////
handlers.SaveProgress = SaveProgress;
handlers.UpdateStatistic = UpdateStatistic;
handlers.UpdateStatistics = UpdateStatistics;
