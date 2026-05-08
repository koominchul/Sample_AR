using Luck9kr.Uisystem;

public class My_UIManager : UIManager
{
    public static new My_UIManager Instance
    {
        get
        {
            if (UIManager.Instance == null)
                UIManager.Instance = FindAnyObjectByType<UIManager>();

            return UIManager.Instance as My_UIManager;
        }
    }
    public static object ViewParam { get { return viewParam; } }

    protected override void OnAwakeUI()
    {

        base.OnAwakeUI();
    }

    protected override void OnDestroyUI()
    {

        base.OnDestroyUI();
    }

}
