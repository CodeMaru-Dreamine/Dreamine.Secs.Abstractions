namespace Dreamine.Secs.Abstractions.Model;

/// <summary>\if KO <para>SECS 데이터 메시지의 세션(장치) 식별자입니다.</para> \endif \if EN <para>Identifies the session (device) of a SECS data message.</para> \endif</summary>
public readonly record struct SecsSessionId
{
    /// <summary>\if KO <para>허용되는 최댓값입니다.</para> \endif \if EN <para>The maximum permitted value.</para> \endif</summary>
    public const ushort MaximumValue = 32767;

    /// <summary>\if KO <para>검증된 식별자를 만듭니다.</para> \endif \if EN <para>Creates a validated identifier.</para> \endif</summary>
    /// <param name="value">\if KO 값입니다. \endif \if EN The value. \endif</param>
    public SecsSessionId(ushort value)
    {
        if (value > MaximumValue) throw new ArgumentOutOfRangeException(nameof(value));
        Value = value;
    }

    /// <summary>\if KO <para>16비트 값을 가져옵니다.</para> \endif \if EN <para>Gets the 16-bit value.</para> \endif</summary>
    public ushort Value { get; }

    /// <summary>\if KO <para>기본 정수로 변환합니다.</para> \endif \if EN <para>Converts to the underlying integer.</para> \endif</summary>
    public static implicit operator ushort(SecsSessionId value) => value.Value;
}

/// <summary>\if KO <para>0부터 127까지의 SECS 스트림 번호입니다.</para> \endif \if EN <para>Represents a SECS stream number from 0 through 127.</para> \endif</summary>
public readonly record struct SecsStream
{
    /// <summary>\if KO <para>검증된 스트림 번호를 만듭니다.</para> \endif \if EN <para>Creates a validated stream number.</para> \endif</summary>
    /// <param name="value">\if KO 스트림 번호입니다. \endif \if EN The stream number. \endif</param>
    public SecsStream(byte value)
    {
        if (value > 127) throw new ArgumentOutOfRangeException(nameof(value));
        Value = value;
    }

    /// <summary>\if KO <para>스트림 번호를 가져옵니다.</para> \endif \if EN <para>Gets the stream number.</para> \endif</summary>
    public byte Value { get; }

    /// <summary>\if KO <para>기본 바이트로 변환합니다.</para> \endif \if EN <para>Converts to the underlying byte.</para> \endif</summary>
    public static implicit operator byte(SecsStream value) => value.Value;
}

/// <summary>\if KO <para>0부터 255까지의 SECS 함수 번호입니다.</para> \endif \if EN <para>Represents a SECS function number from 0 through 255.</para> \endif</summary>
public readonly record struct SecsFunction
{
    /// <summary>\if KO <para>함수 번호를 만듭니다.</para> \endif \if EN <para>Creates a function number.</para> \endif</summary>
    /// <param name="value">\if KO 함수 번호입니다. \endif \if EN The function number. \endif</param>
    public SecsFunction(byte value) => Value = value;

    /// <summary>\if KO <para>함수 번호를 가져옵니다.</para> \endif \if EN <para>Gets the function number.</para> \endif</summary>
    public byte Value { get; }

    /// <summary>\if KO <para>주 메시지인지 나타냅니다.</para> \endif \if EN <para>Gets whether this is a primary-message function.</para> \endif</summary>
    public bool IsPrimary => Value != 0 && (Value & 1) == 1;

    /// <summary>\if KO <para>부 메시지인지 나타냅니다.</para> \endif \if EN <para>Gets whether this is a secondary-message function.</para> \endif</summary>
    public bool IsSecondary => Value != 0 && (Value & 1) == 0;

    /// <summary>\if KO <para>기본 바이트로 변환합니다.</para> \endif \if EN <para>Converts to the underlying byte.</para> \endif</summary>
    public static implicit operator byte(SecsFunction value) => value.Value;
}

/// <summary>\if KO <para>세션별 트랜잭션 상관관계 값입니다.</para> \endif \if EN <para>Represents a session-scoped transaction correlation value.</para> \endif</summary>
public readonly record struct SecsSystemBytes
{
    /// <summary>\if KO <para>시스템 바이트 값을 만듭니다.</para> \endif \if EN <para>Creates a system-bytes value.</para> \endif</summary>
    /// <param name="value">\if KO 32비트 값입니다. \endif \if EN The 32-bit value. \endif</param>
    public SecsSystemBytes(uint value) => Value = value;

    /// <summary>\if KO <para>32비트 값을 가져옵니다.</para> \endif \if EN <para>Gets the 32-bit value.</para> \endif</summary>
    public uint Value { get; }

    /// <summary>\if KO <para>기본 정수로 변환합니다.</para> \endif \if EN <para>Converts to the underlying integer.</para> \endif</summary>
    public static implicit operator uint(SecsSystemBytes value) => value.Value;
}
