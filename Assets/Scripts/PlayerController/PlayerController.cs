
using UnityEngine;

namespace SpaceInvaders
{
  /// <summary>Hanndles the player movement and shooting</summary>
  public class PlayerController : MonoBehaviour
  {
    #region Data
    [SerializeField] private float movementSpeed;
    [SerializeField] private float atackCooldown;
    [SerializeField] private float yLevel;
    [SerializeField] Vector2 xBounds;
    #endregion
  }
}
