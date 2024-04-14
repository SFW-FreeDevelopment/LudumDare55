using UnityEngine;
using UnityEngine.Tilemaps;

namespace LD55
{
    public class SpecialTileInteraction : MonoBehaviour
    {
        public Tilemap interactiveTilemap; // Assign in inspector
        public TileBase specialTile; // Assign the specific tile that should trigger an action

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                Vector3Int cell = interactiveTilemap.WorldToCell(hitPosition);

                TileBase collidedTile = interactiveTilemap.GetTile(cell);
                if (collidedTile == specialTile)
                {
                    InvokeSpecialMethod();
                }
            }
        }

        void InvokeSpecialMethod()
        {
            // Add your method's functionality here
            Debug.Log("Special tile interacted!");
        }
    }
}
