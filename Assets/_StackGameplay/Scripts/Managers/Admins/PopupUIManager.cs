using RotaryHeart.Lib.SerializableDictionary;

using StackGamePlay;

using System.Collections.Generic;

using UnityEngine;

public class PopupUIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject Btn_BackgroundClose;

    [SerializeField]
    private PopupStore PopupsStore;

    private List<GameObject> activePopups;

    #region Monobehaviour

    private void Awake()
    {
        activePopups = new List<GameObject>();

        Singleton();
    }

    private void OnEnable()
    {
        DelegateStore.ShowPopup += OnShowPopup;
    }



    private void OnDisable()
    {
        DelegateStore.ShowPopup -= OnShowPopup;
    }

    #endregion

    #region Singleton

    public static PopupUIManager Instance;

    private void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    #region Primary

    public void ShowPopup(PopupType popupType)
    {
        if (!Btn_BackgroundClose.activeSelf)
            Btn_BackgroundClose.SetActive(true);

        var popup = PopupsStore[popupType];
        popup.SetActive(true);

        popup.transform.localScale = Vector3.one * 0.001f;
        popup.transform.LeanScale(Vector3.one, 0.5f).setEaseOutBack();

    }

    public void HidePopup(PopupType popupType)
    {
        if (Btn_BackgroundClose.activeSelf)
            Btn_BackgroundClose.SetActive(false);

        var popup = PopupsStore[popupType];
        popup.SetActive(false);
    }

    public void HideAllPopups()
    {
        if (Btn_BackgroundClose.activeSelf)
            Btn_BackgroundClose.SetActive(false);

        foreach (var item in PopupsStore)
        {
            var popup = item.Value;
            popup.SetActive(false);
        }
    }

    #endregion

    #region Callbacks

    private void OnShowPopup(PopupType popupType)
    {
        ShowPopup(popupType);
    }

    #endregion

}

public enum PopupType
{
    None = 0,
    ScoreTable = 1,
    NearbyDevice = 2
}

[System.Serializable]
public class PopupStore : SerializableDictionaryBase<PopupType, GameObject> { }

