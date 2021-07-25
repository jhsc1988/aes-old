const isItForFilter = true;

const GetPredmetiDataForFilterUrl = "/RacuniElektra/GetPredmetiDataForFilter";
const GetDopisiDataForFilterUrl = "/RacuniElektra/GetDopisiDataForFilter";

$("#selectDopis").on('change', function () {
    refreshWithFilteredData();
});

$("#selectPredmet").on('change', function () {
    refreshWithFilteredData();
});

function refreshWithFilteredData() {
    $('#RacunElektraTable').DataTable().ajax.reload();
}
