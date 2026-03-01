using UnityEngine;

namespace SpaceInvaders
{
  [RequireComponent(typeof(SpriteRenderer))]
  public class Bunker : MonoBehaviour
  {

    #region Data

    private const int Width = 24;
    private const int Height = 16;



    [Header("Sprite")]
    private Texture2D texture;
    private Color32[] buffer;
    private SpriteRenderer spriteRenderer;
    private bool[,] pixels = new bool[16, 24]
    {
     { false,true,true,true,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,true,true,false},
      { false,true,true,true,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,true,true,true,false},
      { false,true,true,true,true,true,false,false,false,false,false,false,false,false,false,false,false,false,true,true,true,true,true,false},
      { false,true,true,true,true,true,true,false,false,false,false,false,false,false,false,false,false,true,true,true,true,true,true,false},
      { false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false },
      { false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false },
      { false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false },
      { false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false },
      { false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false },
      { false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false },
      { false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false },
      { false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false },
      { false,false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false,false },
      { false,false,false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false,false,false },
      { false,false,false,false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false,false,false,false },
      { false,false,false,false,false,true,true,true,true,true,true,true,true,true,true,true,true,true,true,false,false,false,false,false }
    };
    #endregion


    #region Unity
    /// <summary>Ran by unity on first enable</summary>
    private void Start()
    {
      #region Start
      GetComponents();
      Draw();
      #endregion
    }
    #endregion


    #region Methods

    /// <summary>Gets the necessary components</summary>
    private void GetComponents()
    {
      #region GetComponents
      spriteRenderer = GetComponent<SpriteRenderer>();
      #endregion
    }
    /// <summary>Creates the texture with the current pixel map</summary>
    private void Draw()
    {
      #region Draw
      texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false)
      {
        filterMode = FilterMode.Point,
        wrapMode = TextureWrapMode.Clamp
      };


      buffer = new Color32[Height * Width];

      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          int index = y * Width + x;
          buffer[index] = pixels[y, x]
              ? new Color32(255, 255, 255, 255)
              : new Color32(0, 0, 0, 255);
        }
      }

      texture.SetPixels32(buffer);
      texture.Apply();

      Sprite sprite = Sprite.Create(
        texture,
        new Rect(0, 0, Width, Height),
        new Vector2(0.5f, 0.5f),
        32
    );

      spriteRenderer.sprite = sprite;
      #endregion
    }
    #endregion
  }
}
