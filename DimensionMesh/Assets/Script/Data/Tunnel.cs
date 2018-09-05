using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using UnityEngine;
using Plane = Assets.Script.Plane;

public class Tunnel
{

    public ArrayList TunnelMeshList;

    public ArrayList TunnelPlanesList;
    private Vector3 _tunnelDirection;

    public Tunnel()
    {
        TunnelPlanesList = new ArrayList();
    }

    public void SetTunnelPlane(ArrayList tunnelHexagonsList, int hexagonIndex, int planeIndex)
    {
        Plane tunnelPlane = TunnelPlanesList[planeIndex] as Plane;
        if (tunnelPlane == null)
        {
            //Create tunnel plane
            tunnelPlane = new Plane();
            tunnelPlane.firstHexagon = tunnelHexagonsList[hexagonIndex] as Hexagon;
            tunnelPlane.secondHexagon = tunnelHexagonsList[hexagonIndex + 1] as Hexagon;
            tunnelPlane.planeOrigenIndex = planeIndex;
            tunnelPlane.SetPlane();
            TunnelPlanesList[planeIndex] = tunnelPlane;
        }

        _tunnelDirection = (tunnelPlane.secondHexagon.centerHexagon - tunnelPlane.firstHexagon.centerHexagon)
            .normalized;

    }

    public void SetAllTunnelPlanes(ArrayList tunnelHexagonsList, int hexagonIndex)
    {
        int firstPlaneIndex = 0;
        int lastPlaneIndex = 5;

        for (int planeIndex = firstPlaneIndex; planeIndex < lastPlaneIndex; planeIndex++)
        {
            SetTunnelPlane(tunnelHexagonsList, hexagonIndex, planeIndex);
        }
    }
}
