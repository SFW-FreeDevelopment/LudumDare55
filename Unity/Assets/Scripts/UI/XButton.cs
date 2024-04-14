using UnityEngine;

namespace LD55.UI
{
    public class XButton : ButtonBase
    {
        [SerializeField] private GameObject _window;
        
        protected override void OnClick() => _window.SetActive(false);
    }
}
