$('#RacunElektraTable').DataTable({

    // excel
    dom: 'frtipB',
    "buttons": [
        {
            "extend": 'excel',
            "text": '<i class="button-excel">Excel</i>',
            "titleAttr": 'Excel',
            "action": newexportaction,
            "exportOptions": {
                //columns: [1, 2, 3, 4, 5, 6, 7, 10]
            },
        }
    ],

    "ajax": {
        "url": racuniUrl,
        "type": "POST",
        "datatype": "json",
        "data": {param: param}
    },

    "columns": [
        {
            "data": null, "name": "brojRacuna",
            "render": function (data) {
                return '<a href="../../RacuniElektra/Details/' + data.id + '">' + data.brojRacuna + '</a>';
            },
        },
        {
            "data": null, "name": "elektraKupac.ugovorniRacun",
            "render": function (data) {
                if (data.elektraKupac != null)
                    return '<a href="../../ElektraKupci/Details/' + data.elektraKupac.id + '">' + data.elektraKupac.ugovorniRacun + '</a>';
                return '';
            }
        },
        {"data": "datumIzdavanja", "name": "datumIzdavanja"},
        {
            "data": "iznos", "name": "iznos",
            //"render": $.fn.dataTable.render.number('.', ',', 2, '')
        },
        {"data": "klasaPlacanja", "name": "klasaPlacanja"},
        {"data": "datumPotvrde", "name": "datumPotvrde"},
        {"data": "napomena", "name": "napomena"},
    ],
    "paging": true,
    "serverSide": true,
    "order": [[2, 'desc']], // default sort po datumu
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
            "targets": 1, // UgovorniRacun
            "render": $.fn.dataTable.render.ellipsis(10),
        },
        {
            "targets": 2, // DatumIzdavanja
            "render": function (data) {
                if (data == null)
                    return "";
                return moment(data).format("DD.MM.YYYY")
            }
        },
        {
            "targets": 3, // Iznos
            "render": $.fn.dataTable.render.number('.', ',', 2, '', ' kn'),
        },
        {
            "targets": 4, // KlasaPlacanja
            "render": $.fn.dataTable.render.ellipsis(20),
        },
        {
            "targets": 5, // Datum potvrde
            "render": function (data) {
                if (data == null)
                    return "";
                return moment(data).format("DD.MM.YYYY")
            }
        },
        {
            "targets": 6, // Napomena
            "render": $.fn.dataTable.render.ellipsis(30),
        },
    ]
});