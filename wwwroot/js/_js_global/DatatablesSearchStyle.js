/**
 * sets style for datatables search - puts it to left side
 * @param {any} input
 * @param {any} filter
 */
function setStyle(input, filter) {

    input.attr('placeholder', 'Pretraga');
    input.attr('class', 'form-control form-control-sm');
    input.css({ 'width': '300px', 'display': 'inline-block' });

    filter.removeClass('dataTables_filter');
    filter.addClass('search-top-20 col-lg-12');
}


