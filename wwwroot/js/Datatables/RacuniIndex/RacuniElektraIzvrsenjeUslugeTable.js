// for inline editing
let table;

$(document).ready(function () {

    table = $('#IndexTable').DataTable({

        // excel
        dom: 'frtipB',
        "buttons": [
            {
                "extend": 'excel',
                "className": 'excelButton',
                "text": '<i class="button-excel">Excel</i>',
                "titleAttr": 'Excel',
                "action": newexportaction,
                "exportOptions": {
                    columns: [1, 11, 12, 13, 14, 15, 2, 6, 7, 8]
                },
            }
        ],

        "ajax": {
            "url": "/RacuniElektraIzvrsenjeUsluge/GetList",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.isFilteredForIndex = true;
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
                    return '<a href="RacuniElektraIzvrsenjeUsluge/Details/' + data.id + '">' + data.brojRacuna + '</a>';
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
            { "data": "elektraKupac.ods.stan.stanId", "name": "elektraKupac.ods.stan.stanId" },
            { "data": "elektraKupac.ods.stan.adresa", "name": "elektraKupac.ods.stan.adresa" },
            { "data": "elektraKupac.ods.stan.kat", "name": "elektraKupac.ods.stan.kat" },
            { "data": "elektraKupac.ods.stan.brojSTana", "name": "elektraKupac.ods.stan.brojSTana" },
            { "data": "elektraKupac.ods.stan.povr\u0161ina", "name": "elektraKupac.ods.stan.Površina" },
            { "data": null, "name": null }, // akcija
        ],
        "paging": true,
        "serverSide": true,
        "order": [[4, 'desc']], // default sort po datumu
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
                "visible": false,
                "searchable": false,
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
                "render": $.fn.dataTable.render.number('.', ',', 2, '', ' kn'),
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
                "targets": 11, // stan id - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 12, // adresa - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 13, // kat - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 14, // broj stana - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 15, // površina - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 16, // akcija - hidden
                "visible": false,
                "searchable": false,
                "defaultContent": "<button type='button' class='button-add-remove' id='remove'><i class='bi bi-x'></i>briši</button >"
            },
        ]
    });
    excelButton = table.buttons(['.excelButton']);
    tableColumn = table.column(1);
});