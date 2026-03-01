using System;
using System.Collections.Generic;
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
      IDamagable foundComponent;
      TryGetComponent<IDamagable>(out foundComponent);
      foundComponent?.TakDamage(this);
      #endregion
    }

    /// <summary>Ran by unoty on destroy</summary>
    private void OnDestroy()
    {
      #region OnDestroy
      onDestroy();
      #endregion
    }
    #endregion


    #region Methods
    /// <summary>Initializes this bullet</summary>
    private void Init(BulletType type, Action onDestroy)
    {
      #region Init
      this.type = type;
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
    #endregion

  }
}
