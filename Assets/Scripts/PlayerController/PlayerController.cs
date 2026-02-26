
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceInvaders
{
  /// <summary>Hanndles the player movement and shooting</summary>
  public class PlayerController : MonoBehaviour
  {
    #region Data
    /// <summary>The player movement speed</summary>
    [SerializeField] private float movementSpeed;
    /// <summary>The atack coolddown</summary>
    [SerializeField] private float atackCooldown;
    /// <summary>The x bounds of the movement</summary>
    [SerializeField] private (float, float) xBounds = (-6f, 6f);

    /// <summary>Input action in charge of movement</summary>
    private InputAction moveAction;
    #endregion


    #region Unity
    /// <summary>Ran by Unity on load</summary>
    private void Awake()
    {
      #region Awake
      BindControls();
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

      var (min, max) = xBounds;

      if (transform.position.x > min && transform.position.x < max) transform.position += (Vector3)dir * movementSpeed * Time.deltaTime;
      if (transform.position.x < min) transform.position = new Vector2(min, transform.position.y);
      if (transform.position.x > max) transform.position = new Vector2(max, transform.position.y);
      #endregion
    }
    #endregion

  }
}
