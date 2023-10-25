public enum ServerPackets
{
    SWelcomeMessage = 1,
    SInstantiatePlayer,
    SAlertMsg,
    SLoginOK,
    SPlayerData,
    SImage,
    SPOIData,
    SSearchFin,
}

public enum ClientPackets
{
    CHelloServer = 1,
    CNewAccount,
    CLogin,
    CStoreImage,
    CPOISearch,
    CRequestImage,
}

public enum RoomState
{
    Open = 1,
    Searching,
    Closed
}
