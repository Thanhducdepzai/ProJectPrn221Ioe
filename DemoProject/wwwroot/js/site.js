"use strict";
var con = new signalR.HubConnectionBuilder().withUrl("/hub").build();

con.on("ReceiveDistrictOptions", function (districts) {
    console.log("Received districts:", districts);
    const districtSelect = document.getElementById("districtSelect");
    districtSelect.innerHTML = '<option value="">Select District</option>';
    districts.forEach(function (district) {
        const option = document.createElement("option");
        option.value = district.districtId;
        option.text = district.districtName;
        districtSelect.appendChild(option);
    });
});

con.start().then(function () {
    console.log("SignalR Connected.");
}).catch(function (err) {
    return console.error("SignalR connection error:", err.toString());
});

function ProvinceChanged() {
    const province = document.getElementById("ProvinceId").value;
    console.log("ProvinceChanged triggered. ProvinceId:", province);
    if (province) {
        con.invoke("SendPlaceOptions", parseInt(province)).catch(function (err) {
            return console.error("Error invoking SendPlaceOptions:", err.toString());
        });
    }
}
