

using UnityEngine;

namespace SpaceInvaders
{
  public interface IDamagable
  {
    /// <summary>Applyes the relevant damage to the entity</summary>
    public void TakDamage(GameObject bullet);
  }
}
