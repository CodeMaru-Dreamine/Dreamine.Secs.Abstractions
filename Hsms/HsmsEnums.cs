namespace Dreamine.Secs.Abstractions.Hsms;

/// <summary>\if KO <para>HSMS 메시지 세션 형식입니다.</para> \endif \if EN <para>Identifies an HSMS session type.</para> \endif</summary>
public enum HsmsSType : byte
{
    /// <summary>\if KO 데이터 메시지입니다. \endif \if EN Data message. \endif</summary>
    Data = 0,
    /// <summary>\if KO 선택 요청입니다. \endif \if EN Select request. \endif</summary>
    SelectRequest = 1,
    /// <summary>\if KO 선택 응답입니다. \endif \if EN Select response. \endif</summary>
    SelectResponse = 2,
    /// <summary>\if KO 선택 해제 요청입니다. \endif \if EN Deselect request. \endif</summary>
    DeselectRequest = 3,
    /// <summary>\if KO 선택 해제 응답입니다. \endif \if EN Deselect response. \endif</summary>
    DeselectResponse = 4,
    /// <summary>\if KO 링크 테스트 요청입니다. \endif \if EN Link-test request. \endif</summary>
    LinktestRequest = 5,
    /// <summary>\if KO 링크 테스트 응답입니다. \endif \if EN Link-test response. \endif</summary>
    LinktestResponse = 6,
    /// <summary>\if KO 거절 요청입니다. \endif \if EN Reject request. \endif</summary>
    RejectRequest = 7,
    /// <summary>\if KO 분리 요청입니다. \endif \if EN Separate request. \endif</summary>
    SeparateRequest = 9
}

/// <summary>\if KO <para>HSMS 연결의 명시적 상태입니다.</para> \endif \if EN <para>Represents the explicit state of an HSMS connection.</para> \endif</summary>
public enum HsmsConnectionState
{
    /// <summary>\if KO TCP 연결이 없습니다. \endif \if EN No TCP connection exists. \endif</summary>
    NotConnected,
    /// <summary>\if KO TCP 연결은 있으나 선택되지 않았습니다. \endif \if EN TCP is connected but not selected. \endif</summary>
    ConnectedNotSelected,
    /// <summary>\if KO HSMS 통신이 선택되었습니다. \endif \if EN HSMS communication is selected. \endif</summary>
    Selected
}

/// <summary>\if KO <para>Select.rsp 상태입니다.</para> \endif \if EN <para>Lists Select.rsp status values used by this implementation.</para> \endif</summary>
public enum HsmsSelectStatus : byte
{
    /// <summary>\if KO 통신이 설정되었습니다. \endif \if EN Communication established. \endif</summary>
    Success = 0,
    /// <summary>\if KO 이미 활성 상태입니다. \endif \if EN Communication already active. \endif</summary>
    AlreadyActive = 1,
    /// <summary>\if KO 연결이 준비되지 않았습니다. \endif \if EN Connection not ready. \endif</summary>
    NotReady = 2,
    /// <summary>\if KO 연결 자원이 고갈되었습니다. \endif \if EN Connection resources exhausted. \endif</summary>
    Exhausted = 3
}

/// <summary>\if KO <para>Deselect.rsp 상태입니다.</para> \endif \if EN <para>Lists Deselect.rsp status values used by this implementation.</para> \endif</summary>
public enum HsmsDeselectStatus : byte
{
    /// <summary>\if KO 통신이 종료되었습니다. \endif \if EN Communication ended. \endif</summary>
    Success = 0,
    /// <summary>\if KO 통신이 설정되지 않았습니다. \endif \if EN Communication was not established. \endif</summary>
    NotEstablished = 1,
    /// <summary>\if KO 통신이 사용 중입니다. \endif \if EN Communication is busy. \endif</summary>
    Busy = 2
}

/// <summary>\if KO <para>Reject.req 사유입니다.</para> \endif \if EN <para>Lists Reject.req reasons used by this implementation.</para> \endif</summary>
public enum HsmsRejectReason : byte
{
    /// <summary>\if KO 지원하지 않는 SType입니다. \endif \if EN Unsupported SType. \endif</summary>
    UnsupportedSType = 1,
    /// <summary>\if KO 지원하지 않는 PType입니다. \endif \if EN Unsupported PType. \endif</summary>
    UnsupportedPType = 2,
    /// <summary>\if KO 열린 트랜잭션이 없습니다. \endif \if EN No open transaction. \endif</summary>
    TransactionNotOpen = 3,
    /// <summary>\if KO 선택되지 않은 상태입니다. \endif \if EN Entity is not selected. \endif</summary>
    NotSelected = 4
}

/// <summary>\if KO <para>구현에서 사용하는 E37 타이머입니다.</para> \endif \if EN <para>Lists E37 timers used by the implementation.</para> \endif</summary>
public enum HsmsTimerKind
{
    /// <summary>\if KO 응답 제한 시간입니다. \endif \if EN Reply timeout. \endif</summary>
    T3,
    /// <summary>\if KO 연결 재시도 분리 시간입니다. \endif \if EN Connection retry separation time. \endif</summary>
    T5,
    /// <summary>\if KO 제어 transaction 제한 시간입니다. \endif \if EN Control transaction timeout. \endif</summary>
    T6,
    /// <summary>\if KO 미선택 상태 제한 시간입니다. \endif \if EN Not-selected timeout. \endif</summary>
    T7,
    /// <summary>\if KO 바이트 간 제한 시간입니다. \endif \if EN Inter-character timeout. \endif</summary>
    T8
}
