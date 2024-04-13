using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD55.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        public void OnClickStart()
        {
            SceneManager.LoadScene("World");
        }

        public void OnClickSettings()
        {
            // TODO: Open Settings window
        }

        public void OnClickCredits()
        {
            // TODO: Open Credits window
        }
    }
}
