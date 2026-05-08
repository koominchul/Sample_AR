using UnityEngine;
using Luck9kr.Uisystem;

public class DefaultPopup : UIPopup
{
    public PopupState Open()
    {

        ShowLayer();
        return state;
    }
}
