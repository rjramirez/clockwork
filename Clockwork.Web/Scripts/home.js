$(document).ready(function () {

    var baseURI = "http://localhost:50223/";
    
    //Load the Timezones
    $.ajax({
        type: "GET",
        url: baseURI + "api/timezone",
        contentType: "application/json",
        dataType: "json",
        beforeSend: function () {
            $('#loadingDiv').show();
        },
        success: function (result) {
            var select_timezones = $("#select_timezone");

            $.each(result["timeZones"], function () {
                select_timezones.append($("<option />").val(this.Id).text(this.DisplayName));
            });

            $.each(result["timeTable"], function (index, value) {
                var tbodyToAppend = "";

                tbodyToAppend += '<tr><th scope="row">' + this.currentTimeQueryId + '</th><td>' + this.clientIp + '</td><td>' + this.utcTime + '</td><td>' + this.time + '</td><td>' + (this.timeZoneId == null ? "Default" : this.timeZoneId) + '</td></tr>';

                $("#clockwork_timetable tbody").append(tbodyToAppend);
            });
            //{
            //    "Id": "Dateline Standard Time", "DisplayName": "(UTC-12:00) International Date Line West",
            //        "StandardName": "Dateline Standard Time", "DaylightName": "Dateline Daylight Time",
            //            "BaseUtcOffset": "-12:00:00", "AdjustmentRules": null, "SupportsDaylightSavingTime": false
            //}
        },
        complete: function () {
            $('#loadingDiv').hide();
        },
        failure: function (result) {
            alert("Can't connect to the TimeZone API!");
        }
    });

    $("#btn_getTime").click(function () {
        var strTimeZone = $("#select_timezone option:selected").val();
        var webApiEndpoint = "";
        if (!strTimeZone) {
            //var xhttp = new XMLHttpRequest();
            //xhttp.onreadystatechange = function () {
            //    if (this.readyState == 4 && this.status == 200) {
            //        document.getElementById("output").innerHTML = this.responseText;
            //    }
            //};
            //xhttp.open("GET", baseURI + "api/CurrentTimeQuery", true);
            //xhttp.setRequestHeader("Content-type", "application/json");
            //xhttp.send();
            webApiEndpoint = baseURI + "api/CurrentTimeQuery";
        }
        else {
            webApiEndpoint = baseURI + "api/CurrentTimeQuery/" + escape(strTimeZone);
        }

        //$.get(webApiEndpoint, function (result) {
            
        //    var lastElement = result.pop();
        //    $("#output").text("ID: " + lastElement["currentTimeQueryId"] + " Client IP: " + lastElement["clientIp"] + " UTC: " + lastElement["utcTime"] + "Server Time: " + lastElement["time"] + " Timezone: " + lastElement["timeZoneId"]);

        //    $("#clockwork_timetable tbody").empty();

        //    var tbodyToAppend = "";

        //    $.each(result, function (index, value) {
        //        console.log(value);
        //        tbodyToAppend += '<tr><th scope="row">' + value["currentTimeQueryId"] + '</th><td>' + value["clientIp"] + '</td><td>' + value["utcTime"] + '</td><td>' + value["time"] + '</td></tr>';
        //    });

        //    $("#clockwork_timetable tbody").append(tbodyToAppend);

        //    $('#loadingDiv').hide();

        //})
        //.fail(function () {
        //    alert("Can't connect to the CurrentTime API!");
        //});

        $.ajax({
            type: 'GET',
            url: webApiEndpoint,
            beforeSend: function () {
                $('#loadingDiv').show();
            },
            success: function (result) {
                //var lastElement = result.pop();
                var lastElement = result[result.length - 1];
                $("#output").text("ID: " + lastElement["currentTimeQueryId"] + " Client IP: " + lastElement["clientIp"] + " UTC: " + lastElement["utcTime"] + "Server Time: " + lastElement["time"] + " Timezone: " + (lastElement["timeZoneId"] == null ? "Default" : lastElement["timeZoneId"]));

                $("#clockwork_timetable tbody").empty();

                var tbodyToAppend = "";

                $.each(result, function (index, value) {
                    console.log(value);
                    tbodyToAppend += '<tr><th scope="row">' + value["currentTimeQueryId"] + '</th><td>' + value["clientIp"] + '</td><td>' + value["utcTime"] + '</td><td>' + value["time"] + '</td><td>' + (value["timeZoneId"] == null ? "Default" : value["timeZoneId"]) + '</td></tr>';
                });

                $("#clockwork_timetable tbody").append(tbodyToAppend);

                $('#loadingDiv').hide();
            },
            failure: function () {
                alert("An error has occured. Please contact the Web Administrator.");
            }
        });
    });

    
});

