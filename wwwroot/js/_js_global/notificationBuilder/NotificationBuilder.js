function getData() {
    GetKupciData();
    $.each(kup, function (key, value) {
        brojRacuna = $("#brojRacuna").val();
        ugovorniRacun = brojRacuna.substr(0, 10);
        if ($('#brojRacuna').val() == value.UgovorniRacun) {
            // get data
            data_stanId = value.Ods.StanId;
            data_adresa = value.Ods.Stan.Adresa;
            data_kat = value.Ods.Stan.Kat;
            data_brojstana = value.Ods.Stan.BrojSTana;
            data_cetvrt = value.Ods.Stan.Četvrt;
            data_povrsina = value.Ods.Stan.Površina;
            data_statuskoristenja = value.Ods.Stan.StatusKorištenja;
            data_korisnik = value.Ods.Stan.Korisnik;
            data_vlasnistvo = value.Ods.Stan.Vlasništvo;
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