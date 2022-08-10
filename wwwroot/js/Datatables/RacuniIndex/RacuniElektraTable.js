let table; // for inline editing

$(document).ready(function () {

    table = $('#IndexTable').DataTable({

        // excel
        dom: 'frtipB',
        buttons: [
            {
                "extend": 'excel',
                "className": 'excelButton',
                "text": '<i class="button-excel">Excel</i>',
                "titleAttr": 'Excel',
                "action": newexportaction,
                "exportOptions": {
                    columns: [1, 9, 10, 11, 12, 13, 2, 5, 6]
                },
            }
        ],

        "ajax": {
            "url": GetListUrl,
            "type": "POST",
            "datatype": "json",
            "data": function (d) { // callback za input change
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
                    if (data == null)
                        return "";
                    return moment(data).format(dateFormat)
                }
            },
            {
                "targets": 5, // Iznos
                "render": $.fn.dataTable.render.number('.', ',', 2, '', ' kn'),
            },
            {
                "targets": 6, // KlasaPlacanja
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 7, // Datum potvrde
                "render": function (data) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 8, // Napomena
                "render": $.fn.dataTable.render.ellipsis(30),
            },
            {
                "targets": 9, // stan id - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 10, // adresa - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 11, // kat - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 12, // broj stana - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 13, // površina - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 14, // akcija - hidden
                "visible": false,
                "searchable": false,
                "defaultContent": "<button type='button' class='button-add-remove' id='remove'><i class='bi bi-x'></i>briši</button >"
            },
        ],
    });
    excelButton = table.buttons(['.excelButton']);
    tableColumn = table.column(1);
});

