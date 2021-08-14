function GetStanData(sid) {
    $.ajax({
        type: "POST",
        url: "/Ods/GetStanData",
        data: { sid: sid },
        success: function (stan) {

            const data_stanId = stan.stanId !== null ? stan.stanId : "-";
            const data_adresa = stan.adresa !== null ? stan.adresa : "-";
            const data_kat = stan.kat !== null ? stan.kat : "-";
            const data_brojstana = stan.brojSTana !== null ? stan.brojSTana : "-";
            const data_cetvrt = stan.četvrt !== null ? stan.četvrt : "-";
            const data_povrsina = stan.površina !== null ? stan.površina : "-";
            const data_statuskoristenja = stan.statusKorištenja !== null ? stan.statusKorištenja : "-";
            const data_korisnik = stan.korisnik !== null ? stan.korisnik : "-";
            const data_vlasnistvo = stan.vlasništvo !== null ? stan.vlasništvo : "-";

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