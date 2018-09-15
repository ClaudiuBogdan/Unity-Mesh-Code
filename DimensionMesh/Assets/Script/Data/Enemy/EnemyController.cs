using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using Assets.Script.Data.Enemy;
using UnityEngine;
using Plane = Assets.Script.Plane;

public class EnemyController : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public GameObject tunnelObject;

    private TunnelGenerator _tunnelGenerator;
    private Queue<Enemy> _enemyUnitList;
    private int _currentTunnelIndex;
    private int _lastEnemyIndex;
    private ArrayList enemyPositionList;
    private ArrayList enemyRotationList;
    private float _playerAdvancePosition;
    private Plane _enemyPlaneReference;

    // Use this for initialization
    void Start () {
	    _tunnelGenerator = tunnelObject.GetComponent<TunnelGenerator>();
        _enemyUnitList = new Queue<Enemy>();
        //Generate enemy on demand based on the player position

    }
	
	// Update is called once per frame
	void Update () {
		
        //Render the objects
	}

    public void SetCurrentTunnelPosition(int currentTunnelPositionIndex)
    {
        this._currentTunnelIndex = currentTunnelPositionIndex;
        this._lastEnemyIndex = 0;
        ArrayList[] generatedPositionEnemies = _tunnelGenerator.GetEnemyPositionList(_currentTunnelIndex);
        enemyPositionList = generatedPositionEnemies[0];
        enemyRotationList = generatedPositionEnemies[1];
        _enemyPlaneReference = (generatedPositionEnemies[2] as ArrayList)[0] as Plane;
        DestroyAllEnemies();
    }

    public void CreateEnemiesList()
    {
        if (_lastEnemyIndex < enemyPositionList.Count)
        {
            //Debug.Log("Enemy position list size: " + enemyPositionList.Count);

            int maxEnemyLoaded = 40;
            int enemyToBeLoaded =maxEnemyLoaded - _enemyUnitList.Count;
            for (int i = 0; i < enemyToBeLoaded; i = i + 1)
            {
                Enemy enemyUnit = new Enemy(Instantiate(EnemyPrefab, Vector3.zero, Quaternion.identity));
                Debug.Log("Enemy index: " + _lastEnemyIndex + " count: " + enemyPositionList.Count);
                if (_lastEnemyIndex < enemyPositionList.Count)
                {
                    enemyUnit.SetPosition(enemyPositionList[_lastEnemyIndex] is Vector3 ? (Vector3)enemyPositionList[_lastEnemyIndex] : new Vector3());
                    enemyUnit.SetRotation(enemyRotationList[_lastEnemyIndex] is Quaternion ? (Quaternion)enemyRotationList[_lastEnemyIndex] : Quaternion.identity);
                    _enemyUnitList.Enqueue(enemyUnit);
                }
                else
                {
                    return;
                }
                _lastEnemyIndex++;
               
            }
        }
        
    }

    public void DestroyEnemiesList()
    {
        int differentialPlayerPosition = 4;
        if(GetEnemyAdvancePosition(_enemyUnitList.Peek()) < _playerAdvancePosition - differentialPlayerPosition)
        {
            Enemy enemy = _enemyUnitList.Dequeue();
            Destroy(enemy.GetEnemyReference());
        }
    }

    private void DestroyAllEnemies()
    {
        foreach (Enemy enemy in _enemyUnitList)
        {
            Destroy(enemy.GetEnemyReference());
        }
        _enemyUnitList.Clear();
    }

    private float GetEnemyAdvancePosition(Enemy enemy)
    {
        Vector3 enemyGlobalCoordPosition = enemy.GetEnemyReference().transform.position;
        Vector3 enemyLocalCoordPosition = _enemyPlaneReference.CalculateCoordinateLocalBase(enemyGlobalCoordPosition);
        return enemyLocalCoordPosition.x * _enemyPlaneReference.i.magnitude;
    }

    public IEnumerator AutoGenerateEnemies()
    {
        if (true)
        {
            for (int i = 0; i < _enemyUnitList.Count; i++)
            {

                Enemy enemy = _enemyUnitList.Dequeue();
                Destroy(enemy.GetEnemyReference());
                if (i % 5 == 0)
                {
                    yield return null;
                }

            }
            _enemyUnitList = new Queue<Enemy>();
            //StartCoroutine(CreateEnemiesList());
        }
    }

    //Absolute reference
    public void setPlayerAdvancePosition(float playerAdvancePosition)
    {
        if(_enemyPlaneReference == null)
            return;
        this._playerAdvancePosition = playerAdvancePosition * _enemyPlaneReference.i.magnitude;
    }
}
