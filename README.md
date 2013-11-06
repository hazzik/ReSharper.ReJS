resharper-rejs
==============

Refactorings for JavaScript

###Available quick-fixes
####Detect function invocations with `Function.prototype.call` with the same context as the function's owner.

Example
```javascript
var x = {
  someMethod: function() {
    //something
  }
};

x.someMethod.call(x, 1, 2, 3);
```

The plugin will suggest to replace the `x.someMethod.call(x, 1, 2, 3)` expression with `x.someMethod(1, 2, 3)`

###Available warnings
####Detect access to externally modified clousre.
```javascript
for (var i = 0; i < 10; i++) {
    setTimeout(function() {
        console.log(i); // here we have access to an externally modified closure
    });
}
```

###Installation

Available in [ReSharper Gallery](http://resharper-plugins.jetbrains.com/packages/ReSharper.ReJS/)
