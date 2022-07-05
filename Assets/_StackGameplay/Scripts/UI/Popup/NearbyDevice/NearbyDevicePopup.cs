using StackGamePlay;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NearbyDevicePopup : AbstractPopup
{

    #region Childs

    [SerializeField]
    private Button Btn_BeServer;

    [SerializeField]
    private Button Btn_BeClient;

    [SerializeField]
    private TextMeshProUGUI Txt_NearbyDeviceStatus;

    #endregion

    #region Monobehaviour



    private void OnEnable()
    {
        DelegateStore.ShowNearbyDeviceStatus += OnShowNearbyDeviceStatus;
    }



    private void OnDisable()
    {
        DelegateStore.ShowNearbyDeviceStatus -= OnShowNearbyDeviceStatus;
    }

    private void Start()
    {
        NearbyDeviceManager.Instance.HandlePermissions();
    }

    #endregion

    #region Implementation

    public override void Activate(Action action)
    {
    }

    public override void Deactivate(Action action)
    {
    }

    #endregion


    #region Event: ButtonClick

    public void ButtonClick_BeServer()
    {
        DelegateStore.SetNearbyDeviceType?.Invoke(NearbyDeviceType.Server);
    }

    public void ButtonClick_BeClient()
    {
        DelegateStore.SetNearbyDeviceType?.Invoke(NearbyDeviceType.Client);
    }

    #endregion


    #region Callback: Delegate

    private void OnShowNearbyDeviceStatus(string statusText)
    {
        Txt_NearbyDeviceStatus.text = "Status : " + statusText;
    }

    #endregion

}
