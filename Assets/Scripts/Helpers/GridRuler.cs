using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class GridRuler
    { 
        public static Vector3[,] GetPlanarGrid(int width, int height)
        {
            var centre = new Vector3(0, 0, 0);
            float left = (width / 2f) * -1;
            float top = height / 2f;

            Vector3[,] grid = new Vector3[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var point = new Vector3((x * -1) - left - 1f , y - top + 1f, 0);
                    grid[x, y] = point;
                }
            }

            return grid;
        }
    }
}
