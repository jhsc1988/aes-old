$(document).ready(function () {

    const selectIndexTable = $('#IndexTable');

    table = selectIndexTable.DataTable({
        "ajax": {
            "url": "/RacuniHolding/GetList",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.isFiltered = false;
                d.klasa = null;
                d.urbroj = null;
            }
        },
        "columns": [
            { "data": "redniBroj", "name": "redniBroj" },
            { "data": "brojRacuna", "name": "brojRacuna" },
            {
                "data": null, "name": "stan.stanId",
                "render": function (data) {
                    if (data == null || data.stan == null)
                        return "";
                    return '<a href="../../../Stanovi/Details/' + data.stan.id + '">' + data.stan.stanId + '</a>';
                }
            },
            { "data": "stan.adresa", "name": "stan.adresa" },
            { "data": "stan.korisnik", "name": "stan.korisnik" },
            { "data": "stan.vlasni\u0161tvo", "name": "stan.vlasništvo" },
            { "data": "datumIzdavanja", "name": "datumIzdavanja" },
            {
                "data": "iznos", "name": "iznos",
                //"render": $.fn.dataTable.render.number('.', ',', 2, '')
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
});

