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

The call `x.someMethod.call(x, 1, 2, 3)` will be replaced to `x.someMethod(1, 2, 3)`

###Installation

Available in [ReSharper Gallery](http://resharper-plugins.jetbrains.com/packages/ReSharper.ReJS/)
