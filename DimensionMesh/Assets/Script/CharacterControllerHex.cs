using UnityEngine;

namespace Assets.Script
{
    public class CharacterControllerHex : MonoBehaviour
    {
        private const int MOVED_FROM_CENTER = 0;
        private const int MOVED_FROM_LEFT = 1;
        private const int MOVED_FROM_RIGHT = 2;
        private const int MOVED_FROM_INFERIOR_PLANE = 3;

        public GameObject playerObject;
        public GameObject tunnelObject;
        public Camera playerCamera;
        public Vector3 pos;
        
        private TunnelGenerator tunnelGenerator;

        private Rigidbody rigidBodyPlayer;
        private Plane playerPlane;
        private Vector3 tunnelDirection;
        private Vector3 lateralDirection;

        private float lateralSpeed = -0.2f;
        private float forwardSpeed = 0.05f;
        // Use this for initialization
        void Start ()
        {
            tunnelGenerator = tunnelObject.GetComponent<TunnelGenerator>();
            playerPlane = new Plane();
            pos = new Vector3();


            playerPlane.tunnelGenerator = tunnelGenerator;
            int planeOrigenIndex = 2;
            playerPlane.planeOrigenIndex = planeOrigenIndex;
            playerPlane.firstHexagonIndex = 0;
            SetPlayerPlane();

            //Camera init
            Vector3 firstHexagonCenter = (tunnelGenerator.tunnelHexagonsList[0] as Hexagon).centerHexagon;
            Vector3 firstHexagonLocalCenter = playerPlane.CalculateCoordinateLocalBase(firstHexagonCenter);
            firstHexagonCenter = playerPlane.CalculateGlobalPosition(new Vector3(firstHexagonLocalCenter.x - 0.7f, firstHexagonLocalCenter.y, firstHexagonLocalCenter.z));
            playerCamera.transform.position = firstHexagonCenter;
            Quaternion rotation = new Quaternion();
            rotation.SetLookRotation(tunnelDirection, firstHexagonCenter - playerPlane.firstVertexRight);
            playerCamera.transform.localRotation = rotation;

            rigidBodyPlayer = playerObject.GetComponent<Rigidbody>();

        }

        // Update is called once per frame
        void Update ()
        {
            

        }

        void FixedUpdate()
        {
            pos = playerObject.transform.position;
            playerPlane.playerCenterPosition = pos;
            ControlPlayerMovement();
            playerCamera.transform.position += tunnelDirection * forwardSpeed;
        }

        private void ControlPlayerMovement()
        {
            //Forward movement
            playerObject.transform.position += tunnelDirection * forwardSpeed;

            //Lateral movement
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector3 lateralMovement = playerPlane.j.normalized * moveHorizontal * lateralSpeed;
            playerObject.transform.position += lateralMovement;
            //rigidBodyPlayer.AddForce(movement * lateralSpeed);

            //Detect plane change
            DetectPlaneChange();
        }

        private void DetectPlaneChange()
        {
            if (playerPlane.HasCrossedLeftSide())
            {
                ChangeToNextLeftPlane();
            }
            else if (playerPlane.HasCrossedRightSide())
            {
                ChangeToNextRightPlane();
            }
            if (playerPlane.HasCrossedSuperiorSide())
            {
                ChangeToNextSuperiorPlane();
            }
        }

        private void ChangeToNextSuperiorPlane()
        {
            playerPlane.firstHexagonIndex = playerPlane.firstHexagonIndex + 1;
            SetPlayerPlane(MOVED_FROM_INFERIOR_PLANE);
        }

        private void ChangeToNextRightPlane()
        {
            Debug.Log("Player moved to RIGHT plane");
            GetRightPlane();            
        }

        private void ChangeToNextLeftPlane()
        {
            Debug.Log("Player moved to LEFT plane");
            GetLeftPlane();            
        }

        private void GetRightPlane()
        {
            //Get next plane origin index to the right
            int lastHexVertice = 5;
            int firstHexVertice = 0;
            playerPlane.planeOrigenIndex =
                playerPlane.planeOrigenIndex < lastHexVertice ? playerPlane.planeOrigenIndex + 1 : firstHexVertice;
            SetPlayerPlane(MOVED_FROM_LEFT);
        }

        private void GetLeftPlane()
        {
            //Get next plane origin index to the left
            int lastHexVertice = 5;
            int firstHexVertice = 0;
            playerPlane.planeOrigenIndex =
                playerPlane.planeOrigenIndex > firstHexVertice ? playerPlane.planeOrigenIndex - 1 : lastHexVertice;
            SetPlayerPlane(MOVED_FROM_RIGHT);
        }

        private void SetPlayerPlane()
        {
            SetPlayerPlane(MOVED_FROM_CENTER);
        }

        private void SetPlayerPlane(int movedFromDirection)
        {
            playerPlane.SetPlane();
            Vector3 playerGlobalCoordPosition = playerObject.transform.position;
            Vector3 playerLocalCoordPosition = playerPlane.CalculateCoordinateLocalBase(playerGlobalCoordPosition);

            float initialForwardPercentage = 0.0f;
            float initialLaterlPercentage = 0.5f;
            switch (movedFromDirection)
            {
                case MOVED_FROM_LEFT:
                    initialLaterlPercentage = 0.1f;
                    initialForwardPercentage = playerLocalCoordPosition.x;
                    break;
                case MOVED_FROM_RIGHT:
                    initialLaterlPercentage = 0.9f;
                    initialForwardPercentage = playerLocalCoordPosition.x;
                    break;
                case MOVED_FROM_CENTER:
                    initialLaterlPercentage = 0.5f;
                    initialForwardPercentage = 0;
                    break;
                case MOVED_FROM_INFERIOR_PLANE:
                    initialLaterlPercentage = playerLocalCoordPosition.y;
                    initialForwardPercentage = 0;
                    break;
            }
            Debug.Log("Plane index at set time: " + playerPlane.planeOrigenIndex);
            //lateralSpeed = -lateralSpeed;         

            tunnelDirection = (playerPlane.secondHexagon.centerHexagon - playerPlane.firstHexagon.centerHexagon).normalized;

            Vector3 localStartPosition = new Vector3(initialForwardPercentage, initialLaterlPercentage, 0.0f);
            Vector3 startPlayerPosition = playerPlane.CalculateGlobalPosition(localStartPosition);
            playerObject.transform.position = startPlayerPosition;

            Quaternion rotation = new Quaternion();
            rotation.SetLookRotation(tunnelDirection, -playerPlane.planeNormal);
            playerObject.transform.localRotation = rotation;
        }

        private void MoveToRight(float distance)
        {
            Vector3 lateralMovement = playerPlane.j.normalized * Mathf.Abs(distance) * lateralSpeed;
            playerObject.transform.position += lateralMovement;
        }

        private void MoveToLeft(float distance)
        {
            Vector3 lateralMovement = playerPlane.j.normalized * -Mathf.Abs(distance) * lateralSpeed;
            playerObject.transform.position += lateralMovement;
        }
    }
}
