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

            $('#stanText').html('<span class="stan-text"> ID: </span>' + data_stanId + ' '
                + '<span class="stan-text">adresa: </span>' + data_adresa + ' '
                + '<span class="stan-text">kat: </span>' + data_kat + ' '
                + '<span class="stan-text">broj stana: </span>' + data_brojstana + ' '
                + '<span class="stan-text">četvrt: </span>' + data_cetvrt + ' '
                + '<span class="stan-text">površina: </span>' + data_povrsina + ' '
                + '<span class="stan-text">status korištenja: </span>' + data_statuskoristenja + ' '
                + '<span class="stan-text">korisnik: </span>' + data_korisnik + ' '
                + '<span class="stan-text">vlasništvo: </span>' + data_vlasnistvo + ' '
            )
        }
    });
}

function GetStanDataForKupci(odsId) {
    $.ajax({
        type: "POST",
        url: "/Ods/GetStanDataForOmm",
        data: { OdsId: odsId },
        success: function (ods) {

            const data_stanId = ods.stan.stanId !== null ? ods.stan.stanId : "-";
            const data_adresa = ods.stan.adresa !== null ? ods.stan.adresa : "-";
            const data_kat = ods.stan.kat !== null ? ods.stan.kat : "-";
            const data_brojstana = ods.stan.brojSTana !== null ? ods.stan.brojSTana : "-";
            const data_cetvrt = ods.stan.četvrt !== null ? ods.stan.četvrt : "-";
            const data_povrsina = ods.stan.površina !== null ? ods.stan.površina : "-";
            const data_statuskoristenja = ods.stan.statusKorištenja !== null ? ods.stan.statusKorištenja : "-";
            const data_korisnik = ods.stan.korisnik !== null ? ods.stan.korisnik : "-";
            const data_vlasnistvo = ods.stan.vlasništvo !== null ? ods.stan.vlasništvo : "-";

            $('#stanText').html('<span class="stan-text"> ID: </span>' + data_stanId + ' '
                + '<span class="stan-text">adresa: </span>' + data_adresa + ' '
                + '<span class="stan-text">kat: </span>' + data_kat + ' '
                + '<span class="stan-text">broj stana: </span>' + data_brojstana + ' '
                + '<span class="stan-text">četvrt: </span>' + data_cetvrt + ' '
                + '<span class="stan-text">površina: </span>' + data_povrsina + ' '
                + '<span class="stan-text">status korištenja: </span>' + data_statuskoristenja + ' '
                + '<span class="stan-text">korisnik: </span>' + data_korisnik + ' '
                + '<span class="stan-text">vlasništvo: </span>' + data_vlasnistvo + ' '
            )
        }
    });
}