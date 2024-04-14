using System;
using UnityEngine;

namespace LD55.Models
{
    [Serializable]
    public class DialogueModel
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public Sprite Sprite { get; set; }
        public Action Action { get; set; }
    }
}
