using LD55.Models;
using UnityEngine;

namespace LD55.Managers
{
    public class PlayerDataManager : SceneSingleton<PlayerDataManager>
    {
        protected override void InitSingletonInstance()
        {
        }

        [SerializeField] private Party _party;
        public Party Party => _party;
    }
}
