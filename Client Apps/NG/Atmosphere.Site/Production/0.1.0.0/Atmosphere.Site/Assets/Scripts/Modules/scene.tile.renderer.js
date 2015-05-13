﻿var tileRenderer = function () {

    var render = function (tileDiv, sceneId) {
        if (sceneId == null) {
            sceneId = 0;
        }
        var size = tileDiv.data().scenes[sceneId].size
        var template = tileDiv.data().templates[size]
        var formatterFunc = tileDiv.data().formatter;
        
        tileDiv.html(template);
        if (formatterFunc != null) {
            formatterFunc(tileDiv);
        }
    };

    return {
        render: render
    };

} ();




