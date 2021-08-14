// ************************************ add row ************************************ //

$('#btnAdd').on('click', function () {
    AddNew(brojRacuna, $("#iznos").val(), $("#datumIzdavanja").val(), data_dopis);
    table.row.add(["<td><button type='button' class='remove btn btn-outline-secondary btn-sm border-danger'><i class='bi bi-x'></i></button ></td >"]).draw();
});

// ************************************ remove row ************************************ //

$('#IndexTable').on('click', '#remove',  function () {
    console.log('bu');
    var racunId = table.row($(this).parents('tr')).data().id;
    $.ajax({
        type: "POST",
        url: RemoveRowUrl,
        data: { racunId: racunId },
        success: function (r) {
            if (r.success) {
                alertify.success(r.message);
                table.ajax.reload(null, false);
            }
        },
        error: function (r) {
            table.ajax.reload(null, false);
        }
    });
});

// ************************************ save ************************************ //

$("#btnSave").on("click", function () {
    $.ajax({
        type: "POST",
        url: SaveToDBUrl,
        data: {
            _dopisid: data_dopis,
        },
        success: function (r) {
            if (r.success) {
                alertify.success(r.message);
            } else {
                alertify.error(r.message);
            }
            table.ajax.reload(null, false); // user paging is not reset on reload(callback, resetPaging)
        },
        error: function (r) {
            alertify.error(r.message);
            table.ajax.reload(null, false);
        }
    });
})

// ************************************ delete ************************************ //

$("#btnDelete").on("click", function () {
    $.ajax({
        type: "POST",
        url: RemoveAllFromDbUrl,
        success: function (r) {
            if (r.success) {
                alertify.success(r.message);
            } else {
                alertify.error(r.message);
            }
            table.ajax.reload(null, false);
        },
        error: function (r) {
            alertify.error(r.message);
            table.ajax.reload(null, false);
        }
    });
})

// ************************************ functions ************************************ //

function AddNew(brojRacuna, iznos, _datum, dopisId) {
    $.ajax({
        type: "POST",
        url: AddNewTempUrl,
        data: {
            brojRacuna: brojRacuna,
            iznos: iznos,
            date: _datum,
            dopisId: dopisId,
        },
        success: function (r) {
            if (r.value.success) {
                alertify.success(r.value.message);
                resetInput();

            } else {
                alertify.error(r.value.message);
            }
            table.ajax.reload(null, false); // user paging is not reset on reload(callback, resetPaging)
        },
    });
}

function resetInput() {
    $('#brojRacuna').val("");
    $('#datumIzdavanja').val("");
    $('#iznos').val("");
    $('#stanText').html("");
}
