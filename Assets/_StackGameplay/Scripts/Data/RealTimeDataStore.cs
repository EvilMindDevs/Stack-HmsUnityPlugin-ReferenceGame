using GameDev.MiddleWare;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeDataStore
{

    private static bool remoteConfigIsOk = false;
    private static bool crashIsOk = false;
    private static bool iapIsOk = false;
    private static float blockMoveTimeCoeff = 1f;
    private static int backgroundColorIndex = 0;
    private static bool accountKitIsReady = false;

    public static bool RemoteConfigIsOk { get => remoteConfigIsOk; set => remoteConfigIsOk = value; }
    public static bool CrashIsOk { get => crashIsOk; set => crashIsOk = value; }
    public static float BlockMoveTimeCoeff { get => blockMoveTimeCoeff; set => blockMoveTimeCoeff = value; }
    public static int BackgroundColorIndex { get => backgroundColorIndex; set => backgroundColorIndex = value; }
    public static bool AccountKitIsReady { get => accountKitIsReady; set => accountKitIsReady = value; }
    public static bool IapIsOk { get => iapIsOk; set => iapIsOk = value; }

}
