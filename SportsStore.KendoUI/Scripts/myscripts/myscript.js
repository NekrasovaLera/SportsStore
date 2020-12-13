function onSelect(e) {
    var grid = $("#products").data("kendoGrid");
    var treeview = $("#treeview").data("kendoTreeView");
    var n = e.node;
    var dataItem = treeview.dataItem(n);

    var data = grid.dataSource.data();
    var dataLength = data.length;

    for (var i = 0; i < dataLength; i++) {
        var currntItem = data[i];
        var a = currntItem.Category.CatName;
        var b = dataItem.text;
        if (a !== b) {
            grid.dataSource.pushDestroy(currntItem);
            i -= 1;
            dataLength -= 1;
        }
    }
}

$(document).ready(function () {
    var grid = $("#grid");
    grid.find(".k-grid-toolbar").on("click", ".k-pager-refresh", function (e) {
        e.preventDefault();
        grid.data("kendoGrid").dataSource.read();
    });

});

function categoriesChange() {
    var value = this.value(),
        grid = $("#grid").data("kendoGrid");

    if (value) {
        grid.dataSource.filter({ field: "CategoryID", operator: "eq", value: parseInt(value) });
    } else {
        grid.dataSource.filter({});
    }
}