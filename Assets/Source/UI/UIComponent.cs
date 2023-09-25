using UnityEngine;

namespace UI
{
    public class UIComponent : MonoBehaviour
    {
        [HideInInspector] public UIWindow window = null;

        virtual public void Awake()
        {
            GameObject obj = GameObject.Find("UIWindow");
            window = obj.GetComponent<UIWindow>();
        }
    }
}
