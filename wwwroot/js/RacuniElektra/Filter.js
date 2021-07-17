$(document).ready(function () {
// ************************************ variables ************************************ //
    let predmetiForFilter;
    let dopisiForFilter;
    const selectPredmet = $("#selectPredmet");
    const selectDopis = $("#selectDopis");
// ************************************ init ************************************ //
    getPredmetiData();

// ************************************ get filtered data ************************************ //
    function refreshWithFilteredData() {
        $('#RacunElektraTable').DataTable().ajax.reload();
    }

// ************************************ set dopisi callback for jQuery ************************************ //
    function setDopisiForFilterCallBack(val) {
        dopisiForFilter = val;
    }

// ************************************ predmeti ************************************ //
    function getPredmetiData() {
        $.ajax({
            type: "POST",
            url: "/RacuniElektra/GetPredmetiDataForFilter",
            success: function (predmeti) {
                predmetiForFilter = predmeti;
                drawSelectPredmetOptions();
            }
        });
    }

    function drawSelectPredmetOptions() {
        selectPredmet.append($('<option>', {
            value: 0,
            text: "Predmet",
        }));
        $.each(predmetiForFilter, function (i, item) {
            selectPredmet.append($('<option>', {
                value: item.id, // PredmetId
                text: item.klasa // Naziv
            }));
        });
    }

// ************************************ dopisi ************************************ //
    function getDopisiData() {
        $.ajax({
            type: "POST",
            url: "/RacuniElektra/GetDopisiDataForFilter",
            data: {
                predmetId: $('#selectPredmet').val(), // in val is PredmetId
            },
            success: function (dopisi) {
                setDopisiForFilterCallBack(dopisi);
                refreshWithFilteredData();
                drawSelectDopisOptions();
            }
        });
    }

    function drawSelectDopisOptions() {
        selectDopis.find("option").remove().end();
        selectDopis.append($('<option>', {
            value: 0,
            text: "Dopis",
        }));
        // if Predmet is selected (if array is empty), disable Dopisi 
        if (dopisiForFilter.length === 0) {
            selectDopis.attr('disabled', 'disabled');
        } else {
            $.each(dopisiForFilter, function (i, item) {
                selectDopis.removeAttr('disabled');
                selectDopis.append($('<option>', {
                    // u value atribut od option stavljam ID od predmeta
                    value: item.id,
                    text: item.urbroj,
                }));
            })
        }
    }

// ************************************ event handlers ************************************ //
    selectPredmet.on('change', function () {
        if (selectPredmet.val() === "0") // if Predmet is selected, reset
            drawSelectDopisOptions(); // 1. removes options, then 2. getDopisiData(); send null data
        getDopisiData();
    });
    selectDopis.on('change', function () {
        refreshWithFilteredData();
    });
});