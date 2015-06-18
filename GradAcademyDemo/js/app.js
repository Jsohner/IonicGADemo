angular.module('ionicApp', ['ionic'])

.config(function ($stateProvider, $urlRouterProvider) {

    $stateProvider
      .state('signin', {
          url: '/sign-in',
          templateUrl: 'templates/sign-in.html',
          controller: 'SignInCtrl'
      })
      .state('tabs', {
          url: '/tab',
          abstract: true,
          templateUrl: 'templates/tabs.html'
      })
      .state('tabs.home', {
          url: '/home',
          views: {
              'home-tab': {
                  templateUrl: 'templates/home.html',
                  controller: 'HomeTabCtrl'
              }
          }
      })
        .state('tabs.details', {
            url: '/details',
            views: {
                'scoreboard-tab': {
                    templateUrl: 'templates/details.html'
                }
            }
        })
      .state('tabs.scoreboard', {
          url: '/scoreboard',
          views: {
              'scoreboard-tab': {
                  templateUrl: 'templates/scoreboard.html',
                  controller: "ScoreboardTabCtrl"
              }
          }
      })
    .state('tabs.questions', {
        url: '/questions',
        views: {
            'questions-tab': {
                templateUrl: 'templates/questions.html',
                controller: "QuestionsTabCtrl"
            }
        }
    })
      .state('tabs.navstack', {
          url: '/navstack',
          views: {
              'about-tab': {
                  templateUrl: 'templates/nav-stack.html'
              }
          }
      });

    //window.serverUrl = "http://hmbgascoreboardserver.azurewebsites.net";
    window.serverUrl = "http://localhost:49376";


    $urlRouterProvider.otherwise("/sign-in");

})

.controller('SignInCtrl', function ($scope, $state, $rootScope, playerService) {
    this.rootScope = $rootScope;

    $scope.user = {}
        
        playerService.getPlayers(function (data) {
        $scope.players = data;
    });

    $scope.signIn = function (player) {
        $rootScope.selectedPlayer = player;

        console.log('Sign-In', player);
        $state.go('tabs.home');
    };
})

.controller('HomeTabCtrl', function ($scope, $rootScope, playerService) {
    console.log('HomeTabCtrl');

    $scope.currentPoints = 0;
    $scope.bonusMessage = "";
    $scope.updateBonusMessage = playerService.updateBonusMessage;

    $scope.yellText = "";
    $scope.vote = playerService.voteForPlayer;
    $scope.submitWordForPlayer = playerService.submitWordForPlayer;

    $scope.postToSlack = playerService.postToSlack;
    $scope.postYellToSlack = playerService.postYellToSlack;
    $scope.voteForWord = playerService.voteForWord;
    $scope.clearYellText = function() {
        this.yellText = "";        
    };

    })

.controller("ScoreboardTabCtrl", function ($scope, $rootScope, playerService) {
    console.log("ScoreboardTabCtrl");

    var interval;

    var callbackFunction = function (data) {
        // sort the players by vote
        $scope.players = data.sort(function (x, y) { return x.VoteCount < y.VoteCount; });
    };

    // Get the players list with votes
    // This will be used to render a scoreboard
    $scope.$on('$ionicView.enter', function () {
        // code to run each time view is entered
        playerService.getPlayers(callbackFunction);

        interval = setInterval(function () {
            // we'll keep polling so we don't have to work on websockets during this demo
            playerService.getPlayers(callbackFunction);
        }, 1000);
    });

    $scope.$on("$ionicView.leave", function () {
        clearInterval(interval);
    });
})

    .controller("QuestionsTabCtrl", function ($scope, $rootScope, playerService) {
        console.log("QuestionsTabCtrl");

        playerService.getQuestions(function(data) {
            $scope.questions = data.messages;
        });                                
    })

//.constant('API_END_POINT', 'http://hmbgascoreboardserver.azurewebsites.net')
.constant('API_END_POINT', 'http://localhost:49376')

