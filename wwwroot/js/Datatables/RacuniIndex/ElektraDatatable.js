﻿// for inline editing
let table;

$(document).ready(function () {

    table = $('#IndexTable').DataTable({

        // excel
        dom: 'frtipB',
        "buttons": [
            {
                "extend": 'excel',
                "text": '<i class="button-excel">Excel</i>',
                "titleAttr": 'Excel',
                "action": newexportaction,
                "exportOptions": {
                    columns: [1, 2, 3, 4, 5, 6, 7, 10]
                },
            }
        ],

        "ajax": {
            "url": GetListUrl,
            "type": "POST",
            "datatype": "json",
            "data": function (d) { // callback za input change
                d.klasa = $("#selectPredmet").val();
                d.urbroj = $("#selectDopis").val();
            }
        },
        "columns": [
            { "data": "id", "name": "id" },
            { "data": "redniBroj", "name": "redniBroj" },
            { "data": "elektraKupac.ods.stan.stanId", "name": "elektraKupac.ods.stan.stanId" },
            { "data": "elektraKupac.ods.stan.adresa", "name": "elektraKupac.ods.stan.adresa" },
            { "data": "elektraKupac.ods.stan.kat", "name": "elektraKupac.ods.stan.kat" },
            { "data": "elektraKupac.ods.stan.brojSTana", "name": "elektraKupac.ods.stan.brojSTana" },
            { "data": "elektraKupac.ods.stan.povr\u0161ina", "name": "elektraKupac.ods.stan.Površina" },
            {
                "data": null, "name": "brojRacuna",
                "render": function (data) {
                    return '<a href="' + racunDetailsUrl + data.id + '">' + data.brojRacuna + '</a>';
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
                "targets": 2, // stan id - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 3, // adresa - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 4, // kat - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 5, // broj stana - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 6, // površina - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 7, // BrojRacuna
                "render": $.fn.dataTable.render.ellipsis(19),
            },
            {
                "targets": 8, // UgovorniRacun
                "render": $.fn.dataTable.render.ellipsis(10),
            },
            {
                "targets": 9, // DatumIzdavanja
                "render": function (data) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 10, // Iznos
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 11, // KlasaPlacanja
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 12, // Datum potvrde
                "render": function (data) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 13, // Napomena
                "render": $.fn.dataTable.render.ellipsis(30),
            },
            {
                "targets": 14, // akcija - hidden
                "visible": false,
                "searchable": false,
                "defaultContent": "<button type='button' class='button-add-remove' id='remove'><i class='bi bi-x'></i>briši</button >"
            },
        ],
    });
    column = table.column(14);
});
