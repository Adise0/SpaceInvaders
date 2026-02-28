
using SpaceInvaders.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceInvaders
{
  /// <summary>Hanndles the player movement and shooting</summary>
  public class PlayerController : MonoBehaviour, IDamagable
  {
    #region Data

    [Header("Gameplay")]
    /// <summary>The player movement speed</summary>
    [SerializeField] private const float MovementStep = 2;
    /// <summary>The atack coolddown</summary>
    [SerializeField] private float atackCooldown;


    /// <summary>Input action in charge of movement</summary>
    private InputAction moveAction;

    [Header("Sprite")]
    private float halfWidth;
    #endregion


    #region Unity
    /// <summary>Ran by Unity on load</summary>
    private void Awake()
    {
      #region Awake
      BindControls();
      ConfigureSprite();
      #endregion
    }

    /// <summary>Ran by Unity each fixed tick</summary>
    private void FixedUpdate()
    {
      #region Update
      MovePlayer();
      #endregion
    }
    #endregion

    #region Methods
    /// <summary>Aplyes damage to the player</summary>
    public void TakDamage(GameObject bullet)
    {
      #region TakDamage

      #endregion
    }

    /// <summary>Configures the sprite related aspects</summary>
    private void ConfigureSprite()
    {
      #region ConfigureSprite
      halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
      #endregion
    }

    /// <summary>Binds the controls from the input system</summary>
    private void BindControls()
    {
      #region BindControls
      moveAction = InputSystem.actions["Move"];
      #endregion
    }

    /// <summary>Handles the player movement</summary>
    private void MovePlayer()
    {
      #region MovePlayer
      Vector2 raw = moveAction.ReadValue<Vector2>();
      int dirSign = raw.x > 0.1f ? 1 : raw.x < -0.1f ? -1 : 0;


      if (dirSign == 0) return;
      float xStep = dirSign * MovementStep * PixelPerfect.UnitsPerPixel;

      Vector2 newPos = transform.position;
      newPos.x += xStep;
      newPos.x = Mathf.Clamp(newPos.x, PixelPerfect.MinXBoundWorld + halfWidth, PixelPerfect.MaxXBoundWorld - halfWidth);

      newPos.x = Mathf.Round(newPos.x / PixelPerfect.UnitsPerPixel) * PixelPerfect.UnitsPerPixel;
      transform.position = newPos;


      #endregion
    }
    #endregion

  }
}
