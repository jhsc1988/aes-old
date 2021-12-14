function getData() {
    GetCustomersData();
    $.each(kup, function (key, value) {
        brojRacuna = $("#brojRacuna").val();
        sifraObjekta = brojRacuna.substr(0, 8);
        if (sifraObjekta !== "" && sifraObjekta == value.SifraObjekta) {

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
