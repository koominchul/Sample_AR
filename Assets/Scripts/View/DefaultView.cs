
using Luck9kr.Uisystem;
using UnityEngine.UI;


public class DefaultView : UIView
{
    public override void Show(object param = null)
    {
        ShowLayer();
    }

    protected override void OnFirstShow()
    {
        Find<Button>("PopupCallBtn").onClick.AddListener(OnClick_PopupCallBtn);
    }


    #region Button Event
    private void OnClick_PopupCallBtn()
    {
        UIManager.Instance.Popup<DefaultPopup>().Open();
    }
    #endregion
}
