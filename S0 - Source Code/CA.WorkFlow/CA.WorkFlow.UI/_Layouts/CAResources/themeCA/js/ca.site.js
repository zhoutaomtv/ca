var ca = {
    util: {
        trim: function (str) {
            return str.replace(/(^\s*)|(\s*$)/g, '');
        },
        disableEnterKey: function (e) {
            var key = e.keyCode ? e.keyCode : e.which;
            return key !== 13;
        },
        emptyString: function (str) {
            return $.trim(str).length === 0;
        }
    },
    ui: {
    }
};

/* Code for MOSSMenu */
var flyoutsAllowed = false;
function enableFlyoutsAfterDelay() {
    setTimeout("flyoutsAllowed = true;", 25);
}

function overrideMenu_HoverStatic(item) {
    if (!flyoutsAllowed) {
        setTimeout(delayMenu_HoverStatic(item), 50);
    }
    else {
        var node = Menu_HoverRoot(item);
        var data = Menu_GetData(item);
        if (!data) {
            return;
        }
        __disappearAfter = data.disappearAfter;
        Menu_Expand(node, data.horizontalOffset, data.verticalOffset);
    }
}

function delayMenu_HoverStatic(item) {
    return (function () {
        overrideMenu_HoverStatic(item);
    });
}