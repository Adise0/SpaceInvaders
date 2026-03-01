using UnityEngine;

namespace SpaceInvaders
{

  public class Enemy : MonoBehaviour, IDamagable
  {

    #region Data
    public Sprite[] sprites = new Sprite[2];
    public Sprite deathSprite;
    private short currentSprite = 0;

    public bool isDead = false;
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

    /// <summary>Ran by Unity on destroy</summary>
    private void OnDestroy()
    {
      #region OnDestroy
      EnemyController.Singleton.RemoveEnemy(this);
      #endregion
    }
    #endregion

    #region Methods
    /// <summary>Handles taking damage</summary>
    public void TakDamage(Bullet bullet)
    {
      #region TakDamage
      isDead = true;
      transform.SetParent(null);
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

    /// <summary>Changes this enemy sprite</summary>
    public void ChangeSprite()
    {
      #region ChangeSprite
      if (isDead) return;

      currentSprite++;
      if (currentSprite >= sprites.Length) currentSprite = 0;
      spriteRenderer.sprite = sprites[currentSprite];
      #endregion
    }
    #endregion
  }
}
