$(document).ready(function () {
    table = $('#DopisTable').DataTable({

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
            "url": "/Dopisi/GetList",
            "type": "POST",
            "datatype": "json",
            "data": { predmetId: predmetId }
        },
        "columns": [
            {
                "data": "datum", "name": "datum",
                "render": function (data) {
                    if (data == null)
                        return "";
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "data": null, "name": "urbroj",
                "render": function (data) {
                    return '<a href="../../Dopisi/Details/' + data.id + '">' + data.urbroj + '</a>';
                }
            },
            
        ],
        "paging": true,
        "serverSide": true,
        "order": [[0, 'desc']], // default sort po datumu
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim...",
            "search": "", // remove search text
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 1, // UrBroj
                "render": $.fn.dataTable.render.ellipsis(23),
            }
        ]
    });
});