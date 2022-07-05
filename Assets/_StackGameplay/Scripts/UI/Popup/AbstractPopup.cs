using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractPopup : MonoBehaviour
{

    [SerializeField]
    private PopupType PopupType;

    public abstract void Deactivate(Action action);
    public abstract void Activate(Action action);

    #region CLOSE THIS

    public Button Btn_CloseThis;

    protected virtual void Awake()
    {
        if (Btn_CloseThis != null)
        {
            Btn_CloseThis.onClick.AddListener(CloseThis);
        }
    }

    public void CloseThis()
    {
        PopupUIManager.Instance.HidePopup(PopupType);
    }

    #endregion


}
