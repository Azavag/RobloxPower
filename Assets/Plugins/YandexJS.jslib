mergeInto(LibraryManager.library, {
	// GiveMePlayerData: function () {
    // 	myGameInstance.SendMessage('Yandex', 'SetName', player.getName());
    // 	myGameInstance.SendMessage('Yandex', 'SetPhoto', player.getPhoto("medium"));
  	// },

    // RateGameExtern: function () {  
    // 	ysdk.feedback.canReview()
    //     .then(({ value, reason }) => {
    //         if (value) 
    //         {
    //             ysdk.feedback.requestReview()
    //                 .then(({ feedbackSent }) => {
    //                     console.log(feedbackSent);
    //                     if(feedbackSent == true)
    //                     {
    //                       myGameInstance.SendMessage('ShopChooseController', 'SetRewardingState');                         
    //                       myGameInstance.SendMessage('ShopChooseController', 'UnlockRewardSkin');
    //                     }   
                                       
    //                 })
    //         } 
    //         else {
    //             console.log(reason);
    //             if(reason == "NO_AUTH")
    //               myGameInstance.SendMessage('RateGameController', 'ShowAuthWindow'); 
    //         }
    //     })
  	// },

    // Auth: function()
    // {
    //   ysdk.auth.openAuthDialog();
    //   myGameInstance.SendMessage('RateGameController', 'CloseAuthWindow');  
    // },
  //   RateGameExtern: function(){
  //   ysdk.feedback.canReview()
  //   .then(({ value, reason }) => {
  //     if (value) {
  //       ysdk.feedback.requestReview()
  //       .then(({ feedbackSent }) => {
  //         myGameInstance.SendMessage("Progress","GiveHints");
  //         myGameInstance.SendMessage("Progress","CloseRateUI");
  //       })
  //     } else {
  //       ysdk.auth.openAuthDialog()
  //       //console.log(reason)
  //     }
  //   })
  // },


	SaveExtern: function(date) {
    if(player){
      var dateString = UTF8ToString(date);
      var myobj = JSON.parse(dateString);
      player.setData(myobj);     
    }
    },

  LoadExtern: function(){
    if(player){
      player.getData().then(_data => {
      console.log("Data is getting");
      console.log(_data);
      const myJSON = JSON.stringify(_data);
      myGameInstance.SendMessage('YandexSDK', 'SetPlayerInfo', myJSON);
    });
    }   
  },

  CheckSdkReady: function()
  {   
    if(sdkReady)
      {
        ysdk.features.LoadingAPI.ready(); 
        myGameInstance.SendMessage('SceneLoader', 'ToggleSdkReady'); 
      }
  },

  //Страничная реклама
  ShowIntersitialAdvExtern: function(){
    ysdk.adv.showFullscreenAdv({
      callbacks: {       
         onOpen: () => {
          myGameInstance.SendMessage("SoundController", "MuteGame");         
          console.log('Adv open.');
        },
        onClose: function(wasShown) {
          console.log("Adv closed");
          myGameInstance.SendMessage('AdvManager', 'ResetTimer');
          myGameInstance.SendMessage('AdvManager', 'CloseIntersitialAdv');   
          myGameInstance.SendMessage("SoundController", "UnmuteGame");
        },
        onError: function(error) {
          // some action on error
        }
      }
    })
  },


  ShowRewardedAdvExtern: function(){

    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          myGameInstance.SendMessage("SoundController", "MuteGame");         
          console.log('VideoReward ad open.');
        },
        onRewarded: () => {
          myGameInstance.SendMessage("SkinsShop", "SetRewardingState");                
        },
        onClose: () => {
          myGameInstance.SendMessage("SkinsShop","UnlockRewardSkin");  
          myGameInstance.SendMessage('AdvManager', 'CloseRewardedAdv');  
          myGameInstance.SendMessage("SoundController", "UnmuteGame");
          console.log('VideoReward ad closed');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
      }
    })
  },

 	SetToLeaderboard : function(value){
      if (typeof ysdk === 'undefined' || !ysdk || !ysdk.leaderboards) {
        console.warn('Leaderboards SDK is not ready');
        return;
      }

      window._lbScoreQueue = window._lbScoreQueue || { lastAt: 0, timer: null, pending: null };
      var state = window._lbScoreQueue;
      var leaderboardName = 'Leaderboard';
      var minIntervalMs = 20000;

      var sendScore = function(score) {
        var doSet = function() {
          ysdk.leaderboards.setScore(leaderboardName, score).catch(function(err) {
            console.log('setScore failed', err);
          });
        };

        if (!ysdk.isAvailableMethod) {
          doSet();
          return;
        }

        ysdk.isAvailableMethod('leaderboards.setScore').then(function(available) {
          if (available)
            doSet();
          else
            console.log('leaderboards.setScore is not available');
        }).catch(function(err) {
          console.log('isAvailableMethod failed', err);
        });
      };

      var now = Date.now();
      var elapsed = now - state.lastAt;
      if (elapsed < minIntervalMs) {
        state.pending = value;
        if (!state.timer) {
          state.timer = setTimeout(function() {
            state.timer = null;
            if (state.pending === null)
              return;
            var pending = state.pending;
            state.pending = null;
            state.lastAt = Date.now();
            sendScore(pending);
          }, minIntervalMs - elapsed);
        }
        return;
      }

      state.lastAt = now;
      sendScore(value);
  	},

  
    ShowLeaderBoard : function()
    {  
      if (typeof ysdk === 'undefined' || !ysdk || !ysdk.leaderboards) {
        console.warn('Leaderboards SDK is not ready');
        return;
      }

      ysdk.leaderboards.getEntries('Leaderboard', {
        quantityTop: 3,
        includeUser: true,
        quantityAround: 1
      }).then(function(res) {
        var src = (res && res.entries) ? res.entries : [];
        var entries = [];
        for (var i = 0; i < src.length; i++) {
          var entry = src[i] || {};
          var player = entry.player || {};
          var avatarSrc = '';
          try {
            if (typeof player.getAvatarSrc === 'function')
              avatarSrc = player.getAvatarSrc('medium') || '';
          } catch (e) {}
          entries.push({
            rank: entry.rank || 0,
            score: entry.score || 0,
            extraData: entry.extraData || '',
            player: {
              publicName: player.publicName || '',
              uniqueID: player.uniqueID || '',
              avatarSrc: avatarSrc
            }
          });
        }

        var payload = JSON.stringify({
          userRank: (res && res.userRank) ? res.userRank : 0,
          entries: entries
        });
        console.log(payload);
        myGameInstance.SendMessage('YandexSDK', 'BoardEntriesReady', payload);
      }).catch(function(err) {
        console.log('getEntries failed', err);
      });
    },

    // CheckAuth: function()
    // {    
    //   // initPlayer().then(_player => {
    //   //         if (_player.getMode() === 'lite') {
    //   //           myGameInstance.SendMessage('Leaderboard', 'OpenAuthAlert'); } 
    //   // }).catch(() => {myGameInstance.SendMessage('Leaderboard', 'OpenEntries') });
    //   initPlayer();
    //   if(player) 
    //     myGameInstance.SendMessage('LeaderboardController', 'OpenEntries');    
    //   else
    //     myGameInstance.SendMessage('LeaderboardController', 'OpenAuthAlert');  
    // },

    GetDevice : function()
    {
      var deviceData = ysdk.deviceInfo.type;   
      myGameInstance.SendMessage('YandexSDK', 'SetDeviceInfo', deviceData);
    },

    // GetDomainExtern : function()
    // {
    //   var domain = ysdk.environment.i18n.tld;   
    //   var bufferSize = lengthBytesUTF8(domain) + 1;
    //   var buffer = _malloc(bufferSize);
    //   stringToUTF8(domain, buffer, bufferSize);
    //   return buffer;
    // },
    GetLang : function()
    {
      var lang = ysdk.environment.i18n.lang;
      var bufferSize = lengthBytesUTF8(lang) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(lang, buffer, bufferSize);
      return buffer;
    },

    StartGameplay : function()
    {
      if (ysdk.features && ysdk.features.GameplayAPI) {
        ysdk.features.GameplayAPI.start();
      }
    },

    StopGameplay : function()
    {
      if (ysdk.features && ysdk.features.GameplayAPI) {
        ysdk.features.GameplayAPI.stop();
      }
    }
  });