﻿using UnityEngine;

namespace Assets.Script
{
    public class CharacterControllerHex : MonoBehaviour
    {

        public GameObject playerObject;
        public GameObject tunnelObject;
        public Vector3 pos;

        private TunnelGenerator tunnelGenerator;

        private Rigidbody rigidBodyPlayer;
        private Plane playerPlane;
        private Vector3 tunnelDirection;
        private Vector3 lateralDirection;

        private float lateralSpeed =0.2f;
        private float forwardSpeed = 0.01f;
        // Use this for initialization
        void Start ()
        {
            tunnelGenerator = tunnelObject.GetComponent<TunnelGenerator>();
            playerPlane = new Plane();
            pos = new Vector3();

            int planeOrigenIndex = 1;
            playerPlane.planeOrigenIndex = planeOrigenIndex;
            SetPlayerPlane();

            Vector3 localStartPosition = new Vector3(0f, 0.5f, 0.0f);
            Vector3 startPlayerPosition = playerPlane.CalculateGlobalPosition(localStartPosition);
            playerObject.transform.position = startPlayerPosition;
            

            rigidBodyPlayer = playerObject.GetComponent<Rigidbody>();

        }

        // Update is called once per frame
        void Update ()
        {
            pos = playerObject.transform.position;
            playerPlane.playerCenterPosition = pos;
        }

        void FixedUpdate()
        {
            ControlPlayerMovement();
            
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
            int firstHexVertice = 1;
            playerPlane.planeOrigenIndex =
                playerPlane.planeOrigenIndex < lastHexVertice ? playerPlane.planeOrigenIndex + 2 : firstHexVertice;
            SetPlayerPlane();
            MoveToRight(1.0f);
        }

        private void GetLeftPlane()
        {
            //Get next plane origin index to the left
            int lastHexVertice = 5;
            int firstHexVertice = 1;
            playerPlane.planeOrigenIndex =
                playerPlane.planeOrigenIndex > firstHexVertice ? playerPlane.planeOrigenIndex - 2 : lastHexVertice;
            Debug.Log("Plane index: " + playerPlane.planeOrigenIndex);
            SetPlayerPlane();
            MoveToLeft(1.0f);
        }

        private void SetPlayerPlane()
        {
            Hexagon firstHexagon = tunnelGenerator.tunnelHexagonsList[0] as Hexagon;
            Hexagon secondHexagon = tunnelGenerator.tunnelHexagonsList[1] as Hexagon;

            Vector3 rightVerticesFirstHexagon = (firstHexagon).GetHexVerticesVector(playerPlane.planeOrigenIndex - 1);
            Vector3 leftVerticesFirstHexagon = (firstHexagon).GetHexVerticesVector(playerPlane.planeOrigenIndex);

            Vector3 rightVerticesSecondHexagon = (secondHexagon).GetHexVerticesVector(playerPlane.planeOrigenIndex - 1);
            Vector3 leftVerticesSecondHexagon = (secondHexagon).GetHexVerticesVector(playerPlane.planeOrigenIndex);

            playerPlane.SetPlane(rightVerticesFirstHexagon, leftVerticesFirstHexagon, rightVerticesSecondHexagon, leftVerticesSecondHexagon);

            tunnelDirection = (secondHexagon.centerHexagon - firstHexagon.centerHexagon).normalized;

            Quaternion rotation = new Quaternion();
            rotation.SetLookRotation(tunnelDirection, playerPlane.planeNormal);
            //playerObject.transform.localRotation = rotation;
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
