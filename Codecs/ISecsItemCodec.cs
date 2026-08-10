using Dreamine.Secs.Abstractions.Model;
using Dreamine.Secs.Abstractions.Validation;

namespace Dreamine.Secs.Abstractions.Codecs;

/// <summary>\if KO <para>네트워크와 독립적인 SECS-II item 변환 계약입니다.</para> \endif \if EN <para>Defines network-independent SECS-II item conversion.</para> \endif</summary>
public interface ISecsItemCodec
{
    /// <summary>\if KO item을 wire byte로 인코딩합니다. \endif \if EN Encodes an item to wire bytes. \endif</summary>
    /// <param name="item">\if KO item입니다. \endif \if EN The item. \endif</param><returns>\if KO 인코딩 결과입니다. \endif \if EN Encoded bytes. \endif</returns>
    byte[] Encode(SecsItem item);

    /// <summary>\if KO 전체 입력을 하나의 item으로 디코딩합니다. \endif \if EN Decodes all input as one item. \endif</summary>
    /// <param name="data">\if KO wire data입니다. \endif \if EN Wire data. \endif</param><returns>\if KO 디코딩된 item입니다. \endif \if EN Decoded item. \endif</returns>
    SecsItem Decode(ReadOnlyMemory<byte> data);

    /// <summary>\if KO 예외 없이 입력을 검증합니다. \endif \if EN Validates input without throwing a protocol exception. \endif</summary>
    /// <param name="data">\if KO wire data입니다. \endif \if EN Wire data. \endif</param><returns>\if KO 검증 결과입니다. \endif \if EN Validation result. \endif</returns>
    SecsValidationResult Validate(ReadOnlyMemory<byte> data);
}
