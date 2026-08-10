namespace Dreamine.Secs.Abstractions.Providers;

/// <summary>
/// \if KO
/// <para>설정 저장과 공급자 등록에 사용하는 안정적인 SECS 공급자 키를 제공합니다.</para>
/// \endif
/// \if EN
/// <para>Provides stable SECS provider keys for configuration persistence and provider registration.</para>
/// \endif
/// </summary>
public static class SecsProviderKeys
{
    /// <summary>
    /// \if KO
    /// <para>Dreamine 자체 공급자의 키입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The key reserved for the Dreamine native provider.</para>
    /// \endif
    /// </summary>
    public const string Dreamine = "dreamine";

    /// <summary>
    /// \if KO
    /// <para>향후 별도 Linkgenesis 어댑터가 등록할 키입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The key reserved for a future separately packaged Linkgenesis adapter.</para>
    /// \endif
    /// </summary>
    public const string Linkgenesis = "linkgenesis";

    /// <summary>
    /// \if KO
    /// <para>향후 별도 EnviaSoft 어댑터가 등록할 키입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The key reserved for a future separately packaged EnviaSoft adapter.</para>
    /// \endif
    /// </summary>
    public const string EnviaSoft = "enviasoft";
}
