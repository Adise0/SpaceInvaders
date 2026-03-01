using UnityEngine;

namespace SpaceInvaders
{

  public class Enemy : MonoBehaviour, IDamagable
  {

    #region Data
    public Sprite[] sprites = new Sprite[2];
    public Sprite deathSprite;
    private short currentSprite = 0;

    private SpriteRenderer spriteRenderer;
    #endregion

    #region Unity
    /// <summary>Ran by unity on load</summary>
    private void Awake()
    {
      #region Awake
      GetComponents();
      #endregion
    }

    /// <summary>Called by unity each fixed tick</summary>
    private void FixedUpdate()
    {
      #region FixedUpdate
      #endregion
    }
    #endregion

    #region Methods
    /// <summary>Handles taking damage</summary>
    public void TakDamage(Bullet bullet)
    {
      #region TakDamage

      GetComponent<BoxCollider2D>().enabled = false;
      spriteRenderer.sprite = deathSprite;
      Destroy(gameObject, 0.5f);
      #endregion
    }

    /// <summary>Gets the required components</summary>
    private void GetComponents()
    {
      #region GetComponents
      spriteRenderer = GetComponent<SpriteRenderer>();
      #endregion
    }

    /// <summary>Method</summary>
    public void ChangeSprite()
    {
      #region ChangeSprite
      currentSprite++;
      if (currentSprite >= sprites.Length) currentSprite = 0;
      spriteRenderer.sprite = sprites[currentSprite];
      #endregion
    }
    #endregion
  }
}
