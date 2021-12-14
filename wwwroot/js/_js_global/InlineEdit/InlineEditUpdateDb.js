/**
* Saves data to DB by Ajax POST method
* (racun = 1, datumIzdavanja = 2, iznos = 3, klasa = 4, datumPotvrde = 5, napomena = 6, datumIzdavanja = 7, usluga = 8)
* @param {any} updatedColumn Number of column (as defined in Column enum in Controller)
* @param {any} input edited text
*/
function updateDb(updatedColumn, input) {
    $.ajax({
        type: "POST",
        url: updateDbUrl,
        data: {
            id: racunId, // racun id
            updatedColumn: updatedColumn,
            x: input,
        },
        success: function (r) {
            if (r.success) {
                //alertify.alert("This is an alert dialog.", function () { alertify.message('OK'); });
                alertify.warning(r.message);
            } else {
                alertify.error(r.message);
                table.ajax.reload(null, false); // user paging is not reset on reload(callback, resetPaging)
            }
        },
    });
}