using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class CircleRuler
    {
        public static List<Vector3> GetCirclePositions(int count, float radius)
        {
            var points = new List<Vector3>();

            for(int i = 1; i <= count; i++)
            {
                double x = radius * Mathf.Cos(Mathf.PI / -2 + (2 * i * Mathf.PI) / count);
                double y = radius * Mathf.Sin(Mathf.PI / -2 + (2 * i * Mathf.PI) / count);

                var newVec = new Vector3((float)x, (float)y, 0);
                points.Add(newVec);
            }

            return points;
        }
    }
}
