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
        public GameObject playerCamera;
        public Vector3 pos;
        
        private TunnelGenerator tunnelGenerator;

        private Rigidbody rigidBodyPlayer;
        private Plane playerPlane;
        private Vector3 tunnelDirection;
        private Vector3 lateralDirection;

        private float lateralSpeed = -0.2f;
        private float forwardSpeed = 0.15f;
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
            SetPlayerCamera();

            //Initial camera rotation
            Quaternion cameraRotation = new Quaternion();
            cameraRotation.SetLookRotation(tunnelDirection, playerPlane.firstHexagon.centerHexagon - playerPlane.firstHexagon.GetHexVerticesVector(0));
            playerCamera.transform.localRotation = cameraRotation;

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
            Vector3 lateralMovement = Vector3.Cross(playerPlane.planeNormal, tunnelDirection).normalized * GetLateralFactor(moveHorizontal);
            playerObject.transform.position += lateralMovement;
            //rigidBodyPlayer.AddForce(movement * lateralSpeed);
            //Detect plane change
            DetectPlaneChange();
        }

        private float GetLateralFactor(float moveHorizontal)
        {
            float lateralLimitFactor = 0.4f;
            float lateralFactor = moveHorizontal * lateralSpeed;
            int factorSign = lateralFactor > 0 ? 1 : -1;
            return Mathf.Abs(lateralFactor) > lateralLimitFactor ? lateralLimitFactor * factorSign : lateralFactor;
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

            if (IsCameraInTransition())
            {
                PerformCameraTransition();
            }
        }
        

        private void ChangeToNextSuperiorPlane()
        {
            playerPlane.firstHexagonIndex = playerPlane.firstHexagonIndex + 1;
            SetPlayerPlane(MOVED_FROM_INFERIOR_PLANE);
            SetPlayerCamera();                  
        }

        private void ChangeToNextRightPlane()
        {
            GetRightPlane();            
        }

        private void ChangeToNextLeftPlane()
        {
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
            playerPlane.SetPlane(playerPlane.firstHexagonIndex);
            Vector3 playerGlobalCoordPosition = playerObject.transform.position;
            Vector3 playerLocalCoordPosition = playerPlane.CalculateCoordinateLocalBase(playerGlobalCoordPosition);

            float initialForwardPercentage = 0.0f;
            float initialLaterlPercentage = 0.5f;
            switch (movedFromDirection)
            {
                case MOVED_FROM_LEFT:
                    initialLaterlPercentage = 0.15f;
                    initialForwardPercentage = playerLocalCoordPosition.x;
                    break;
                case MOVED_FROM_RIGHT:
                    initialLaterlPercentage = 0.85f;
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

        private void SetPlayerCamera()
        {
            //Camera init
            Vector3 firstHexagonCenter = playerPlane.firstHexagon.centerHexagon;
            Vector3 firstHexagonLocalCenter = playerPlane.CalculateCoordinateLocalBase(firstHexagonCenter);
            firstHexagonCenter = playerPlane.CalculateGlobalPosition(new Vector3(0, firstHexagonLocalCenter.y, firstHexagonLocalCenter.z));
            playerCamera.transform.position = firstHexagonCenter;
            /*Quaternion cameraRotation = new Quaternion();
            cameraRotation.SetLookRotation(tunnelDirection, playerPlane.firstHexagon.centerHexagon - playerPlane.firstHexagon.GetHexVerticesVector(0));
            playerCamera.transform.localRotation = cameraRotation;*/
        }

        private void PerformCameraTransition()
        {
            Quaternion cameraRotation = new Quaternion();
            Vector3 vectorTransition = (playerCamera.transform.forward - tunnelDirection).normalized * 0.01f;
            cameraRotation.SetLookRotation(playerCamera.transform.forward - vectorTransition, playerPlane.firstHexagon.centerHexagon - playerPlane.firstHexagon.GetHexVerticesVector(0));
            playerCamera.transform.localRotation = cameraRotation;
        }

        private bool IsCameraInTransition()
        {
            Quaternion cameraRotation = new Quaternion();
            cameraRotation.SetLookRotation(tunnelDirection, playerPlane.firstHexagon.centerHexagon - playerPlane.firstHexagon.GetHexVerticesVector(0));
            Vector3 cameraAngles = playerCamera.transform.forward;
            Vector3 distanceToEndTransition =  Vector3.Cross(cameraAngles.normalized, tunnelDirection.normalized);
            return distanceToEndTransition.magnitude > 0.1f;
        }

        public Plane GetPlayerPlane()
        {
            return this.playerPlane;
        }

        public float getPlayerAdvencePosition()
        {
            Vector3 playerGlobalCoordPosition = playerObject.transform.position;
            Vector3 playerLocalCoordPosition = playerPlane.CalculateCoordinateLocalBase(playerGlobalCoordPosition);
            return playerLocalCoordPosition.x;
        }
    }
}
