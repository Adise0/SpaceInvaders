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

      foreach (MonoBehaviour behaviour in collision.gameObject.GetComponents<MonoBehaviour>())
      {
        if (behaviour is not IDamagable foundComponent) continue;
        didHit = true;
        foundComponent?.TakDamage(this);
        Destroy(gameObject);
        break;
      }

      // IDamagable damagable;
      // if (type == BulletType.Player)
      // {
      //   TryGetComponent(out Enemy foundEnemy);
      //   damagable = foundEnemy;
      // }
      // else
      // {
      //   TryGetComponent(out PlayerController player);
      //   damagable = player;
      // }

      // didHit = true;
      // damagable?.TakDamage(this);
      // Destroy(gameObject);
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
      if (transform.position.y < PixelPerfect.TopBoundPx * PixelPerfect.UnitsPerPixel) return;
      Destroy(gameObject);
      #endregion
    }

    #endregion

  }
}
