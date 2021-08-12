$(document).ready(function () {
    $('#ElektraKupacTable').DataTable({

        // excel
        dom: 'frtipB',
        "buttons": [
            {
                "extend": 'excel',
                "text": '<i class="" style="color: green; font-style: normal;">Excel</i>',
                "titleAttr": 'Excel',
                "action": newexportaction,
                "exportOptions": {
                    //columns: [1, 2, 3, 4, 5, 6, 7, 10]
                },
            }
        ],

        "ajax": {
            "url": "/ElektraKupci/GetList",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                "data": null, "name": "ugovorniRacun",
                "render": function (data, type, row, meta) {
                    return '<a href="ElektraKupci/Details/' + data.id + '">' + data.ugovorniRacun + '</a>';
                }
            },
            {
                "data": null, "name": "Ods.Omm",
                "render": function (data, type, row, meta) {
                    return '<a href="Ods/Details/' + data.ods.id + '">' + data.ods.omm + '</a>';
                }
            },
            {
                "data": null, "name": "ods.stan.stanId",
                "render": function (data, type, row, meta) {
                    return '<a href="Stanovi/Details/' + data.ods.stan.id + '">' + data.ods.stan.stanId + '</a>';
                }
            },
            {
                "data": null, "name": "ods.stan.sifraObjekta",
                "render": function (data, type, row, meta) {
                    return '<a href="Stanovi/Details/' + data.ods.stan.id + '">' + data.ods.stan.sifraObjekta + '</a>';
                }
            },
            {"data": "ods.stan.adresa", "name": "ods.stan.adresa"},
            {"data": "ods.stan.kat", "name": "ods.stan.kat"},
            {"data": "ods.stan.brojSTana", "name": "ods.stan.brojSTana"},
            {"data": "ods.stan.\u010Detvrt", "name": "ods.stan.Četvrt"},
            {
                "data": "ods.stan.povr\u0161ina", "name": "ods.stan.Površina",
                "render": $.fn.dataTable.render.number('.', ',', 2, '')
            },
            {"data": "napomena", "name": "Napomena"}
        ],
        "paging": true,
        "serverSide": true,
        "order": [[4, 'asc']],
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim...",
            "search": "", // remove search text
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // Ugovorni račun
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 1, // Omm
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 2, // Stan ID
                "render": $.fn.dataTable.render.ellipsis(5),
            },
            {
                "targets": 3, // Šifra objekta
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 4, // Adresa
                "render": $.fn.dataTable.render.ellipsis(34),
            },
            {
                "targets": 5, // Kat
                "render": $.fn.dataTable.render.ellipsis(10),
            },
            {
                "targets": 6, // Broj stana
                "render": $.fn.dataTable.render.ellipsis(12),
            },
            {
                "targets": 7, // Četvrt
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 8, // Površina
                "render": $.fn.dataTable.render.ellipsis(6),
            },
            {
                "targets": 9, // Napomena
                "render": $.fn.dataTable.render.ellipsis(40)
            }
        ]
    });
});