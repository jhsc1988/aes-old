
// ************************************ remove row ************************************ //

$('#IndexTable').on('click', '#remove', function () {
    var racunId = table.row($(this).parents('tr')).data().id;
    $.ajax({
        type: "POST",
        url: "RemoveRow",
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
        url: "SaveToDB",
        data: {
            _dopisid: data_dopis,
        },
        success: function (r) {
            if (r.success) {
                alertify.success(r.message);
            } else {
                alertify.error(r.message);
            }
            // user paging is not reset on reload(callback, resetPaging)
            table.ajax.reload(null, false);
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
        url: "RemoveAllFromDb",
        success: function (r) {
            if (r.success) {
                alertify.success(r.message);
            } else {
                alertify.error(r.message);
            }
            table.ajax.reload(null, false);
        },
        error: function (r) {
            alertify.error(r.value.message);
            table.ajax.reload(null, false);
        }
    });
})

// ************************************ refresh ************************************ //

$("#buttonRefresh").on("click", function () {
    $.ajax({
        type: "POST",
        url: "RefreshCustomers",
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