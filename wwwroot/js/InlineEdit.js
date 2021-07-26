// ************************************ variables ************************************ //

let racunId;
let klasaUpdated;
let datumUpdated;
let napomenaUpdated;
let datum;
let klasa;
let napomena;

// for mouseclick anywhere else
$(document).mousedown(function (e) {

    // reset elements and update data
    if (!$(e.target).is('#klasa_input_clicked') && !$(e.target).is("#klasa_td_clicked")) {
        $("#klasa_td_clicked").replaceWith("<td>" + klasa + "</td>"); // reset input field to td
        // if text has changed - update db
        if (klasaUpdated) {
            updateDb(klasa, null, null); // null checking on controller side
            klasaUpdated = false; // reset updated flag
        }
    }

    if (!$(e.target).is('#datum_input_clicked') && !$(e.target).is("#datum_td_clicked")) {
        $("#datum_td_clicked").replaceWith("<td>" + datum + "</td>");
        if (datumUpdated) {
            updateDb(null, datum, null);
            datumUpdated = false;
        }
    }
    if (!$(e.target).is('#napomena_input_clicked') && !$(e.target).is("#napomena_td_clicked")) {
        $("#napomena_td_clicked").replaceWith("<td>" + napomena + "</td>");
        if (napomenaUpdated) {
            updateDb(null, null, napomena);
            napomenaUpdated = false;
        }
    }

    // get id from closest row to #_input_clicked 
    $('#klasa_td_clicked, #datum_td_clicked, #napomena_td_clicked').on('click', function () {
        const tr = this.closest('tr');
        const table = $('#RacunElektraTable').DataTable();
        racunId = table.row(tr).data().id;
        console.log(table.row(tr).data().id);
    });
});



// for mouseclick on specific columns
$("#RacunElektraTable").on('mousedown', elementForEdit, function (e) {

    if ($(e.target).is('td:nth-child(6)') && !$(e.target).is("#klasa_td_clicked")) {

        // make all other un-clicked and save them
        $("#klasa_td_clicked").not(e.target).replaceWith("<td>" + klasa + "</td>");
        if (klasaUpdated) {
            updateDb(klasa, null, null);
            klasaUpdated = false;
        }

        $(this).attr('id', 'klasa_td_clicked'); // mark this td clicked
        klasa = $(this).html(); // get value

        // set as text input
        $(this).html("").append(
            "<div id='clicked' class='input-group input-group-sm my-auto'>" +
            "<input type='text' id ='klasa_input_clicked' class='form-control' value='" + klasa + "'></div>");
    }

    if ($(e.target).is('td:nth-child(7)')) {
        $("#datum_td_clicked").not(e.target).replaceWith("<td>" + datum + "</td>");

        if (datumUpdated) {
            updateDb(null, datum, null);
            datumUpdated = false;
        }
        $(this).attr('id', 'datum_td_clicked');
        datum = $(this).html();
        $(this).html("").append(
            "<div id='clicked' class='input-group input-group-sm my-auto'>" +
            "<input type='date' id ='datum_input_clicked' class='form-control' value='" + datum + "'></div>");
    }

    if ($(e.target).is('td:nth-child(8)')) {
        $("#napomena_td_clicked").not(e.target).each().replaceWith("<td>" + napomena + "</td>");
        $(this).attr('id', 'napomena_td_clicked');

        if (napomenaUpdated) {
            updateDb(null, null, napomena);
            napomenaUpdated = false;
        }

        napomena = $(this).html();
        $(this).html("").append(
            "<div id='clicked' class='input-group input-group-sm my-auto'>" +
            "<input type='text' id ='napomena_input_clicked' class='form-control' value='" + napomena + "'></div>");
    }

// ************************************ write data to variables on input ************************************ //

    const selectKlasaTdClicked = $('#klasa_td_clicked');
    const selectDatumTdClicked = $('#datum_td_clicked');
    const selectNapomenaTdClicked = $('#napomena_td_clicked');

    selectKlasaTdClicked.on('input', function () {
        klasa = $('#klasa_input_clicked').val();
        klasaUpdated = true;
    });

    selectKlasaTdClicked.keypress(function (e) {
        if (e.which === 13) { // 13 - enter key, 9 - tab key
            selectKlasaTdClicked.replaceWith("<td>" + klasa + "</td>");
            if (klasaUpdated) {
                updateDb(klasa, null, null);
                klasaUpdated = false;
            }
        }
    });

    selectDatumTdClicked.on('input', function () {
        datum = $('#datum_input_clicked').val();
        datumUpdated = true;
    });

    selectDatumTdClicked.keypress(function (e) {
        if (e.which === 13) {
            selectDatumTdClicked.replaceWith("<td>" + datum + "</td>");
            if (datumUpdated) {
                updateDb(null, datum, null);
                datumUpdated = false;
            }
        }
    });

    selectNapomenaTdClicked.on('input', function () {
        napomena = $('#napomena_input_clicked').val();
        napomenaUpdated = true;
    });

    selectNapomenaTdClicked.keypress(function (e) {
        if (e.which === 13) {
            selectNapomenaTdClicked.replaceWith("<td>" + napomena + "</td>");
            if (napomenaUpdated) {
                updateDb(null, null, napomena);
                napomenaUpdated = false;
            }
        }
    });
});

// ************************************ update database fn ************************************ //

function updateDb(klasa, datum, napomena) {
    $.ajax({
        type: "POST",
        url: "/RacuniElektra/updateDbForInline",
        data: {
            id: racunId, // racun id
            klasa: klasa,
            datum: datum,
            napomena: napomena,
        },
        success: function (r) {
            if (r.success) {
                alertify.success(r.message);
                //table.ajax.reload(null, false);
            } else {
                alertify.error(r.message);
                table.ajax.reload(null, false); // user paging is not reset on reload(callback, resetPaging)
            }
        },
    });
}