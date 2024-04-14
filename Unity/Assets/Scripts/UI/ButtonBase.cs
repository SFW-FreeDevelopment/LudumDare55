using UnityEngine;
using UnityEngine.UI;

namespace LD55.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonBase : MonoBehaviour
    {
        protected Button _button;
        
        protected void Awake()
        {
            _button = GetComponent<Button>();
        }

        protected void Start()
        {
            _button.onClick.AddListener(OnClick);
        }

        protected abstract void OnClick();
    }
}
