'use strict';

$(function () {
    const cssActive = "active pc-trigger"

    // auto active menu in NAV

    const menuLevel1 = $(".pc-item .active").parents("ul");
    menuLevel1.parents("li").first().addClass(cssActive);
    menuLevel1.removeAttr('style');

    const menuLevel2 = $(".pc-item .active").parents("ul").parents("li").parents("ul");
    menuLevel2.parents("li").first().addClass(cssActive);
    menuLevel2.removeAttr('style');
    

    //var a = $(".pc-item .active").parents("ul").first().attr("class");
    //alert(a);
});