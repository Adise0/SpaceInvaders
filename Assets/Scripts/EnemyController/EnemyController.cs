

using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
  /// <summary>The controller for the enemy ships</summary>
  public class EnemyController : MonoBehaviour
  {
    #region Singleton
    /// <summary>The singleton instance of this object</summary>
    public static EnemyController Singleton;

    /// <summary>Sets up this script Sinngleton</summary>
    private void SetupSingleton()
    {
      #region SetupSingleton
      if (Singleton != null & Singleton != this) { Destroy(this); return; }
      Singleton = this;
      #endregion
    }
    #endregion

    #region Data
    /// <summary>the umber of enemy rows</summary>
    private const short Rows = 5;
    /// <summary>The number of enemies per row</summary>
    private const short EnemiesPerRow = 11;
    private const float InitialRowY = 4f;
    private const float RowHeight = 0.8f;


    [SerializeField] private Transform mapHolder;
    /// <summary>The base enemy prefab</summary>
    [SerializeField] private GameObject enemyPrefab;
    /// <summary>The store of enemy sprites</summary>
    [SerializeField] private Sprite[] enemySprites = new Sprite[Rows];
    /// <summary>The store of all enemies</summary>
    private List<GameObject> enemies;
    /// <summary>The row transforms</summary>
    private Transform[] rows = new Transform[Rows];
    #endregion

    #region Unity
    /// <summary>Ran by unity on load</summary>
    private void Awake()
    {
      #region Awake
      SetupSingleton();
      #endregion
    }

    /// <summary>Ran by unity on first enable</summary>
    private void Start()
    {
      #region Start
      InitializeEnemies();
      #endregion
    }
    #endregion

    #region Methods
    /// <summary>Spawns and configures all enemies</summary>
    private void InitializeEnemies()
    {
      #region InitializeEnemies
      for (short row = 0; row < Rows; row++)
      {
        GameObject rowGO = new GameObject($"Row-{row}");
        rowGO.transform.SetParent(mapHolder);
        rowGO.transform.position = new Vector2(0, InitialRowY - RowHeight * row);
        rows[row] = rowGO.transform;

        for (short enemyIndex = 0; enemyIndex < EnemiesPerRow; enemyIndex++)
        {
          GameObject enemy = Instantiate(enemyPrefab, rows[row]);
          enemy.transform.position = new Vector2(enemyIndex - (EnemiesPerRow * 0.5f), rows[row].position.y);
          enemy.GetComponent<SpriteRenderer>().sprite = enemySprites[row];
        }
      }
      #endregion
    }
    #endregion

  }
}
