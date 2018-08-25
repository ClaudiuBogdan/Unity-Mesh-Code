using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    private Mesh meshObject;
    private Vector3[] vertices;
    private Vector3[] normals;
    private Vector2[] uv;

    private int[] tri;
    // Use this for initialization
    void Start ()
    {
        meshObject = GameObject.Find("Transformar").GetComponent<MeshFilter>().mesh;


        tri = new int[3];

        //  Lower left triangle.
        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;

        //  Upper right triangle.   
        //tri[3] = 2;
        //tri[4] = 3;
        //tri[5] = 1;

        meshObject.triangles = tri;


        int width = 10;
	    int height = 10;
		vertices = new Vector3[3];
	    vertices[0] = new Vector3(0, 0, 0);
	    vertices[1] = new Vector3(width, 0, 0);
	    vertices[2] = new Vector3(0, height, 0);
	    //vertices[3] = new Vector3(width, height, 0);
	    meshObject.vertices = vertices;


        normals = new Vector3[3];

	    normals[0] = -Vector3.forward;
	    normals[1] = -Vector3.forward;
	    normals[2] = -Vector3.forward;
	    //normals[3] = -Vector3.forward;

	    meshObject.normals = normals;

	    uv = new Vector2[3];

	    uv[0] = new Vector2(0, 0);
	    uv[1] = new Vector2(0.5f, 0);
	    uv[2] = new Vector2(0, 1);
	    //uv[3] = new Vector2(1, 1);

	    meshObject.uv = uv;

        GameObject.Find("Transformar").GetComponent<MeshFilter>().mesh = meshObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
