// ************************************ add new ************************************ //

function AddNew(brojRacuna, iznos, _datum) {
    $.ajax({
        type: "POST",
        url: "AddNewTemp",
        data: {
            brojRacuna: brojRacuna,
            iznos: iznos,
            date: _datum,
        },
        success: function (r) {
            if (r.success) {
                alertify.success(r.message);

            } else {
                alertify.error(r.message);
            }
            resetInput();
            table.ajax.reload(null, false);
        },
        error: function (r) {
            alertify.error(r.message);
            table.ajax.reload(null, false);
        }
    });
}

function resetInput() {
    $('#brojRacuna').val("");
    $('#datumIzdavanja').val("");
    $('#iznos').val("");
    $('#stanText').html("");
}