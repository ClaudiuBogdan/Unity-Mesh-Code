using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using Assets.Script.Data.Enemy;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject tunnelObject;

    private TunnelGenerator _tunnelGenerator;
    private ArrayList _enemyUnitList;
    private int _currentTunnelIndex;

	// Use this for initialization
	void Start () {
	    _tunnelGenerator = tunnelObject.GetComponent<TunnelGenerator>();
        _enemyUnitList = new ArrayList();
        //Generate enemy on demand based on the player position

	}
	
	// Update is called once per frame
	void Update () {
		
        //Render the objects
	}

    public void SetCurrentTunnelPosition(int currentTunnelPositionIndex)
    {
        this._currentTunnelIndex = currentTunnelPositionIndex;
    }

    public void CreateEnemiesList()
    {
        ArrayList enemyPositionList = _tunnelGenerator.GetEnemyPositionList(_currentTunnelIndex);
        Debug.Log("Enemy position list size: " + enemyPositionList.Count);
        int increment = 1;
        for (int i = 5; i < enemyPositionList.Count - increment; i = i + increment)
        {
            Enemy enemyUnit = new Enemy(Instantiate(EnemyPrefab, Vector3.zero, Quaternion.identity));
            Transform enemyTransform = new RectTransform();
            Debug.Log("Enemy index: " + i + " count: " + enemyPositionList.Count);
            enemyUnit.SetPosition(enemyPositionList[i] is Vector3 ? (Vector3)enemyPositionList[i] : new Vector3());

            _enemyUnitList.Add(enemyUnit);
        }
    }

    public void DestroyEnemiesList()
    {
        for (int i = 0; i < _enemyUnitList.Count; i++)
        {
            Enemy enemy = _enemyUnitList[i] as Enemy;
            Destroy(enemy.GetEnemyReference());
        }
        _enemyUnitList = new ArrayList();
    }
}
