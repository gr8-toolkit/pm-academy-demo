```csharp
private static void CollectionsComparison()
{
    var int32List = new List<int>();
    var strList = new List<string>();
    var objArray = new ArrayList();

    int32List.Add(123);
    strList.Add("123");

    objArray.Add(123);
    objArray.Add(123.456);
    objArray.Add("789");
}
```



```IL
.method private hidebysig static 
	void CollectionsComparison () cil managed 
{
	// Method begins at RVA 0x2064
	// Code size 94 (0x5e)
	.maxstack 2
	.locals init (
		[0] class [System.Collections]System.Collections.Generic.List`1<int32> int32List,
		[1] class [System.Collections]System.Collections.Generic.List`1<string> strList,
		[2] class [System.Runtime]System.Collections.ArrayList int32Array,
		[3] class [System.Runtime]System.Collections.ArrayList strArray
	)

	// {
	IL_0000: nop
	// List<int> list = new List<int>();
	IL_0001: newobj instance void class [System.Collections]System.Collections.Generic.List`1<int32>::.ctor()
	IL_0006: stloc.0
	// List<string> list2 = new List<string>();
	IL_0007: newobj instance void class [System.Collections]System.Collections.Generic.List`1<string>::.ctor()
	IL_000c: stloc.1
	// ArrayList arrayList = new ArrayList();
	IL_000d: newobj instance void [System.Runtime]System.Collections.ArrayList::.ctor()
	IL_0012: stloc.2
	// ArrayList arrayList2 = new ArrayList();
	IL_0013: newobj instance void [System.Runtime]System.Collections.ArrayList::.ctor()
	IL_0018: stloc.3
	// list.Add(123);
	IL_0019: ldloc.0
	IL_001a: ldc.i4.s 123
	IL_001c: callvirt instance void class [System.Collections]System.Collections.Generic.List`1<int32>::Add(!0)
	// list2.Add("123");
	IL_0021: nop
	IL_0022: ldloc.1
	IL_0023: ldstr "123"
	IL_0028: callvirt instance void class [System.Collections]System.Collections.Generic.List`1<string>::Add(!0)
	// arrayList.Add(123);
	IL_002d: nop
	IL_002e: ldloc.2
	IL_002f: ldc.i4.s 123
	IL_0031: box [System.Runtime]System.Int32
	IL_0036: callvirt instance int32 [System.Runtime]System.Collections.ArrayList::Add(object)
	IL_003b: pop
	// arrayList.Add(123.456);
	IL_003c: ldloc.2
	IL_003d: ldc.r8 123.456
	IL_0046: box [System.Runtime]System.Double
	IL_004b: callvirt instance int32 [System.Runtime]System.Collections.ArrayList::Add(object)
	IL_0050: pop
	// arrayList.Add("789");
	IL_0051: ldloc.2
	IL_0052: ldstr "789"
	IL_0057: callvirt instance int32 [System.Runtime]System.Collections.ArrayList::Add(object)
	IL_005c: pop
	// }
	IL_005d: ret
} // end of method Program::CollectionsComparison

```
