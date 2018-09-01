$(document).ready(function () {

    var baseURI = "http://localhost:50223/";

    $('#loadingDiv')
        .hide()  // Hide it initially
        .ajaxStart(function () {
            $(this).show();
        })
        .ajaxStop(function () {
            $(this).hide();
    });

    $.ajax({
        type: "GET",
        url: baseURI + "api/timezone",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            var select_timezones = $("#select_timezone");
            $.each(result, function () {
                select_timezones.append($("<option />").val(this.Id).text(this.DisplayName));
            });
            //{
            //    "Id": "Dateline Standard Time", "DisplayName": "(UTC-12:00) International Date Line West",
            //        "StandardName": "Dateline Standard Time", "DaylightName": "Dateline Daylight Time",
            //            "BaseUtcOffset": "-12:00:00", "AdjustmentRules": null, "SupportsDaylightSavingTime": false
            //}
        },
        failure: function (result) {
            alert("Can't connect to the TimeZone API!");
        }
    });

    $("#btn_getTime").click(function () {
        var select_timezone = document.getElementById("select_timezone");
        var strTimeZone = select_timezone.options[select_timezone.selectedIndex].value;
        if (strTimeZone == "") {
            var xhttp = new XMLHttpRequest();
            xhttp.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    document.getElementById("output").innerHTML = this.responseText;
                }
            };
            xhttp.open("GET", baseURI + "api/currenttime", true);
            xhttp.setRequestHeader("Content-type", "application/json");
            xhttp.send();
        }
        else {
            $.post(baseURI + "api/currenttime/GetTimeByTimeZoneId", function (result) {
                $("#output").html(result);
            })
            .done(function () {
                alert("second success");
            })
            .fail(function () {
                alert("Can't connect to the CurrentTime API!");
            })
            
        }
    });
});

