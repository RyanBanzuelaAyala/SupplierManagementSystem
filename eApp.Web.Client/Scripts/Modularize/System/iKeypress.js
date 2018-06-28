var iKeypress = function () {


    var init = function () {
                
        $(document).on('keypress', '.alpha', function ($event) {

            if (!isNaN(String.fromCharCode($event.keyCode))) {
                $event.preventDefault();
            }

        });

        $(document).on('keypress', '.numeric', function ($event) {

            if (isNaN(String.fromCharCode($event.keyCode))) {
                $event.preventDefault();
            }


        });
        
    };

    return {

        init: init
    }

}();