$('#UgovoriOPrijenosuTable').DataTable({
    "ajax": {
        "url": "/Stanovi/GetUgovoriOPrijenosuForStan",
        "type": "POST",
        "datatype": "json",
        "data": {stanid: stanid}
    },
    // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
    // koristi se kao selector (nije posve jasna dokumentacija)
    "columns": [
        {"data": "brojUgovora", "name": "brojUgovora"},
        {"data": "ugovorOKoristenju.brojUgovora", "name": "ugovorOKoristenju.brojUgovora"},
        {
            "data": null, "name": "datumPrijenosa",
            "render": function (data, type, row) {
                return moment(data).format("DD.MM.YYYY")
            }
        },
        {
            "data": null, "name": "datumPotpisa",
            "render": function (data, type, row) {
                return moment(data).format("DD.MM.YYYY")
            }
        },
        {"data": "kupac", "name": "kupac"},
        {"data": "kupacOIB", "name": "kupacOIB"},
        {"data": "dopis.predmet.klasa", "name": "dopis.predmet.klasa"},
        {"data": "rbrUgovora", "name": "rbrUgovora"},
        {"data": "dopisDostave.predmet.klasa", "name": "dopisDostave.predmet.klasa"},
        {"data": "rbrDostave", "name": "rbrDostave"},
    ],
    "paging": true,
    "serverSide": true,
    "order": [[2, 'asc']], // default sort po datumu potpisa HEP-a
    "bLengthChange": false,
    //"processing": true,
    "language": {
        "processing": "tražim...",
        "search": "", // remove search text
    },
    "scrollX": true,
    "columnDefs": [
        {
            "targets": 0, // Broj ugovora
            "render": $.fn.dataTable.render.ellipsis(14),
        },
        {
            "targets": 1, // Broj ugovora o korištenju
            "render": $.fn.dataTable.render.ellipsis(14),
        },
        {
            "targets": 4, // Kupac
            "render": $.fn.dataTable.render.ellipsis(40),
        },
        {
            "targets": 5, // Kupac OIB
            "render": $.fn.dataTable.render.ellipsis(13),
        },
        {
            "targets": 6, // Klasa ugovora
            "render": $.fn.dataTable.render.ellipsis(21),
        },
        {
            "targets": 7, // RBR ugovora
            "render": $.fn.dataTable.render.ellipsis(3),
        },
        {
            "targets": 8, // Klasa dostave
            "render": $.fn.dataTable.render.ellipsis(21),
        },
        {
            "targets": 9, // RBR dostave
            "render": $.fn.dataTable.render.ellipsis(3),
        },
    ]
});