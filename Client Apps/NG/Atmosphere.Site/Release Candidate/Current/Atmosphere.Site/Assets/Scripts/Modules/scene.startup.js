//Contains the initial screen startup routines
var startup = function () {
    var windowFocused = true,
    init = function () {
        //track if user switches tabs or not otherwise
        //timers may queue up in some browsers like Chrome
        $(window).focus(function () {
            windowFocused = true;
        });

        $(window).blur(function () {
            windowFocused = false;
        });

        var defaultPositions = sceneLayoutService.get();

        $('#gridButton').click(function () {
            sceneStateManager.changeScene();
        });

        $('#cloudButton').click(function () {
            sceneStateManager.changeScene();
        });

        sceneStateManager.init(defaultPositions);
        sceneStateManager.renderTiles();

        setTimeout(function () {
            //dataService.getTickerQuotes(renderTicker);

            $("#aaglogo").delay('500').fadeIn('slow');

            $('.tile').each(function () {
                $(this)./*delay(Math.floor(Math.random() * 450)).*/fadeIn(360, 'easeInCubic', function () {
                    if ($(this).attr('id') == "Quote") {
                        $("#QuoteButton").click();
                    }
                });
            });

        }, 1000);

        //Update markets and account data on timer basis
        //        setInterval(function () {
        //            if (!windowFocused) return;
        //            dataService.getMarketIndexes(renderMarketTiles);
        //            dataService.getAccount(acctNumber, renderAccountTiles);
        //        }, 15000);

        //        var sceneChangeTimer = setInterval(function () {
        //            if ($('#AccountDetails').data().templates != undefined && $('#AccountPositions').data().templates != undefined) {
        //                clearInterval(sceneChangeTimer);
        //                sceneStateManager.changeScene();
        //                $('.scroller').delay(1000).each(function () {
        //                    $(this).fadeIn('slow', 'easeInCubic');
        //                });
        //            }
        //        }, 2000);
    },

    //    renderMarketTiles = function (json) {
    //        sceneStateManager.renderTile(json, $('#DOW'), 1200);
    //        sceneStateManager.renderTile(json, $('#NASDAQ'), 800);
    //        sceneStateManager.renderTile(json, $('#SP500'), 500);
    //    },

    //    renderAccountTiles = function (json) {
    //        $('div.tile[id^="Account"]').each(function () {
    //            sceneStateManager.renderTile(json, $(this), 500);
    //        });
    //    },

    renderTicker = function (json) {
        $("#contentHolder").delay('800').fadeIn('slow');

        $('#ticker').delay('2360').fadeIn('slow', 'easeInCubic', function () {
            $('#TickerTemplate_News').tmpl(json).appendTo('#MarqueeNewsText');
            $('#TickerTemplate_Quotes').tmpl(json).appendTo('#MarqueeQuotesText');
            var scrollOptions = {
                velocity: 50,
                direction: 'horizontal',
                startfrom: 'right',
                loop: 'infinite',
                movetype: 'linear',
                onmouseover: 'play',
                onmouseout: 'play',
                onstartup: 'play',
                cursor: 'pointer'
            };

 //           $('#MarqueeNews').SetScroller(scrollOptions);
 //           $('#MarqueeQuotes').SetScroller(scrollOptions);
        });
    };

    return {
        init: init,
        //renderTicker: renderTicker
    };
} ();

