$('#UgovoriOKoristenjuTable').DataTable({
    "ajax": {
        "url": "/Stanovi/GetUgovoriOKoristenjuForStan",
        "type": "POST",
        "datatype": "json",
        "data": {stanid: stanid}
    },
    // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
    // koristi se kao selector (nije posve jasna dokumentacija)
    "columns": [
        {"data": "brojUgovora", "name": "brojUgovora"},
        {"data": "ods.omm", "name": "ods.omm"},
        {
            "data": null, "name": "datumPotpisaHEP",
            "render": function (data, type, row) {
                return moment(data).format("DD.MM.YYYY")
            }
        },
        {
            "data": null, "name": "datumPotpisaGZ",
            "render": function (data, type, row) {
                return moment(data).format("DD.MM.YYYY")
            }
        },
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
        }, {
            "targets": 1, // OMM
            "render": $.fn.dataTable.render.ellipsis(8),
        },
        {
            "targets": 3, // Klasa ugovora
            "render": $.fn.dataTable.render.ellipsis(21),
        },
        {
            "targets": 4, // RBR ugovora
            "render": $.fn.dataTable.render.ellipsis(3),
        },
        {
            "targets": 5, // Klasa dostave
            "render": $.fn.dataTable.render.ellipsis(21),
        },
        {
            "targets": 6, // RBR dostave
            "render": $.fn.dataTable.render.ellipsis(3),
        },
    ]
});