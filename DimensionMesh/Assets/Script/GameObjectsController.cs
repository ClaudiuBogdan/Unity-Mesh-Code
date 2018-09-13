using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using UnityEngine;

public class GameObjectsController : MonoBehaviour {

    //Reference to the tunnel generator
    private TunnelGenerator _tunnelGenerator;
    //Reference to the tunnel hexagons
    private ArrayList _hexagonsList;
    //Reference to the tunnel lights
    private ArrayList _lightObjectsList;
    //Reference to the tunnel enemies
    private EnemyController _enemiesController;
    //Reference to the tunnel mesh list
    private ArrayList _tunnelMeshList;
    //Reference to the player controller
    private CharacterControllerHex _playerController;
    //Current player position
    private int _playerTunnelPosition;


	// Use this for initialization
	void Start () {
        //init variables
	    _tunnelGenerator = GameObject.Find("ScriptContainer").GetComponent<TunnelGenerator>();
        _hexagonsList = _tunnelGenerator.tunnelHexagonsList;
        _playerController = GameObject.Find("ScriptContainer").GetComponent<CharacterControllerHex>();
	    _enemiesController = GameObject.Find("ScriptContainer").GetComponent<EnemyController>();
        GenerateTunnelMesh();

        RenderTunnelMesh();

	}
	
	// Update is called once per frame
	void Update () {
	    if (_playerTunnelPosition < GetCurrentPlayerTunnelPosition())
	    {
	        _playerTunnelPosition = GetCurrentPlayerTunnelPosition();
	        UpdateControllersCurrentPosition(_playerTunnelPosition);
            //Deactivate last assets
            //List with game objects that will be deactivated
	        RenderEnemies();
            RenderLights();
            RenderTunnelMesh();
            //Activate next assets
            //List with game objects that will be activated

            Debug.Log("Current player position: " + _playerTunnelPosition);
	    }
	}

    private void UpdateControllersCurrentPosition(int playerTunnelPosition)
    {
        _enemiesController.SetCurrentTunnelPosition(playerTunnelPosition);
        _tunnelGenerator.SetCurrentTunnelPosition(playerTunnelPosition);
    }

    private void RenderTunnelMesh()
    {
        GenerateTunnelMesh();
        GameObject.Find("Transformar").GetComponent<MeshFilter>().mesh =
            MeshGenerator.combineMeshes(_tunnelMeshList.ToArray(typeof(Mesh)) as Mesh[]);
    }

    private void RenderEnemies()
    {
        _enemiesController.DestroyEnemiesList();
        _enemiesController.CreateEnemiesList();
    }

    private void RenderLights()
    {
        _tunnelGenerator.DestroyTunnelLights();
        _tunnelGenerator.CreateTunnelLights();
    }

    private void GenerateTunnelMesh()
    {
        _tunnelMeshList = new ArrayList();
        Debug.Log("Hexagons list capacity: " + _hexagonsList.Capacity);
        int startSectionIndex = _playerTunnelPosition > 0 ? _playerTunnelPosition - 1 : 0;
        int tunnelSectionsToGenerate = 2;
        for (int i = startSectionIndex; i < _playerTunnelPosition + tunnelSectionsToGenerate; i++)
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
        return _playerController.GetPlayerPlane().firstHexagonIndex;
    }
}
