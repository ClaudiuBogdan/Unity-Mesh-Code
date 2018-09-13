using UnityEngine;

namespace Assets.Script.Data.Enemy
{
    public class Enemy
    {

        private readonly GameObject _enemyReference;
        private GameObject _playerReference;

        public Enemy(GameObject enemyCloneInstance)
        {
            _enemyReference = enemyCloneInstance;
        }

        public void Shut()
        {

        }

        public void Distroy()
        {
        }

        public void SetPlayerReference(GameObject playerReference)
        {
            this._playerReference = playerReference;
        }

        public void SetPosition(Vector3 enemyPosition)
        {
            this._enemyReference.transform.position = enemyPosition;
        }

        public GameObject GetEnemyReference()
        {
            return _enemyReference;
        }
    }
}
