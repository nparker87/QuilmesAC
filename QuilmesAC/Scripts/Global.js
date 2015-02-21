$(function () {
    $("#Theme").change(function () {
        less.modifyVars({ '@mainTheme': $(this).val() });
        localStorage.clear();
        less.refresh(true);
    });

    $("#deletePlayer").click(function () {
        return confirm('Delete all data for this player?');
    });

    $('select').change(function () {
        $('#list').trigger("reloadGrid");
    });
})