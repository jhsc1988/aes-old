﻿$(document).ready(function () {

    // ************************************ variables ************************************ //

    const selectIndexTable = $('#indexTable');
    let guid;

    // ************************************ DataTables definition ************************************ //

    GetGUID();
    table = selectIndexTable.DataTable({

        dom: 'frtipB',
        buttons: ['excelHtml5','pdfHtml5'],

        "rowCallback": function (row, data, index) {
            if (data.elektraKupac.ods.stanId == 10049) {
                $(row).addClass("redClass");
                //$('td:eq(1)', row).css('background-color', 'Red');
            }
        },

        "ajax": {
            "url": "/RacuniElektra/GetListCreate",
            "type": "POST",
            "datatype": "json",
        },

        "columns": [
            { "data": "redniBroj", "name": "redniBroj" },
            { "data": "brojRacuna", "name": "brojRacuna" },
            { "data": "elektraKupac.ods.stan.stanId", "name": "elektraKupac.ods.stan.stanId" },
            { "data": "elektraKupac.ods.stan.adresa", "name": "elektraKupac.ods.stan.adresa" },
            { "data": "elektraKupac.ods.stan.korisnik", "name": "elektraKupac.ods.stan.korisnik" },
            { "data": "elektraKupac.ods.stan.status", "name": "elektraKupac.ods.stan.status" },
            { "data": "elektraKupac.ods.stan.vlasni\u0161tvo", "name": "elektraKupac.ods.stan.vlasništvo" },
            { "data": "datumIzdavanja", "name": "datumIzdavanja" },
            {
                "data": "iznos", "name": "iznos",
                "render": $.fn.dataTable.render.number('.', ',', 2, '')
            },
            { "data": null, "name": "akcija" },

        ],
        "paging": true,
        "serverSide": true,
        "order": [[0, 'asc']], // default sort po datumu
        "bLengthChange": false,

        //"processing": true,
        "language": {
            "processing": "tražim...",
            "search": "", // remove search text
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // rbr
                "render": $.fn.dataTable.render.ellipsis(3),
            },
            {
                "targets": 1, // BrojRacuna
                "render": $.fn.dataTable.render.ellipsis(19),
            },
            {
                "targets": 2, // Stan ID
                "render": $.fn.dataTable.render.ellipsis(5),
            },
            {
                "targets": 3, // Adresa
                "render": $.fn.dataTable.render.ellipsis(34),
            },
            {
                "targets": 4, // Korisnik
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 5, // Status
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 6, // Vlasništvo
                "render": $.fn.dataTable.render.ellipsis(10),
            },
            {
                "targets": 7, // Datum izdavanja
                "render": function (data, type, row) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 8, // Iznos
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 9, // remove
                "orderable": false,
                "searchable": false,
                "defaultContent": "<button type='button' class='button-add-remove' id='remove'><i class='bi bi-x'></i>briši</button >"

            },
            {
                // if no data in JSON (for null references)
                "defaultContent": "",
                "targets": "_all"
            }
        ],
    });

    // ************************************ get GUID ************************************ //

    function GetGUID() {
        $.ajax({
            type: "POST",
            url: "/RacuniElektra/GetGUID",
            dataType: "json",
            success: function (r) {
                if (r.success)
                    setGuid(r.message);
            },
            error: function (r) {
            }
        });
    }

    function setGuid(_guid) {
        guid = _guid
    }

    // ************************************ add row ************************************ //

    $('#btnAdd').on('click', function () {
        if (brojRacuna === "") {
            return false
        } else {
            AddNew(brojRacuna, $("#iznos").val(), $("#datumIzdavanja").val(), guid);
            table.row.add(["<td><button type='button' class='remove btn btn-outline-secondary btn-sm border-danger'><i class='bi bi-x'></i></button ></td >"]).draw();
        }
    });


    // ************************************ remove row ************************************ //

    $('#indexTable tbody').on('click', '#remove', function () {
        var racunId = table.row($(this).parents('tr')).data().id;
        $.ajax({
            type: "POST",
            url: "/RacuniElektra/RemoveRow",
            data: { racunId: racunId },
            success: function (r) {
                if (r.success) {
                    alertify.success(r.message);
                    table.ajax.reload(null, false);
                }
            },
            error: function (r) {
            }
        });
    });
});

