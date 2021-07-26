let data_stanId;
let data_adresa;
let data_kat;
let data_brojstana;
let data_cetvrt;
let data_povrsina;
let data_statuskoristenja;
let data_korisnik;
let data_vlasnistvo;

function GetStanData(stanid) {
    $.ajax({
        type: "POST",
        url: "/Ods/GetStanData",
        data: { stanid: stanid },
        success: function (stan) {
            data_stanId = stan.stanId;
            data_adresa = stan.adresa;
            data_kat = stan.kat;
            data_brojstana = stan.brojSTana;
            data_cetvrt = stan.četvrt;
            data_povrsina = stan.površina;
            data_statuskoristenja = stan.statusKorištenja;
            data_korisnik = stan.korisnik;
            data_vlasnistvo = stan.vlasništvo;

            $('#stanText').html('<span style="font-weight:bold;"> ID: </span>' + data_stanId + ' '
                + '<span style="font-weight:bold;">adresa: </span>' + data_adresa + ' '
                + '<span style="font-weight:bold;">kat: </span>' + data_kat + ' '
                + '<span style="font-weight:bold;">broj stana: </span>' + data_brojstana + ' '
                + '<span style="font-weight:bold;">četvrt: </span>' + data_cetvrt + ' '
                + '<span style="font-weight:bold;">površina: </span>' + data_povrsina + ' '
                + '<span style="font-weight:bold;">status korištenja: </span>' + data_statuskoristenja + ' '
                + '<span style="font-weight:bold;">korisnik: </span>' + data_korisnik + ' '
                + '<span style="font-weight:bold;">vlasništvo: </span>' + data_vlasnistvo + ' '
            )
        }
    });
}

$("#StanId").on("change", function () {
    let i = $('#StanId').val();
    GetStanData(i);
});