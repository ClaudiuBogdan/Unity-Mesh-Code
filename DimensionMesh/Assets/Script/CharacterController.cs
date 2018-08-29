using UnityEngine;

namespace Assets.Script
{
    public class CharacterController : MonoBehaviour
    {

        public GameObject playerObject;
        public GameObject tunnelObject;

        private Plane playerPlane;
        private Vector3 tunnelDirection;

        // Use this for initialization
        void Start ()
        {
            TunnelGenerator tunnelGenerator = tunnelObject.GetComponent<TunnelGenerator>();
            playerPlane = new Plane();
            Hexagon firstHexagon = tunnelGenerator.tunnelHexagonsList[0] as Hexagon;
            Hexagon secondHexagon = tunnelGenerator.tunnelHexagonsList[1] as Hexagon;
            Vector3 rightVerticesFirstHexagon = (firstHexagon).GetHexVerticesVector(0);
            Vector3 leftVerticesFirstHexagon = (firstHexagon).GetHexVerticesVector(1);

            Vector3 rightVerticesSecondHexagon = (secondHexagon).GetHexVerticesVector(0);
            Vector3 leftVerticesSecondHexagon = (secondHexagon).GetHexVerticesVector(1);

            playerPlane.SetPlane(rightVerticesFirstHexagon, leftVerticesFirstHexagon, rightVerticesSecondHexagon, leftVerticesSecondHexagon);
            tunnelDirection = (secondHexagon.centerHexagon - firstHexagon.centerHexagon).normalized;

            Vector3 startPlayerPosition = leftVerticesFirstHexagon + ((rightVerticesFirstHexagon - leftVerticesFirstHexagon).normalized*(rightVerticesFirstHexagon - leftVerticesFirstHexagon).magnitude / 2);
            playerObject.transform.position = startPlayerPosition;
            /*//Test 3 calculateCoordenateBase()
            Vector3 secondVertefLeft = new Vector3(2, 0, 0); //Base reference
            Vector3 firstVertexRight = new Vector3(0, 2, 0);
            Vector3 testVector4 = new Vector3(2, 2, 0);
            Plane plane4 = new Plane(firstVertexRight, /*Vector3.zero#1# new Vector3(1.0f, 1.0f, 0), Vector3.zero, secondVertefLeft);
            Debug.Log(plane4.CalculateOriginCoordinateBase(testVector4).ToString());  // [1, 1]*/

        }

        // Update is called once per frame
        void Update ()
        {
            playerObject.transform.position += tunnelDirection * 0.01f;
        }
        
        

    }
}
