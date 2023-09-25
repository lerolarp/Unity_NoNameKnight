using UI;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    protected UIWindow uiWindow = null;
    public UIWindow UiWindow => uiWindow;

    virtual public void Awake()
    {
        GameObject obj = GameObject.Find("UIWindow");
        uiWindow = obj.GetComponent<UIWindow>();
    }

    virtual public void OnSceneLoaded()
    {
        if(uiWindow != null)
            uiWindow.Initialize();
    }

    virtual public void Initialize()
    {
    }
}
