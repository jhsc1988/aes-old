$(document).ready(function () {
    table = $('#PredmetiTable').DataTable({

        // excel
        //dom: 'frtipB',
        //"buttons": [
        //    {
        //        "extend": 'excel',
        //        "text": '<i class="button-excel">Excel</i>',
        //        "titleAttr": 'Excel',
        //        "action": newexportaction,
        //        "exportOptions": {
        //            //columns: [1, 9, 10, 11, 12, 13, 2, 5, 6]
        //        },
        //    }
        //],

        "ajax": {
            "url": "/Predmeti/GetList",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {"data": "vrijemeUnosa", "name": "vrijemeUnosa"},
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
        "order": [[0, 'desc']], // default sort po vremenu unosa
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim...",
            "search": "", // remove search text
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // vrijeme unosa - hidden
                "visible": false,
                "searchable": false,
            },
            {
                "targets": 1, // Klasa
                "render": $.fn.dataTable.render.ellipsis(21),
            },
            {
                "targets": 2, // Naziv
                "render": $.fn.dataTable.render.ellipsis(40),
            }
        ]
    });
});