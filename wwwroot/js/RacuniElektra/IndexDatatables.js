﻿// for inline editing
let table;
let txt;

// notify test
//$.notify("Hello Worldaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "info");

$(document).ready(function () {

    table = $('#RacunElektraTable').DataTable({

        "ajax": {
            "url": "/RacuniElektra/GetList",
            "type": "POST",
            "datatype": "json",
            "data": function (d){
                d.klasa= $("#selectPredmet").val();
                d.urbroj= $("#selectDopis").val();
            }
        },
        // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
        // koristi se kao selector (nije posve jasna dokumentacija)
        "columns": [
            {
                "data": null, "name": "brojRacuna",
                "render": function (data, type, row, meta) {
                    return '<a href="RacuniElektra/Details/' + data.id + '">' + data.brojRacuna + '</a>';
                }
            },
            {
                "data": null, "name": "elektraKupac.ugovorniRacun",
                "render": function (data, type, row, meta) {
                    return '<a href="RacuniElektra/Details/' + data.elektraKupac.id + '">' + data.elektraKupac.ugovorniRacun + '</a>';
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
        "order": [[2, 'asc']], // default sort po datumu
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim..."
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
                "render": function (data, type, row) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 3, // Iznos
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 4, // KlasaPlacanja
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 5, // Napomena
                "render": $.fn.dataTable.render.ellipsis(30),
            },
        ],
    });
});
    