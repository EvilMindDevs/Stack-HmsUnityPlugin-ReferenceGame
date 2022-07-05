
using StackGamePlay;

using UnityEngine;
using UnityEngine.UI;

public class PreUI : MonoBehaviour
{

    [SerializeField]
    private Image Img_Bar;

    #region Monobehaviour

    private void OnEnable()
    {
        DelegateStore.AdvanceProgressBar += OnAdvanceProgressBar;
    }

    private void OnDisable()
    {
        DelegateStore.AdvanceProgressBar -= OnAdvanceProgressBar;
    }



    #endregion

    #region Callbacks

    private void OnAdvanceProgressBar(float val)
    {
        Img_Bar.fillAmount = val;
    }

    #endregion


}
