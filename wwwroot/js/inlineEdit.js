$(document).mousedown(function (e) {

    // get row data
    // console.log(table.row(e.target).index()); // return undefined on previous (do i need to redraw ? draw())

    if (!$(e.target).is('#text_clicked') && !$(e.target).is("#td_clicked"))
        $("#td_clicked").replaceWith("<td>" + txt + "</td>");
});
         
$("#RacunElektraTable").on('mousedown', "tr td:nth-last-child(-n+3)", function (e) {

    if (!$(e.target).is('#text_clicked'))
        $("#td_clicked").replaceWith("<td>" + txt + "</td>");

    if ($(e.target).is('td'))
        $(this).attr('id', 'td_clicked');

    if ($(e.target).is('#td_clicked') && $(e.target).find('#text_clicked').length === 0) {
        txt = $(this).html();
        if ($(e.target).is("td:nth-child(6)")) {
            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'><input type='date' id ='text_clicked' class='form-control' value='" + txt + "'></div>");
        } else {
            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'><input type='text' id ='text_clicked' class='form-control' value='" + txt + "'></div>");
        }
    }

    $('#text_clicked').on('input', function () {
        txt = $("#text_clicked").val();
    });

    $('#text_clicked').keypress(function (e) {
        if (e.which == 13) {
            $("#td_clicked").replaceWith("<td>" + txt + "</td>");
        }
    });
});



