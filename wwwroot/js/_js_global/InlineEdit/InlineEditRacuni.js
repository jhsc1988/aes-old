let klasa, klasaUpdated;
let datumPotvrde;
let datumPotvrdeUpdated;
let napomena, napomenaUpdated;
let iznos, iznosUpdated;

// ************************************ for mouseclick anywhere - reset ************************************ //

$(document).mousedown(function (e) {

    if (isItEditing) {

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
            if (klasaUpdated) { // if text has changed - update db
                updateDb(4, klasa);
                klasaUpdated = false; // reset updated flag
            }
        }

        // Datum potvrde
        if (!$(e.target).is('#datumPotvrde_input_clicked') && !$(e.target).is("#datumPotvrde_td_clicked")) {
            if (datumPotvrde === "") {
                $("#datumPotvrde_td_clicked").replaceWith("<td>" + datumPotvrde + "</td>");
            } else
                $("#datumPotvrde_td_clicked").replaceWith("<td>" + moment(datumPotvrde).format("DD.MM.YYYY") + "</td>");
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
        $('#iznos_td_clicked, #klasa_td_clicked, #datumPotvrde_td_clicked, #napomena_td_clicked').on('click', function () {
            const tr = this.closest('tr');
            const table = $('#IndexTable').DataTable();
            racunId = table.row(tr).data().id;
        });
    }
});

// ************************************ for mouseclick on editable columns ************************************ //

$("#IndexTable").on('mousedown', 'tr td', function (e) {

    if (isItEditing) {

        // Iznos
        //if ($(e.target).is('td:nth-child(5)') && !$(e.target).is("#iznos_td_clicked")) {
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
                "<input type='text' id ='klasa_input_clicked' class='form-control' style='height: 25px; margin-top: -7px;margin-bottom: -7px' value='" + klasa + "'></div>");

        }

        // Datum potvrde
        if ($(e.target).is('td:nth-child(7)') && !$(e.target).is("#datumPotvrde_td_clicked")) {
            if (datumPotvrde === "")
                datumPotvrde = "";
            else if (datumPotvrde !== null)
                datumPotvrde = moment(datumPotvrde).format("DD.MM.YYYY");
            $("#datumPotvrde_td_clicked").not(e.target).replaceWith("<td>" + datumPotvrde + "</td>");
            if (datumPotvrdeUpdated) {
                updateDb(5, datumPotvrde);
                datumPotvrdeUpdated = false;
            }
            $(this).attr('id', 'datumPotvrde_td_clicked');
            let datumUnformated = $(this).html();
            datumUnformated = datumUnformated.split(".");
            let dateString = "";
            if (datumUnformated.length > 1) {
                let date = new Date(datumUnformated[2] + "-" + datumUnformated[1] + "-" + datumUnformated[0]);
                let month = date.getMonth() + 1;
                let day = datumUnformated[0];
                month = (month < 10 ? '0' : '') + month;
                dateString = date.getFullYear() + "-" + month + "-" + day;
            }
            datumPotvrde = dateString;

            $(this).html("").append(
                "<div id='clicked' class='input-group input-group-sm my-auto'>" +
                "<input type='date' id ='datumPotvrde_input_clicked' class='form-control' style='height: 25px; margin-top: -7px;margin-bottom: -7px' value='" + dateString + "'></div>");
        }

        // Napomena
        if ($(e.target).is('td:nth-child(8)') && !$(e.target).is("#napomena_td_clicked")) {
            $("#napomena_td_clicked").not(e.target).replaceWith("<td>" + napomena + "</td>");

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

    const selectKlasaTdClicked = $('#klasa_td_clicked');
    const selectDatumTdClicked = $('#datumPotvrde_td_clicked');
    const selectNapomenaTdClicked = $('#napomena_td_clicked');
    //const selectIznosTdClicked = $('#iznos_td_clicked');

    // Iznos
    //selectIznosTdClicked.on('input', function () {
    //    iznos = $('#iznos_input_clicked').val();
    //    iznosUpdated = true;
    //});

    // Klasa
    selectKlasaTdClicked.on('input', function () {
        klasa = $('#klasa_input_clicked').val();
        klasaUpdated = true;
    });

    // Datum potvrde
    selectDatumTdClicked.on('input', function () {
        datumPotvrde = $('#datumPotvrde_input_clicked').val();
        datumPotvrdeUpdated = true;
    });

    // Napomena
    selectNapomenaTdClicked.on('input', function () {
        napomena = $('#napomena_input_clicked').val();
        napomenaUpdated = true;
    });

    // ************************************ on keypress events ************************************ //

    //selectIznosTdClicked.keypress(function (e) {
    //    if (e.which === 13) { // 13 - enter key, 9 - tab key
    //        selectIznosTdClicked.replaceWith("<td>" + iznos + "</td>");
    //        if (iznosUpdated) {
    //            updateDb(3, iznos);
    //            iznosUpdated = false;
    //        }
    //    }
    //});

    selectKlasaTdClicked.keypress(function (e) {
        if (e.which === 13) {
            selectKlasaTdClicked.replaceWith("<td>" + klasa + "</td>");
            if (klasaUpdated) {
                updateDb(4, klasa);
                klasaUpdated = false;
            }
        }
    });


    selectDatumTdClicked.keypress(function (e) {
        if (e.which === 13) {
            selectDatumTdClicked.replaceWith("<td>" + datumPotvrde + "</td>");
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


