let racunId;
let klasa, klasaUpdated;
let datum, datumUpdated;
let napomena, napomenaUpdated;
let racunBr, racunUpdated;
let datumIzdavanja, datumIzdavanjaUpdated;
let iznos, iznosUpdated;

// ************************************ for mouseclick anywhere - reset ************************************ //

$(document).mousedown(function (e) {

    if (isItEditing) {

        // Broj Racuna
        if (!$(e.target).is('#racun_input_clicked') && !$(e.target).is("#racun_td_clicked")) {
            $("#racun_td_clicked").replaceWith("<td>" + racunBr + "</td>");
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

        // Iznos
        if (!$(e.target).is('#iznos_input_clicked') && !$(e.target).is("#iznos_td_clicked")) {
            $("#iznos_td_clicked").replaceWith("<td>" + iznos + "</td>");
            if (iznosUpdated) {
                updateDb(3, iznos);
                iznosUpdated = false;
            }
        }

        // Klasa
        if (!$(e.target).is('#klasa_input_clicked') && !$(e.target).is("#klasa_td_clicked")) {
            $("#klasa_td_clicked").replaceWith("<td>" + klasa + "</td>"); // reset input field to td
            // if text has changed - update db
            if (klasaUpdated) {
                updateDb(4, klasa);
                klasaUpdated = false; // reset updated flag
            }
        }

        // Datum potvrde
        if (!$(e.target).is('#datum_input_clicked') && !$(e.target).is("#datum_td_clicked")) {
            $("#datum_td_clicked").replaceWith("<td>" + datum + "</td>");
            if (datumUpdated) {
                updateDb(5, datum);
                datumUpdated = false;
            }
        }

        // Napomena
        if (!$(e.target).is('#napomena_input_clicked') && !$(e.target).is("#napomena_td_clicked")) {
            $("#napomena_td_clicked").replaceWith("<td>" + napomena + "</td>");
            if (napomenaUpdated) {
                updateDb(6, napomena);
                napomenaUpdated = false;
            }
        }

        // get id from closest row to #_input_clicked 
        $('#klasa_td_clicked, #datum_td_clicked, #napomena_td_clicked, #racun_td_clicked, #iznos_td_clicked, #datum_izdavanja_td_clicked').on('click', function () {
            const tr = this.closest('tr');
            const table = $('#RacunElektraTable').DataTable();
            racunId = table.row(tr).data().id;
        });
    }
});

// ************************************ for mouseclick on editable columns ************************************ //

$("#RacunElektraTable").on('mousedown', 'tr td', function (e) {

    if (isItEditing) {

        // Broj racuna
        if ($(e.target).is('td:nth-child(2)') && !$(e.target).is("#racun_td_clicked")) {
            $("#racun_td_clicked").not(e.target).replaceWith("<td>" + racunBr + "</td>"); // reset input
            if (racunUpdated) {
                updateDb(1, racunBr);
                racunUpdated = false;
            }

            $(this).attr('id', 'racun_td_clicked'); // mark this td clicked
            racunBr = $(this).html(); // get value

            // set as text input
            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='text' id ='racun_input_clicked' class='form-control' value='" + racunBr + "'></div>");
        }

        // Datum izdavanja
        if ($(e.target).is('td:nth-child(4)')) {
            $("#datum_izdavanja_td_clicked").not(e.target).replaceWith("<td>" + datumIzdavanja + "</td>");
            if (datumIzdavanjaUpdated) {
                updateDb(2, datumIzdavanja);
                datumIzdavanjaUpdated = false;
            }
            $(this).attr('id', 'datum_izdavanja_td_clicked');
            datumIzdavanja = $(this).html();

            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='date' id ='datum_izdavanja_input_clicked' class='form-control' value='" + datumIzdavanja + "'></div>");
        }

        // Iznos
        if ($(e.target).is('td:nth-child(5)')) {
            $("#iznos_td_clicked").not(e.target).replaceWith("<td>" + iznos + "</td>");
            if (iznosUpdated) {
                updateDb(3, iznos);
                iznosUpdated = false;
            }

            $(this).attr('id', 'iznos_td_clicked');
            iznos = $(this).html();

            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='text' id ='iznos_input_clicked' class='form-control' value='" + iznos + "'></div>");
        }

        // Klasa
        if ($(e.target).is('td:nth-child(6)') && !$(e.target).is("#klasa_td_clicked")) {
            $("#klasa_td_clicked").not(e.target).replaceWith("<td>" + klasa + "</td>");
            if (klasaUpdated) {
                updateDb(4, klasa);
                klasaUpdated = false;
            }

            $(this).attr('id', 'klasa_td_clicked');
            klasa = $(this).html();

            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='text' id ='klasa_input_clicked' class='form-control' value='" + klasa + "'></div>");
        }

        // Datum potvrde
        if ($(e.target).is('td:nth-child(7)')) {
            $("#datum_td_clicked").not(e.target).replaceWith("<td>" + datum + "</td>");

            if (datumUpdated) {
                updateDb(5, datum);
                datumUpdated = false;
            }
            $(this).attr('id', 'datum_td_clicked');
            datum = $(this).html();
            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='date' id ='datum_input_clicked' class='form-control' value='" + datum + "'></div>");
        }

        // Napomena
        if ($(e.target).is('td:nth-child(8)')) {
            $("#napomena_td_clicked").not(e.target).each().replaceWith("<td>" + napomena + "</td>");
            $(this).attr('id', 'napomena_td_clicked');

            if (napomenaUpdated) {
                updateDb(6, napomena);
                napomenaUpdated = false;
            }

            napomena = $(this).html();
            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='text' id ='napomena_input_clicked' class='form-control' value='" + napomena + "'></div>");
        }
    }

    // ************************************ mark as updated ************************************ //

    const selectKlasaTdClicked = $('#klasa_td_clicked');
    const selectDatumTdClicked = $('#datum_td_clicked');
    const selectNapomenaTdClicked = $('#napomena_td_clicked');
    const selectRacunTdClicked = $('#racun_td_clicked');
    const selectDatumIzdavanjaTdClicked = $('#datum_izdavanja_td_clicked');
    const selectIznosTdClicked = $('#iznos_td_clicked');

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

    // Iznos
    selectIznosTdClicked.on('input', function () {
        iznos = $('#iznos_input_clicked').val();
        iznosUpdated = true;
    });

    // Klasa
    selectKlasaTdClicked.on('input', function () {
        klasa = $('#klasa_input_clicked').val();
        klasaUpdated = true;
    });

    // Datum potvrde
    selectDatumTdClicked.on('input', function () {
        datum = $('#datum_input_clicked').val();
        datumUpdated = true;
    });

    // Napomena
    selectNapomenaTdClicked.on('input', function () {
        napomena = $('#napomena_input_clicked').val();
        napomenaUpdated = true;
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
    });

    selectIznosTdClicked.keypress(function (e) {
        if (e.which === 13) { // 13 - enter key, 9 - tab key
            selectIznosTdClicked.replaceWith("<td>" + iznos + "</td>");
            if (iznosUpdated) {
                updateDb(3, iznos);
                iznosUpdated = false;
            }
        }
    });

    selectKlasaTdClicked.keypress(function (e) {
        if (e.which === 13) { // 13 - enter key, 9 - tab key
            selectKlasaTdClicked.replaceWith("<td>" + klasa + "</td>");
            if (klasaUpdated) {
                updateDb(4, klasa);
                klasaUpdated = false;
            }
        }
    });

    
    selectDatumTdClicked.keypress(function (e) {
        if (e.which === 13) {
            selectDatumTdClicked.replaceWith("<td>" + datum + "</td>");
            if (datumUpdated) {
                updateDb(5, datum);
                datumUpdated = false;
            }
        }
    });

    selectNapomenaTdClicked.keypress(function (e) {
        if (e.which === 13) {
            selectNapomenaTdClicked.replaceWith("<td>" + napomena + "</td>");
            if (napomenaUpdated) {
                updateDb(6, napomena);
                napomenaUpdated = false;
            }
        }
    });
});

/**
 * Saves data to DB by Ajax POST method
 * @param {any} updatedColumn Number of visible column
 * @param {any} x edited text
 */
function updateDb(updatedColumn, x) {
    $.ajax({
        type: "POST",
        url: "/RacuniElektra/updateDbForInline",
        data: {
            id: racunId, // racun id
            updatedColumn: updatedColumn,
            x: x,
        },
        success: function (r) {
            if (r.success) {
                alertify.success(r.message);
            } else {
                alertify.error(r.message);
                table.ajax.reload(null, false); // user paging is not reset on reload(callback, resetPaging)
            }
        },
    });
}
