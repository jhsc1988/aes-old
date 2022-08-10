
$(document).ready(function () {

    table = $('#IndexTable').DataTable({

        dom: 'rt',
        "ajax": {
            "url": "/RacuniElektra/GetObracunPotrosnjeForRacun",
            "type": "POST",
            "datatype": "json",
            "data": { RacunId: RacunId },
        },
        "columns": [
            { "data": "brojBrojila", "name": "brojBrojila" },
            { "data": "tarifnaStavka.naziv", "name": "tarifnaStavka.naziv" },
            { "data": "datumOd", "name": "datumOd" },
            { "data": "datumDo", "name": "datumDo" },
            { "data": "stanjeOd", "name": "stanjeOd" },
            { "data": "stanjeDo", "name": "stanjeDo" },
            { "data": null, "name": "akcija" },
        ],
        "paging": true,
        "serverSide": true,
        "order": [[0, "desc"]],
        "bLengthChange": false,

        //"processing": true,
        "language": {
            "processing": "tražim...",
            "search": "", // remove search text
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // Broj brojila
                "searchable": false,
            },
            {
                "targets": 1, // Tarfina stavka
                "searchable": false,
                "render": $.fn.dataTable.render.ellipsis(6),
            },
            {
                "targets": 2, // Datum od
                "render": function (data) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                },
            },
            {
                "targets": 3, // Datum do
                "render": function (data) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 4, // Stanje od
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 5, // Stanje do
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 6, // uredi
                "orderable": false,
                "render": function (data) {
                    return '<a href="../../../ObracunPotrosnje/Edit/' + data.id + '">' + "edit" + '</a>';
                }
            },
         
        ],
    });
});

