
$(document).ready(function () {
    $.getJSON("http://localhost:11785/api/KMLStream/names", function (data) {


        var $streamNameDropdown = $("#streamNameDropdown");
        $streamNameDropdown.empty();
        $.each(data, function (index, value) {
            $streamNameDropdown.append("<option>" + value + "</option>");
        });

        $streamNameDropdown.change();

    });
});

$("#streamNameDropdown").change(function () {

    var $dropdown = $(this);

    if (!$dropdown.val()) {
        $dropdown.empty();
        return;
    }

    $.getJSON("http://localhost:11785/api/KMLStream/sources?name=" + $dropdown.val(), function (data) {

        var $sourceDropdown = $("#sourceDropdown");
        $sourceDropdown.empty();
        $.each(data, function (index, value) {
            $sourceDropdown.append("<option>" + value + "</option>");
        });

        $sourceDropdown.change();
    });
});

$("#sourceDropdown").change(function () {

    var $dropdown = $(this);
    var $streamNameDropdown = $("#streamNameDropdown");

    if (!$dropdown || !$streamNameDropdown) {
        $dropdown.empty();
        return;
    }

    $.getJSON("http://localhost:11785/api/KMLStream/dataTypes?name=" + $streamNameDropdown.val() + "&source=" + $dropdown.val(), function (data) {

        var $dataTypeDropdown = $("#dataTypeDropdown");
        $dataTypeDropdown.empty();
        $.each(data, function (index, value) {
            $dataTypeDropdown.append("<option>" + value + "</option>");
        });

        $dataTypeDropdown.change();

    });
});

$("#dataTypeDropdown").change(function () {

    var $dropdown = $(this);
    var $streamNameDropdown = $("#streamNameDropdown");
    var $sourceDropdown = $("#sourceDropdown");
    var $timeDropdown = $("#timeDropdown");

    if (!$dropdown || !$streamNameDropdown) {
        $dropdown.empty();
        return;
    }

    $.getJSON("http://localhost:11785/api/KMLData?typeName=" + $dropdown.val(), function (data) {

        $timeDropdown.empty();
        $.each(data, function (index, value) {
            $timeDropdown.append("<option value='" + value.ID + "'>" + new Date(value.CreatedAt).toLocaleString() + "</option>");
        });
        $timeDropdown.change();

    });

    getLatestData();

});

$("#updateStreamButton").click(function() {
    var source = $("#sourceDropdown").val();
    var name = $("#streamNameDropdown").val();
    var id = $("#timeDropdown").val();

    $("#updateStreamButton").prop("disabled",true);

    $.ajax({
        url: "http://localhost:11785/api/KMLStream/Update?setUpdated=true&source=" + source + "&name=" + name + "&kmlDataId=" + id,
        type: 'PUT',
        success: function(result) {
            $("#updateStreamButton").prop("disabled", false);
            getLatestData();
        },
        error: function(result) {
            $("#updateStreamButton").prop("disabled", false);
        }
    });
});

var getLatestData = function () {

    var source = $("#sourceDropdown").val();
    var typeName = $("#dataTypeDropdown").val();
    var name = $("#streamNameDropdown").val();

    $.getJSON("http://localhost:11785/api/KMLStream?source=" + source + "&typeName=" + typeName + "&name=" + name, function (data) {
        $("#dataCreatedAt").text(new Date(data.KMLData.CreatedAt).toLocaleString());
    });

}