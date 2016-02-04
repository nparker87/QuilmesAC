﻿$(function () {
    var hexDigits = new Array
        ("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f");

    //Function to convert hex format to a rgb color
    function rgb2Hex(rgb) {
        rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
        return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
    }

    function hex(x) {
        return isNaN(x) ? "00" : hexDigits[(x - x % 16) / 16] + hexDigits[x % 16];
    }

    function modifyTheme(color) {
        less.modifyVars({
            '@mainTheme': color
        });
        less.refreshStyles();
    }

    $("#deletePlayer").click(function () {
        return confirm('Delete all data for this player?');
    });

    $(".theme").click(function () {
        var color = $(this).css('backgroundColor');
        modifyTheme(rgb2Hex(color));
    });
})