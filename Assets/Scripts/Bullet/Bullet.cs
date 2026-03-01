using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpaceInvaders.Data;
using UnityEngine;

namespace SpaceInvaders
{
  public class Bullet : MonoBehaviour
  {
    #region Data
    public BulletType type;
    private int stepPx;
    private Vector2 dir;
    private Vector2 prevPos;
    private bool didHit = false;
    private Action onDestroy;
    private SpriteRenderer spriteRenderer;

    public Sprite playerSprite;
    public Sprite rollingSprite;
    public Sprite plungerSprite;
    public Sprite squigillySprite;
    #endregion


    #region Unity
    /// <summary>Ran by Unity each fixed tick</summary>
    private void FixedUpdate()
    {
      #region FixedUpdate
      Move();
      Die();
      #endregion
    }

    /// <summary>Ran by unity on trigger enter</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
      #region OnTriggerEnter2D
      if (didHit) return;

      IDamagable damagable;
      if (type == BulletType.Player)
      {
        collision.gameObject.TryGetComponent(out Enemy foundEnemy);
        damagable = foundEnemy;
      }
      else
      {
        collision.gameObject.TryGetComponent(out PlayerController player);
        damagable = player;
      }

      if (damagable == null)
      {
        collision.gameObject.TryGetComponent(out Bunker bunker);
        if (bunker) Destroy(gameObject);
        return;
      }
      didHit = true;
      damagable?.TakDamage(this);
      Destroy(gameObject);
      #endregion
    }

    /// <summary>Ran by unoty on destroy</summary>
    private void OnDestroy()
    {
      #region OnDestroy
      onDestroy?.Invoke();
      #endregion
    }
    #endregion


    #region Methods
    /// <summary>Initializes this bullet</summary>
    public void Init(BulletType type, Action onDestroy)
    {
      #region Init
      spriteRenderer = GetComponent<SpriteRenderer>();

      this.type = type;
      this.onDestroy = onDestroy;
      stepPx = type == BulletType.Player ? 4 : 3;
      dir = type == BulletType.Player ? Vector2.up : Vector2.down;

      switch (type)
      {
        case BulletType.Player:
          spriteRenderer.color = Color.green;
          spriteRenderer.sprite = playerSprite;
          break;

        case BulletType.Rolling:
          spriteRenderer.sprite = rollingSprite;
          break;

        case BulletType.Squigilly:
          spriteRenderer.sprite = squigillySprite;
          break;

        case BulletType.Plunger:
          spriteRenderer.sprite = plungerSprite;
          break;

      }
      #endregion
    }




    /// <summary>Moves the bullet each tick</summary>
    private void Move()
    {
      #region Move
      prevPos = transform.position;
      transform.position += (Vector3)(dir * stepPx * PixelPerfect.UnitsPerPixel);
      #endregion
    }

    /// <summary>Kills the bullet</summary>
    private void Die()
    {
      #region Die
      if (transform.position.y < PixelPerfect.TopBoundPx * PixelPerfect.UnitsPerPixel && transform.position.y > PixelPerfect.LowBoundPx * PixelPerfect.UnitsPerPixel) return;
      Destroy(gameObject);
      #endregion
    }

    #endregion

  }
}
