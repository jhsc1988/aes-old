// ************************************ variables ************************************ //

let racunId;
let klasaUpdated;
let datumUpdated;
let napomenaUpdated;
let datum;
let klasa;
let napomena;

$(document).mousedown(function (e) {

    // enter text to text input on click anywhere on page
    if (!$(e.target).is('#klasa_input_clicked') && !$(e.target).is("#klasa_td_clicked")) {

        //notifyPos = $("#klasa_td_clicked");
        $("#klasa_td_clicked").replaceWith("<td>" + klasa + "</td>");

        // if text has changed - update db
        if (klasaUpdated) {
            updateDb(klasa, null, null);
            klasaUpdated = false;
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

    // get id from closest row to #klasa_input_clicked 
    $('#klasa_td_clicked, #datum_td_clicked, #napomena_td_clicked').on('click', function () {

        const tr = this.closest('tr');
        const table = $('#RacunElektraTable').DataTable();
        racunId = table.row(tr).data().id;
        console.log(table.row(tr).data().id);
    });
});

$("#RacunElektraTable").on('mousedown', "tr td:nth-last-child(-n+3)", function (e) {

// ************************************ make td un-clicked ************************************ //

    if (!$(e.target).is('#klasa_input_clicked'))
        $("#klasa_td_clicked").replaceWith("<td>" + klasa + "</td>");

    if (!$(e.target).is('#datum_input_clicked'))
        $("#datum_td_clicked").replaceWith("<td>" + datum + "</td>");

    if (!$(e.target).is('#napomena_input_clicked'))
        $("#napomena_td_clicked").replaceWith("<td>" + napomena + "</td>");

// ************************************ make td clicked ************************************ //

    if ($(e.target).is('td:nth-child(5)'))
        $(this).attr('id', 'klasa_td_clicked');

    if ($(e.target).is('td:nth-child(6)'))
        $(this).attr('id', 'datum_td_clicked');

    if ($(e.target).is('td:nth-child(7)'))
        $(this).attr('id', 'napomena_td_clicked');


// ************************************ get clicked text and make div as input ************************************ //

    if ($(e.target).is('#klasa_td_clicked') && $(e.target).find('#clicked').length === 0) {
        klasa = $(this).html();
        $(this).html("").append(
            "<div id='clicked' class='input-group input-group-sm my-auto'><input type='text' id ='klasa_input_clicked' class='form-control' value='" + klasa + "'></div>");
    }

    if ($(e.target).is('#datum_td_clicked') && $(e.target).find('#clicked').length === 0) {
        datum = $(this).html();
        $(this).html("").append(
            "<div id='clicked' class='input-group input-group-sm my-auto'><input type='date' id ='datum_input_clicked' class='form-control' value='" + datum + "'></div>");
    }

    if ($(e.target).is('#napomena_td_clicked') && $(e.target).find('#clicked').length === 0) {
        napomena = $(this).html();
        $(this).html("").append(
            "<div id='clicked' class='input-group input-group-sm my-auto'><input type='text' id ='napomena_input_clicked' class='form-control' value='" + napomena + "'></div>");
    }

// ************************************ write data to variables on input ************************************ //
    
    const selectKlasaInputClicked = $('#klasa_input_clicked');
    const selectDatumInputClicked = $('#datum_input_clicked');
    const selectNapomenaInputClicked = $('#napomena_input_clicked');

    selectKlasaInputClicked.on('input', function () {
        klasa = selectKlasaInputClicked.val();
        klasaUpdated = true;
    });

    selectKlasaInputClicked.keypress(function (e) {
        if (e.which === 13) {
            selectKlasaInputClicked.replaceWith("<td>" + klasa + "</td>");
        }
    });

    selectDatumInputClicked.on('input', function () {
        datum = selectDatumInputClicked.val();
        datumUpdated = true;
    });

    selectDatumInputClicked.keypress(function (e) {
        if (e.which === 13) {
            selectDatumInputClicked.replaceWith("<td>" + datum + "</td>");
        }
    });
    selectNapomenaInputClicked.on('input', function () {
        napomena = selectNapomenaInputClicked.val();
        napomenaUpdated = true;
    });
    selectNapomenaInputClicked.keypress(function (e) {
        if (e.which === 13) {
            selectNapomenaInputClicked.replaceWith("<td>" + napomena + "</td>");
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
            if (r.success){
                alertify.success(r.message);
            } else{
                alertify.error(r.message);
            }
        },
    });
}