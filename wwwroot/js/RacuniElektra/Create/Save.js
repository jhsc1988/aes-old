// ************************************ save to db ************************************ //

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


// ************************************ functions ************************************ //
function verifyData(data) {
    if (data.length == 0) {
        alert("u tablici nema podataka");
        return false;
    }
    let funRes = false;
    for (i = 0; i < data.length; ++i) {
        if (data[i][2] == "") {
            alert("U tablici ima stanova bez ID-a !");
            return false;
        }
        if (data_dopis == 0) {
            alert("Nije odabran dopis !");
            return false;
        }
        if (data[i][8] == "") {
            alert("U tablici ima računa bez iznosa !");
            return false;
        }
        funRes = true;
    }
    return funRes;
}