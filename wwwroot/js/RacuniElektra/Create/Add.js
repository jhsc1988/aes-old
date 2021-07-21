
        function AddNew(brojRacuna,iznos) {
            $.ajax({
                type: "POST",
                url: "/RacuniElektra/AddNewTemp",
                data: {
                    brojRacuna: brojRacuna,
                    iznos: iznos
                },
                success: function (r) {
                    if (r.success) {
                        alertify.success(r.message);
                        table.ajax.reload(null, false);
                    } else {
                        alertify.error(r.message);
                        table.ajax.reload(null, false); // user paging is not reset on reload(callback, resetPaging)
                    }
                },
            });
        }
