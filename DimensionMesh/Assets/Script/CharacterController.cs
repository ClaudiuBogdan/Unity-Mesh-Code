﻿using UnityEngine;

namespace Assets.Script
{
    public class CharacterController : MonoBehaviour {

        // Use this for initialization
        void Start () {

	        //Test 2 calculateCoordenateBase()
	        Vector3 i2 = new Vector3(3, 1, 0);
	        Vector3 j2 = new Vector3(-2, 1, 0);
	        Vector3 testVector2 = new Vector3(4, 3, 5);
            Plane plane1 = new Plane(j2, Vector3.zero, Vector3.zero, i2 );
            Debug.Log(plane1.CalculateCoordinateBase(testVector2).ToString()); // [2, 1]

	        //Test 3 calculateCoordenateBase()
	        Vector3 i3 = new Vector3(1, 2, 0);
	        Vector3 j3 = new Vector3(-2, 3, 0);
	        Vector3 testVector3 = new Vector3(3, 5, 0);
            Plane plane2 = new Plane(j3, Vector3.zero, Vector3.zero, i3);
            Debug.Log(plane2.CalculateCoordinateBase(testVector3).ToString());  // [2.7, -0.14]

        }

        // Update is called once per frame
        void Update () {
		
        }
        
        

    }
}
