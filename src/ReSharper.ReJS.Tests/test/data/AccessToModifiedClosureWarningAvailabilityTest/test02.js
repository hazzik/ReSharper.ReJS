(function() {
    for (var i = 0; i < 10; i++) {
        var callback = function() {
            console.log(i);
        };
        setTimeout(callback);
    }
})();