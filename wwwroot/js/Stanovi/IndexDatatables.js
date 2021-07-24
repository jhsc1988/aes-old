﻿$(document).ready(function () {
    //$('div.dataTables_filter input').addClass('form-control form-control-sm');
    var table = $('#indexTable').DataTable({
        dom: 'frtipB',
        //buttons: ['excelHtml5', 'pdfHtml5'],

        buttons: [{
            extend: 'excelHtml5',
            exportOptions: {
                modifier: {
                    order: 'index', // 'current', 'applied','index', 'original'
                    page: 'all', // 'all', 'current'
                    search: 'none' // 'none', 'applied', 'removed'
                },
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
            }
        },],

        "ajax": {

            "url": "/Stanovi/GetList",
            "type": "POST",
            "datatype": "json"
        },
        // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
        // koristi se kao selector (nije posve jasna dokumentacija)
        "columns": [
            {
                "data": null, "name": "StanId",
                "render": function (data, type, row, meta) {
                    return '<a href="Stanovi/Details/' + data.id + '">' + data.stanId + '</a>';
                }

            },
            {
                "data": null, "name": "SifraObjekta",
                "render": function (data, type, row, meta) {
                    return '<a href="Stanovi/Details/' + data.id + '">' + data.sifraObjekta + '</a>';
                }
            },
            { "data": "adresa", "name": "Adresa" },
            { "data": "kat", "name": "Kat" },
            { "data": "brojSTana", "name": "BrojSTana" },
            { "data": "\u010Detvrt", "name": "Četvrt" },
            {
                "data": "povr\u0161ina", "name": "Površina",
                "render": $.fn.dataTable.render.number('.', ',', 2, '')
            },
            { "data": "korisnik", "name": "Korisnik" },
            { "data": "statusKori\u0161tenja", "name": "StatusKorištenja" },
            { "data": "vlasni\u0161tvo", "name": "Vlasništvo" },
        ],

        "paging": true,
        "serverSide": true,
        "order": [[2, 'asc']],
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim...",
            "search": "", // remove search text
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // Stan ID
                "render": $.fn.dataTable.render.ellipsis(5),
            },
            {
                "targets": 1, // Šifra objekta
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 2, // Adresa
                "render": $.fn.dataTable.render.ellipsis(34),
            },
            {
                "targets": 3, // Kat
                "render": $.fn.dataTable.render.ellipsis(10),
            },
            {
                "targets": 4, // Broj stana
                "render": $.fn.dataTable.render.ellipsis(12),
            },
            {
                "targets": 5, // Četvrt
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 6, // Površina
                "render": $.fn.dataTable.render.ellipsis(6),
            },
            {
                "targets": 7, // Korisnik
                "render": $.fn.dataTable.render.ellipsis(20)
            },
            {
                "targets": 8, // Status
                "render": $.fn.dataTable.render.ellipsis(8)
            },
            {
                "targets": 9, // Vlasništvo
                "render": $.fn.dataTable.render.ellipsis(10)
            }
        ],
    });
});