namespace Dreamine.Secs.Abstractions.Model;

/// <summary>\if KO <para>E5-0813에서 확인한 SECS-II item format code입니다.</para> \endif \if EN <para>Lists SECS-II item format codes confirmed from E5-0813.</para> \endif</summary>
public enum SecsItemFormat : byte
{
    /// <summary>\if KO 목록입니다. \endif \if EN List. \endif</summary>
    List = 0,
    /// <summary>\if KO 이진 바이트입니다. \endif \if EN Binary bytes. \endif</summary>
    Binary = 8,
    /// <summary>\if KO 논리값입니다. \endif \if EN Boolean values. \endif</summary>
    Boolean = 9,
    /// <summary>\if KO ASCII 문자입니다. \endif \if EN ASCII characters. \endif</summary>
    Ascii = 16,
    /// <summary>\if KO JIS-8 원시 바이트입니다. \endif \if EN Raw JIS-8 bytes. \endif</summary>
    Jis8 = 17,
    /// <summary>\if KO 8바이트 부호 있는 정수입니다. \endif \if EN Eight-byte signed integers. \endif</summary>
    Int64 = 24,
    /// <summary>\if KO 1바이트 부호 있는 정수입니다. \endif \if EN One-byte signed integers. \endif</summary>
    Int8 = 25,
    /// <summary>\if KO 2바이트 부호 있는 정수입니다. \endif \if EN Two-byte signed integers. \endif</summary>
    Int16 = 26,
    /// <summary>\if KO 4바이트 부호 있는 정수입니다. \endif \if EN Four-byte signed integers. \endif</summary>
    Int32 = 28,
    /// <summary>\if KO 8바이트 부동소수점입니다. \endif \if EN Eight-byte floating-point values. \endif</summary>
    Float64 = 32,
    /// <summary>\if KO 4바이트 부동소수점입니다. \endif \if EN Four-byte floating-point values. \endif</summary>
    Float32 = 36,
    /// <summary>\if KO 8바이트 부호 없는 정수입니다. \endif \if EN Eight-byte unsigned integers. \endif</summary>
    UInt64 = 40,
    /// <summary>\if KO 1바이트 부호 없는 정수입니다. \endif \if EN One-byte unsigned integers. \endif</summary>
    UInt8 = 41,
    /// <summary>\if KO 2바이트 부호 없는 정수입니다. \endif \if EN Two-byte unsigned integers. \endif</summary>
    UInt16 = 42,
    /// <summary>\if KO 4바이트 부호 없는 정수입니다. \endif \if EN Four-byte unsigned integers. \endif</summary>
    UInt32 = 44
}
