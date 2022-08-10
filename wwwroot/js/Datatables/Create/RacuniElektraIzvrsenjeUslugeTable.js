$(document).ready(function () {

    const selectIndexTable = $('#IndexTable');

    table = selectIndexTable.DataTable({
        "ajax": {
            "url": "/RacuniElektraIzvrsenjeUsluge/GetList",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
            }
        },
        "columns": [
            { "data": "redniBroj", "name": "redniBroj" },
            {
                "data": null, "name": "brojRacuna",
                "render": function (data) {
                    return '<a href="../../../RacuniElektraIzvrsenjeUsluge/Edit/' + data.id + '">' + data.brojRacuna + '</a>';
                }
            },
            {
                "data": null, "name": "elektraKupac.ods.stan.stanId",
                "render": function (data) {
                    if (data.elektraKupac == null || data.elektraKupacId == 2002)
                        return "";
                    return '<a href="../../../Stanovi/Details/' + data.elektraKupac.ods.stan.id + '">' + data.elektraKupac.ods.stan.stanId + '</a>';
                }
            },
            {
                "data": null, "name": "elektraKupac.ods.stan.adresa",
                "render": function (data) {
                    if (data.elektraKupac == null || data.elektraKupacId == 2002)
                        return "";
                    return data.elektraKupac.ods.stan.adresa;
                }
            },
            {
                "data": null, "name": "elektraKupac.ods.stan.korisnik",
                "render": function (data) {
                    if (data.elektraKupac == null || data.elektraKupacId == 2002)
                        return "";
                    return data.elektraKupac.ods.stan.korisnik;
                }
            },
            {
                "data": null, "name": "elektraKupac.ods.stan.vlasništvo",
                "render": function (data) {
                    if (data.elektraKupac == null || data.elektraKupacId == 2002)
                        return "";
                    return data.elektraKupac.ods.stan.vlasništvo;
                }
            },
            { "data": "datumIzdavanja", "name": "datumIzdavanja" },
            { "data": "datumIzvrsenja", "name": "datumIzvrsenja" },
            { "data": "usluga", "name": "usluga" },
            {
                "data": "iznos", "name": "iznos",
                //"render": $.fn.dataTable.render.number('.', ',', 2, '')
            },
            { "data": "napomena", "name": "napomena" },
            { "data": null, "name": "akcija" },
        ],
        "paging": true,
        "serverSide": true,
        "order": [[0, 'asc']], // default sort po datumu
        "bLengthChange": false,

        //"processing": true,
        "language": {
            "processing": "tražim...",
            "search": "", // remove search text
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // rbr
                "render": $.fn.dataTable.render.ellipsis(3),
            },
            {
                "targets": 1, // BrojRacuna
                "render": $.fn.dataTable.render.ellipsis(19),
                "orderable": false,
            },
            {
                "targets": 2, // Stan ID
                "render": $.fn.dataTable.render.ellipsis(5),
                "orderable": false,
            },
            {
                "targets": 3, // Adresa
                "render": $.fn.dataTable.render.ellipsis(34),
                "orderable": false,
            },
            {
                "targets": 4, // Korisnik
                "render": $.fn.dataTable.render.ellipsis(20),
                "orderable": false,
            },
            {
                "targets": 5, // Vlasništvo
                "render": $.fn.dataTable.render.ellipsis(10),
                "orderable": false,
            },
            {
                "targets": 6, // Datum izdavanja
                "render": function (data, type, row) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 7, // Datum izvršenja
                "render": function (data, type, row) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 8, // Usluga
                "render": $.fn.dataTable.render.ellipsis(10),
                "orderable": false,
            },
            {
                "targets": 9, // Iznos
                "render": $.fn.dataTable.render.number('.', ',', 2, '', ' kn'),
            },
            {
                "targets": 10, // Napomena
                "render": $.fn.dataTable.render.ellipsis(28),
            },
            {
                "targets": 11, // remove
                "orderable": false,
                "searchable": false,
                "defaultContent": "<button type='button' class='button-add-remove' id='remove'><i class='bi bi-x'></i>briši</button>"
            },
            {
                // if no data in JSON (for null references)
                "defaultContent": "",
                "targets": "_all"
            }
        ],
    });
});

