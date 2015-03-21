
$(document).ready(function () {
    $.getJSON("http://localhost:11785/api/KMLStream/names", function (data) {

        var $streamNameDropdown = $("#streamNameDropdown");
        $streamNameDropdown.empty();
        $.each(data, function (index, value) {
            $streamNameDropdown.append("<option>" + value + "</option>");
        });

    });
});

/*
$("#streamNameDropdown").change(function () {

    var $dropdown = $(this);

    $.getJSON("http://localhost:11785/api/KMLStream/names", function (data) {

        var key = $dropdown.val();
        var vals = [];

        switch (key) {
            case 'beverages':
                vals = data.beverages.split(",");
                break;
            case 'snacks':
                vals = data.snacks.split(",");
                break;
            case 'base':
                vals = ['Please choose from above'];
        }

        var $secondChoice = $("#second-choice");
        $secondChoice.empty();
        $.each(vals, function (index, value) {
            $secondChoice.append("<option>" + value + "</option>");
        });

    });
});

*/