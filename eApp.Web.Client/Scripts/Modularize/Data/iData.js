
var iData = function () {

    var iDataBR = function (obj, url) {

        var x = document.getElementById(obj);

        while (x.options.length > 0) { x.remove(0); }

        $.get(url, function (response) {

            var option = document.createElement("option");

            x.add(option);
            x.disabled = false;

            $.each(response, function (index, element) {

                var option = document.createElement("option");

                option.text = element.BrandName + " - " + element.BrandCode;
                option.value = element.BrandCode;
                x.add(option);

            });
        });


    }

    var iDataDEPT = function (obj, url) {

        console.log(url);

        var x = document.getElementById(obj);

        while (x.options.length > 0) { x.remove(0); }

        $.get(url, function (response) {

            var option = document.createElement("option");
            
            $.each(response, function (index, element) {

                var option = document.createElement("option");

                option.text = element.Code + " - " + element.Name;
                option.value = element.Code + " - " + element.Name;

                x.add(option);

            });
        });


    }


    return {
        iDataBR: iDataBR,
        iDataDEPT: iDataDEPT

    }

}();
