// ************************************ variables ************************************ //
const selectPredmet = $("#selectPredmet");
const selectDopis = $("#selectDopis");
const selectBrojRacuna = $("#brojRacuna");

// ************************************ predmet, dopis ************************************ //
selectPredmet.change(function () {
    drawSelectDopisOptions();
});
selectPredmet.on('mousedown keypress', function () {
    getPredmetiData();
    getDopisiData();
    drawSelectPredmetOptions();
})
selectDopis.on('mousedown keypress', function () {
    getDopisiData();
    drawSelectDopisOptions();
})
selectDopis.change(function () {
    data_dopis = $("#selectDopis :selected").val();
});
// ************************************ broj racuna ************************************ //
selectBrojRacuna.on("change focusin focusout", function () {
    getData();
});