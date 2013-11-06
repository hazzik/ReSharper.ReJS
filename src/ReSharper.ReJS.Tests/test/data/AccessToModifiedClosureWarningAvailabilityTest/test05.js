(function () {
    var a = [1, 2, 3];
    for (var k in a) {
        var i = a[k];
        setTimeout(function () {
            console.log(i);
        }, 0);
    }
})();