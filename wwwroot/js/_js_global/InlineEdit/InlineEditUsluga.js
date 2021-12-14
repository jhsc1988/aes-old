let datumIzvrsenja, datumIzvrsenjaUpdated;
let usluga, uslugaUpdated;
let iznos, iznosUpdated;
let klasaPlacanja, klasaPlacanjaUpdated;
let datumPotvrde, datumPotvrdeUpdated;
let napomena, napomenaUpdated;

// ************************************ for mouseclick anywhere - reset ************************************ //

$(document).mousedown(function (e) {

    if (isItEditing) {

        // Datum izvrsenja
        if (!$(e.target).is('#datumIzvrsenja_input_clicked') && !$(e.target).is("#datumIzvrsenja_td_clicked")) {
            $("#datumIzvrsenja_td_clicked").replaceWith("<td>" + datumIzvrsenja + "</td>");
            if (datumIzvrsenjaUpdated) {
                updateDb(7, datumIzvrsenja);
                datumIzvrsenjaUpdated = false;
            }
        }

        // Usluga
        if (!$(e.target).is('#usluga_input_clicked') && !$(e.target).is("#usluga_td_clicked")) {
            $("#usluga_td_clicked").replaceWith("<td>" + usluga + "</td>"); // reset input field to td
            if (uslugaUpdated) { // if text has changed - update db
                updateDb(8, usluga);
                uslugaUpdated = false; // reset updated flag
            }
        }

        // Iznos
        //if (!$(e.target).is('#iznos_input_clicked') && !$(e.target).is("#iznos_td_clicked")) {
        //    $("#iznos_td_clicked").replaceWith("<td>" + iznos + "</td>");
        //    if (iznosUpdated) {
        //        updateDb(3, iznos);
        //        iznosUpdated = false;
        //    }
        //}

        // Klasa plaćanja
        if (!$(e.target).is('#klasa_input_clicked') && !$(e.target).is("#klasa_td_clicked")) {
            $("#klasa_td_clicked").replaceWith("<td>" + klasaPlacanja + "</td>"); // reset input field to td
            if (klasaPlacanjaUpdated) { // if text has changed - update db
                updateDb(4, klasaPlacanja);
                klasaPlacanjaUpdated = false; // reset updated flag
            }
        }

        // Datum potvrde
        if (!$(e.target).is('#datumPotvrde_input_clicked') && !$(e.target).is("#datumPotvrde_td_clicked")) {
            $("#datumPotvrde_td_clicked").replaceWith("<td>" + datumPotvrde + "</td>");
            if (datumPotvrdeUpdated) {
                updateDb(5, datumPotvrde);
                datumPotvrdeUpdated = false;
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
        $('#datumIzvrsenja_td_clicked, #usluga_td_clicked, #iznos_td_clicked, #klasa_td_clicked, #datumPotvrde_td_clicked,  #napomena_td_clicked').on('click', function () {
            const tr = this.closest('tr');
            const table = $('#IndexTable').DataTable();
            racunId = table.row(tr).data().id;
        });
    }
});

$("#IndexTable").on('mousedown', 'tr td', function (e) {

    if (isItEditing) {

        // Datum izvrsenja
        if ($(e.target).is('td:nth-child(5)') && !$(e.target).is("#datumIzvrsenja_td_clicked")) {
            $("#datumIzvrsenja_td_clicked").not(e.target).replaceWith("<td>" + datumIzvrsenja + "</td>");

            if (datumIzvrsenjaUpdated) {
                updateDb(7, datumIzvrsenja);
                datumIzvrsenjaUpdated = false;
            }

            $(this).attr('id', 'datumIzvrsenja_td_clicked');
            datumIzvrsenja = $(this).html();

            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='date' id ='datumIzvrsenja_input_clicked' class='form-control' style='height: 25px; margin-top: -7px;margin-bottom: -7px' value='" + datumIzvrsenja + "'></div>");
        }

        // Usluga
        if ($(e.target).is('td:nth-child(6)') && !$(e.target).is("#usluga_td_clicked")) {
            $("#usluga_td_clicked").not(e.target).replaceWith("<td>" + usluga + "</td>");

            if (uslugaUpdated) {
                updateDb(8, usluga);
                uslugaUpdated = false;
            }

            $(this).attr('id', 'usluga_td_clicked');
            usluga = $(this).html();

            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='text' id ='usluga_input_clicked' class='form-control' style='height: 25px; margin-top: -7px;margin-bottom: -7px' value='" + usluga + "'></div>");
        }

        // Iznos
        //if ($(e.target).is('td:nth-child(7)') && !$(e.target).is("#iznos_td_clicked")) {
        //    $("#iznos_td_clicked").not(e.target).replaceWith("<td>" + iznos + "</td>");

        //    if (iznosUpdated) {
        //        updateDb(3, iznos);
        //        iznosUpdated = false;
        //    }

        //    $(this).attr('id', 'iznos_td_clicked');
        //    iznos = $(this).html();

        //    $(this).html("").append(
        //        "<div id='clicked' class='input-group input-group-sm my-auto'>" +
        //        "<input type='text' id ='iznos_input_clicked' class='form-control' style='height: 25px; margin-top: -7px;margin-bottom: -7px' value='" + iznos + "'></div>");
        //}

        // Klasa plaćanja
        if ($(e.target).is('td:nth-child(8)') && !$(e.target).is("#klasa_td_clicked")) {
            $("#klasa_td_clicked").not(e.target).replaceWith("<td>" + klasaPlacanja + "</td>");

            if (klasaPlacanjaUpdated) {
                updateDb(4, klasaPlacanja);
                klasaPlacanjaUpdated = false;
            }

            $(this).attr('id', 'klasa_td_clicked');
            klasaPlacanja = $(this).html();

            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='text' id ='klasa_input_clicked' class='form-control' style='height: 25px; margin-top: -7px;margin-bottom: -7px' value='" + klasaPlacanja + "'></div>");

        }

        // Datum potvrde
        if ($(e.target).is('td:nth-child(9)') && !$(e.target).is("#datumPotvrde_td_clicked")) {
            $("#datumPotvrde_td_clicked").not(e.target).replaceWith("<td>" + datumPotvrde + "</td>");

            if (datumPotvrdeUpdated) {
                updateDb(5, datumPotvrde);
                datumPotvrdeUpdated = false;
            }

            $(this).attr('id', 'datumPotvrde_td_clicked');
            datumPotvrde = $(this).html();

            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='date' id ='datumPotvrde_input_clicked' class='form-control' style='height: 25px; margin-top: -7px;margin-bottom: -7px' value='" + datumPotvrde + "'></div>");
        }

        // Napomena
        if ($(e.target).is('td:nth-child(10)') && !$(e.target).is("#napomena_td_clicked")) {
            $("#napomena_td_clicked").not(e.target).each().replaceWith("<td>" + napomena + "</td>");

            if (napomenaUpdated) {
                updateDb(6, napomena);
                napomenaUpdated = false;
            }

            $(this).attr('id', 'napomena_td_clicked');
            napomena = $(this).html();

            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='text' id ='napomena_input_clicked' class='form-control' style='height: 25px; margin-top: -7px;margin-bottom: -7px' value='" + napomena + "'></div>");
        }
    }

    // ************************************ mark as updated ************************************ //

    const selectDatumIzvrsenjaTdUpdated = $('#datumIzvrsenja_td_clicked');
    const selectUslugaTdClicked = $('#usluga_td_clicked');
    //const selectIznosTdClicked = $('#iznos_td_clicked');
    const selectKlasaTdClicked = $('#klasa_td_clicked');
    const selectDatumPotvrdeTdClicked = $('#datumPotvrde_td_clicked');
    const selectNapomenaTdClicked = $('#napomena_td_clicked');

    // Datum izvrsenja
    selectDatumIzvrsenjaTdUpdated.on('input', function () {
        datumIzvrsenja = $('#datumIzvrsenja_input_clicked').val();
        datumIzvrsenjaUpdated = true;
    });

    // Usluga
    selectUslugaTdClicked.on('input', function () {
        usluga = $('#usluga_input_clicked').val();
        uslugaUpdated = true;
    });

    // Iznos
    //selectIznosTdClicked.on('input', function () {
    //    iznos = $('#iznos_input_clicked').val();
    //    iznosUpdated = true;
    //});

    // Klasa
    selectKlasaTdClicked.on('input', function () {
        klasaPlacanja = $('#klasa_input_clicked').val();
        klasaPlacanjaUpdated = true;
    });

    // Datum potvrde
    selectDatumPotvrdeTdClicked.on('input', function () {
        datumPotvrde = $('#datumPotvrde_input_clicked').val();
        datumPotvrdeUpdated = true;
    });

    // Napomena
    selectNapomenaTdClicked.on('input', function () {
        napomena = $('#napomena_input_clicked').val();
        napomenaUpdated = true;
    });

    // ************************************ on keypress events ************************************ //

    selectDatumIzvrsenjaTdUpdated.keypress(function (e) {
        if (e.which === 13) {
            selectDatumIzvrsenjaTdUpdated.replaceWith("<td>" + datumIzvrsenja + "</td>");
            if (datumIzvrsenjaUpdated) {
                updateDb(7, datumIzvrsenja);
                datumIzvrsenjaUpdated = false;
            }
        }
    });

    selectUslugaTdClicked.keypress(function (e) {
        if (e.which === 13) {
            selectUslugaTdClicked.replaceWith("<td>" + usluga + "</td>");
            if (uslugaUpdated) {
                updateDb(8, usluga);
                uslugaUpdated = false;
            }
        }
    });

    //selectIznosTdClicked.keypress(function (e) {
    //    if (e.which === 13) {
    //        selectIznosTdClicked.replaceWith("<td>" + iznos + "</td>");
    //        if (iznosUpdated) {
    //            updateDb(3, iznos);
    //            iznosUpdated = false;
    //        }
    //    }
    //});

    selectKlasaTdClicked.keypress(function (e) {
        if (e.which === 13) {
            selectKlasaTdClicked.replaceWith("<td>" + klasaPlacanja + "</td>");
            if (klasaPlacanjaUpdated) {
                updateDb(4, klasaPlacanja);
                klasaPlacanjaUpdated = false;
            }
        }
    });

    selectDatumPotvrdeTdClicked.keypress(function (e) {
        if (e.which === 13) {
            selectDatumPotvrdeTdClicked.replaceWith("<td>" + datumPotvrde + "</td>");
            if (datumPotvrdeUpdated) {
                updateDb(5, datumPotvrde);
                datumPotvrdeUpdated = false;
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
