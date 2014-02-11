resharper-rejs
==============

Refactorings for JavaScript

###Available quick-fixes
####Convert object's property access from reference form to indexed form and vise-versa

Example

```javascript
var z = x.y; // => var z = x['y'];
var z = x['y']; // => var z = x.y;
```

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

####Remove unreachable code

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

###Donations

Donations are welcome to 

* BTC: 19woiHcAZqDBLDAsi5QDVGwqxdaQawwt6J
* LTC: LP3wMjumuutC45MVwqbNitavUXFqAD8YjU
