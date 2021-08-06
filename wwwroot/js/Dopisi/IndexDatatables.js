$(document).ready(function () {
    $('#DopisTable').DataTable({
        "ajax": {
            "url": "/Dopisi/GetList",
            "type": "POST",
            "datatype": "json",
            "data": { predmetId: predmetId }
        },
        // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
        // koristi se kao selector (nije posve jasna dokumentacija)
        "columns": [
            {"data": "predmet.klasa", "name": "predmet.klasa"},
            {"data": "predmet.naziv", "name": "predmet.naziv"},
            {"data": "urbroj", "name": "urbroj"},
            {
                "data": "datum", "name": "datum",
                "render": function (data) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                }
            },
        ],
        "paging": true,
        "serverSide": true,
        "order": [[3, 'asc']], // default sort po datumu
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
            }, {
                "targets": 2, // UrBroj
                "render": $.fn.dataTable.render.ellipsis(23),
            }
        ]
    });
});