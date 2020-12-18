```csharp
// System.Nullable<T>
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

[Serializable]
[NonVersionable]
[TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
public struct Nullable<T> where T : struct
{
	private readonly bool hasValue;

	internal T value;

	public bool HasValue
	{
		[NonVersionable]
		readonly get
		{
			return hasValue;
		}
	}

	public T Value
	{
		readonly get
		{
			if (!hasValue)
			{
				ThrowHelper.ThrowInvalidOperationException_InvalidOperation_NoValue();
			}
			return value;
		}
	}

	[NonVersionable]
	public Nullable(T value)
	{
		this.value = value;
		hasValue = true;
	}

	[NonVersionable]
	public readonly T GetValueOrDefault()
	{
		return value;
	}

	[NonVersionable]
	public readonly T GetValueOrDefault(T defaultValue)
	{
		if (!hasValue)
		{
			return defaultValue;
		}
		return value;
	}

	public override bool Equals(object? other)
	{
		if (!hasValue)
		{
			return other == null;
		}
		if (other == null)
		{
			return false;
		}
		return value.Equals(other);
	}

	public override int GetHashCode()
	{
		if (!hasValue)
		{
			return 0;
		}
		return value.GetHashCode();
	}

	public override string? ToString()
	{
		if (!hasValue)
		{
			return "";
		}
		return value.ToString();
	}

	[NonVersionable]
	public static implicit operator T?(T value)
	{
		return value;
	}

	[NonVersionable]
	public static explicit operator T(T? value)
	{
		return value.Value;
	}
}

```