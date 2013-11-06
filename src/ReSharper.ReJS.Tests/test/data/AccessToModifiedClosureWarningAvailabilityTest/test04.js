(function() {
    for (var i = 0; i < 10; i++) {
        var x = i;
        setTimeout(function () {
            console.log(x);
        });
    }
})();