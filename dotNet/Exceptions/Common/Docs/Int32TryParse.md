```csharp
// System.Int32
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

public static bool TryParse([NotNullWhen(true)] string? s, out int result)
{
	if (s == null)
	{
		result = 0;
		return false;
	}
	return Number.TryParseInt32IntegerStyle(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;
}

```


```csharp
// System.Number
using System.Globalization;

internal static ParsingStatus TryParseInt32IntegerStyle(ReadOnlySpan<char> value, NumberStyles styles, NumberFormatInfo info, out int result)
{
	int i;
	int num;
	if (!value.IsEmpty)
	{
		i = 0;
		num = value[0];
		if ((styles & NumberStyles.AllowLeadingWhite) == 0 || !IsWhite(num))
		{
			goto IL_0048;
		}
		while (true)
		{
			i++;
			if ((uint)i >= (uint)value.Length)
			{
				break;
			}
			num = value[i];
			if (IsWhite(num))
			{
				continue;
			}
			goto IL_0048;
		}
	}
	goto IL_025a;
	IL_0170:
	if (IsDigit(num))
	{
		goto IL_017b;
	}
	goto IL_026a;
	IL_0262:
	result = 0;
	return ParsingStatus.Overflow;
	IL_017b:
	int num2 = num - 48;
	i++;
	int num3 = 0;
	while (num3 < 8)
	{
		if ((uint)i >= (uint)value.Length)
		{
			goto IL_024e;
		}
		num = value[i];
		if (IsDigit(num))
		{
			i++;
			num2 = 10 * num2 + num - 48;
			num3++;
			continue;
		}
		goto IL_026a;
	}
	if ((uint)i >= (uint)value.Length)
	{
		goto IL_024e;
	}
	num = value[i];
	bool flag;
	int num4;
	if (IsDigit(num))
	{
		i++;
		flag = num2 > 214748364;
		num2 = num2 * 10 + num - 48;
		flag = flag || (uint)num2 > int.MaxValue + ((uint)num4 >> 31);
		if ((uint)i >= (uint)value.Length)
		{
			goto IL_024b;
		}
		num = value[i];
		while (IsDigit(num))
		{
			flag = true;
			i++;
			if ((uint)i < (uint)value.Length)
			{
				num = value[i];
				continue;
			}
			goto IL_0262;
		}
	}
	goto IL_026a;
	IL_024e:
	result = num2 * num4;
	return ParsingStatus.OK;
	IL_0048:
	num4 = 1;
	if ((styles & NumberStyles.AllowLeadingSign) != 0)
	{
		if (info.HasInvariantNumberSigns)
		{
			if (num == 45)
			{
				num4 = -1;
				i++;
				if ((uint)i >= (uint)value.Length)
				{
					goto IL_025a;
				}
				num = value[i];
			}
			else if (num == 43)
			{
				i++;
				if ((uint)i >= (uint)value.Length)
				{
					goto IL_025a;
				}
				num = value[i];
			}
		}
		else
		{
			value = value.Slice(i);
			i = 0;
			string positiveSign = info.PositiveSign;
			string negativeSign = info.NegativeSign;
			if (!string.IsNullOrEmpty(positiveSign) && value.StartsWith(positiveSign))
			{
				i += positiveSign.Length;
				if ((uint)i >= (uint)value.Length)
				{
					goto IL_025a;
				}
				num = value[i];
			}
			else if (!string.IsNullOrEmpty(negativeSign) && value.StartsWith(negativeSign))
			{
				num4 = -1;
				i += negativeSign.Length;
				if ((uint)i >= (uint)value.Length)
				{
					goto IL_025a;
				}
				num = value[i];
			}
		}
	}
	flag = false;
	num2 = 0;
	if (IsDigit(num))
	{
		if (num != 48)
		{
			goto IL_017b;
		}
		while (true)
		{
			i++;
			if ((uint)i >= (uint)value.Length)
			{
				break;
			}
			num = value[i];
			if (num == 48)
			{
				continue;
			}
			goto IL_0170;
		}
		goto IL_024e;
	}
	goto IL_025a;
	IL_026a:
	if (IsWhite(num))
	{
		if ((styles & NumberStyles.AllowTrailingWhite) == 0)
		{
			goto IL_025a;
		}
		for (i++; i < value.Length && IsWhite(value[i]); i++)
		{
		}
		if ((uint)i >= (uint)value.Length)
		{
			goto IL_024b;
		}
	}
	if (TrailingZeros(value, i))
	{
		goto IL_024b;
	}
	goto IL_025a;
	IL_024b:
	if (!flag)
	{
		goto IL_024e;
	}
	goto IL_0262;
	IL_025a:
	result = 0;
	return ParsingStatus.Failed;
}

```