'use strict';

$("#submit").click(function(e) {
    $("#weather-table").slideUp(300);

    var zip = $("#zip-code").val();

    e.preventDefault();
    if (zip.length != 5 || isNaN(zip)){
        alert("You didn't enter a valid zip code.");
    }
    
    //$("#weather-spinner").show();
    //$("#weather-table").delay(500).slideDown(200);
    
    getWeather(zip);
});

function getWeather(getZip) {
    var urlf = "http://api.wunderground.com/api/76c3fb77841aa97d/forecast/q/" + getZip + ".json";
    var resultForecast;

    //Put tasks to be done using the received weather data within these functions.
    $.getJSON(urlf, function(jd) {
        //$("#weather-spinner").hide();
        $("#weather-table").slideDown(300);
        $('#result').html(jd); 
        resultForecast = jd;

        updateTextFore(resultForecast);
    });

    var urlc = "http://api.wunderground.com/api/76c3fb77841aa97d/conditions/q/" + getZip + ".json";
    var resultConditions;

    //Put tasks to be done using the received weather data within these functions.
    $.getJSON(urlc, function(jd) {
        $('#result').html(jd); 
        resultConditions = jd;

        //For some reason, the variables assigned within this function are not defined outside of it
        updateTextCond(resultConditions);
    });
}

function updateTextFore(resultForecast) {
    if (typeof resultForecast.forecast === 'undefined') {
        return;
    }

    var shortFore = resultForecast.forecast.simpleforecast

    //Date
    $("#Day1").text(shortFore.forecastday[0].date.weekday + " - " + shortFore.forecastday[0].date.month + "/" + shortFore.forecastday[0].date.day + "/" + shortFore.forecastday[0].date.year);
    $("#Day2").text(shortFore.forecastday[1].date.weekday + " - " + shortFore.forecastday[1].date.month + "/" + shortFore.forecastday[1].date.day + "/" + shortFore.forecastday[1].date.year);
    $("#Day3").text(shortFore.forecastday[2].date.weekday + " - " + shortFore.forecastday[2].date.month + "/" + shortFore.forecastday[2].date.day + "/" + shortFore.forecastday[2].date.year);
    $("#Day4").text(shortFore.forecastday[3].date.weekday + " - " + shortFore.forecastday[3].date.month + "/" + shortFore.forecastday[3].date.day + "/" + shortFore.forecastday[3].date.year);

    //Conditions
    $("#Day1Cond").text(shortFore.forecastday[0].conditions);
    $("#Day2Cond").text(shortFore.forecastday[1].conditions);
    $("#Day3Cond").text(shortFore.forecastday[2].conditions);
    $("#Day4Cond").text(shortFore.forecastday[3].conditions);

    //Icon
    $("#Day1Icon").attr('src', shortFore.forecastday[0].icon_url);
    $("#Day2Icon").attr('src', shortFore.forecastday[1].icon_url);
    $("#Day3Icon").attr('src', shortFore.forecastday[2].icon_url);
    $("#Day4Icon").attr('src', shortFore.forecastday[3].icon_url);


    //High-Low
    $("#Day1HighLow").text("High: " + shortFore.forecastday[0].high.fahrenheit + "°F Low: " + shortFore.forecastday[0].low.fahrenheit + "°F");
    $("#Day2HighLow").text("High: " + shortFore.forecastday[1].high.fahrenheit + "°F Low: " + shortFore.forecastday[1].low.fahrenheit + "°F");
    $("#Day3HighLow").text("High: " + shortFore.forecastday[2].high.fahrenheit + "°F Low: " + shortFore.forecastday[2].low.fahrenheit + "°F");
    $("#Day4HighLow").text("High: " + shortFore.forecastday[3].high.fahrenheit + "°F Low: " + shortFore.forecastday[3].low.fahrenheit + "°F");

    //Estimated Precipitation
    $("#EstPrecip1").text("Est. Precipitation: " + shortFore.forecastday[0].qpf_allday.in + '"');
    $("#EstPrecip2").text("Est. Precipitation: " + shortFore.forecastday[1].qpf_allday.in + '"');
    $("#EstPrecip3").text("Est. Precipitation: " + shortFore.forecastday[2].qpf_allday.in + '"');
    $("#EstPrecip4").text("Est. Precipitation: " + shortFore.forecastday[3].qpf_allday.in + '"');

}
function updateTextCond(resultConditions) {
    if (typeof resultConditions.current_observation !== 'undefined') {
        $("#title").text("Weather for " + resultConditions.current_observation.display_location.full);
    }
}