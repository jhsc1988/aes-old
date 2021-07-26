// ************************************ variables ************************************ //

let rId;


let racunbr
let racunUpdated;

let datumIzdavanja
let datumIzdavanjaUpdated;

// for mouseclick anywhere else
$(document).mousedown(function (e) {

    if (isItEditing) {
        elementForEdit = "tr td:nth-child(n+6):not(:last-child)";
    }
    else
        elementForEdit = "tr td:nth-child(n+6)";

    if (isItEditing) {

        // reset elements and update data
        if (!$(e.target).is('#racun_input_clicked') && !$(e.target).is("#racun_td_clicked")) {
            $("#racun_td_clicked").replaceWith("<td>" + racunbr + "</td>"); // reset input field to td
            // if text has changed - update db
            if (racunUpdated) {
                updateDb(racunbr, null); // null checking on controller side
                racunUpdated = false; // reset updated flag
            }
        }

        if (!$(e.target).is('#datum_izdavanja_input_clicked') && !$(e.target).is("#datum_izdavanja_td_clicked")) {
            $("#datum_izdavanja_td_clicked").replaceWith("<td>" + datumIzdavanja + "</td>");
            if (datumIzdavanjaUpdated) {
                updateDb(null, datumIzdavanja, null);
                datumIzdavanjaUpdated = false;
            }
        }


        // get id from closest row to #_input_clicked 
        $('#racun_td_clicked').on('click', function () {
            const tr = this.closest('tr');
            const table = $('#RacunElektraTable').DataTable();
            rId = table.row(tr).data().id;
        });
    }
});

// for mouseclick on specific columns
$("#RacunElektraTable").on('mousedown', "tr td", function (e) {

    if (isItEditing) {

        if ($(e.target).is('td:nth-child(2)') && !$(e.target).is("#racun_td_clicked")) {

            // make all other un-clicked and save them
            $("#racun_td_clicked").not(e.target).replaceWith("<td>" + racunbr + "</td>");
            if (racunUpdated) {
                updateDb(racunbr, null);
                racunUpdated = false;
            }

            $(this).attr('id', 'racun_td_clicked'); // mark this td clicked
            racunbr = $(this).html(); // get value

            // set as text input
            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='text' id ='racun_input_clicked' class='form-control' value='" + racunbr + "'></div>");
        }

        if ($(e.target).is('td:nth-child(4)')) {
            $("#datum_izdavanja_td_clicked").not(e.target).replaceWith("<td>" + datumIzdavanja + "</td>");

            if (datumIzdavanjaUpdated) {
                updateDb(null, datumIzdavanja, null);
                datumIzdavanjaUpdated = false;
            }
            $(this).attr('id', 'datum_izdavanja_td_clicked');
            datumIzdavanja = $(this).html();
            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='date' id ='datum_izdavanja_input_clicked' class='form-control' value='" + datumIzdavanja + "'></div>");
        }
    }


    // ************************************ write data to variables on input ************************************ //

    const selectRacunTdClicked = $('#racun_td_clicked');
    const selectDatumIzdavanjaTdClicked = $('#datum_izdavanja_td_clicked');

    selectRacunTdClicked.on('input', function () {
        racunbr = $('#racun_input_clicked').val();
        racunUpdated = true;
    });

    selectRacunTdClicked.keypress(function (e) {
        if (e.which === 13) { // 13 - enter key, 9 - tab key
            selectRacunTdClicked.replaceWith("<td>" + racunbr + "</td>");
            if (racunUpdated) {
                updateDb(racunbr, null);
                racunUpdated = false;
            }
        }
    });

    selectDatumIzdavanjaTdClicked.keypress(function (e) {
        if (e.which === 13) {
            selectDatumIzdavanjaTdClicked.replaceWith("<td>" + datumIzdavanja + "</td>");
            if (datumIzdavanjaUpdated) {
                updateDb(null, datumIzdavanja, null);
                datumIzdavanjaUpdated = false;
            }
        }
    });

});

// ************************************ update database fn ************************************ //

function updateDb(racunBr, datumIzdavanja) {
    $.ajax({
        type: "POST",
        url: "/RacuniElektra/updateDbForRacunUpdate",
        data: {
            id: rId, // racun id
            racunBr: racunBr,
            datumIzdavanja: datumIzdavanja,
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