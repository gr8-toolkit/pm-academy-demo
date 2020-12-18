## C# covariance & contravariance

**Covariance** allows you to use a derived class where a base class is expected. 
Covariance in delegates allows you to assign a method to the delegate that has a less derived return type.

In the examples `Factory<DerivedClass>` cannot be explicitly converted to `Factory<BaseClass>`. You can enable this by making the generic type parameter `T` with the `out` keyword.

**Cotravariance** allows a method with the parameter of a base class to be assigned to a delegate that expects the parameter of a derived class. You can enable this by making the generic type parameter `T` with the `in` keyword.

```csharp
delegate T Factory<out T>();
delegate void Action<in T>(T p);

void Foo()
{
    Factory<DerivedClass> derivedMaker = MakeDerivedInst;
    // covariance enables this assignment
    Factory<BaseClass> baseMaker = derivedMaker;

    Action<BaseClass> baseInstAction = BaseAction;
    // contrvariance enables this assignment
    Action<DerivedClass> derivedInstAction = baseInstAction;
}

DerivedClass MakeDerivedInst() => new DerivedClass();

BaseClass MakeBaseInst() => new BaseClass();

void BaseAction(BaseClass p) => Console.WriteLine("Base");

void DerivedAction(DerivedClass p) => Console.WriteLine("Derived");

class BaseClass { }

class DerivedClass : BaseClass { }
```