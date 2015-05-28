$(document).ready(function () {
    $.getJSON("http://localhost:11785/api/KMLStream/names", function (data) {

        var $streamNameDropdown = $("#streamNameDropdown");
        $streamNameDropdown.empty();

        $.each(data, function (index, streamName) {
            $streamNameDropdown.append("<option>" + streamName + "</option>");
        });

        $streamNameDropdown.change();

    });
});

$("#streamNameDropdown").change(function () {

    var $streamNameDropdown = $(this);

    if (!$streamNameDropdown.val()) {
        $streamNameDropdown.empty();
        return;
    }

    $.getJSON("http://localhost:11785/api/KMLStream/DataTypes?name=" + $streamNameDropdown.val(), function (data) {

        var $dataTypeDropdown = $("#dataTypeDropdown");

        $dataTypeDropdown.empty();
        $.each(data, function (index, dataTypeName) {
            $dataTypeDropdown.append("<option>" + dataTypeName + "</option>");
        });

        $dataTypeDropdown.change();
    });
});

$("#dataTypeDropdown").change(function () {

    var $dataTypeDropdown = $(this);
    var $streamNameDropdown = $("#streamNameDropdown");
    var $kmlDataDropdown = $("#kmlDataDropdown");

    if (!$dataTypeDropdown || !$streamNameDropdown) {
        $dataTypeDropdown.empty();
        return;
    }

    $.getJSON("http://localhost:11785/api/KMLData?typeName=" + $dataTypeDropdown.val(), function (data) {

        $kmlDataDropdown.empty();
        $.each(data, function (index, kmlData) {
            $kmlDataDropdown.append("<option value='" + kmlData.Id + "'>" + new Date(kmlData.CreatedAt).toLocaleString() + "</option>");
        });

        $kmlDataDropdown.change();

    });

    getLatestData($streamNameDropdown.val(), $dataTypeDropdown.val());

});

$("#updateStreamButton").click(function () {
    var streamName = $("#streamNameDropdown").val();
    var dataTypeName = $("#dataTypeDropdown").val();
    var kmlDataId = $("#kmlDataDropdown").val();

    $("#updateStreamButton").prop("disabled", true);

    $.ajax({
        url: "http://localhost:11785/api/KMLStream?streamName=" + streamName + "&dataTypeName=" + dataTypeName + "&kmlDataId=" + kmlDataId,
        type: "PUT",
        success: function (result) {
            $("#updateStreamButton").prop("disabled", false);
            getLatestData(streamName, dataTypeName);
        },
        error: function (result) {
            $("#updateStreamButton").prop("disabled", false);
        }
    });
});

var getLatestData = function (streamName, dataTypeName) {
    $.getJSON("http://localhost:11785/api/KMLStream?streamName=" + streamName + "&dataTypeName=" + dataTypeName, function (stream) {
        $("#dataCreatedAt").text(new Date(stream.KMLData.CreatedAt).toLocaleString());
    });
}