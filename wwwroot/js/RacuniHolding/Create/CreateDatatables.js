﻿$(document).ready(function () {

    // ************************************ variables ************************************ //

    const selectIndexTable = $('#IndexTable');

    // ************************************ DataTables definition ************************************ //
    table = selectIndexTable.DataTable({
        dom: 'frtipB',
        buttons: ['excelHtml5', 'pdfHtml5'],
        "ajax": {
            "url": "/RacuniHolding/GetListCreate",
            "type": "POST",
            "datatype": "json",
        },
        "columns": [
            { "data": "redniBroj", "name": "redniBroj" },
            { "data": "brojRacuna", "name": "brojRacuna" },
            { "data": "stan.stanId", "name": "stan.stanId" },
            { "data": "stan.adresa", "name": "stan.adresa" },
            { "data": "stan.korisnik", "name": "stan.korisnik" },
            { "data": "stan.vlasni\u0161tvo", "name": "stan.vlasništvo" },
            { "data": "datumIzdavanja", "name": "datumIzdavanja" },
            {
                "data": "iznos", "name": "iznos",
                "render": $.fn.dataTable.render.number('.', ',', 2, '')
            },
            { "data": "napomena", "name": "napomena" },
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
                "orderable": false,
            },
            {
                "targets": 2, // Stan ID
                "render": $.fn.dataTable.render.ellipsis(5),
                "orderable": false,
            },
            {
                "targets": 3, // Adresa
                "render": $.fn.dataTable.render.ellipsis(34),
                "orderable": false,
            },
            {
                "targets": 4, // Korisnik
                "render": $.fn.dataTable.render.ellipsis(20),
                "orderable": false,
            },
            {
                "targets": 5, // Vlasništvo
                "render": $.fn.dataTable.render.ellipsis(10),
                "orderable": false,
            },
            {
                "targets": 6, // Datum izdavanja
                "render": function (data, type, row) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 7, // Iznos
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 8, // Napomena
                "render": $.fn.dataTable.render.ellipsis(28),
            },
            {
                "targets": 9, // remove
                "orderable": false,
                "searchable": false,
                "defaultContent": "<button type='button' class='button-add-remove' id='remove'><i class='bi bi-x'></i>briši</button>"
            },
            {
                // if no data in JSON (for null references)
                "defaultContent": "",
                "targets": "_all"
            }
        ],
    });

    // ************************************ add row ************************************ //

    $('#btnAdd').on('click', function () {
        AddNew(brojRacuna, $("#iznos").val(), $("#datumIzdavanja").val(), data_dopis);
        table.row.add(["<td><button type='button' class='remove btn btn-outline-secondary btn-sm border-danger'><i class='bi bi-x'></i></button ></td >"]).draw();
    });


    // ************************************ remove row ************************************ //

    $('#IndexTable tbody').on('click', '#remove', function () {
        var racunId = table.row($(this).parents('tr')).data().id;
        $.ajax({
            type: "POST",
            url: "/RacuniElektraRate/RemoveRow",
            data: { racunId: racunId },
            success: function (r) {
                if (r.success) {
                    alertify.success(r.message);
                    table.ajax.reload(null, false);
                }
            },
            error: function (r) {
                table.ajax.reload(null, false);
            }
        });
    });

    // ************************************ save ************************************ //

    $("#btnSave").on("click", function () {
        $.ajax({
            type: "POST",
            url: "/RacuniHolding/SaveToDB",
            data: {
                _dopisid: data_dopis,
            },
            success: function (r) {
                if (r.success) {
                    alertify.success(r.message);
                } else {
                    alertify.error(r.message);
                }
                table.ajax.reload(null, false); // user paging is not reset on reload(callback, resetPaging)
            },
            error: function (r) {
                alertify.error(r.message);
                table.ajax.reload(null, false);
            }
        });
    })

    // ************************************ delete ************************************ //

    $("#btnDelete").on("click", function () {
        $.ajax({
            type: "POST",
            url: "/RacuniHolding/RemoveAllFromDb",
            success: function (r) {
                if (r.success) {
                    alertify.success(r.message);
                } else {
                    alertify.error(r.message);
                }
                table.ajax.reload(null, false);
            },
            error: function (r) {
                alertify.error(r.message);
                table.ajax.reload(null, false);
            }
        });
    })

    // ************************************ functions ************************************ //

    function AddNew(brojRacuna, iznos, _datum, dopisId) {
        $.ajax({
            type: "POST",
            url: "/RacuniHolding/AddNewTemp",
            data: {
                brojRacuna: brojRacuna,
                iznos: iznos,
                date: _datum,
                dopisId: dopisId,
            },
            success: function (r) {
                if (r.value.success) {
                    alertify.success(r.value.message);
                    resetInput();

                } else {
                    alertify.error(r.value.message);
                }
                table.ajax.reload(null, false); // user paging is not reset on reload(callback, resetPaging)
            },
        });
    }

    function resetInput() {
        $('#brojRacuna').val("");
        $('#datumIzdavanja').val("");
        $('#iznos').val("");
        $('#stanText').html("");
    }
});
