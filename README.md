resharper-rejs
==============

Refactorings for JavaScript

###Available quick-fixes
####Detect function call with the same context as function owner.

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
