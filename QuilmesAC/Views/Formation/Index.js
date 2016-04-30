jQuery(document).ready(function ($) {
    function updateDialog(url) {
        return {
            url: url,
            closeAfterAdd: true,
            closeAfterEdit: true,
            modal: true
        };
    }

    // Import all the variables from the model
    var $vars = $('#Index\\.js').data();

    // Default columns for jqGrid
    var columnModel = [
        { name: "ID", index: "ID", editable: false, hidden: true, editoptions: { readonly: true } },
        { displayname: "Name", name: "Name", index: "Name", align: "left", width: 200 }
    ];

    var columnNames = [];
    for (i = 0; i < columnModel.length; i++) {
        columnNames[i] = (columnModel[i].hasOwnProperty('displayname') ? columnModel[i].displayname : columnModel[i].name);
    }

    jQuery("#list").jqGrid({
        url: $vars.url,
        datatype: "local",
        gridview: true,
        height: "100%",
        hidegrid: false,
        loadtext: "Retrieving Formations...",
        mtype: "POST",
        forceFit: true,
        autowidth: false,
        sortable: true,
        width: "610px",
        cellLayout: 6, // gets rid of annoying horizontal scrollbar
        colNames: columnNames,
        colModel: columnModel,
        cellEdit: false,
        pager: "#pager",
        rowList: [15, 50, 100],
        sortname: $vars.sortBy,
        sortorder: $vars.sortOrder,
        page: $vars.page,
        rowNum: $vars.rows,
        viewrecords: true
    }).navGrid("#pager", { add: true, edit: true, del: true, search: true, refresh: true },
            updateDialog("Formation/EditFormation"),
            updateDialog("Formation/AddFormation"),
            updateDialog("Formation/DeleteFormation"))
        .filterToolbar({ stringResult: true, searchOnEnter: false, defaultSearch: "cn" })
        .setGridParam({ datatype: "json" }); // Done this way for default filter values.

    $("#list")[0].triggerToolbar();
});