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