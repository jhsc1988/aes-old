// ************************************ add new ************************************ //


function AddNew(brojRacuna, iznos, _datum, datumIzvrsenja, usluga) {
    $.ajax({
        type: "POST",
        url: "AddNewTemp",
        data: {
            brojRacuna: brojRacuna,
            iznos: iznos,
            date: _datum,
            datumIzvrsenja: datumIzvrsenja,
            usluga: usluga
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
    $('#usluga').val("");
    $('#datumIzvrsenja').val("");
    $('#stanText').html("");
}