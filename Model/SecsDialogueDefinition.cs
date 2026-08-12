namespace Dreamine.Secs.Abstractions.Model;

/// <summary>
/// \if KO
/// <para>하나의 정상 SECS-II Primary 대화를 정의하는 불변 값입니다.</para>
/// <para>Secondary가 없으면 W0이고, W1 대화의 정상 Secondary는 Primary Function 바로 다음의 짝수 Function이어야 합니다. F0은 특별한 transaction 종료이므로 정상 Secondary로 선언할 수 없습니다.</para>
/// \endif
/// \if EN
/// <para>Defines one immutable normal SECS-II primary dialogue.</para>
/// <para>A missing secondary represents W0. For W1, the normal secondary must be the even function immediately following the primary function. F0 is special transaction termination and cannot be declared as a normal secondary.</para>
/// \endif
/// </summary>
public sealed record SecsDialogueDefinition
{
    /// <summary>
    /// \if KO 검증된 대화 정의를 만듭니다. \endif
    /// \if EN Creates a validated dialogue definition. \endif
    /// </summary>
    /// <param name="stream">\if KO 1부터 127까지의 Stream입니다. \endif \if EN The stream from 1 through 127. \endif</param>
    /// <param name="primaryFunction">\if KO 0이 아닌 홀수 Primary Function입니다. \endif \if EN The nonzero odd primary function. \endif</param>
    /// <param name="secondaryFunction">\if KO W0이면 null이고, W1이면 Primary 바로 다음의 정상 짝수 Secondary Function입니다. \endif \if EN Null for W0, or the normal adjacent even secondary function for W1. \endif</param>
    public SecsDialogueDefinition(
        SecsStream stream,
        SecsFunction primaryFunction,
        SecsFunction? secondaryFunction = null)
    {
        if (stream.Value == 0)
            throw new ArgumentOutOfRangeException(nameof(stream), stream, "A normal SECS-II dialogue requires a stream from 1 through 127.");
        if (!primaryFunction.IsPrimary)
            throw new ArgumentException("A normal SECS-II primary function must be nonzero and odd.", nameof(primaryFunction));
        if (secondaryFunction is { } secondary)
        {
            if (primaryFunction.Value == byte.MaxValue ||
                !secondary.IsSecondary ||
                secondary.Value != primaryFunction.Value + 1)
                throw new ArgumentException("A normal secondary must be the adjacent even function immediately following the primary; F0 is not a normal secondary.", nameof(secondaryFunction));
        }

        Stream = stream;
        PrimaryFunction = primaryFunction;
        SecondaryFunction = secondaryFunction;
    }

    /// <summary>\if KO 대화의 Stream을 가져옵니다. \endif \if EN Gets the dialogue stream. \endif</summary>
    public SecsStream Stream { get; }

    /// <summary>\if KO 0이 아닌 홀수 Primary Function을 가져옵니다. \endif \if EN Gets the nonzero odd primary function. \endif</summary>
    public SecsFunction PrimaryFunction { get; }

    /// <summary>\if KO W0이면 null이고 W1이면 정상 인접 Secondary Function인 값을 가져옵니다. \endif \if EN Gets null for W0 or the normal adjacent secondary function for W1. \endif</summary>
    public SecsFunction? SecondaryFunction { get; }

    /// <summary>\if KO 이 대화가 W1이고 정상 Secondary 응답을 기대하는지 가져옵니다. \endif \if EN Gets whether this is a W1 dialogue expecting a normal secondary reply. \endif</summary>
    public bool ReplyExpected => SecondaryFunction.HasValue;
}
