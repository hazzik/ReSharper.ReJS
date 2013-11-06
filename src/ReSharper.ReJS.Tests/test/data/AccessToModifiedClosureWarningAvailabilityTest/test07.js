(function() {
    var i = 0;
    do {
        setTimeout(function() {
            console.log(i);
        }, 0);
        i++;
    } while (i < 10);
})();