let token = getCookie("token");
let doc = document;
$(document).ready(function () {

    $("#AddDevaceClick").click(function (event) {
        event.preventDefault();

        AddDevace();
    });

});

function GetDivace() {

    var header = "Bearer " + token;

    $.ajax({
        url: '/api/v1/devace',
        type: 'GET',
        beforeSend: function (xhrObj) {
            xhrObj.setRequestHeader("Authorization", header)
        },
        success: function (response) {
            console.log(response);
            ParseDevice(response);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function AddDevace() {

    var serialNumber = $('#AddDevaceInput').val();
    var header = "Bearer " + token;

    $.ajax({
        url: '/api/v1/devace',
        type: 'POST',
        beforeSend: function (xhrObj) {
            xhrObj.setRequestHeader("Authorization", header),
                xhrObj.setRequestHeader("serialNumber", serialNumber)
        },
        success: function (response) {
            alert("Device added");
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function ParseDevice(response) {

    var e = document.getElementById("deviceList");

    //e.firstElementChild can be used. 
    var child = e.lastElementChild;
    while (child) {
        e.removeChild(child);
        child = e.lastElementChild;
    }
    let device = JSON.parse(response);

    let index = Object.keys(device).length;
    for (var i = 0; i < index; i++) {
        Factory(device[i]);
    }

    setTimeout(10000, GetDivace());
  
}

function UpdateDevice() {

    
}

function Factory(model) {


    let obj = JSON.parse(model);

    switch (obj.t) {
        case 'Temperature':
            RenderTemperature(obj);
            break;
        case 'Soilmoisture':
            RenderSoilMoisture(obj);
            break;
        case 'Led':
            RenderLed(obj);
            break;
    }
}

function RenderSoilMoisture(SoilMoistureModel) {

    var list = doc.getElementById('deviceList');

    var div = doc.createElement('div');
    div.className = "col-md-4";
    var status;
    if (SoilMoistureModel.Status == "Offline")
        status = "h2 fw-bold float-right text-danger";
    if (SoilMoistureModel.Status == "Online")
        status = "h2 fw-bold float-right text-success";
    var element = "<div class='card'><div class='card-body pb-0'><div class='" + status + "'>" + SoilMoistureModel.Status + "</div><h2 class='mb-2'>" + SoilMoistureModel.Moisture + "</h2><p class='text-muted'>Soil moisture</p><div class='pull-in sparkline-fix'><div id='" + SoilMoistureModel.id + "'><canvas  width='384' height='70' style='display: inline-block; width: 384.925px; height:70px; vertical-align: top;'> </canvas></div></div></div></div>";
  
    div.onmouseover = function () {
        $("#" + SoilMoistureModel.id + "").sparkline([SoilMoistureModel.LogMoisture[0], SoilMoistureModel.LogMoisture[1], SoilMoistureModel.LogMoisture[2], SoilMoistureModel.LogMoisture[3], SoilMoistureModel.LogMoisture[4], SoilMoistureModel.LogMoisture[5], SoilMoistureModel.LogMoisture[6], SoilMoistureModel.LogMoisture[7], SoilMoistureModel.LogMoisture[8], SoilMoistureModel.LogMoisture[9]], {
            type: 'line',
            height: '70',
            width: '100%',
            lineWidth: '2',
            lineColor: 'rgba(255, 255, 255, .5)',
            fillColor: 'rgba(255, 255, 255, .15)'
        });
    };

        div.innerHTML = element;
        list.appendChild(div);
}


function RenderTemperature(TemperatureModel) {

    var list = doc.getElementById('deviceList');

    var div = doc.createElement('div');
    div.className = "col-md-4";
    var status;
    if (TemperatureModel.Status == "Offline")
        status = "h2 fw-bold float-right text-danger";
    if (TemperatureModel.Status == "Online")
        status = "h2 fw-bold float-right text-success";
    var element = "<div class='card'><div class='card-body pb-0'><div class='" + status + "'>" + TemperatureModel.Status + "</div><h2 class='mb-2'>" + TemperatureModel.Temp + " ℃ </h2><p class='text-muted'>Temperatura </p><div class='pull-in sparkline-fix'><div id='" + TemperatureModel.id + "'><canvas  width='384' height='70' style='display: inline-block; width: 384.925px; height:70px; vertical-align: top;'> </canvas></div></div></div></div>";

    div.onmouseover = function () {
        $("#" + TemperatureModel.id + "").sparkline([TemperatureModel.LogTemp[0], TemperatureModel.LogTemp[1], TemperatureModel.LogTemp[2], TemperatureModel.LogTemp[3], TemperatureModel.LogTemp[4], TemperatureModel.LogTemp[5], TemperatureModel.LogTemp[6], TemperatureModel.LogTemp[7], TemperatureModel.LogTemp[8], TemperatureModel.LogTemp[9]], {
            type: 'line',
            height: '70',
            width: '100%',
            lineWidth: '2',
            lineColor: 'rgba(255, 255, 255, .5)',
            fillColor: 'rgba(255, 255, 255, .15)'
        });
    };
  
    div.innerHTML = element;    
    list.appendChild(div);
}

function RenderLed(LedModel) {
    var list = doc.getElementById('deviceList');

    var div = doc.createElement('div');
    div.className = "col-md-4";
    var status;
    if (LedModel.Status == "Offline")
        status = "h2 fw-bold float-right text-danger";
    if (LedModel.Status == "Online")
        status = "h2 fw-bold float-right text-success";

    var power = false;
    if (LedModel.Power == "off")
        power = false;
    if (LedModel.Power == "on")
        power = true;

    var element = "<div class='card'><div class='card-body pb-0'><div class='" + status + "'>" + LedModel.Status + "</div><p class='text-muted'>Lamp</p><div style='margin-bottom: 10px;' ><input id=" + LedModel.id + " style='padding:0.15em; cursor:pointer' name='test' class='l' value=" + power + " type='checkbox'></div></div></div>";

    div.innerHTML = element;
    div.onclick = function clickHandler() {

        var ledId = doc.getElementById(LedModel.id);
        let check = ledId.checked;
        if (check == true)
            LedAction(LedModel.id, 'on');
        else LedAction(LedModel.id, 'off');
    };

    list.appendChild(div);
}

function LedAction(id, payload) {

    var header = "Bearer " + token;

    $.ajax({
        url: '/api/v1/topic/led/' + id,
        type: 'PUT',
        contentType: "application/json",
        data: JSON.stringify(payload),
        beforeSend: function (xhrObj) {
            xhrObj.setRequestHeader("Authorization", header)
        },
        success: function (response) {
            console.log(response);

        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}