// ************************************ save to db ************************************ //

$("#btnSave").on("click", function () {
    let table = $('#indexTable').DataTable()
    let data = table.rows().data(); // javascript object
    let racuni = []; // empty array
    let racun = {}; // empty javascript object
    if (verifyData(data)) {
        getKupciData();
        $.each(kup, function (i, item) {
            if (item.UgovorniRacun === ugovorniRacun)
                data_elektraKupacId = item.Id; // bind this ugovorniRacun to variable for later use
        });
        $.each(data, function (i) {
            racun.BrojRacuna = data[i][1];
            racun.ElektraKupacId = data_elektraKupacId; // this
            racun.DatumIzdavanja = data[i][7];
            racun.Iznos = data[i][8];
            racun.DopisId = data_dopis;
            racun.RedniBroj = i + 1;
            racuni.push(racun);
        });
        // TODO: check if racun vec postoji
        // TODO: truncate all if failed u bazi
        // TODO: savetodb bi trebala biti async
        $.ajax({
            type: "POST",
            url: "/RacuniElektra/SaveToDB",
            data: JSON.stringify(racuni),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                alert(r.message);
                table.clear().draw();
            },
            error: function (r) {
                alert(r.message);
            }
        });
    }
});

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