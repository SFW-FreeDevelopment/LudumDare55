using UnityEngine;

namespace LD55.Controllers
{
    public class BattleHudMenuController : MonoBehaviour
    {
        public GameObject[,] buttons = new GameObject[2, 2];
        private int currentRow = 0;
        private int currentCol = 0;

        public void Start()
        {
            SetButtonHighlight(buttons[currentRow, currentCol], true);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                ChangeButtonSelection(-1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                ChangeButtonSelection(1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                ChangeButtonSelection(0, -1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                ChangeButtonSelection(0, 1);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                ExecuteAction(buttons[currentRow, currentCol]);
            }
        }

        private void ChangeButtonSelection(int rowOffset, int colOffset)
        {
            SetButtonHighlight(buttons[currentRow, currentCol], false);

            currentRow = (currentRow + rowOffset + buttons.GetLength(0)) % buttons.GetLength(0);
            currentCol = (currentCol + colOffset + buttons.GetLength(1)) % buttons.GetLength(1);

            SetButtonHighlight(buttons[currentRow, currentCol], true);
        }

        private void SetButtonHighlight(GameObject button, bool isHighlighted)
        {
            // Implement button highlight effect
        }

        private void ExecuteAction(GameObject button)
        {
            string buttonText = button.name;
            Debug.Log(buttonText + " selected");

            switch (buttonText)
            {
                case "AttackButton":
                    // Handle attack action
                    break;
                case "PartyButton":
                    // Handle party action
                    break;
                case "ItemButton":
                    // Handle item action
                    break;
                case "RunButton":
                    // Handle run action
                    break;
                default:
                    break;
            }
        }
    }
}