jQuery(document).ready(function ($) {
    // Import all the variables from the model
    var $vars = $('#Edit\\.js').data();

    // Default columns for jqGrid
    var columnModel = [
        { name: "ID", index: "ID", editable: false, hidden: true, editoptions: { readonly: true } },
        { displayname: "Season", name: "Season", index: "Season.DisplayName", hidden: true, editable: true, edittype: "select", editoptions: { dataUrl: "/GetSelectList")" }, formoptions: { elmsuffix: "(*)" } },
            { displayname: "", name: "Opponent", index: "Opponent.Name", align: "left", editable: true, formoptions: { elmsuffix: "(*)" } },
{ displayname: "GP", name: "GamesPlayed", index: "GamesPlayed", align: "left", editable: true, formoptions: { elmsuffix: "(*)" } },
{ displayname: "W", name: "Win", index: "Win", align: "left", editable: true },
{ displayname: "D", name: "Draw", index: "Draw", align: "left", editable: true },
{ displayname: "L", name: "Loss", index: "Loss", align: "left", editable: true },
{ displayname: "GF", name: "GoalsFor", index: "GoalsFor", align: "left", editable: true, formoptions: { elmsuffix: "(*)" } },
{ displayname: "GA", name: "GoalsAgainst", index: "GoalsAgainst", align: "left", editable: true, formoptions: { elmsuffix: "(*)" } },
{ displayname: "Diff", name: "Difference", index: "Difference", align: "left", editable: false, formoptions: { elmsuffix: "(*)" } },
{ displayname: "Pts.", name: "Points", index: "Points", align: "left", editable: false, formoptions: { elmsuffix: "(*)" } }
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
    loadtext: "Retrieving League Table...",
    mtype: "POST",
    forceFit: true,
    autowidth: false,
    sortable: true,
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
}).navGrid("#pager", { add: true, edit: true, del: true, search: false, refresh: true })
    .filterToolbar({ stringResult: true, searchOnEnter: false, defaultSearch: "cn" })
    .setGridParam({ datatype: "json" }); // Done this way for default filter values.

$("#list")[0].triggerToolbar();
})