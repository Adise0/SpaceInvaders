

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

    [Header("Gameplay")]
    /// <summary>The movement timer</summary>
    private float moveTimer = 0;
    /// <summary>The delay between movements</summary>
    [SerializeField] private float movementDelay;
    /// <summary>The current move direciton</summary>
    [SerializeField] private Vector2 currentDirection = Vector2.right;
    /// <summary>The pixels per move</summary>
    [SerializeField] const int MoveStepPx = 8;



    [Header("Pixel perfect settings")]
    /// <summary>the pixels per unit. This matches the sprite ppu</summary>
    [SerializeField] private const int PixelsPerUnit = 32;
    private const float UnitsPerPixel = 1f / PixelsPerUnit;

    [Header("Formation settings")]
    /// <summary>The Y start position in pixels</summary>
    [SerializeField] private const short StartYpx = 120;
    /// <summary>The row spacing in pixels</summary>
    [SerializeField] private const short RowStepYpx = 16;
    /// <summary>The column spacing in pixels</summary>
    [SerializeField] private const short ColStepXpx = 16;

    [SerializeField] private const short MinXPx = -98;
    [SerializeField] private const short MaxXPx = 98;

    private Vector2 step = new Vector2(MoveStepPx * UnitsPerPixel, 0);

    [Header("Assets")]
    /// <summary>The enemy holder transform</summary>
    [SerializeField] private Transform enemyHolder;
    /// <summary>The base enemy prefab</summary>
    [SerializeField] private GameObject enemyPrefab;
    /// <summary>The store of enemy sprites</summary>
    [SerializeField] private Sprite[] enemySprites = new Sprite[Rows];

    /// <summary>The row transforms</summary>
    private Transform[] rows = new Transform[Rows];
    /// <summary>The store of all enemies</summary>
    private List<GameObject> enemies = new();
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

    /// <summary>Ran by Unity each frame</summary>
    private void Update()
    {
      #region Update
      MoveEnemies();
      #endregion
    }
    #endregion

    #region Methods
    /// <summary>Spawns and configures all enemies</summary>
    private void InitializeEnemies()
    {
      #region InitializeEnemies
      int formationWidthPx = (EnemiesPerRow - 1) * ColStepXpx;
      int halfWidthPx = formationWidthPx / 2;

      for (short row = 0; row < Rows; row++)
      {
        int rowYpx = StartYpx - row * RowStepYpx;

        GameObject rowGO = new GameObject($"Row-{row}");
        rowGO.transform.SetParent(enemyHolder, worldPositionStays: false);
        rowGO.transform.localPosition = new Vector2(0f, rowYpx * UnitsPerPixel);

        rows[row] = rowGO.transform;

        for (short enemyIndex = 0; enemyIndex < EnemiesPerRow; enemyIndex++)
        {
          GameObject enemy = Instantiate(enemyPrefab, rows[row]);
          int xPx = enemyIndex * ColStepXpx - halfWidthPx;
          enemy.transform.localPosition = new Vector3(xPx * UnitsPerPixel, 0f, 0f);
          enemy.GetComponent<SpriteRenderer>().sprite = enemySprites[row];
          enemies.Add(enemy);
        }
      }
      #endregion
    }

    /// <summary>Moves the enemies</summary>
    private void MoveEnemies()
    {
      #region MoveEnemies
      moveTimer += Time.deltaTime;
      if (moveTimer < movementDelay) return;
      moveTimer = 0;



      enemyHolder.position += (Vector3)(currentDirection * step);
      if (!HasReachedWall()) return;

      currentDirection *= -1;
      enemyHolder.position += (Vector3)(Vector2.down * step);
      #endregion
    }

    /// <summary>Checks if any spacechip has hit a wall</summary>
    private bool HasReachedWall()
    {
      #region HasReachedWall
      foreach (GameObject enemy in enemies)
      {
        if (!enemy) continue;

        if ((enemy.transform.position.x <= MinXPx * UnitsPerPixel && currentDirection == Vector2.left)
        || (enemy.transform.position.x >= MaxXPx * UnitsPerPixel && currentDirection == Vector2.right)
        ) return true;
      }
      return false;
      #endregion
    }
    #endregion
  }
}
