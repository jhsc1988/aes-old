const isItForFilter = false;
const GetPredmetiDataForFilterUrl = "/RacuniElektra/GetPredmetiCreate";
const GetDopisiDataForFilterUrl = "/RacuniElektra/GetDopisiCreate";

$("#selectDopis").change(function () {
    data_dopis = $("#selectDopis :selected").val();
    refreshWithFilteredData();
});


function refreshWithFilteredData() {
    $('#RacunElektraTable').DataTable().ajax.reload();
}

