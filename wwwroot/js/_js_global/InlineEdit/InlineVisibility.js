const selectDopis = $("#selectDopis");
const isItForFilter = true;
let isItEditing = false;

selectDopis.on('change', function (e) {
    e.preventDefault();
    if (selectDopis.val() === "0" || selectDopis.val() === null) {
        column.visible(false);
        isItEditing = false;
    }
    else {
        column.visible(true);
        isItEditing = true;
    }
});

$("#selectPredmet").on('change', function (e) {
    e.preventDefault();
    if (selectDopis.val() === "0" || selectDopis.val() === null || column.visible() === true) {
        column.visible(false);
        isItEditing = false;
    }
    else {
        column.visible(true);
        isItEditing = true;
    }
});
