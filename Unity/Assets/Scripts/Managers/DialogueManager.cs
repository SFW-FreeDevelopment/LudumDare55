using System;
using LD55.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD55.Managers
{
    public class DialogueManager : SceneSingleton<DialogueManager>
    {
        [SerializeField] private GameObject _dialogueWindow;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _textText;
        [SerializeField] private Image _image;
        [SerializeField] private Image _bigImage;
        [SerializeField] private Button _continueButton;
        
        public bool IsTalking { get; set; }
        
        private DialogueModel Model { get; set; }
        
        protected override void InitSingletonInstance()
        {
            _continueButton.onClick.AddListener(Continue);
        }

        public void Show(DialogueModel model)
        {
            Instance.IsTalking = true;
            Model = model;
            _nameText.text = model.Name;
            _textText.text = model.Text;
            if (model.IsTrainer)
            {
                _bigImage.sprite = model.Sprite;
                _bigImage.gameObject.SetActive(true);
                _image.gameObject.SetActive(false);
            }
            else
            {
                _image.sprite = model.Sprite;
                _image.gameObject.SetActive(true);
                _bigImage.gameObject.SetActive(false);
            }
            _image.sprite = model.Sprite;
            _dialogueWindow.SetActive(true);
        }

        public void Hide()
        {
            _dialogueWindow.SetActive(false);
        }

        private void Update()
        {
            if (!_dialogueWindow.activeSelf) return;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Continue();
            }
        }

        private void Continue()
        {
            Model.Action?.Invoke();
            Hide();
        }
    }
}
