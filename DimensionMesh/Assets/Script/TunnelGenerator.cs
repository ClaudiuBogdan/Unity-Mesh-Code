using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class TunnelGenerator : MonoBehaviour {

        public ArrayList tunnelHexagonsList = new ArrayList();
        public ArrayList tunnelMeshList = new ArrayList();
        public GameObject LightObject;
        // Use this for initialization
        void Awake()
        {
            GenerateTunnelHexagons();
            GenerateTunnelLights();
        }

        void Start () {

           
            Debug.Log("Mesh tunnel tokens: " + tunnelHexagonsList.Count);
            GameObject.Find("Transformar").GetComponent<MeshFilter>().mesh =
            MeshGenerator.combineMeshes(tunnelMeshList.ToArray(typeof(Mesh)) as Mesh[]);
            Debug.Log("First hexagon vertices: " + GameObject.Find("Transformar").GetComponent<MeshFilter>().mesh.vertices[0]);

        }
	
        // Update is called once per frame
        void Update () {

        }

        private void GenerateTunnelLights()
        {
            Tunnel tunnelToken = new Tunnel();
            for (int hexagonIndex = 0; hexagonIndex < tunnelHexagonsList.Count - 1; hexagonIndex++)
            {
                int indexFirstPlane = 3;
                int indexSecondPlane = 4;
                tunnelToken.SetAllTunnelPlanes(tunnelHexagonsList, hexagonIndex);
                Vector3 firstNormal = (tunnelToken.TunnelPlanesList[indexFirstPlane] as Plane).planeNormal;
                Vector3 secondNormal = (tunnelToken.TunnelPlanesList[indexSecondPlane] as Plane).planeNormal;
                Vector3 lightNormal = (-firstNormal).normalized;
                Quaternion lightRotation = new Quaternion();
                Plane referencePlane = tunnelToken.TunnelPlanesList[indexFirstPlane] as Plane;
                lightRotation.SetLookRotation(lightNormal, referencePlane.firstHexagon.centerHexagon - referencePlane.firstHexagon.GetHexVerticesVector(0));
                Vector3 positionFirstLight = referencePlane.CalculateGlobalPosition(new Vector3(0.33f,0,-0.2f));
                Vector3 positionSecondLight = referencePlane.CalculateGlobalPosition(new Vector3(0.66f, 0, -0.2f));
                GameObject lightClone = (GameObject)Instantiate(LightObject, positionFirstLight, lightRotation);
                GameObject lightClone2 = (GameObject)Instantiate(LightObject, positionSecondLight, lightRotation);
            }
        }

        private void GenerateTunnelHexagons() {

            Vector3 centerHexagon = new Vector3(0, 0, 0);
            Vector3 normalVectorHexagon = new Vector3(0, 1, 0);
            Hexagon firstHexagon = Hexagon.NewInstance(centerHexagon, normalVectorHexagon, radioDimension: 2, initialAngle: 0);

            Vector3 centerHexagon2 = new Vector3(0, 5, 0);
            Vector3 normalVectorHexagon2 = new Vector3(0, 1, 0);
            Hexagon secondHexagon = Hexagon.NewInstance(centerHexagon2, normalVectorHexagon2, radioDimension: 2, initialAngle: 0);



            foreach (Mesh sideMesh in MeshGenerator.generateHexToken(firstHexagon, secondHexagon))
            {
                tunnelMeshList.Add(sideMesh);
            }

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
                foreach (Mesh sideMesh in MeshGenerator.generateHexToken(lastHexagon, nextHexagon))
                {
                    tunnelMeshList.Add(sideMesh);
                }
                tunnelHexagonsList.Add(nextHexagon);
            }
        }
    }
}
