$(document).ready(function () {
    $('#PredmetiTable').DataTable({
        "ajax": {
            "url": "/Predmeti/GetList",
            "type": "POST",
            "datatype": "json"
        },
        // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
        // koristi se kao selector (nije posve jasna dokumentacija)
        "columns": [

            {
                "data": null, "name": "klasa",
                "render": function (data) {
                    return '<a href="Predmeti/Details/' + data.id + '">' + data.klasa + '</a>';
                }
            },
            {"data": "naziv", "name": "naziv"},
        ],
        "paging": true,
        "serverSide": true,
        "order": [[1, 'asc']], // default sort po nazivu
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim...",
            "search": "", // remove search text
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // Klasa
                "render": $.fn.dataTable.render.ellipsis(21),
            },
            {
                "targets": 1, // Naziv
                "render": $.fn.dataTable.render.ellipsis(40),
            }
        ]
    });
});