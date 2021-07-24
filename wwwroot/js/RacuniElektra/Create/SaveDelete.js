// ************************************ save to temp db ************************************ //

$("#btnSave").on("click", function () {
    $.ajax({
        type: "POST",
        url: "/RacuniElektra/SaveToDB",
        data: {
            _dopisid: data_dopis,
        },
        success: function (r) {
            if (r.success) {
                alertify.success(r.message);
                table.ajax.reload(null, false); // user paging is not reset on reload(callback, resetPaging)
            } else {
                alertify.error(r.message);
                table.ajax.reload(null, false); 
            }
        },
        error: function (r) {
            alertify.error(r.message);
        }
    });
})

// ************************************ remove from temp db ************************************ //

$("#btnDelete").on("click", function () {
    $.ajax({
        type: "POST",
        url: "/RacuniElektra/RemoveAllFromDb",
        success: function (r) {
            if (r.success) {
                alertify.success(r.message);
                table.ajax.reload(null, false); // user paging is not reset on reload(callback, resetPaging)
            } else {
                alertify.error(r.message);
                table.ajax.reload(null, false);
            }
        },
        error: function (r) {
            alertify.error(r.message);
        }
    });
})