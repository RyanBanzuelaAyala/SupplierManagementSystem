var GenerateFactory = function ($http) {

    var searchBR = function () {

        var x = document.getElementById('mtype');
        while (x.options.length > 0) { x.remove(0); }

        $.get('/Vehicles/GetAllModelbyBrand?brand=' + $('#vmanu').val(), function (response) {

            var option = document.createElement("option");
            $.each(response, function (index, element) {

                var option = document.createElement("option");
                option.text = element;
                x.add(option);

            });
        });

        $('#bptype').removeAttr('disabled');

    }

    return {

        searchBR: searchBR
    };

}

GenerateFactory.$inject = ['$http'];