$('#RacunOdsIzvrsenjaUslugeTable').DataTable({

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
        "url": "/Stanovi/GetRacuniOdsIzvrsenjeForStan",
        "type": "POST",
        "datatype": "json",
        "data": {stanid: stanid}
    },
    // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
    // koristi se kao selector (nije posve jasna dokumentacija)
    "columns": [
        {
            "data": null, "name": "brojRacuna",
            "render": function (data, type, row, meta) {
                return '<a href="RacunOdsIzvrsenjaUsluge/Details/' + data.id + '">' + data.brojRacuna + '</a>';
            }
        },
        {
            "data": null, "name": "odsKupac.sifraKupca",
            "render": function (data, type, row, meta) {
                return '<a href="OdsKupci/Details/' + data.odsKupac.id + '">' + data.odsKupac.sifraKupca + '</a>';
            }
        },
        {"data": "datumIzdavanja", "name": "datumIzdavanja"},
        {"data": "datumIzvrsenja", "name": "datumIzvrsenja"},
        {"data": "usluga", "name": "usluga"},
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
            "targets": 0, // BrojRacuna
            "render": $.fn.dataTable.render.ellipsis(19),
        },
        {
            "targets": 1, // Šifra kupca
            "render": $.fn.dataTable.render.ellipsis(8),
        },
        {
            "targets": 2, // DatumIzdavanja
            "render": function (data, type, row) {
                return moment(data).format("DD.MM.YYYY")
            }
        },
        {
            "targets": 3, // DatumIzvrsenja
            "render": function (data, type, row) {
                return moment(data).format("DD.MM.YYYY")
            }
        },
        {
            "targets": 4, // Usluga
            "render": $.fn.dataTable.render.ellipsis(30),
        }, {
            "targets": 5, // Iznos
            "render": $.fn.dataTable.render.ellipsis(8),
        },
        {
            "targets": 6, // KlasaPlacanja
            "render": $.fn.dataTable.render.ellipsis(20),
        },
        {
            "targets": 7, // Napomena
            "render": $.fn.dataTable.render.ellipsis(30),
        },
    ]
});