using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;

namespace Assets.Script
{
    public class TunnelGenerator : MonoBehaviour {

        public ArrayList tunnelHexagonsList = new ArrayList();
        public ArrayList tunnelMeshList = new ArrayList();
        public ArrayList tunnelLightsList = new ArrayList();
        public GameObject LightObject;
        private int _currentPlayerTunnelPosition;
        // Use this for initialization
        void Awake()
        {
            GenerateTunnelHexagons();
        }

        void Start () {
            
        }
	
        // Update is called once per frame
        void Update () {

        }

        public IEnumerator CreateTunnelLights()
        {
        Tunnel tunnelToken = new Tunnel();
            int indexFirstPlane = 3;
            int indexSecondPlane = 4;
            tunnelToken.SetAllTunnelPlanes(tunnelHexagonsList, _currentPlayerTunnelPosition);
            Vector3 firstNormal = (tunnelToken.TunnelPlanesList[indexFirstPlane] as Plane).planeNormal;
            Vector3 secondNormal = (tunnelToken.TunnelPlanesList[indexSecondPlane] as Plane).planeNormal;
            Vector3 lightNormal = (-firstNormal).normalized;
            Quaternion lightRotation = new Quaternion();
            Plane referencePlane = tunnelToken.TunnelPlanesList[indexFirstPlane] as Plane;
            lightRotation.SetLookRotation(lightNormal, referencePlane.firstHexagon.centerHexagon - referencePlane.firstHexagon.GetHexVerticesVector(0));
            int lightsPerTunnel = ((int)(tunnelToken.TunnelPlanesList[indexFirstPlane] as Plane).i.magnitude / 20) + 1;
          
            for (int lightIndex = 1; lightIndex <= lightsPerTunnel; lightIndex++)
            {
                Vector3 positionFirstLight = referencePlane.CalculateGlobalPosition(new Vector3(lightIndex * 1.0f/(lightsPerTunnel + 1), 0, -0.2f));
                GameObject lightClone = (GameObject)Instantiate(LightObject, positionFirstLight, lightRotation);
                tunnelLightsList.Add(lightClone);
                yield return null;
            }
            
        }

        public void DestroyTunnelLights()
        {
            for (int i = 0; i < tunnelLightsList.Count; i++)
            {
                GameObject lightObject = tunnelLightsList[i] as GameObject;
                Destroy(lightObject);
            }
            tunnelLightsList = new ArrayList();
        }

        private void GenerateTunnelHexagons() {

            Vector3 centerHexagon = new Vector3(0, 0, 0);
            Vector3 normalVectorHexagon = new Vector3(0, 1, 0);
            Hexagon firstHexagon = Hexagon.NewInstance(centerHexagon, normalVectorHexagon, radioDimension: 2, initialAngle: 0);

            Vector3 centerHexagon2 = new Vector3(0, 5, 0);
            Vector3 normalVectorHexagon2 = new Vector3(0, 1, 0);
            Hexagon secondHexagon = Hexagon.NewInstance(centerHexagon2, normalVectorHexagon2, radioDimension: 2, initialAngle: 0);

            tunnelHexagonsList.Add(firstHexagon);
            tunnelHexagonsList.Add(secondHexagon);


            int maxTunnelTokens = 30;
            for (int i = 1; i < maxTunnelTokens; i++)
            {
                int a = 10;
                int b = 20;
                Hexagon lastHexagon = tunnelHexagonsList[tunnelHexagonsList.Count - 1] as Hexagon;
                Vector3 rotateReferenceHexNormal =
                    Vector3.Cross(lastHexagon.normalVectorHexagon, lastHexagon.rotationalVectorHexagon);
                Vector3 normalVectorHexagonNext = Hexagon.RotateVector(lastHexagon.normalVectorHexagon, 30, rotateReferenceHexNormal);
                Hexagon nextHexagon = Hexagon.NewInstance(lastHexagon, distanceFromRefCenter: 5 * i, normalVectorHexagon: normalVectorHexagonNext, radioDimension: 2, initialAngle: 0);
                tunnelHexagonsList.Add(nextHexagon);
            }
        }

        

        public ArrayList GetEnemyPositionList(int playerTunnelPosition)
        {
            Tunnel tunnelToken = new Tunnel();
            ArrayList enemyPositionList = new ArrayList();

            int indexFirstPlane = Random.Range(0, 5);
            int indexSecondPlane = indexFirstPlane < 5 ? indexFirstPlane + 1 : 0;
            tunnelToken.SetAllTunnelPlanes(tunnelHexagonsList, playerTunnelPosition);
            Vector3 firstNormal = (tunnelToken.TunnelPlanesList[indexFirstPlane] as Plane).planeNormal;
            Vector3 secondNormal = (tunnelToken.TunnelPlanesList[indexSecondPlane] as Plane).planeNormal;
            Vector3 enemyNormal = (-firstNormal).normalized;
            Quaternion enemyRotation = new Quaternion();
            Plane referencePlane = tunnelToken.TunnelPlanesList[indexFirstPlane] as Plane;
            enemyRotation.SetLookRotation(enemyNormal, referencePlane.firstHexagon.centerHexagon - referencePlane.firstHexagon.GetHexVerticesVector(0));
            int enemiesPerTunnel = ((int)(tunnelToken.TunnelPlanesList[indexFirstPlane] as Plane).i.magnitude / 2) + 1;
            for (int lightIndex = 1; lightIndex <= enemiesPerTunnel; lightIndex++)
            {
                Vector3 enemyPosition = referencePlane.CalculateGlobalPosition(new Vector3(lightIndex * 1.0f / (enemiesPerTunnel + 1), Random.Range(0.0f, 1.0f), -0.2f));

                enemyPositionList.Add(enemyPosition);
                //enemyPositionList.Add(enemyRotation);

                indexFirstPlane = Random.Range(0, 5);
                indexSecondPlane = indexFirstPlane < 5 ? indexFirstPlane + 1 : 0;
                tunnelToken.SetAllTunnelPlanes(tunnelHexagonsList, playerTunnelPosition);
                firstNormal = (tunnelToken.TunnelPlanesList[indexFirstPlane] as Plane).planeNormal;
                secondNormal = (tunnelToken.TunnelPlanesList[indexSecondPlane] as Plane).planeNormal;
                enemyNormal = (-firstNormal).normalized;
                enemyRotation = new Quaternion();
                referencePlane = tunnelToken.TunnelPlanesList[indexFirstPlane] as Plane;
            }
            //Debug.Log("Enemy initial list capacity: " + +enemyPositionList.Capacity);
            return enemyPositionList;
        }

        public void SetCurrentTunnelPosition(int playerTunnelPosition)
        {
            this._currentPlayerTunnelPosition = playerTunnelPosition;
        }


        public IEnumerator AutoGenerateTunnelLights()
        {
            for (int i = 0; i < tunnelLightsList.Count; i++)
            {
                GameObject lightObject = tunnelLightsList[i] as GameObject;
                Destroy(lightObject);
                yield return null;
            }
            tunnelLightsList = new ArrayList();
            StartCoroutine(CreateTunnelLights());
        }
    }
}
