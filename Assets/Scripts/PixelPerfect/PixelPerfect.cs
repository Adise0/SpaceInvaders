


namespace SpaceInvaders.Data
{

  public static class PixelPerfect
  {
    public const int PixelsPerUnit = 32;
    public const float UnitsPerPixel = 1f / PixelsPerUnit;

    public const float MinXBoundPx = -98;
    public const float MaxXBoundPx = 98;

    public const float MinXBoundWorld = MinXBoundPx * UnitsPerPixel;
    public const float MaxXBoundWorld = MaxXBoundPx * UnitsPerPixel;

    public const float BunkerYWorld = -1.75f;

    public const int TopBoundPx = 256 / 2 - 16;
    public const int LowBoundPx = -(256 / 2) + 16;
  }
}
