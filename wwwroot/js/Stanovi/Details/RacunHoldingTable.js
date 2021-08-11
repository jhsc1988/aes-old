$('#RacunHoldingTable').DataTable({

    "ajax": {
        "url": holdingUrl,
        "type": "POST",
        "datatype": "json",
        "data": {param: param}
    },
    // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
    // koristi se kao selector (nije posve jasna dokumentacija)
    "columns": [
        {
            "data": null, "name": "brojRacuna",
            "render": function (data, type, row, meta) {
                return '<a href="../../RacuniHolding/Details/' + data.id + '">' + data.brojRacuna + '</a>';
            }
        },
        {
            "data": null, "name": "stan.sifraObjekta",
            "render": function (data, type, row, meta) {
                return '<a href="../../Stanovi/Details/' + data.stan.id + '">' + data.stan.sifraObjekta + '</a>';
            }
        }, {
            "data": null, "name": "stan.stanId",
            "render": function (data, type, row, meta) {
                return '<a href="../../Stanovi/Details/' + data.stan.id + '">' + data.stan.stanId + '</a>';
            }
        },

        {"data": "datumIzdavanja", "name": "datumIzdavanja"},
        {
            "data": "iznos", "name": "iznos",
            "render": $.fn.dataTable.render.number('.', ',', 2, '')
        },

        {"data": "klasaPlacanja", "name": "klasaPlacanja"},
        {"data": "datumPotvrde", "name": "datumPotvrde"},
        {"data": "napomena", "name": "napomena"},
    ],
    "paging": true,
    "serverSide": true,
    "order": [[3, 'asc']], // default sort po datumu
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
            "targets": 1, // Šifra objekta
            "render": $.fn.dataTable.render.ellipsis(10),
        }, {
            "targets": 2, // Stan ID
            "render": $.fn.dataTable.render.ellipsis(10),
        },
        {
            "targets": 3, // DatumIzdavanja
            "render": function (data, type, row) {
                return moment(data).format("DD.MM.YYYY")
            }
        },
        {
            "targets": 4, // Iznos
            "render": $.fn.dataTable.render.ellipsis(8),
        },
        {
            "targets": 5, // KlasaPlacanja
            "render": $.fn.dataTable.render.ellipsis(20),
        },
        {
            "targets": 6, // Napomena
            "render": $.fn.dataTable.render.ellipsis(30),
        },
    ]
});