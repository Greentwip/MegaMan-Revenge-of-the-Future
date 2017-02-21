using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GeometryHelper
{
    public static bool Contains(Rect self, Rect rect) {
        return self.Contains(new Vector2(rect.xMin, rect.yMin)) 
            && self.Contains(new Vector2(rect.xMax, rect.yMax));
    }

    
    public static bool Contains(Rect self, Bounds container)
    {
        return self.Contains(new Vector2(container.min.x, container.min.y))
            && self.Contains(new Vector2(container.max.x, container.max.y));
    }

    public static bool Contains(Camera camera, Bounds bounds)
    {
        Plane[] planes = 
            GeometryUtility.CalculateFrustumPlanes(camera);
        
        if (GeometryUtility.TestPlanesAABB(planes, bounds))
            return true;
        else
            return false;
    }



}

