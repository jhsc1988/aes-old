function AddNew(brojRacuna, iznos, _datum, _guid) {
    $.ajax({
        type: "POST",
        url: "/RacuniElektra/AddNewTemp",
        data: {
            brojRacuna: brojRacuna,
            iznos: iznos,
            date: _datum,
            __guid: _guid,
        },
        success: function (r) {
            if (r.success) {
                alertify.success(r.message);
                //table.ajax.reload(null, false);
            } else {
                alertify.error(r.message);
                //table.ajax.reload(null, false); // user paging is not reset on reload(callback, resetPaging)
            }
        },
    });
}
