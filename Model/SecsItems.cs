using System.Collections.ObjectModel;

namespace Dreamine.Secs.Abstractions.Model;

/// <summary>\if KO <para>불변 SECS-II item의 기본 계약입니다.</para> \endif \if EN <para>Defines the base contract of an immutable SECS-II item.</para> \endif</summary>
public abstract class SecsItem
{
    /// <summary>\if KO item format을 가져옵니다. \endif \if EN Gets the item format. \endif</summary>
    public abstract SecsItemFormat Format { get; }
    /// <summary>\if KO 원소 수를 가져옵니다. 목록은 자식 수를 반환합니다. \endif \if EN Gets the element count; lists return their child count. \endif</summary>
    public abstract int Count { get; }
    /// <summary>\if KO 인코딩된 본문 바이트 수를 가져옵니다. \endif \if EN Gets the encoded body byte count. \endif</summary>
    public abstract int BodyLength { get; }
}

/// <summary>\if KO <para>불변 SECS-II 목록 item입니다.</para> \endif \if EN <para>Represents an immutable SECS-II list item.</para> \endif</summary>
public sealed class SecsListItem : SecsItem
{
    private readonly ReadOnlyCollection<SecsItem> _items;

    /// <summary>\if KO <para>자식 item으로 목록을 만듭니다.</para> \endif \if EN <para>Creates a list from child items.</para> \endif</summary>
    /// <param name="items">\if KO 자식 item입니다. \endif \if EN The child items. \endif</param>
    public SecsListItem(params SecsItem[] items)
    {
        ArgumentNullException.ThrowIfNull(items);
        if (Array.Exists(items, static item => item is null)) throw new ArgumentException("List items cannot contain null.", nameof(items));
        _items = Array.AsReadOnly((SecsItem[])items.Clone());
    }

    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.List;
    /// <inheritdoc />
    public override int Count => _items.Count;
    /// <inheritdoc />
    public override int BodyLength
    {
        get
        {
            var length = 0;
            foreach (var item in _items) length = checked(length + GetEncodedLength(item));
            return length;
        }
    }
    /// <summary>\if KO 자식 item의 읽기 전용 목록입니다. \endif \if EN Gets the read-only child-item list. \endif</summary>
    public IReadOnlyList<SecsItem> Items => _items;

    private static int GetEncodedLength(SecsItem item)
    {
        var lengthField = item is SecsListItem list ? list.Count : item.BodyLength;
        var lengthBytes = lengthField <= byte.MaxValue ? 1 : lengthField <= ushort.MaxValue ? 2 : 3;
        return checked(1 + lengthBytes + item.BodyLength);
    }
}

/// <summary>\if KO <para>형식이 지정된 불변 원자 item의 기본 클래스입니다.</para> \endif \if EN <para>Provides the base class for a typed immutable atomic item.</para> \endif</summary>
/// <typeparam name="T">\if KO 원소 형식입니다. \endif \if EN The element type. \endif</typeparam>
public abstract class SecsValueItem<T> : SecsItem
{
    private readonly T[] _values;

