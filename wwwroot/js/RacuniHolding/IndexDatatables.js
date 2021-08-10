$(document).ready(function () {

    table = $('#IndexTable').DataTable({
        "ajax": {
            "url": "/RacuniHolding/GetList",
            "type": "POST",
            "datatype": "json"
        },
        // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
        // koristi se kao selector (nije posve jasna dokumentacija)
        "columns": [
            { "data": "id", "name": "id" },
            { "data": "redniBroj", "name": "redniBroj" },
            {
                "data": null, "name": "brojRacuna",
                "render": function (data) {
                    return '<a href="RacuniHolding/Details/' + data.id + '">' + data.brojRacuna + '</a>';
                },
            },
            {
                "data": null, "name": "stan.sifraObjekta",
                "render": function (data, type, row, meta) {
                    return '<a href="Stanovi/Details/' + data.stan.id + '">' + data.stan.sifraObjekta + '</a>';
                }
            }, 

            {"data": "datumIzdavanja", "name": "datumIzdavanja"},
            {
                "data": "iznos", "name": "iznos",
                "render": $.fn.dataTable.render.number('.', ',', 2, '')
            },

            {"data": "klasaPlacanja", "name": "klasaPlacanja"},
            {"data": "datumPotvrde", "name": "datumPotvrde"},
            { "data": "napomena", "name": "napomena" },
            { "data": null, "name": null }, // akcija
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
                "targets": 3, // Šifra objekta
                "render": $.fn.dataTable.render.ellipsis(10),
            }, 
            {
                "targets": 4, // DatumIzdavanja
                "render": function (data, type, row) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 5, // Iznos
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 6, // KlasaPlacanja
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 7, // Datum potvrde
                "render": function (data, type, row) {
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
                "targets": 9, // akcija - hidden
                "visible": false,
                "searchable": false,
                "defaultContent": "<button type='button' class='button-add-remove' id='remove'><i class='bi bi-x'></i>briši</button >"
            },
        ]
    });
});