﻿$(document).ready(function () {

    // ************************************ variables ************************************ //

    const selectIndexTable = $('#IndexTable');

    // ************************************ DataTables definition ************************************ //
    table = selectIndexTable.DataTable({
        "ajax": {
            "url": "/RacunElektraIzvrsenjaUsluges/GetListCreate",
            "type": "POST",
            "datatype": "json",
        },
        "columns": [
            { "data": "redniBroj", "name": "redniBroj" },
            { "data": "brojRacuna", "name": "brojRacuna" },
            {
                "data": null, "name": "elektraKupac.ods.stan.stanId",
                "render": function (data) {
                    if (data == null)
                        return "";
                    return '<a href="../../Stanovi/Details/' + data.elektraKupac.ods.stan.id + '">' + data.elektraKupac.ods.stan.stanId + '</a>';
                }
            },
            { "data": "elektraKupac.ods.stan.adresa", "name": "elektraKupac.ods.stan.adresa" },
            { "data": "elektraKupac.ods.stan.korisnik", "name": "elektraKupac.ods.stan.korisnik" },
            { "data": "elektraKupac.ods.stan.vlasni\u0161tvo", "name": "elektraKupac.ods.stan.vlasništvo" },
            { "data": "datumIzdavanja", "name": "datumIzdavanja" },
            { "data": "datumIzvrsenja", "name": "datumIzvrsenja" },
            { "data": "usluga", "name": "usluga" },
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
                "targets": 7, // Datum izvršenja
                "render": function (data, type, row) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 8, // Usluga
                "render": $.fn.dataTable.render.ellipsis(10),
                "orderable": false,
            },
            {
                "targets": 9, // Iznos
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 10, // Napomena
                "render": $.fn.dataTable.render.ellipsis(28),
            },
            {
                "targets": 11, // remove
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
        AddNew(brojRacuna, $("#iznos").val(), $("#datumIzdavanja").val(), data_dopis, $("#datumIzvrsenja").val(), $("#usluga").val());
            table.row.add(["<td><button type='button' class='remove btn btn-outline-secondary btn-sm border-danger'><i class='bi bi-x'></i></button ></td >"]).draw();
    });


    // ************************************ remove row ************************************ //

    $('#IndexTable tbody').on('click', '#remove', function () {
        var racunId = table.row($(this).parents('tr')).data().id;
        $.ajax({
            type: "POST",
            url: "/RacunElektraIzvrsenjaUsluges/RemoveRow",
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
            url: "/RacunElektraIzvrsenjaUsluges/SaveToDB",
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
            url: "/RacunElektraIzvrsenjaUsluges/RemoveAllFromDb",
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

    function AddNew(brojRacuna, iznos, _datum, dopisId, datumIzvrsenja, usluga) {
        $.ajax({
            type: "POST",
            url: "/RacunElektraIzvrsenjaUsluges/AddNewTemp",
            data: {
                brojRacuna: brojRacuna,
                iznos: iznos,
                date: _datum,
                dopisId: dopisId,
                datumIzvrsenja: datumIzvrsenja,
                usluga: usluga
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
        $('#usluga').val("");
        $('#datumIzvrsenja').val("");
        $('#stanText').html("");
    }
});

