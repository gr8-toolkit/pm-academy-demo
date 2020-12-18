## C# constraints on type parameters 

Constraints inform the compiler about the capabilities a type argument must have. Without any constraints, the type argument could be any type. 

```
where T : struct
```
The type argument must be a non-nullable value type.

```
where T : class
```
The type argument must be a reference type (non-nullable reference type in a  nullable context). 

```
where T : class?
```
The type argument must be a reference type, either nullable or non-nullable. This constraint applies also to any class, interface, delegate, or array type.


```
where T : notnull
```
The type argument must be a non-nullable type. 

```
where T : unmanaged
```
The type argument must be a non-nullable unmanaged type.

```
where T : new()
```
The type argument must have a public parameterless constructor.

```
where T : <base class name>
```
The type argument must be or derive from the specified base class. In a nullable context `T` must be a non-nullable reference type derived from the specified base class.

```
where T : <base class name>?
```
The type argument must be or derive from the specified base class.

```
where T : <interface name>
```
The type argument must be or implement the specified interface. In a nullable context `T` must be a non-nullable type that implements the specified interface.

```
where T : <interface name>?
```
The type argument must be or implement the specified interface.


```
where T : U
```
The type argument supplied for `T` must be or derive from the argument supplied for `U`.


---

[Constraints on type parameters (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters)