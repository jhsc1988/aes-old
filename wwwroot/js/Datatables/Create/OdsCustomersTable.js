$(document).ready(function () {
    $('#OdsTable').DataTable({
        "ajax": {
            "url": "/Ods/GetList", // samo stanovi sa mjernim mjestom
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                "data": null, "name": "Omm",
                "render": function (data, type, row, meta) {
                    return '<a href="../../../Ods/Details/' + data.id + '">' + data.omm + '</a>';
                }
            },
            {
                "data": null, "name": "StanId",
                "render": function (data, type, row, meta) {
                    return '<a href="../../../Stanovi/Details/' + data.stan.id + '">' + data.stan.stanId + '</a>';
                }
            },
            {
                "data": null, "name": "Stan.SifraObjekta",
                "render": function (data, type, row, meta) {
                    return '<a href="../../../Stanovi/Details/' + data.stan.id + '">' + data.stan.sifraObjekta + '</a>';
                }
            },
            { "data": "stan.adresa", "name": "stan.adresa" },
            { "data": "stan.kat", "name": "stan.kat" },
            { "data": "stan.brojSTana", "name": "stan.brojSTana" },
            { "data": "stan.\u010Detvrt", "name": "stan.Četvrt" },
            {
                "data": "stan.povr\u0161ina", "name": "stan.Površina",
                //"render": $.fn.dataTable.render.number('.', ',', 2, '')
            },
            { "data": "napomena", "name": "Napomena" }

        ],
        "paging": true,
        "serverSide": true,
        "order": [[3, 'asc']],
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim...",
            "search": "", // remove search text
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // Omm
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 1, // Stan ID
                "render": $.fn.dataTable.render.ellipsis(5),
            },
            {
                "targets": 2, // Šifra objekta
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 3, // Adresa
                "render": $.fn.dataTable.render.ellipsis(34),
            },
            {
                "targets": 4, // Kat
                "render": $.fn.dataTable.render.ellipsis(10),
            },
            {
                "targets": 5, // Broj stana
                "render": $.fn.dataTable.render.ellipsis(12),
            },
            {
                "targets": 5, // Četvrt
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 7, // Površina
                "render": $.fn.dataTable.render.number(' ', ',', 2, '', ' m2'),
            },
            {
                "targets": 8, // Napomena
                "render": $.fn.dataTable.render.ellipsis(40),
            },
            {
                "targets": 9, // Akcija
                "data": null,
                "render":
                    function (data, type, row, meta) {
                        return '<button class="button-add-remove" onclick="getData(\'' + data.id + '\', \'' + data.omm + '\')"><i class="bi bi-plus"></i>dodaj</button>'
                    }
            }
        ]
    });
});