    /// <summary>\if KO <para>복사된 원소로 item을 초기화합니다.</para> \endif \if EN <para>Initializes the item with copied elements.</para> \endif</summary>
    /// <param name="values">\if KO 원소입니다. \endif \if EN The elements. \endif</param>
    protected SecsValueItem(T[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        _values = (T[])values.Clone();
    }

    /// <summary>\if KO 원소의 읽기 전용 메모리입니다. \endif \if EN Gets read-only memory containing the elements. \endif</summary>
    public ReadOnlyMemory<T> Values => _values;
    /// <inheritdoc />
    public override int Count => _values.Length;
}

/// <summary>\if KO <para>Binary item입니다.</para> \endif \if EN <para>Represents a Binary item.</para> \endif</summary>
public sealed class SecsBinaryItem : SecsValueItem<byte>
{
    /// <summary>\if KO 값을 만듭니다. \endif \if EN Creates the item. \endif</summary><param name="values">\if KO 값입니다. \endif \if EN Values. \endif</param>
    public SecsBinaryItem(params byte[] values) : base(values) { }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.Binary;
    /// <inheritdoc />
    public override int BodyLength => Count;
}

/// <summary>\if KO <para>Boolean item입니다.</para> \endif \if EN <para>Represents a Boolean item.</para> \endif</summary>
public sealed class SecsBooleanItem : SecsValueItem<bool>
{
    /// <summary>\if KO 값을 만듭니다. \endif \if EN Creates the item. \endif</summary><param name="values">\if KO 값입니다. \endif \if EN Values. \endif</param>
    public SecsBooleanItem(params bool[] values) : base(values) { }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.Boolean;
    /// <inheritdoc />
    public override int BodyLength => Count;
}

/// <summary>\if KO <para>ASCII item입니다.</para> \endif \if EN <para>Represents an ASCII item.</para> \endif</summary>
public sealed class SecsAsciiItem : SecsItem
{
    /// <summary>\if KO ASCII 문자열을 만듭니다. \endif \if EN Creates an ASCII string item. \endif</summary><param name="value">\if KO 문자열입니다. \endif \if EN The string. \endif</param>
    public SecsAsciiItem(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.Any(static character => character > 0x7f)) throw new ArgumentException("The value contains a non-ASCII character.", nameof(value));
        Value = value;
    }
    /// <summary>\if KO 문자열을 가져옵니다. \endif \if EN Gets the string. \endif</summary>
    public string Value { get; }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.Ascii;
    /// <inheritdoc />
    public override int Count => Value.Length;
    /// <inheritdoc />
    public override int BodyLength => Value.Length;
}

/// <summary>\if KO <para>인코딩 변환을 하지 않는 JIS-8 원시 바이트 item입니다.</para> \endif \if EN <para>Represents raw JIS-8 bytes without an implicit encoding conversion.</para> \endif</summary>
public sealed class SecsJis8Item : SecsValueItem<byte>
{
    /// <summary>\if KO 원시 바이트를 만듭니다. \endif \if EN Creates raw bytes. \endif</summary><param name="values">\if KO 값입니다. \endif \if EN Values. \endif</param>
    public SecsJis8Item(params byte[] values) : base(values) { }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.Jis8;
    /// <inheritdoc />
    public override int BodyLength => Count;
}

/// <summary>\if KO <para>I1 item입니다.</para> \endif \if EN <para>Represents an I1 item.</para> \endif</summary>
public sealed class SecsInt8Item : SecsValueItem<sbyte>
{
    /// <summary>\if KO 값을 만듭니다. \endif \if EN Creates the item. \endif</summary><param name="values">\if KO 값입니다. \endif \if EN Values. \endif</param>
    public SecsInt8Item(params sbyte[] values) : base(values) { }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.Int8;
    /// <inheritdoc />
    public override int BodyLength => Count;
}

/// <summary>\if KO <para>I2 item입니다.</para> \endif \if EN <para>Represents an I2 item.</para> \endif</summary>
public sealed class SecsInt16Item : SecsValueItem<short>
{
    /// <summary>\if KO 값을 만듭니다. \endif \if EN Creates the item. \endif</summary><param name="values">\if KO 값입니다. \endif \if EN Values. \endif</param>
    public SecsInt16Item(params short[] values) : base(values) { }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.Int16;
    /// <inheritdoc />
    public override int BodyLength => checked(Count * 2);
}

/// <summary>\if KO <para>I4 item입니다.</para> \endif \if EN <para>Represents an I4 item.</para> \endif</summary>
public sealed class SecsInt32Item : SecsValueItem<int>
{
    /// <summary>\if KO 값을 만듭니다. \endif \if EN Creates the item. \endif</summary><param name="values">\if KO 값입니다. \endif \if EN Values. \endif</param>
    public SecsInt32Item(params int[] values) : base(values) { }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.Int32;
    /// <inheritdoc />
    public override int BodyLength => checked(Count * 4);
}

