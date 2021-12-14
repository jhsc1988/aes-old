$(document).ready(function () {
    InitDragAndDrop();
    DragDropOperation();
});
function InitDragAndDrop() {
    $(".content-bg").on("dragenter", function (evt) {
        evt.preventDefault();
        evt.stopPropagation();
    });
    $(".content-bg").on("dragover", function (evt) {
        evt.preventDefault();
        evt.stopPropagation();
    });
    $(".content-bg").on("drop", function (evt) {
        evt.preventDefault();
        evt.stopPropagation();
    });
}
function DragDropOperation() {
    $('.content-bg').on("drop", function (evt) {
        evt.preventDefault();
        evt.stopPropagation();
        var files = evt.originalEvent.dataTransfer.files;
        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append(files[i].name, files[i]);
        }
        alertify.success("Uploading and processing data ...");
        $.ajax({
            type: "POST",
            url: uploadActionUrl,
            contentType: false,
            processData: false,
            data: data,
            success: function (r) {
                if (r.success) {
                    alertify.success(r.message);
                }
                else {
                    alertify.error(r.message);
                }
                table.ajax.reload(null, false);
            },
            error: function (r) {
                alertify.error(r.message);
                table.ajax.reload(null, false);
            }
        })
    })
}
