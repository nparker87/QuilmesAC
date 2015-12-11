jQuery(document).ready(function ($) {
    // Import all the variables from the model
    var $vars = $('#Index\\.js').data();

    // Default columns for jqGrid
    var columnModel = [
                { name: "ID", index: "ID", editable: false, hidden: true, editoptions: { readonly: true } },
                { displayname: "Username", name: "Username", index: "Username", align: "left", width: "50px" },
                { displayname: "Email", name: "Email", index: "Email", align: "left", width: "95px" }
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
        loadtext: "Retrieving Users...",
        mtype: "POST",
        forceFit: true,
        autowidth: false,
        sortable: true,
        width: "500",
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
        viewrecords: true,
        ondblClickRow: function (id) {
            location.href = $vars.actionEdit + '/' + $("#list").jqGrid('getCell', id, 'ID');
        }
    }).navGrid("#pager", { add: false, edit: false, del: false, search: false, refresh: false })
        .filterToolbar({ stringResult: true, searchOnEnter: false, defaultSearch: "cn" })
        .setGridParam({ datatype: "json" }); // Done this way for default filter values.

    $("#list")[0].triggerToolbar();
});