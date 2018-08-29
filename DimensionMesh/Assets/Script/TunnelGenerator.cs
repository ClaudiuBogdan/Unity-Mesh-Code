using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class TunnelGenerator : MonoBehaviour {

        public ArrayList tunnelHexagonsList = new ArrayList();
        public ArrayList tunnelMeshList = new ArrayList();
        // Use this for initialization
        void Awake()
        {
            Vector3 centerHexagon = new Vector3(0, 0, 0);
            Vector3 normalVectorHexagon = new Vector3(0, 1, 0);
            Hexagon firstHexagon = Hexagon.NewInstance(centerHexagon, normalVectorHexagon, radioDimension: 2, initialAngle: 0);

            Vector3 centerHexagon2 = new Vector3(0, 10, 0);
            Vector3 normalVectorHexagon2 = new Vector3(0, 1, 0);
            Hexagon secondHexagon = Hexagon.NewInstance(centerHexagon2, normalVectorHexagon2, radioDimension: 2, initialAngle: 0);


            
            foreach (Mesh sideMesh in MeshGenerator.generateHexToken(firstHexagon, secondHexagon))
            {
                tunnelMeshList.Add(sideMesh);
            }

            tunnelHexagonsList.Add(firstHexagon);
            tunnelHexagonsList.Add(secondHexagon);
            int maxTunnelTokens = 100;
            for (int i = 0; i < maxTunnelTokens; i++)
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

        void Start () {

           
            Debug.Log("Mesh tunnel tokens: " + tunnelHexagonsList.Count);
            GameObject.Find("Transformar").GetComponent<MeshFilter>().mesh =
                MeshGenerator.combineMeshes(tunnelMeshList.ToArray(typeof(Mesh)) as Mesh[]);

        }
	
        // Update is called once per frame
        void Update () {
		
        }
    }
}
