using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using UnityEngine;
using Plane = Assets.Script.Plane;

public class Tunnel
{

    public ArrayList TunnelMeshList;

    private readonly ArrayList _tunnelPlanesList;
    private Vector3 _tunnelDirection;

    public Tunnel()
    {
        _tunnelPlanesList = new ArrayList();
    }

    private void SetTunnelPlane(ArrayList tunnelHexagonsList, int hexagonIndex, int planeIndex)
    {
        Plane tunnelPlane = _tunnelPlanesList[planeIndex] as Plane;
        if (tunnelPlane == null)
        {
            //Create tunnel plane
            tunnelPlane = new Plane();
            tunnelPlane.firstHexagon = tunnelHexagonsList[hexagonIndex] as Hexagon;
            tunnelPlane.secondHexagon = tunnelHexagonsList[hexagonIndex + 1] as Hexagon;
            tunnelPlane.planeOrigenIndex = planeIndex;
            tunnelPlane.SetPlane();
            _tunnelPlanesList[planeIndex] = tunnelPlane;
        }

        _tunnelDirection = (tunnelPlane.secondHexagon.centerHexagon - tunnelPlane.firstHexagon.centerHexagon).normalized;
            
    }




}
