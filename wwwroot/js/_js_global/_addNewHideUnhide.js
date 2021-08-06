$("#aAddNew").click(function () {
    if (hidden) {
        $("#AddNew").show("slow");
        hidden = false;
    }

    else {
        $("#AddNew").hide("slow");
        hidden = true;
    }
});