using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using UnityEngine;

public class GameObjectsRender : MonoBehaviour {

    //Reference to the tunnel hexagons
    private ArrayList _hexagonsList;
    //Reference to the tunnel lights
    private ArrayList _lightObjectsList;
    //Reference to the tunnel enemies
    private ArrayList _enemiesList;
    //Reference to the tunnel mesh list
    private ArrayList _tunnelMeshList;
    //Reference to the player controller
    private CharacterControllerHex _playerController;
    //Current player position
    private int _playerTunnelPosition;


	// Use this for initialization
	void Start () {
        //init variables
	    _hexagonsList = GameObject.Find("ScriptContainer").GetComponent<TunnelGenerator>().tunnelHexagonsList;
        _playerController = GameObject.Find("ScriptContainer").GetComponent<CharacterControllerHex>();
        GenerateTunnelMesh();

        RenderTunnelMesh();

	}
	
	// Update is called once per frame
	void Update () {
	    if (_playerTunnelPosition < GetCurrentPlayerTunnelPosition())
	    {
	        _playerTunnelPosition = GetCurrentPlayerTunnelPosition();
            Debug.Log("Current player position: " + _playerTunnelPosition);
	    }
	}

    private void RenderTunnelMesh()
    {
        GameObject.Find("Transformar").GetComponent<MeshFilter>().mesh =
            MeshGenerator.combineMeshes(_tunnelMeshList.ToArray(typeof(Mesh)) as Mesh[]);
    }

    private void RenderEnemies()
    {

    }

    private void RenderLights()
    {

    }

    private void GenerateTunnelMesh()
    {
        _tunnelMeshList = new ArrayList();
        Debug.Log("Hexagons list capacity: " + _hexagonsList.Capacity);
        for (int i = 0; i < _hexagonsList.Capacity - 2; i++)
        {
            Hexagon lastHexagon = _hexagonsList[i] as Hexagon;
            Hexagon nextHexagon = _hexagonsList[i + 1] as Hexagon;
            foreach (Mesh sideMesh in MeshGenerator.generateHexToken(lastHexagon, nextHexagon))
            {
                _tunnelMeshList.Add(sideMesh);
            }

        }
    }

    private int GetCurrentPlayerTunnelPosition()
    {
        return _playerController.getPlayerPlane().firstHexagonIndex;
    }
}
