(function() {
    var i = 0;
    while (i < 10) {
        setTimeout(function() {
            console.log(i);
        }, 0);
        i++;
    }
})();