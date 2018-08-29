using MatrixInverse;
using UnityEngine;
public class CharacterController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Test calculateCoordenateBase()
        Vector3 i = new Vector3(1, 0 , 0);
        Vector3 j = new Vector3(0, 1, 0);
	    Vector3 testVector = new Vector3(2, 2, 3);
        Debug.Log(CalculateCoordinateBase(testVector, i, j).ToString());

	    //Test 2 calculateCoordenateBase()
	    Vector3 i2 = new Vector3(3, 1, 0);
	    Vector3 j2 = new Vector3(-2, 1, 0);
	    Vector3 testVector2 = new Vector3(4, 3, 5);
	    Debug.Log(CalculateCoordinateBase(testVector2, i2, j2).ToString()); // [2, 1]

	    //Test 3 calculateCoordenateBase()
	    Vector3 i3 = new Vector3(1, 2, 0);
	    Vector3 j3 = new Vector3(-2, 3, 0);
	    Vector3 testVector3 = new Vector3(3, 5, 0);
	    Debug.Log(CalculateCoordinateBase(testVector3, i3, j3).ToString()); // [2.7, -0.14]

    }

    // Update is called once per frame
    void Update () {
		
	}


    /**
     * You should normalize the vector base before calling the function.
     */
    public Vector3 CalculateCoordinateBase(Vector3 mVector, Vector3 i, Vector3 j)
    {
        Vector3 k = Vector3.Cross(i, j).normalized;
        double[][] baseMatrix = MatrixInverseProgram.MatrixCreate(3, 3);
        baseMatrix[0][0] = i.x; baseMatrix[0][1] = j.x; baseMatrix[0][2] = k.x;
        baseMatrix[1][0] = i.y; baseMatrix[1][1] = j.y; baseMatrix[1][2] = k.y;
        baseMatrix[2][0] = i.z; baseMatrix[2][1] = j.z; baseMatrix[2][2] = k.z;

        double[][] inverseMatrix = MatrixInverseProgram.MatrixInverse(baseMatrix);
        double[][] baseVector = MatrixInverseProgram.MatrixCreate(3, 1);
        baseVector[0][0] = mVector.x;
        baseVector[1][0] = mVector.y;
        baseVector[2][0] = mVector.z;

        double[][] transformedVector = MatrixInverseProgram.MatrixProduct(inverseMatrix, baseVector);
        Vector3 vectorTransformedCoordinates = new Vector3((float) transformedVector[0][0], (float) transformedVector[1][0], (float) transformedVector[2][0]);
        return vectorTransformedCoordinates;

    }

}
