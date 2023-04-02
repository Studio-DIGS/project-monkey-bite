using System.Collections.Generic;
using UnityEngine;


namespace MushiCore.GizmoDrawer
{
    /// <summary>
    /// A utility class that caches gizmos, making generating gizmos outside of OnDrawGizmos more convenientp
    /// </summary>
    public class GizmoDrawer
    {
        private static Color[] colorCycle = new Color[]
        {
            Color.red,
            Color.cyan,
            Color.magenta,
            Color.green
        };

        private int colorCycleIndex = 0;
        
        private List<GizmoDrawerObject> gizmoObjects = new();

        public void Clear()
        {
            gizmoObjects.Clear();
        }

        public void Add(GizmoDrawerObject gizmoObject)
        {
            gizmoObjects.Add(gizmoObject);
        }
        
        public void Add(IEnumerable<GizmoDrawerObject> gizmoObject)
        {
            gizmoObjects.AddRange(gizmoObject);
        }

        public void Draw()
        {
            foreach (var gizmoObject in gizmoObjects)
            {
                gizmoObject.DrawGizmo();
            }
        }

        public Color GetCycledColor(int cycle = 2)
        {
            colorCycleIndex++;
            return colorCycle[colorCycleIndex % cycle];
        }
    }

    public interface GizmoDrawerObject
    {
        public void DrawGizmo();
    }

    public class GizmoDrawerLine : GizmoDrawerObject
    {
        public Color c;
        public Vector3 p1;
        public Vector3 p2;
        
        public GizmoDrawerLine(Color c, Vector3 p1, Vector3 p2)
        {
            this.c = c;
            this.p1 = p1;
            this.p2 = p2;
        }

        public void DrawGizmo()
        {
            Gizmos.color = c;
            Gizmos.DrawLine(p1, p2);
        }
    }
    
    public class GizmoDrawerWireSphere : GizmoDrawerObject
    {
        public Color c;
        public Vector3 p;
        public float r;
        
        public GizmoDrawerWireSphere(Color c, Vector3 p, float r)
        {
            this.c = c;
            this.p = p;
            this.r = r;
        }

        public void DrawGizmo()
        {
            Gizmos.color = c;
            Gizmos.DrawWireSphere(p, r);
        }
    }
}
