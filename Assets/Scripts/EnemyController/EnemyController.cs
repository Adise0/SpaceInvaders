

using System.Collections.Generic;
using SpaceInvaders.Data;
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
      if (Singleton != null && Singleton != this) { Destroy(this); return; }
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
    [SerializeField] float movementDelay = 1;
    /// <summary>The current move direciton</summary>
    [SerializeField] private Vector2 currentDirection = Vector2.right;
    /// <summary>The pixels per move</summary>
    [SerializeField] const int MoveStepXPx = 2;
    [SerializeField] const int MoveStepYPx = 8;

    private const float StepX = MoveStepXPx * PixelPerfect.UnitsPerPixel;
    private const float StepY = MoveStepYPx * PixelPerfect.UnitsPerPixel;

    private bool stepDown = false;


    [Header("Shooting")]
    private float reloadTime = 0.35f;
    private float timeSinceLastShot;
    private int nOfBullets;
    public GameObject bulletPrefab;

    public GameObject player;


    private Dictionary<BulletType, bool> bulletSlots = new()
    {
      {BulletType.Plunger, false},
      {BulletType.Rolling, false},
      {BulletType.Squigilly, false}
    };
    private int nextSlotIndex;

    [Header("Formation settings")]

    [SerializeField] private const short TopMarginPx = 48;
    /// <summary>The Y start position in pixels</summary>
    [SerializeField] private const short StartYpx = 120;
    /// <summary>The row spacing in pixels</summary>
    [SerializeField] private const short RowStepYpx = 16;
    /// <summary>The column spacing in pixels</summary>
    [SerializeField] private const short ColStepXpx = 16;



    [Header("Assets")]
    /// <summary>The enemy holder transform</summary>
    [SerializeField] private Transform enemyHolder;
    /// <summary>The base enemy prefab</summary>
    [SerializeField] private GameObject enemyPrefab;
    /// <summary>The store of enemy sprites</summary>
    [SerializeField] private Sprite[] enemySprites = new Sprite[Rows * 2];

    /// <summary>The row transforms</summary>
    private Transform[] rows = new Transform[Rows];
    /// <summary>The store of all enemies</summary>
    private List<Enemy> enemies = new();
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

    /// <summary>Ran by Unity each fixed tick</summary>
    private void FixedUpdate()
    {
      #region Update
      MoveEnemies();
      Shoot();
      #endregion
    }
    #endregion

    #region Methods
    /// <summary>Removes the passed enemy from the arrat</summary>
    public void RemoveEnemy(Enemy enemy)
    {
      #region RemoveEnemy
      enemies.Remove(enemy);
      movementDelay = enemies.Count / 60f;
      movementDelay = Mathf.Max(movementDelay, 1f / 60f);
      #endregion
    }

    /// <summary>Spawns and configures all enemies</summary>
    private void InitializeEnemies()
    {
      #region InitializeEnemies
      movementDelay = 1;

      int formationWidthPx = (EnemiesPerRow - 1) * ColStepXpx;
      int halfWidthPx = formationWidthPx / 2;

      for (short row = 0; row < Rows; row++)
      {
        int rowYpx = StartYpx - TopMarginPx - row * RowStepYpx;

        GameObject rowGO = new GameObject($"Row-{row}");
        rowGO.transform.SetParent(enemyHolder, worldPositionStays: false);
        rowGO.transform.localPosition = new Vector2(0f, rowYpx * PixelPerfect.UnitsPerPixel);

        rows[row] = rowGO.transform;

        for (short enemyIndex = 0; enemyIndex < EnemiesPerRow; enemyIndex++)
        {
          GameObject enemy = Instantiate(enemyPrefab, rows[row]);
          int xPx = enemyIndex * ColStepXpx - halfWidthPx;
          enemy.transform.localPosition = new Vector3(xPx * PixelPerfect.UnitsPerPixel, 0f, 0f);
          Enemy controller = enemy.GetComponent<Enemy>();
          controller.col = enemyIndex;
          controller.row = row;
          controller.sprites[0] = enemySprites[row * 2];
          controller.sprites[1] = enemySprites[row * 2 + 1];
          enemy.GetComponent<SpriteRenderer>().sprite = controller.sprites[0];
          enemies.Add(controller);
        }
      }
      #endregion
    }

    /// <summary>Moves the enemies</summary>
    private void MoveEnemies()
    {
      #region MoveEnemies
      moveTimer += Time.fixedDeltaTime;
      if (moveTimer < movementDelay) return;
      moveTimer = 0;

      foreach (Enemy enemy in enemies) enemy.ChangeSprite();

      if (stepDown)
      {
        enemyHolder.position += (Vector3)(Vector2.down * StepY);
        stepDown = false;
        foreach (Enemy enemy in enemies)
        {
          if (enemy.transform.position.y <= PixelPerfect.BunkerYWorld)
          {
            // TODO: Game over
            return;
          }
        }
        return;
      }


      enemyHolder.position += (Vector3)(currentDirection * StepX);
      if (!HasReachedWall()) return;

      currentDirection *= -1;
      stepDown = true;
      #endregion
    }

    /// <summary>Checks if any spacechip has hit a wall</summary>
    private bool HasReachedWall()
    {
      #region HasReachedWall
      foreach (Enemy enemy in enemies)
      {
        if (!enemy) continue;

        if ((enemy.transform.position.x <= PixelPerfect.MinXBoundWorld && currentDirection == Vector2.left)
        || (enemy.transform.position.x >= PixelPerfect.MaxXBoundWorld && currentDirection == Vector2.right)
        ) return true;
      }
      return false;
      #endregion
    }

    /// <summary>Attempts to shoot a bullet</summary>
    private void Shoot()
    {
      #region Shoot
      timeSinceLastShot += Time.fixedDeltaTime;
      if (nOfBullets >= 3) return;
      if (timeSinceLastShot < reloadTime) return;


      for (int i = 0; i < bulletSlots.Count; i++)
      {
        BulletType type = (BulletType)((nextSlotIndex + i) % 3);
        if (bulletSlots[type]) continue;

        int col = (type == BulletType.Rolling)
      ? GetClosestPlayerCol()
      : Random.Range(0, EnemiesPerRow);

        if (col == -1) continue;
        List<Enemy> possibleShooters = enemies.FindAll(e => e != null && e.col == col);
        Enemy shooter = null;
        int lowest = -1;
        foreach (Enemy enemy in possibleShooters)
        {
          if (enemy.row <= lowest) continue;
          shooter = enemy;
          lowest = enemy.row;
        }

        if (!shooter) continue;

        SpawnBullet(type, shooter);
        bulletSlots[type] = true;
        nextSlotIndex = ((int)type + 1) % 3;
        nOfBullets++;
        timeSinceLastShot = 0f;
        break;
      }


      #endregion
    }

    /// <summary>Gets the closest column to the player with alive enemies</summary>
    private int GetClosestPlayerCol()
    {
      #region GetClosestPlayerCol
      int closestColumn = -1;
      float minDistance = float.PositiveInfinity;
      for (int col = 0; col < EnemiesPerRow; col++)
      {
        Enemy rowEnemy = enemies.Find(e => e != null && e.col == col);
        if (!rowEnemy) continue;
        Vector2 enemyX = new Vector2(rowEnemy.transform.position.x, 0);
        Vector2 playerX = new Vector2(player.transform.position.x, 0);
        float colDistance = Mathf.Abs(rowEnemy.transform.position.x - player.transform.position.x);
        if (colDistance >= minDistance) continue;
        minDistance = colDistance;
        closestColumn = col;
      }
      return closestColumn;
      #endregion
    }

    /// <summary>Method</summary>
    private void SpawnBullet(BulletType type, Enemy shooter)
    {
      #region SpawnBullet
      Debug.Log($"Shooting from {shooter.col} {shooter.row}");
      GameObject instantiatedBullet = Instantiate(bulletPrefab, shooter.transform.position, Quaternion.identity);
      instantiatedBullet.GetComponent<Bullet>().Init(type, () =>
      {
        nOfBullets--;
        bulletSlots[type] = false;
      });
      #endregion
    }
    #endregion
  }
}
