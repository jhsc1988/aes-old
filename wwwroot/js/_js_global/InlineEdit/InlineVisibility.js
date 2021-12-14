const selectDopis = $("#selectDopis");
const isItForFilter = true;
let isItEditing = false;

selectDopis.on('change', function (e) {
    e.preventDefault();
    if (selectDopis.val() === "0" || selectDopis.val() === null) {
        table.order([4, 'desc']);
        tableColumn.visible(false);
        isItEditing = false;
        excelButton.disable();
    }
    else {
        table.order([1, 'asc']);
        tableColumn.visible(true);
        isItEditing = true;
        excelButton.enable();
    }
});

$("#selectPredmet").on('change', function (e) {
    e.preventDefault();
    if (selectDopis.val() === "0" || selectDopis.val() === null || tableColumn.visible() === true) {
        table.order([4, 'desc']);
        tableColumn.visible(false);
        isItEditing = false;
        excelButton.disable();
    }
    else {
        table.order([1, 'asc']);
        tableColumn.visible(true);
        isItEditing = true;
        excelButton.enable();
    }
});
