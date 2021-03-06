﻿jQuery(document).ready(function ($) {
	// Import all the variables from the model
	var $vars = $('#Match\\.js').data();

	// Default columns for jqGrid
	var columnModel = [
        { name: "ID", index: "ID", editable: false, hidden: true, editoptions: { readonly: true } },
        { displayname: "Season", name: "SeasonID", index: "Season.DisplayName", hidden: true, align: "left", width: "120px", editable: true, formoptions: { elmsuffix: "(*)" } },
        { displayname: "Match Day", name: "MatchDay", index: "MatchDay", align: "center", width: "55px", editable: true },
        { displayname: "Match Date", name: "MatchDate", index: "MatchDate", align: "left", width: "60px", editable: true },
        { displayname: "Opponent", name: "Opponent", index: "Opponent.Name", align: "left", width: "150px", editable: true, formoptions: { elmsuffix: "(*)" } },
        { displayname: "GF", name: "GoalsFor", index: "GoalsFor", align: "center", width: "20px", editable: true },
        { displayname: "GA", name: "GoalsAgainst", index: "GoalsAgainst", align: "center", width: "20px", editable: true },
        { displayname: "Result", name: "Result", index: "Result", align: "center", width: "45px", editable: true }
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
		loadtext: "Retrieving Matches...",
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
		postData: {
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

	$("#SeasonID").change(function () {
		$("#list").trigger("reloadGrid");
	});
});