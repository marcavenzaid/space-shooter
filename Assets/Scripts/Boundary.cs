using UnityEngine;

[System.Serializable]
public class Boundary {

    [SerializeField] private float xMin, xMax, zMin, zMax;

    public Boundary(float xMin, float xMax, float zMin, float zMax) {
        this.xMin = xMin;
        this.xMax = xMax;
        this.zMin = zMin;
        this.zMax = zMax;
    }

    public float GetXMin() {
        return xMin;
    }

    public float GetXMax() {
        return xMax;
    }

    public float GetZMin() {
        return zMin;
    }

    public float GetZMax() {
        return zMax;
    }
}
