using UnityEngine;

namespace Assets.Script
{
    public class CharacterControllerHex : MonoBehaviour
    {

        public GameObject playerObject;
        public GameObject tunnelObject;

        private Rigidbody rigidBodyPlayer;
        private Plane playerPlane;
        private Vector3 tunnelDirection;
        private Vector3 lateralDirection;

        private float lateralSpeed =0.2f;
        private float forwardSpeed = 0.01f;
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

            Vector3 localStartPosition = new Vector3(0f, 0.5f, 0.0f);
            Vector3 startPlayerPosition = playerPlane.CalculateGlobalPosition(localStartPosition);
            playerObject.transform.position = startPlayerPosition;

            Quaternion rotation = new Quaternion();
            rotation.SetLookRotation(tunnelDirection, playerPlane.planeNormal);
            playerObject.transform.localRotation = rotation;

            rigidBodyPlayer = playerObject.GetComponent<Rigidbody>();

        }

        // Update is called once per frame
        void Update ()
        {
            playerObject.transform.position += tunnelDirection * forwardSpeed;
        }

        void FixedUpdate()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");

            Vector3 lateralMovement = playerPlane.j.normalized * moveHorizontal * lateralSpeed;

            playerObject.transform.position += lateralMovement;
            //rigidBodyPlayer.AddForce(movement * lateralSpeed);
        }



    }
}
