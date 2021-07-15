function stanDetailsDatatables(stanid) {
    // table head rendering fix: 
    // https://datatables.net/forums/discussion/48422/table-header-not-displaying-correctly-initially#Comment_128514
    // https://datatables.net/examples/api/tabs_and_scrolling.html

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $.fn.dataTable.tables({visible: true, api: true}).columns.adjust();
    });

    $('#RacunElektraTable').DataTable({
        "ajax": {
            "url": "/Stanovi/GetRacuniForStan",
            "type": "POST",
            "datatype": "json",
            "data": {stanid: stanid}
        },

        // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
        // koristi se kao selector (nije posve jasna dokumentacija)
        "columns": [
            {
                "data": "brojRacuna", "name": "brojRacuna",
                // "render": function (data, type, row, meta) {
                //     return '<a href="RacuniElektra/Details/' + data.id + '">' + data.brojRacuna + '</a>';
                // }
            },
            {
                "data": "elektraKupac.ugovorniRacun", "name": "elektraKupac.ugovorniRacun",
                // "render": function (data, type, row, meta) {
                //     return '<a href="RacuniElektra/Details/' + data.elektraKupac.id + '">' + data.elektraKupac.ugovorniRacun + '</a>';
                // }
            },
            {"data": "datumIzdavanja", "name": "datumIzdavanja"},
            {
                "data": "iznos", "name": "iznos",
                "render": $.fn.dataTable.render.number('.', ',', 2, '')
            },
            {"data": "klasaPlacanja", "name": "klasaPlacanja"},
            {"data": "datumPotvrde", "name": "datumPotvrde"},
            {"data": "napomena", "name": "napomena"},
        ],
        "paging": true,
        "serverSide": true,
        "order": [[2, 'asc']], // default sort po datumu
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim..."
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // BrojRacuna
                "render": $.fn.dataTable.render.ellipsis(19),
            },
            {
                "targets": 1, // UgovorniRacun
                "render": $.fn.dataTable.render.ellipsis(10),
            },
            {
                "targets": 2, // DatumIzdavanja
                "render": function (data, type, row) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 3, // Iznos
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 4, // KlasaPlacanja
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 5, // Datum potvrde
                "render": $.fn.dataTable.render.ellipsis(11),
            },
            {
                "targets": 6, // Napomena
                "render": $.fn.dataTable.render.ellipsis(30),
            },
        ]
    });


    $('#RacunElektraRateTable').DataTable({
        "ajax": {
            "url": "/Stanovi/GetRacuniRateForStan",
            "type": "POST",
            "datatype": "json",
            "data": {stanid: stanid}
        },
        // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
        // koristi se kao selector (nije posve jasna dokumentacija)
        "columns": [
            {
                "data": null, "name": "brojRacuna",
                "render": function (data, type, row, meta) {
                    return '<a href="RacuniElektraRate/Details/' + data.id + '">' + data.brojRacuna + '</a>';
                }
            },
            {
                "data": null, "name": "elektraKupac.ugovorniRacun",
                "render": function (data, type, row, meta) {
                    return '<a href="ElektraKupci/Details/' + data.elektraKupac.id + '">' + data.elektraKupac.ugovorniRacun + '</a>';
                }
            },
            {"data": "razdoblje", "name": "razdoblje"},
            {
                "data": "iznos", "name": "iznos",
                "render": $.fn.dataTable.render.number('.', ',', 2, '')
            },
            {"data": "klasaPlacanja", "name": "klasaPlacanja"},
            {"data": "datumPotvrde", "name": "datumPotvrde"},
            {"data": "napomena", "name": "napomena"},
        ],
        "paging": true,
        "serverSide": true,
        "order": [[2, 'asc']], // default sort po datumu
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim..."
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // BrojRacuna
                "render": $.fn.dataTable.render.ellipsis(19),
            },
            {
                "targets": 1, // UgovorniRacun
                "render": $.fn.dataTable.render.ellipsis(10),
            },
            {
                "targets": 2, // Razdoblje
                "render": function (data, type, row) {
                    return moment(data).format("MM.YYYY")
                }
            },
            {
                "targets": 3, // Iznos
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 4, // KlasaPlacanja
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 5, // Napomena
                "render": $.fn.dataTable.render.ellipsis(30),
            },
        ]
    });


    $('#RacunHoldingTable').DataTable({

        "ajax": {
            "url": "/Stanovi/GetHoldingRacuniForStan",
            "type": "POST",
            "datatype": "json",
            "data": {stanid: stanid}
        },
        // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
        // koristi se kao selector (nije posve jasna dokumentacija)
        "columns": [
            {
                "data": null, "name": "brojRacuna",
                "render": function (data, type, row, meta) {
                    return '<a href="RacuniHolding/Details/' + data.id + '">' + data.brojRacuna + '</a>';
                }
            },
            {
                "data": null, "name": "stan.sifraObjekta",
                "render": function (data, type, row, meta) {
                    return '<a href="Stanovi/Details/' + data.stan.id + '">' + data.stan.sifraObjekta + '</a>';
                }
            }, {
                "data": null, "name": "stan.stanId",
                "render": function (data, type, row, meta) {
                    return '<a href="Stanovi/Details/' + data.stan.id + '">' + data.stan.stanId + '</a>';
                }
            },

            {"data": "datumIzdavanja", "name": "datumIzdavanja"},
            {
                "data": "iznos", "name": "iznos",
                "render": $.fn.dataTable.render.number('.', ',', 2, '')
            },

            {"data": "klasaPlacanja", "name": "klasaPlacanja"},
            {"data": "datumPotvrde", "name": "datumPotvrde"},
            {"data": "napomena", "name": "napomena"},
        ],
        "paging": true,
        "serverSide": true,
        "order": [[3, 'asc']], // default sort po datumu
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim..."
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // BrojRacuna
                "render": $.fn.dataTable.render.ellipsis(19),
            },
            {
                "targets": 1, // Šifra objekta
                "render": $.fn.dataTable.render.ellipsis(10),
            }, {
                "targets": 2, // Stan ID
                "render": $.fn.dataTable.render.ellipsis(10),
            },
            {
                "targets": 3, // DatumIzdavanja
                "render": function (data, type, row) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 4, // Iznos
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 5, // KlasaPlacanja
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 6, // Napomena
                "render": $.fn.dataTable.render.ellipsis(30),
            },
        ]
    });
    $('#RacunOdsIzvrsenjaUslugeTable').DataTable({
        "ajax": {
            "url": "/Stanovi/GetRacuniOdsIzvrsenjeForStan",
            "type": "POST",
            "datatype": "json",
            "data": {stanid: stanid}
        },
        // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
        // koristi se kao selector (nije posve jasna dokumentacija)
        "columns": [
            {
                "data": null, "name": "brojRacuna",
                "render": function (data, type, row, meta) {
                    return '<a href="RacunOdsIzvrsenjaUsluge/Details/' + data.id + '">' + data.brojRacuna + '</a>';
                }
            },
            {
                "data": null, "name": "odsKupac.sifraKupca",
                "render": function (data, type, row, meta) {
                    return '<a href="OdsKupci/Details/' + data.odsKupac.id + '">' + data.odsKupac.sifraKupca + '</a>';
                }
            },
            {"data": "datumIzdavanja", "name": "datumIzdavanja"},
            {"data": "datumIzvrsenja", "name": "datumIzvrsenja"},
            {"data": "usluga", "name": "usluga"},
            {
                "data": "iznos", "name": "iznos",
                "render": $.fn.dataTable.render.number('.', ',', 2, '')
            },
            {"data": "klasaPlacanja", "name": "klasaPlacanja"},
            {"data": "datumPotvrde", "name": "datumPotvrde"},
            {"data": "napomena", "name": "napomena"},
        ],
        "paging": true,
        "serverSide": true,
        "order": [[2, 'asc']], // default sort po datumu
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim..."
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // BrojRacuna
                "render": $.fn.dataTable.render.ellipsis(19),
            },
            {
                "targets": 1, // Šifra kupca
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 2, // DatumIzdavanja
                "render": function (data, type, row) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 3, // DatumIzvrsenja
                "render": function (data, type, row) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 4, // Usluga
                "render": $.fn.dataTable.render.ellipsis(30),
            }, {
                "targets": 5, // Iznos
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 6, // KlasaPlacanja
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 7, // Napomena
                "render": $.fn.dataTable.render.ellipsis(30),
            },
        ]
    });

    $('#RacunElektraIzvrsenjeUslugeTable').DataTable({
        "ajax": {
            "url": "/Stanovi/GetRacuniElektraIzvrsenjeForStan",
            "type": "POST",
            "datatype": "json",
            "data": {stanid: stanid}
        },
        // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
        // koristi se kao selector (nije posve jasna dokumentacija)
        "columns": [
            {
                "data": null, "name": "brojRacuna",
                "render": function (data, type, row, meta) {
                    return '<a href="RacuniElektra/Details/' + data.id + '">' + data.brojRacuna + '</a>';
                }
            },
            {
                "data": null, "name": "elektraKupac.ugovorniRacun",
                "render": function (data, type, row, meta) {
                    return '<a href="RacuniElektra/Details/' + data.elektraKupac.id + '">' + data.elektraKupac.ugovorniRacun + '</a>';
                }
            },
            {"data": "datumIzdavanja", "name": "datumIzdavanja"},
            {"data": "datumIzvrsenja", "name": "datumIzvrsenja"},
            {"data": "usluga", "name": "usluga"},
            {
                "data": "iznos", "name": "iznos",
                "render": $.fn.dataTable.render.number('.', ',', 2, '')
            },
            {"data": "klasaPlacanja", "name": "klasaPlacanja"},
            {"data": "datumPotvrde", "name": "datumPotvrde"},
            {"data": "napomena", "name": "napomena"},
        ],
        "paging": true,
        "serverSide": true,
        "order": [[2, 'asc']], // default sort po datumu
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim..."
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // BrojRacuna
                "render": $.fn.dataTable.render.ellipsis(19),
            },
            {
                "targets": 1, // UgovorniRacun
                "render": $.fn.dataTable.render.ellipsis(10),
            },
            {
                "targets": 2, // DatumIzdavanja
                "render": function (data, type, row) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 3, // DatumIzvrsenja
                "render": function (data, type, row) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "targets": 4, // Usluga
                "render": $.fn.dataTable.render.ellipsis(30),
            }, {
                "targets": 5, // Iznos
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 6, // KlasaPlacanja
                "render": $.fn.dataTable.render.ellipsis(20),
            },
            {
                "targets": 7, // Napomena
                "render": $.fn.dataTable.render.ellipsis(30),
            },
        ]
    });

    $('#UgovoriOKoristenjuTable').DataTable({
        "ajax": {
            "url": "/Stanovi/GetUgovoriOKoristenjuForStan",
            "type": "POST",
            "datatype": "json",
            "data": {stanid: stanid}
        },
        // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
        // koristi se kao selector (nije posve jasna dokumentacija)
        "columns": [
            {"data": "brojUgovora", "name": "brojUgovora"},
            {"data": "ods.omm", "name": "ods.omm"},
            {
                "data": null, "name": "datumPotpisaHEP",
                "render": function (data, type, row) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "data": null, "name": "datumPotpisaGZ",
                "render": function (data, type, row) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {"data": "dopis.predmet.klasa", "name": "dopis.predmet.klasa"},
            {"data": "rbrUgovora", "name": "rbrUgovora"},
            {"data": "dopisDostave.predmet.klasa", "name": "dopisDostave.predmet.klasa"},
            {"data": "rbrDostave", "name": "rbrDostave"},
        ],
        "paging": true,
        "serverSide": true,
        "order": [[2, 'asc']], // default sort po datumu potpisa HEP-a
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim..."
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // Broj ugovora
                "render": $.fn.dataTable.render.ellipsis(14),
            }, {
                "targets": 1, // OMM
                "render": $.fn.dataTable.render.ellipsis(8),
            },
            {
                "targets": 3, // Klasa ugovora
                "render": $.fn.dataTable.render.ellipsis(21),
            },
            {
                "targets": 4, // RBR ugovora
                "render": $.fn.dataTable.render.ellipsis(3),
            },
            {
                "targets": 5, // Klasa dostave
                "render": $.fn.dataTable.render.ellipsis(21),
            },
            {
                "targets": 6, // RBR dostave
                "render": $.fn.dataTable.render.ellipsis(3),
            },
        ]
    });

    $('#UgovoriOPrijenosuTable').DataTable({
        "ajax": {
            "url": "/Stanovi/GetUgovoriOPrijenosuForStan",
            "type": "POST",
            "datatype": "json",
            "data": {stanid: stanid}
        },
        // name mi treba za filter u controlleru - taj se parametar pretražuje po nazivu
        // koristi se kao selector (nije posve jasna dokumentacija)
        "columns": [
            {"data": "brojUgovora", "name": "brojUgovora"},
            {"data": "ugovorOKoristenju.brojUgovora", "name": "ugovorOKoristenju.brojUgovora"},
            {
                "data": null, "name": "datumPrijenosa",
                "render": function (data, type, row) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {
                "data": null, "name": "datumPotpisa",
                "render": function (data, type, row) {
                    return moment(data).format("DD.MM.YYYY")
                }
            },
            {"data": "kupac", "name": "kupac"},
            {"data": "kupacOIB", "name": "kupacOIB"},
            {"data": "dopis.predmet.klasa", "name": "dopis.predmet.klasa"},
            {"data": "rbrUgovora", "name": "rbrUgovora"},
            {"data": "dopisDostave.predmet.klasa", "name": "dopisDostave.predmet.klasa"},
            {"data": "rbrDostave", "name": "rbrDostave"},
        ],
        "paging": true,
        "serverSide": true,
        "order": [[2, 'asc']], // default sort po datumu potpisa HEP-a
        "bLengthChange": false,
        //"processing": true,
        "language": {
            "processing": "tražim..."
        },
        "scrollX": true,
        "columnDefs": [
            {
                "targets": 0, // Broj ugovora
                "render": $.fn.dataTable.render.ellipsis(14),
            },
            {
                "targets": 1, // Broj ugovora o korištenju
                "render": $.fn.dataTable.render.ellipsis(14),
            },
            {
                "targets": 4, // Kupac
                "render": $.fn.dataTable.render.ellipsis(40),
            },
            {
                "targets": 5, // Kupac OIB
                "render": $.fn.dataTable.render.ellipsis(13),
            },
            {
                "targets": 6, // Klasa ugovora
                "render": $.fn.dataTable.render.ellipsis(21),
            },
            {
                "targets": 7, // RBR ugovora
                "render": $.fn.dataTable.render.ellipsis(3),
            },
            {
                "targets": 8, // Klasa dostave
                "render": $.fn.dataTable.render.ellipsis(21),
            },
            {
                "targets": 9, // RBR dostave
                "render": $.fn.dataTable.render.ellipsis(3),
            },
        ]
    });
}