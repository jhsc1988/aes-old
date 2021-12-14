function getData() {
    GetCustomersData();
    $.each(kup, function (key, value) {
        brojRacuna = $("#brojRacuna").val();
        ugovorniRacun = brojRacuna.substr(0, 10);
        if (ugovorniRacun !== "" && ugovorniRacun == value.UgovorniRacun) {

            // get data
            data_stanId = value.Ods.Stan.StanId;
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
            $('#stanText').html('<span class="stan-text"> ID: </span>' + data_stanId + ' '
                + '<span class="stan-text">adresa: </span>' + data_adresa + ' '
                + '<span class="stan-text">kat: </span>' + data_kat + ' '
                + '<span class="stan-text">broj stana: </span>' + data_brojstana + ' '
                + '<span class="stan-text">četvrt: </span>' + data_cetvrt + ' '
                + '<span class="stan-text">površina: </span>' + data_povrsina + ' '
                + '<span class="stan-text">status korištenja: </span>' + data_statuskoristenja + ' '
                + '<span class="stan-text">korisnik: </span>' + data_korisnik + ' '
                + '<span class="stan-text">vlasništvo: </span>' + data_vlasnistvo + ' ')
            return false;
        } else {
            $('#stanText').html('');
        }
    });
}

$("#brojRacuna").on("change focusin focusout", function () {
    getData();
});

function GetCustomersData() {
    $.ajax({
        type: "POST",
        url: GetCustomersDataUrl,
        success: function (kupci) {
            kup = JSON.parse(kupci);
        }
    });
}
