// ************************************ variables ************************************ //
const selectPredmet = $("#selectPredmet");
const selectDopis = $("#selectDopis");
const selectBrojRacuna = $("#brojRacuna");

// ************************************ predmet, dopis ************************************ //
selectPredmet.change(function () {
    drawSelectDopisOptions();
});
selectPredmet.on('mousedown keypress', function () {
    GetPredmetiData();
    GetDopisiData();
    drawSelectPredmetOptions();
})
selectDopis.on('mousedown keypress', function () {
    GetDopisiData();
    drawSelectDopisOptions();
})
selectDopis.change(function () {
    data_dopis = $("#selectDopis :selected").val();
});
// ************************************ broj racuna ************************************ //
selectBrojRacuna.on("change focusin focusout", function () {
    getData();
});