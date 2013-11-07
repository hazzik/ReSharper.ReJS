var x = {
    someMethod: function () {
    },
    otherMethod: function() {
        this./*{caret}*/someMethod.call(this);
    }
};