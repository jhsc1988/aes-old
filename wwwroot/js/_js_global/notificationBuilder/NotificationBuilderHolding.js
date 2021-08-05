function getData() {
    GetKupciData();
    $.each(kup, function (key, value) {
        brojRacuna = $("#brojRacuna").val();
        sifraObjekta = brojRacuna.substr(0, 8);
        if ($('#brojRacuna').val() == sifraObjekta) {
            // get data
            data_stanId = value.StanId;
            data_adresa = value.Adresa;
            data_kat = value.Kat;
            data_brojstana = value.BrojSTana;
            data_cetvrt = value.Četvrt;
            data_povrsina = value.Površina;
            data_statuskoristenja = value.StatusKorištenja;
            data_korisnik = value.Korisnik;
            data_vlasnistvo = value.Vlasništvo;
            data_elektraKupacId = value.Id;
            // #stanText string builder
            $('#stanText').html('<span style="font-weight:bold;"> ID: </span>' + data_stanId + ' '
                + '<span style="font-weight:bold;">adresa: </span>' + data_adresa + ' '
                + '<span style="font-weight:bold;">kat: </span>' + data_kat + ' '
                + '<span style="font-weight:bold;">broj stana: </span>' + data_brojstana + ' '
                + '<span style="font-weight:bold;">četvrt: </span>' + data_cetvrt + ' '
                + '<span style="font-weight:bold;">površina: </span>' + data_povrsina + ' '
                + '<span style="font-weight:bold;">status korištenja: </span>' + data_statuskoristenja + ' '
                + '<span style="font-weight:bold;">korisnik: </span>' + data_korisnik + ' '
                + '<span style="font-weight:bold;">vlasništvo: </span>' + data_vlasnistvo + ' ')
            return false;
        } else {
            $('#stanText').html('');
        }
    });
}

$("#brojRacuna").on("change focusin focusout", function () {
    getData();
});

function GetKupciData() {
    $.ajax({
        type: "POST",
        url: GetKupciDataUrl,
        success: function (kupci) {
            kup = JSON.parse(kupci);
        }
    });
}
