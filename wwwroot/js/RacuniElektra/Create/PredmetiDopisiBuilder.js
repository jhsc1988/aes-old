function drawSelectDopisOptions() {
    let resultFlag = false;
    $('#selectDopis').find("option").remove().end();
    $("#selectDopis").append($('<option>', {
        value: 0,
        text: "Dopis",
    }));
    // u value atribut od option stavljam ID od predmeta
    $.each(dop, function (i, item) {
        if ($("#selectPredmet :selected").val() == item.PredmetId) {
            $('#selectDopis').removeAttr('disabled');
            $("#selectDopis").append($('<option>', {
                value: item.Id,
                text: item.Urbroj,
            }));
            resultFlag = true;
        }
    });
    if (!resultFlag) {
        $('#selectDopis').find("option").remove().end();
        $("#selectDopis").append($('<option>', {
            value: 0,
            text: "Ovaj predmet nema niti jedan dopis"
        }));
        $('#selectDopis').attr('disabled', 'disabled');
        data_dopis = 0;
    }
}

function GetPredmetiData() {
    $.ajax({
        type: "POST",
        url: "/RacuniElektra/GetPredmeti",
        success: function (predmeti) {
            pred = JSON.parse(predmeti);
        }
    });
}

function GetDopisiData() {
    $.ajax({
        type: "POST",
        url: "/RacuniElektra/GetDopisi",
        success: function (dopisi) {
            dop = JSON.parse(dopisi);
        }
    });
}

function drawSelectPredmetOptions() {
    $('#selectPredmet').find("option").remove().end();
    $("#selectPredmet").append($('<option>', {
        value: 0,
        text: "Predmet",
    }));
    $.each(pred, function (i, item) {
        $("#selectPredmet").append($('<option>', {
            value: item.Id,
            text: item.Klasa
        }));
    });
}