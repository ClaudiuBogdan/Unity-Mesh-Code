using UnityEngine;

public class Hexagon
{
    public Vector3 centerHexagon;
    public Vector3 normalVectorHexagon;
    public Vector3 rotationalVectorHexagon;


    public Hexagon(Vector3 centerHexagon, Vector3 normalVectorHexagon, Vector3 rotationalVectorHexagon)
    {
        this.centerHexagon = centerHexagon;
        this.normalVectorHexagon = normalVectorHexagon;
        this.rotationalVectorHexagon = rotationalVectorHexagon;
    }


    public static Hexagon newInstance(Vector3 centerHexagon, Vector3 normalVectorHexagon, Vector3 rotationalVectorHexagon)
    {
        return new Hexagon(centerHexagon, normalVectorHexagon, rotationalVectorHexagon);
    }


}
