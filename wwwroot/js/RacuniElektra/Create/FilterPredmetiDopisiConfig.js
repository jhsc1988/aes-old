const isItForFilter = false;
const GetPredmetiDataForFilterUrl = "/RacuniElektra/GetPredmetiCreate";
const GetDopisiDataForFilterUrl = "/RacuniElektra/GetDopisiCreate";

$("#selectDopis").change(function () {
    data_dopis = $("#selectDopis :selected").val();
});
// ************************************ broj racuna ************************************ //
$("#brojRacuna").on("change focusin focusout", function () {
    getData();
});