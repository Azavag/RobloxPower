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
    	ysdk.getLeaderboards()
      	.then(lb => {
          lb.setLeaderboardScore('Leaderboard', value);
      });
  	},

  
    ShowLeaderBoard : function()
    {  
      ysdk.getLeaderboards()
          .then(lb => {             
              lb.getLeaderboardEntries('Leaderboard', { includeUser: true})
                  .then(res => {
                  console.log(res);
                  const JSONEntry = JSON.stringify(res);
                  myGameInstance.SendMessage('YandexSDK', 'BoardEntriesReady', JSONEntry);        
                  })
          })
          .catch(err => {
            console.log("Ошибка");
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
  });