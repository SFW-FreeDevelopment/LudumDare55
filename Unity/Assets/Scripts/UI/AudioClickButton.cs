using LD55.Enums;
using LD55.Managers;

namespace LD55.UI
{
    public class AudioClickButton : ButtonBase
    {
        public SoundName SoundName = SoundName.Click1;
        
        protected override void OnClick() =>
            AudioManager.Instance.Play(SoundName);
    }
}
