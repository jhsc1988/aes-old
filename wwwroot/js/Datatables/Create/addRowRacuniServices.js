// ************************************ add row ************************************ //

$('#btnAdd').on('click', function () {
    AddNew(brojRacuna, $("#iznos").val(), $("#datumIzdavanja").val(), $("#datumIzvrsenja").val(), $("#usluga").val());
    table.row.add(["<td><button type='button' class='remove btn btn-outline-secondary btn-sm border-danger'><i class='bi bi-x'></i></button ></td >"]).draw();
});