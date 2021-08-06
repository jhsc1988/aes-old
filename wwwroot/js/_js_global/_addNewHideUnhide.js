let hidden = true;
$("#aAddNew").click(function () {
    if (hidden) {
        $("#predmetiAddNew").show("slow");
        hidden = false;
    }

    else {
        $("#predmetiAddNew").hide("slow");
        hidden = true;
    }
});