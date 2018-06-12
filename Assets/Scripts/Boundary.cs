using UnityEngine;

[System.Serializable]
public class Boundary {

    [SerializeField] private float xMin, xMax, zMin, zMax;

    public float XMin {
        get { return xMin; }
        set { xMin = value; }
    }
    public float XMax {
        get { return xMax; }
        set { xMax = value; }
    }
    public float ZMin {
        get { return zMin; }
        set { zMin = value; }
    }
    public float ZMax {
        get { return zMax; }
        set { zMax = value; }
    }

    public Boundary(float xMin, float xMax, float zMin, float zMax) {
        XMin = xMin;
        XMax = xMax;
        ZMin = zMin;
        ZMax = zMax;
    }    
}
