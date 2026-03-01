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

    private const int Lifetime = 1;
    private Action onDestroy;
    #endregion


    #region Unity
    /// <summary>Ran by Unity each fixed tick</summary>
    private void FixedUpdate()
    {
      #region FixedUpdate
      Move();
      #endregion
    }

    /// <summary>Ran by unity on trigger enter</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
      #region OnTriggerEnter2D
      foreach (MonoBehaviour behaviour in collision.gameObject.GetComponents<MonoBehaviour>())
      {
        if (behaviour is not IDamagable foundComponent) continue;

        foundComponent?.TakDamage(this);
        Destroy(gameObject);
        break;
      }
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
      this.type = type;
      this.onDestroy = onDestroy;
      stepPx = type == BulletType.Player ? 4 : 3;
      dir = type == BulletType.Player ? Vector2.up : Vector2.down;

      Destroy(gameObject, Lifetime);
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

    #endregion

  }
}
