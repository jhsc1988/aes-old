$(document).ready(function () {
// ************************************ variables ************************************ //
    const selectIndexTable = $('#indexTable');

// ************************************ DataTables definition ************************************ //
    const table = selectIndexTable.DataTable({
        "paging": true,
        "ordering": false,
        "bLengthChange": false,
        "columns": [
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
                "render": $.fn.dataTable.render.ellipsis(10),
            },
            {
                "targets": 8, // iznos
                "render": $.fn.dataTable.render.ellipsis(10),
            },
            {
                "targets": 9, // remove
                //"render": $.fn.dataTable.render.ellipsis(10),
            },
        ]
    })
// ************************************ add row ************************************ //
    $('#btnAdd').on('click', function () {
        if (brojRacuna === "") {
            return false
        } else {
            // insert variables
            table.row.add([
                0, // dummy variable - rbr
                brojRacuna,
                data_stanId,
                data_adresa,
                data_korisnik,
                data_statuskoristenja,
                data_vlasnistvo,
                datumIzdavanja.value,
                iznos.value,
                "<td><button type='button' class='remove btn btn-outline-secondary btn-sm border-danger'><i class='bi bi-x'></i></button ></td >"
            ]).draw(false);
            // reset variables
            data_stanId = "";
            data_adresa = "";
            data_kat = "";
            data_brojstana = "";
            data_cetvrt = "";
            data_povrsina = "";
            data_statuskoristenja = "";
            data_korisnik = "";
            data_vlasnistvo = "";
            brojRacuna = "";
            ugovorniRacun = "";
            $("#brojRacuna").val("");
            $("#datumIzdavanja").val("");
            $("#iznos").val("");
            $("#dopisId").val("");
            $("#redniBroj").val("");
            $("#stanText").html("");
        }
    });
// ************************************ auto ordering (rbr) ************************************ //
    table.on('order.dt search.dt', function () {
        table.column(0, {search: 'applied', order: 'applied'}).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
// ************************************ remove row ************************************ //
    selectIndexTable.on('click', '.remove', function () {
        table
            .row($(this).parents('tr'))
            .remove()
            .draw();
    });
});