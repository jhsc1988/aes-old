let table;

$(document).ready(function () {
    table = $('#RacunHoldingTable').DataTable({

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
                    //columns: [1, 2, 3, 4, 5, 6, 7, 10]
                },
            }
        ],

        "ajax": {
            "url": holdingUrl,
            "type": "POST",
            "datatype": "json",
            "data": { param: param }
        },
        "columns": [
            {
                "data": null, "name": "brojRacuna",
                "render": function (data, type, row, meta) {
                    return '<a href="../../RacuniHolding/Details/' + data.id + '">' + data.brojRacuna + '</a>';
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
        ],
        "paging": true,
        "serverSide": true,
        "order": [[1, 'desc']], // default sort po datumu
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim...",
            "search": "", // remove search text
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // BrojRacuna
                "render": $.fn.dataTable.render.ellipsis(19),
            },
            {
                "targets": 1, // DatumIzdavanja
                "render": function (data) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 2, // Iznos
                "render": $.fn.dataTable.render.number('.', ',', 2, '', ' kn'),
            },
            {
                "targets": 3, // KlasaPlacanja
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 4, // DatumPotvrde
                "render": function (data) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 5, // Napomena
                "render": $.fn.dataTable.render.ellipsis(30),
            },
        ]
    });
    excelButton = table.buttons(['']);
    //excelButton = table.buttons(['.excelButton']);
});