
using SpaceInvaders.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceInvaders
{
  /// <summary>Hanndles the player movement and shooting</summary>
  public class PlayerController : MonoBehaviour
  {
    #region Data

    [Header("Gameplay")]
    /// <summary>The player movement speed</summary>
    [SerializeField] private float movementSpeed;
    /// <summary>The atack coolddown</summary>
    [SerializeField] private float atackCooldown;
    /// <summary>The x bounds of the movement</summary>

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

      float upp = 1f / 32f;
      var p = Camera.main.transform.position;
      p.x = Mathf.Round(p.x / upp) * upp;
      p.y = Mathf.Round(p.y / upp) * upp;
      Camera.main.transform.position = p;
      #endregion
    }

    /// <summary>Ran by Unity each frame</summary>
    private void Update()
    {
      #region Update
      MovePlayer();
      #endregion
    }
    #endregion

    #region Methods
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
      Vector2 dir = moveAction.ReadValue<Vector2>();
      if (dir == Vector2.zero) return;
      dir.y = 0;

      Vector2 delta = dir * movementSpeed * Time.deltaTime;
      Vector3 newPos = transform.position + (Vector3)delta;

      newPos.x = Mathf.Clamp(newPos.x, PixelPerfect.MinXBoundWorld + halfWidth, PixelPerfect.MaxXBoundWorld - halfWidth);
      newPos.x = Mathf.Round(newPos.x / PixelPerfect.UnitsPerPixel) * PixelPerfect.UnitsPerPixel;
      transform.position = newPos;
      #endregion
    }
    #endregion

  }
}
