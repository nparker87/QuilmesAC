jQuery(document).ready(function ($) {
    // Import all the variables from the model
    var $vars = $('#Index\\.js').data();

    // Default columns for jqGrid
    var columnModel = [
        { name: "ID", index: "ID", editable: false, hidden: true, editoptions: { readonly: true } },
        { name: "FirstName", index: "FirstName", align: "left", width: 100, editable: true, formoptions: { elmsuffix: "(*)" } },
        { name: "LastName", index: "LastName", align: "left", width: 100, editable: true },
        { name: "Number", index: "Number", align: "left", width: 50, editable: true },
        { name: "Goals", index: "Goal.Count()", align: "left", width: 55, editable: true },
        { name: "Assists", index: "Assist.Count()", align: "left", width: 55, editable: true },
        { name: "YellowCard", index: "Card.Count()", align: "left", width: 50, editable: true },
        { name: "RedCard", index: "Card.Count()", align: "left", width: 50, editable: true },
        { name: "Status", index: "Status.Name", align: "left", width: 100, editable: true }
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
        loadtext: "Retrieving Players...",
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
        postData: {
            statusID: function () { return $("#StatusID").val(); },
            seasonID: function () { return $("#SeasonID").val(); }
        },
        viewrecords: true,
        ondblClickRow: function (id) {
            location.href = $vars.actionEdit + '/' + $("#list").jqGrid('getCell', id, 'ID');
        }
    }).navGrid("#pager", { add: false, edit: false, del: false, search: false, refresh: false })
        .filterToolbar({ stringResult: true, searchOnEnter: false, defaultSearch: "cn" })
        .setGridParam({ datatype: "json" }); // Done this way for default filter values.

    $("#list")[0].triggerToolbar();

    $("#StatusID, #SeasonID").change(function () {
        $("#list").trigger("reloadGrid");
    });
});