/// <summary>\if KO <para>I8 item입니다.</para> \endif \if EN <para>Represents an I8 item.</para> \endif</summary>
public sealed class SecsInt64Item : SecsValueItem<long>
{
    /// <summary>\if KO 값을 만듭니다. \endif \if EN Creates the item. \endif</summary><param name="values">\if KO 값입니다. \endif \if EN Values. \endif</param>
    public SecsInt64Item(params long[] values) : base(values) { }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.Int64;
    /// <inheritdoc />
    public override int BodyLength => checked(Count * 8);
}

/// <summary>\if KO <para>U1 item입니다.</para> \endif \if EN <para>Represents a U1 item.</para> \endif</summary>
public sealed class SecsUInt8Item : SecsValueItem<byte>
{
    /// <summary>\if KO 값을 만듭니다. \endif \if EN Creates the item. \endif</summary><param name="values">\if KO 값입니다. \endif \if EN Values. \endif</param>
    public SecsUInt8Item(params byte[] values) : base(values) { }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.UInt8;
    /// <inheritdoc />
    public override int BodyLength => Count;
}

/// <summary>\if KO <para>U2 item입니다.</para> \endif \if EN <para>Represents a U2 item.</para> \endif</summary>
public sealed class SecsUInt16Item : SecsValueItem<ushort>
{
    /// <summary>\if KO 값을 만듭니다. \endif \if EN Creates the item. \endif</summary><param name="values">\if KO 값입니다. \endif \if EN Values. \endif</param>
    public SecsUInt16Item(params ushort[] values) : base(values) { }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.UInt16;
    /// <inheritdoc />
    public override int BodyLength => checked(Count * 2);
}

/// <summary>\if KO <para>U4 item입니다.</para> \endif \if EN <para>Represents a U4 item.</para> \endif</summary>
public sealed class SecsUInt32Item : SecsValueItem<uint>
{
    /// <summary>\if KO 값을 만듭니다. \endif \if EN Creates the item. \endif</summary><param name="values">\if KO 값입니다. \endif \if EN Values. \endif</param>
    public SecsUInt32Item(params uint[] values) : base(values) { }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.UInt32;
    /// <inheritdoc />
    public override int BodyLength => checked(Count * 4);
}

/// <summary>\if KO <para>U8 item입니다.</para> \endif \if EN <para>Represents a U8 item.</para> \endif</summary>
public sealed class SecsUInt64Item : SecsValueItem<ulong>
{
    /// <summary>\if KO 값을 만듭니다. \endif \if EN Creates the item. \endif</summary><param name="values">\if KO 값입니다. \endif \if EN Values. \endif</param>
    public SecsUInt64Item(params ulong[] values) : base(values) { }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.UInt64;
    /// <inheritdoc />
    public override int BodyLength => checked(Count * 8);
}

/// <summary>\if KO <para>F4 item입니다.</para> \endif \if EN <para>Represents an F4 item.</para> \endif</summary>
public sealed class SecsFloat32Item : SecsValueItem<float>
{
    /// <summary>\if KO 값을 만듭니다. \endif \if EN Creates the item. \endif</summary><param name="values">\if KO 값입니다. \endif \if EN Values. \endif</param>
    public SecsFloat32Item(params float[] values) : base(values) { }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.Float32;
    /// <inheritdoc />
    public override int BodyLength => checked(Count * 4);
}

/// <summary>\if KO <para>F8 item입니다.</para> \endif \if EN <para>Represents an F8 item.</para> \endif</summary>
public sealed class SecsFloat64Item : SecsValueItem<double>
{
    /// <summary>\if KO 값을 만듭니다. \endif \if EN Creates the item. \endif</summary><param name="values">\if KO 값입니다. \endif \if EN Values. \endif</param>
    public SecsFloat64Item(params double[] values) : base(values) { }
    /// <inheritdoc />
    public override SecsItemFormat Format => SecsItemFormat.Float64;
    /// <inheritdoc />
    public override int BodyLength => checked(Count * 8);
}
