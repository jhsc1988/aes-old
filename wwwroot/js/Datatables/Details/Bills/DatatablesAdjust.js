function stanDetailsDatatablesAdjust() {
    // table head rendering fix: 
    // https://datatables.net/forums/discussion/48422/table-header-not-displaying-correctly-initially#Comment_128514
    // https://datatables.net/examples/api/tabs_and_scrolling.html
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $.fn.dataTable.tables({visible: true, api: true}).columns.adjust();
    });
}