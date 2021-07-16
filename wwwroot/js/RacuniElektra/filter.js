let predmetiForFilter;
let dopisiForFilter;

const selectPredmet = $("#selectPredmet");
const selectDopis = $("#selectDopis");

getPredmetiData();
drawSelectPredmetOptions();

// get filtered data
function refreshWithFilteredData() {
    $('#RacunElektraTable').DataTable().ajax.reload();
}
function setDopisiForFilterCallBack(val) {
    dopisiForFilter = val;
}
selectPredmet.change(function () {
    if( selectPredmet.val()==="0"){
        refreshWithFilteredData()
    }
    getDopisiData();
});
selectDopis.change(function () {
    refreshWithFilteredData();
});
selectPredmet.on('mousedown keypress', function () {
    getPredmetiData();
    drawSelectPredmetOptions();
})
function getPredmetiData() {
    $.ajax({
        type: "POST",
        url: "/RacuniElektra/GetPredmetiDataForFilter",
        success: function (predmeti) {
            predmetiForFilter = predmeti;
        }
    });
}
function getDopisiData() {
    $.ajax({
        type: "POST",
        url: "/RacuniElektra/GetDopisiDataForFilter",
        data: {
            predmetId: $('#selectPredmet').val(),
        },
        success: function (dopisi) {
            setDopisiForFilterCallBack(dopisi);
            refreshWithFilteredData();
            drawSelectDopisOptions();
        }
    });
}
function drawSelectPredmetOptions() {
    selectPredmet.find("option").remove().end();
    selectPredmet.append($('<option>', {
        value: 0,
        text: "Predmet",
    }));

    $.each(predmetiForFilter, function (i, item) {
        selectPredmet.append($('<option>', {
            value: item.id,
            text: item.klasa
        }));
    });
}
function drawSelectDopisOptions() {
    selectDopis.find("option").remove().end();
    selectDopis.append($('<option>', {
        value: 0,
        text: "Dopis",
    }));
    $.each(dopisiForFilter, function (i, item) {
        selectDopis.removeAttr('disabled');
        selectDopis.append($('<option>', {
            // u value atribut od option stavljam ID od predmeta
            value: item.id,
            text: item.urbroj,
        }));
    })
}