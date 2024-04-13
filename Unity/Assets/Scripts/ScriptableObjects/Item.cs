using System;
using UnityEngine;

namespace LD55.ScriptableObjects
{
    [CreateAssetMenu(menuName = "LD55/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private string _guid = Guid.NewGuid().ToString();
        
        [SerializeField] private string _name;
        public string Name => _name;
        
        [SerializeField] private string _description;
        public string Description => _description;
        
        [SerializeField] private Sprite _sprite;
        public Sprite Sprite => _sprite;
        
        [SerializeField] private ushort _price;
        public ushort Price => _price;
    }
}
