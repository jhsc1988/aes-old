// for inline editing
let table;

$(document).ready(function () {

    table = $('#IndexTable').DataTable({
        "ajax": {
            "url": "/RacunElektraIzvrsenjaUsluges/GetList",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.klasa = $("#selectPredmet").val();
                d.urbroj = $("#selectDopis").val();
            }
        },
        "columns": [
            { "data": "id", "name": "id" },
            { "data": "redniBroj", "name": "redniBroj" },
            {
                "data": null, "name": "brojRacuna",
                "render": function (data) {
                    return '<a href="RacunElektraIzvrsenjaUsluges/Details/' + data.id + '">' + data.brojRacuna + '</a>';
                },
            },
            {
                "data": null, "name": "elektraKupac.ugovorniRacun",
                "render": function (data) {
                    if (data.elektraKupac != null)
                        return '<a href="ElektraKupci/Details/' + data.elektraKupac.id + '">' + data.elektraKupac.ugovorniRacun + '</a>';
                    return '';
                }
            },
            { "data": "datumIzdavanja", "name": "datumIzdavanja" },
            { "data": "datumIzvrsenja", "name": "datumIzvrsenja" },
            { "data": "usluga", "name": "usluga" },
            {
                "data": "iznos", "name": "iznos",
                //"render": $.fn.dataTable.render.number('.', ',', 2, '')
            },
            { "data": "klasaPlacanja", "name": "klasaPlacanja" },
            { "data": "datumPotvrde", "name": "datumPotvrde" },
            { "data": "napomena", "name": "napomena" },
            { "data": null, "name": null }, // akcija
        ],
        "paging": true,
        "serverSide": true,
        "order": [[2, 'asc']], // default sort po datumu
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim...",
            "search": "", // remove search text
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // id - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 1, // Redni broj
                "render": $.fn.dataTable.render.ellipsis(3),
            },
            {
                "targets": 2, // BrojRacuna
                "render": $.fn.dataTable.render.ellipsis(19),
            },
            {
                "targets": 3, // UgovorniRacun
                "render": $.fn.dataTable.render.ellipsis(10),
            },
            {
                "targets": 4, // DatumIzdavanja
                "render": function (data) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 5, // DatumIzvrsenja
                "render": function (data) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 6, // Usluga
                "render": $.fn.dataTable.render.ellipsis(30),
            }, {
                "targets": 7, // Iznos
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 8, // KlasaPlacanja
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 9, // Datum potvrde
                "render": function (data) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 10, // Napomena
                "render": $.fn.dataTable.render.ellipsis(30),
            },
            {
                "targets": 11, // akcija - hidden
                "visible": false,
                "searchable": false,
                "defaultContent": "<button type='button' class='button-add-remove' id='remove'><i class='bi bi-x'></i>briši</button >"
            },
        ]
    });
});