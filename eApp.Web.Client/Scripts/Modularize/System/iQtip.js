
var iQtip = function () {

    var init = function (className, descTxt) {

        $('.' + className).qtip({ // Grab some elements to apply the tooltip to
            content: {
                text: descTxt
            },
            style: {
                classes: 'qtip-light qtip-shadow'
            },
            position: {
                my: 'top center',  // Position my top left...
                at: 'bottom center', // at the bottom right of...
            }
        });
    }

    var initStack = function (className, descTxt) {

        $('.' + className).qtip({ // Grab some elements to apply the tooltip to                       
            content: {
                text: descTxt
            },
            style: {
                classes: 'qtip-light qtip-shadow'
            },
            position: {
                my: 'top center',  // Position my top left...
                at: 'bottom center', // at the bottom right of...
            },
            show: {
                when: true, // Don't specify a show event
                ready: true // Show the tooltip when ready
            },
            hide: false // Don't specify a hide event
        });
    }

    var initStackTop = function (className, descTxt) {

        $('.' + className).qtip({ // Grab some elements to apply the tooltip to                       
            content: {
                text: descTxt
            },
            style: {
                classes: 'qtip-light qtip-shadow'
            },
            position: {
                my: 'bottom center',  // Position my top left...
                at: 'top center', // at the bottom right of...
            },
            show: {
                when: true, // Don't specify a show event
                ready: true // Show the tooltip when ready
            },
            hide: false // Don't specify a hide event
        });
    }


    return {
        init: init,
        initStack: initStack,
        initStackTop: initStackTop
    }

}();
