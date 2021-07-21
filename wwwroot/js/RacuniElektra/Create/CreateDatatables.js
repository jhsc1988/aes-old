$(document).ready(function () {
// ************************************ variables ************************************ //
    const selectIndexTable = $('#indexTable');

// ************************************ DataTables definition ************************************ //

        table = selectIndexTable.DataTable({

            "ajax": {
                "url": "/RacuniElektra/GetListCreate",
                "type": "POST",
                "datatype": "json",
            },

            "columns": [
                {"data": "redniBroj", "name": "redniBroj"},
                {"data": "brojRacuna", "name": "brojRacuna"},
                {"data": "stanid", "name": "elektraKupac.ods.stan.StanId"},
                {"data": "adresa", "name": "elektraKupac.ods.stan.Adresa"},
                {"data": "korisnik", "name": "elektraKupac.ods.stan.Korisnik"},
                {"data": "status", "name": "elektraKupac.ods.stan.Status"},
                {"data": "vlasni\u0161tvo", "name": "elektraKupac.ods.stan.Vlasništvo"},
                {"data": "datumIzdavanja", "name": "datumIzdavanja"},
                {
                    "data": "iznos", "name": "iznos",
                    "render": $.fn.dataTable.render.number('.', ',', 2, '')
                },
                // {"data" : null, "name": "akcija"},
                
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
                    "render": $.fn.dataTable.render.ellipsis(3),
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
                    "render": $.fn.dataTable.render.ellipsis(20)
                },
                {
                    "targets": 5, // Status
                    "render": $.fn.dataTable.render.ellipsis(8)
                },
                {
                    "targets": 6, // Vlasništvo
                    "render": $.fn.dataTable.render.ellipsis(10)
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
                // {
                //     "targets": 9, // remove
                //     "orderable" : false,
                //     "searchable": false,
                //     "defaultContent": '',
                //     //"render": $.fn.dataTable.render.ellipsis(10),
                // },
            ],
        });
// ************************************ add row ************************************ //
    $('#btnAdd').on('click', function () {
        if (brojRacuna === "") {
            return false
        } else {
            
            AddNew(brojRacuna, $("#iznos").val());
        }
    });

    
    
    
// ************************************ remove row ************************************ //
    selectIndexTable.on('click', '.remove', function () {
        table
            .row($(this).parents('tr'))
            .remove()
            .draw();
    });
});