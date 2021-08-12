let racunId;
let racunBr, racunUpdated;
let datumIzdavanja, datumIzdavanjaUpdated;

// ************************************ for mouseclick anywhere - reset ************************************ //

$(document).mousedown(function (e) {

    if (isItEditing) {

        // Broj Racuna
        if (!$(e.target).is('#racun_input_clicked') && !$(e.target).is("#racun_td_clicked")) {
            $("#racun_td_clicked").not(e.target).replaceWith("<td><a href='RacuniElektra/Details/" + racunId + "'>" + racunBr + "</a></td>"); // reset input
            if (racunUpdated) {
                updateDb(1, racunBr);
                racunUpdated = false;
            }
        }

        // Datum izdavanja
        if (!$(e.target).is('#datum_izdavanja_input_clicked') && !$(e.target).is("#datum_izdavanja_td_clicked")) {
            $("#datum_izdavanja_td_clicked").replaceWith("<td>" + datumIzdavanja + "</td>");
            if (datumIzdavanjaUpdated) {
                updateDb(2, datumIzdavanja);
                datumIzdavanjaUpdated = false;
            }
        }

        // get id from closest row to #_input_clicked 
        $('#racun_td_clicked, #datum_izdavanja_td_clicked').on('click', function () {
            const tr = this.closest('tr');
            const table = $('#IndexTable').DataTable();
            racunId = table.row(tr).data().id;
        });
    }
});

// ************************************ for mouseclick on editable columns ************************************ //

$("#IndexTable").on('mousedown', 'tr td', function (e) {

    if (isItEditing) {

        // Broj racuna
        if ($(e.target).is('td:nth-child(2)') && !$(e.target).is("#racun_td_clicked")) {
            $("#racun_td_clicked").not(e.target).replaceWith("<td><a href='RacuniElektra/Details/" + racunId + "'>" + racunBr + "</a></td>"); // reset input
            if (racunUpdated) {
                updateDb(1, racunBr);
                racunUpdated = false;
            }

            $(this).attr('id', 'racun_td_clicked'); // mark this td clicked
            racunBr = $(this).text(); // get value
            

            // set as text input
            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='text' id ='racun_input_clicked' class='form-control' style='height: 25px; margin-top: -7px;margin-bottom: -7px' value='" + racunBr + "'></div>");
        }

        // Datum izdavanja
        if ($(e.target).is('td:nth-child(4)') && !$(e.target).is("#datum_izdavanja_td_clicked")) {
            $("#datum_izdavanja_td_clicked").not(e.target).replaceWith("<td>" + datumIzdavanja + "</td>");
            if (datumIzdavanjaUpdated) {
                updateDb(2, datumIzdavanja);
                datumIzdavanjaUpdated = false;
            }
            $(this).attr('id', 'datum_izdavanja_td_clicked');
            datumIzdavanja = $(this).html();

            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='date' id ='datum_izdavanja_input_clicked' class='form-control' style='height: 25px; margin-top: -7px;margin-bottom: -7px' value='" + datumIzdavanja + "'></div>");
        }
    }

    // ************************************ mark as updated ************************************ //

    const selectRacunTdClicked = $('#racun_td_clicked');
    const selectDatumIzdavanjaTdClicked = $('#datum_izdavanja_td_clicked');

    // Broj racuna
    selectRacunTdClicked.on('input', function () {
        racunBr = $('#racun_input_clicked').val();
        racunUpdated = true;
    });

    // Datum izdavanja
    selectDatumIzdavanjaTdClicked.on('input', function () {
        datumIzdavanja = $('#datum_izdavanja_input_clicked').val();
        datumIzdavanjaUpdated = true;
    });

    // ************************************ on keypress events ************************************ //

    selectRacunTdClicked.keypress(function (e) {
        if (e.which === 13) { // 13 - enter key, 9 - tab key
            selectRacunTdClicked.replaceWith("<td>" + racunBr + "</td>");
            if (racunUpdated) {
                updateDb(1, racunBr);
                racunUpdated = false;
            }
        }
    });

    selectDatumIzdavanjaTdClicked.keypress(function (e) {
        if (e.which === 13) {
            selectDatumIzdavanjaTdClicked.replaceWith("<td>" + datumIzdavanja + "</td>");
            if (datumIzdavanjaUpdated) {
                updateDb(2, datumIzdavanja);
                datumIzdavanjaUpdated = false;
            }
        }
    })
});
