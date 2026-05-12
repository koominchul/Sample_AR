using System.Collections;
using Luck9kr.Uisystem;
using TMPro;
using UnityEngine;



public class CommonMessagePopup : UIPopup
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] TextMeshProUGUI titleText;

    //Buttons
    [SerializeField] GameObject okBtn;
    [SerializeField] GameObject cancelBtn;
    [SerializeField] GameObject yesBtn;
    [SerializeField] GameObject noBtn;
    [SerializeField] GameObject closeBtn;


    PopupResults results;
    string _message, _title;
    CommonPopupType popupType;
    float autoCloseTime = 2f;



    public PopupState Open(string message, string title = "알림")
    {
        return Open(CommonPopupType.C, message, title);
    }

    public PopupState Open(CommonPopupType popupType, string message, string title = "알림", float autoCloseTime = 2f)
    {
        this.popupType = popupType;
        switch (popupType)
        {
            case CommonPopupType.B:
                results = PopupResults.OK | PopupResults.Cancel;
                break;

            case CommonPopupType.C:
                results = PopupResults.Close;
                break;

            case CommonPopupType.D:
                results = PopupResults.Yes | PopupResults.No;
                break;
        }

        _message = message;
        _title = title == "" ? "Notice" : title;
        this.autoCloseTime = autoCloseTime;

        // _title = LocalizationManager.Instance.GetLocalizeText(title);
        // _message = LocalizationManager.Instance.GetLocalizeText(message);

        InitButtons();
        ShowLayer();
        SetInfo();

        return state;
    }

    private void InitButtons()
    {
        okBtn?.SetActive(false);
        cancelBtn?.SetActive(false);
        closeBtn?.SetActive(false);
        yesBtn?.SetActive(false);
        noBtn?.SetActive(false);
    }


    protected override void OnShow()
    {
        Find("Container/Buttons").SetActive(popupType != CommonPopupType.A);
        titleText.text = _title;
        messageText.text = _message;
    }

    void SetInfo()
    {
        switch (popupType)
        {
            case CommonPopupType.A:
                StartCoroutine(AutoClose());
                break;

            case CommonPopupType.B:
            case CommonPopupType.C:
            case CommonPopupType.D:
                {
                    okBtn?.SetActive((results & PopupResults.OK) == PopupResults.OK);
                    cancelBtn?.SetActive((results & PopupResults.Cancel) == PopupResults.Cancel);
                    yesBtn?.SetActive((results & PopupResults.Yes) == PopupResults.Yes);
                    noBtn?.SetActive((results & PopupResults.No) == PopupResults.No);
                    closeBtn?.SetActive((results & PopupResults.Close) == PopupResults.Close);
                }
                break;
        }
    }

    IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(autoCloseTime);

        Close();
    }
}