.service("playerService", function ($http, API_END_POINT) {
    // public methods

    // takes a callback that takes one parameter data
    function getPlayers(callback) {
        $http({
            method: "GET",
            url: API_END_POINT + "/Players"
        }).success(callback);
    }

    function voteForPlayer(player) {
        $http.post(API_END_POINT + "/Vote/" + player.PlayerId);
    }

    function submitWordForPlayer(player, yellText) {
        $http.post(API_END_POINT + "/Word/" + player.PlayerId +"/" + yellText);
        
    }

    function postToSlack(player) {
        var objectToPost =
         {
             token: "xoxp-6261658387-6261658403-6272659622-7f1736",
             channel: "C067PK2U8",
             text: "*Robot John is dead. All hail Miketron*\nAnd " + player.FirstName + " _just scored a point._\n And this is some code: ```User john = new User();```", 
             username: player.FirstName,
             icon_url: "http://store.bbcomcdn.com/images/store/prodimage/prod_prod910018/image_prodprod910018_largeImage_X_450_white.jpg",
             mrkdwn: true,
             mrkdwn_in: "text"
         };

        $.ajax({
            type: "POST",
            url: "https://slack.com/api/chat.postMessage",
            dataType: "application/JSON",
            data: objectToPost
            
        });
    }

    function postYellToSlack(player, yellText) {

        var genderPronoun = "";
        var possesiveGenderPronoun = "";

        if (player.FirstName == "Katherine" || player.FirstName == "Kat" || player.FirstName == "katherine") {
            genderPronoun = "she";
            possesiveGenderPronoun = "her";
        } else {
            genderPronoun = "he";
            possesiveGenderPronoun = "his";
        }

        var yellVerbs = [
            "shouted",
            "screamed",
            "roared",
            "shrieked",
            "groaned",
            "sang",
            "muttered",
            "coughed",
            "danced",
            "mouthed",
            "chirped"
        ];

        var yellEndings = [
            "like a howling banshee.",
            "as " + genderPronoun + " loosened " + possesiveGenderPronoun + " necktie.",
            "at Jared.",
            "in a fit of rage.",
            "in a thick Australian accent.",
            "just before jumping out of the boat.",
            "as " + genderPronoun + " breathed " + possesiveGenderPronoun + " last.",
            "into a supermarket.",
            "from under " + possesiveGenderPronoun + " bed.",
            "as " + genderPronoun + " dumped several large salt-water fish from " + possesiveGenderPronoun + " briefcase.",
            "enthusiastically",
            "with a hollow tone of remorse",
            "before " + genderPronoun + " realized " + genderPronoun + " was dreaming.",
            "before " + genderPronoun + " started to weep.",
            "looking Voldemort right in the eyes"
        ];

        var getYellVerb = function() {
            var verbIndex = Math.floor((Math.random() * yellVerbs.length) + 1);
            var yellVerb = yellVerbs[verbIndex];
            return yellVerb;
        }

        var getYellEnding = function () {

            var yellEnding = "";
            var addEndingCheck = Math.floor((Math.random() * 3) + 1);
            if (addEndingCheck > 1) {
                var endingIndex = Math.floor((Math.random() * yellEndings.length) + 1);
                yellEnding = yellEndings[endingIndex];
            }
            return yellEnding;
        }

        var yellVerb = getYellVerb();
        var yellEnding = getYellEnding();

        var objectToPost =
         {
             token: "xoxp-6261658387-6261658403-6272659622-7f1736",
             channel: "C067PK2U8",
             text: "\"" + yellText.toUpperCase() + "!!!\" " + yellVerb + " " + player.FirstName +" "+ yellEnding,
             username: player.FirstName,
             icon_emoji: ":" + player.FirstName.toLowerCase() + ":"
         };

        $.ajax({
            type: "POST",
            url: "https://slack.com/api/chat.postMessage",
            dataType: "application/JSON",
            data: objectToPost

        });

    }

    function calculateAdditionalBonuses(points, yellText) {
        var multiplier = 1;
        var bonus = 0;
        yellText = yellText.toLowerCase();

        if (yellText.indexOf("hmb") >= 0) {
            bonus = bonus + 50;
        }

        if (yellText.indexOf("jared") >= 0) {
            bonus = bonus + 50;
        }

        if (yellText.indexOf("hail miketron") >= 0) {
            bonus = bonus + 1000;
        }

        points = points * multiplier + bonus;

        return points;

    }

    function updateBonusMessage(yellText, points) {


        yellText = yellText.toLowerCase();

        this.bonusMessage = "";

        if (yellText.indexOf("hmb") >= 0) {
            this.bonusMessage = "You mentioned HMB! You earned 50 bonus points.";
        }

        if (yellText.indexOf("jared") >= 0) {
            this.bonusMessage = "You mentioned Jared! I hope you had something nice to say. You earned 50 bonus points.";
        }

        if (yellText.indexOf("hail miketron") >= 0) {
            this.bonusMessage = "HAIL MIKETRON. 1000 POINTS";
        }

        this.bonusMessage = this.bonusMessage 
            + "\n" + "You earned " + points + " points total!";

    }

    function voteForWord(player, yellText) {

        var points = 0;

        points = yellText.length;

        function countWords(str) {
            return str.split(/\s+/).length;
        }

        var wordBonus = countWords(yellText) * 10;
        if (wordBonus > 150) {
            wordBonus = 150;
        }

        points = points + wordBonus;

        points = calculateAdditionalBonuses(points, yellText);

        for (var i = 0; i < points; i++) {
            $http.post(API_END_POINT + "/Vote/" + player.PlayerId);
        }

        this.points = points;
    }


    function getQuestions(callback) {
        var query =
          {
              token: "xoxp-6261658387-6261658403-6272659622-7f1736",
              channel: "C067PK2U8"       
          };
        
        $http({
            method: "GET",
            url: "https://slack.com/api/channels.history",
            params: query
        }).success(callback);
    
    }

    // public api
    return {
        getPlayers: getPlayers,
        voteForPlayer: voteForPlayer,
        submitWordForPlayer: submitWordForPlayer,
        postToSlack: postToSlack,
        postYellToSlack: postYellToSlack,
        voteForWord: voteForWord,
        getQuestions: getQuestions,
        updateBonusMessage: updateBonusMessage
    }
});


// Not sure about this yet. I want to figure out how to display the clock and battery still
ionic.Platform.ready(function () {
    //hide the status bar using the StatusBar plugin
    if (window.StatusBar) {
        // org.apache.cordova.statusbar required
        //StatusBar.hide();
        //StatusBar.overlaysWebView(true);
        //ionic.Platform.fullScreen();
        StatusBar.styleDefault();
    }
});