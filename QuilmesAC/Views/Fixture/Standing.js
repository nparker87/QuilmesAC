jQuery(document).ready(function ($) {
    // Import all the variables from the model
    var $vars = $('#Standing\\.js').data();

    // Default columns for jqGrid
    var columnModel = [
        // index must be the column name for sorting with orderby
        { name: "ID", index: "ID", editable: false, hidden: true, editoptions: { readonly: true } },
        { displayname: "Season", name: "SeasonID", index: "Season.DisplayName", hidden: true, align: "left", width: "50px", editable: false },
        { displayname: "League Table", name: "OpponentID", index: "Opponent.Name", align: "left", width: "150px", editable: false },
        { displayname: "GP", name: "GamesPlayed", index: "GamesPlayed", align: "center", width: "20px", editable: false },
        { displayname: "W", name: "Win", index: "Win", align: "center", width: "20px", editable: true },
        { displayname: "D", name: "Draw", index: "Draw", align: "center", width: "20px", editable: true },
        { displayname: "L", name: "Loss", index: "Loss", align: "center", width: "20px", editable: true },
        { displayname: "GF", name: "GoalsFor", index: "GoalsFor", align: "center", width: "20px", editable: false },
        { displayname: "GA", name: "GoalsAgainst", index: "GoalsAgainst", align: "center", width: "20px", editable: false },
        { displayname: "Diff.", name: "Difference", index: "Difference", align: "center", width: "20px", editable: false },
        { displayname: "Pts.", name: "Points", index: "Points", align: "center", width: "20px", editable: false }
    ];

    var columnNames = [];
    for (i = 0; i < columnModel.length; i++) {
        columnNames[i] = (columnModel[i].hasOwnProperty('displayname') ? columnModel[i].displayname : columnModel[i].name);
    }

    jQuery("#standingList").jqGrid({
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
        pager: "#standingPager",
        rowList: [15, 50, 100],
        sortname: $vars.sortBy,
        sortorder: $vars.sortOrder,
        page: $vars.page,
        rowNum: $vars.rows,
        postData: {
            seasonID: function () { return $("#SeasonID").val(); }
        },
        viewrecords: true,
    }).navGrid("#standingPager", { add: false, edit: false, del: false, search: false, refresh: false })
        .filterToolbar({ stringResult: true, searchOnEnter: false, defaultSearch: "cn" })
        .setGridParam({ datatype: "json" }); // Done this way for default filter values.

    $("#standingList")[0].triggerToolbar();

    highlightQuilmes($("#SeasonID").val());

    $("#SeasonID").change(function () {
        $("#standingList").trigger("reloadGrid");
        highlightQuilmes($("#SeasonID").val());
    });
});

function highlightQuilmes(seasonID) {
    $.ajax({
        type: "POST",
        url: "/Fixture/GetQuilmesStandingID",
        data: { seasonID: seasonID },
        success: function (data) {
            var quilmesRow = "#standingTable tr#" + data;
            $(quilmesRow).addClass("ui-state-highlight");
        }
    });
}

$(function () {
    highlightQuilmes($("#SeasonID").val());
});