const isItForFilter = true;

const GetPredmetiDataForFilterUrl = "/RacuniElektra/GetPredmetiDataForFilter";
const GetDopisiDataForFilterUrl = "/RacuniElektra/GetDopisiDataForFilter";

$("#selectDopis").on('change', function (e) {
    refreshWithFilteredData();

    e.preventDefault();
    var column = table.column(9);
    if (selectDopis.val() === "0" || selectDopis.val() === null) {
        column.visible(false);
        isItEditing = false;
    }
    else {
        column.visible(true);
        isItEditing = true;
    }



});

$("#selectPredmet").on('change', function (e) {
    refreshWithFilteredData();

    e.preventDefault();
    var column = table.column(9);
    if (selectDopis.val() === "0" || selectDopis.val() === null || column.visible() === true) {
        column.visible(false);
        isItEditing = false;
    }
    else {
        column.visible(true);
        isItEditing = true;
    }
});

function refreshWithFilteredData() {
    $('#RacunElektraTable').DataTable().ajax.reload();
}
