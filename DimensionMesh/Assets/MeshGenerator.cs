using System.Collections;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    private Mesh meshObject;
    private int trianglesSideDimension = 6;
    private int verticesSideDimension = 4;
    private int normalsSideDimension = 4;
    private int uvSideDimension = 4;
    private int displayIndex = 0;

    // Use this for initialization
    void Start() {
        Vector3 centerHexagon = new Vector3(0, 0, 0);
        Vector3 normalVectorHexagon = new Vector3(0, 1, 0);
        Vector3 rotationalVectorHexagon = new Vector3(0, 0, 2);
        Vector3[] firstHexagon = generateHexInstance(centerHexagon, normalVectorHexagon, rotationalVectorHexagon);

        Vector3 centerHexagon2 = new Vector3(0, 10, 0);
        Vector3 normalVectorHexagon2 = new Vector3(0, 1, 0);
        Vector3 rotationalVectorHexagon2 = new Vector3(0, 0, 2);
        Vector3[] secondHexagon = generateHexInstance(centerHexagon2, normalVectorHexagon2, rotationalVectorHexagon2);

        GameObject.Find("Transformar").GetComponent<MeshFilter>().mesh = generateHexToken(firstHexagon, secondHexagon);
        //StartCoroutine(changeSide());
        getLastHex(generateHexToken(firstHexagon, secondHexagon));
    }
	
	// Update is called once per frame
	void Update ()
	{
	    
	}

    IEnumerator changeSide()
    {
        //GameObject.Find("Transformar").GetComponent<MeshFilter>().mesh = generateHexToken();
        displayIndex = displayIndex < 5 ? displayIndex + 1 : 0;
        yield return new WaitForSeconds(0.016f);
        //Debug.Log("Mesh changed: " + displayIndex);
        StartCoroutine(changeSide());

    }

    private Mesh generateHexToken(Vector3[] firstHexagon, Vector3[] secondHexagon)
    {
        int sidesPerHexagon = 6;
        Mesh[] generatedHexMeshes = new Mesh[sidesPerHexagon];
        for (int i = 0; i < sidesPerHexagon; i++)
        {
            Vector3[] sideTokenVertices = new Vector3[4];
            sideTokenVertices[0] = getHexVerticesVector(firstHexagon, i);
            sideTokenVertices[1] = getHexVerticesVector(firstHexagon, i < sidesPerHexagon  - 1 ? i + 1 : 0);
            sideTokenVertices[2] = getHexVerticesVector(secondHexagon, i < sidesPerHexagon - 1 ? i + 1 : 0);
            sideTokenVertices[3] = getHexVerticesVector(secondHexagon, i);

            generatedHexMeshes[i] = generateSideTokenMesh(sideTokenVertices);
        }

        return combineMeshes(generatedHexMeshes);
    }

    private Vector3[] generateHexInstance(Vector3 centerHexagon, Vector3 normalVectorHexagon, Vector3 rotationalVectorHexagon)
    {
        Vector3[] mHexagon = new Vector3[3];
        mHexagon[0] = centerHexagon;
        mHexagon[1] = normalVectorHexagon;
        mHexagon[2] = rotationalVectorHexagon;

        return mHexagon;
    }

    private Mesh combineMeshes(Mesh[] generatedHexMeshes)
    {
        Mesh finalMesh = new Mesh();

        ArrayList trianglesArray = new ArrayList();
        ArrayList verticesArray = new ArrayList();
        ArrayList normalsArray = new ArrayList();
        ArrayList uvArray = new ArrayList();

        for (int i = 0; i < generatedHexMeshes.Length; i++)
        {
            Debug.Log("Triangle length: " + generatedHexMeshes[i].triangles.Length);
            foreach (int triangle in generatedHexMeshes[i].triangles)
            {
                trianglesArray.Add(triangle + (i * 4));
            }

            foreach (Vector3 vertex in generatedHexMeshes[i].vertices)
            {
                verticesArray.Add(vertex);
            }

            foreach (Vector3 normal in generatedHexMeshes[i].normals)
            {
                normalsArray.Add(normal);
            }

            foreach (Vector2 uv in generatedHexMeshes[i].uv)
            {
                uvArray.Add(uv);
            }

        }
        finalMesh.vertices = verticesArray.ToArray(typeof(Vector3)) as Vector3[];
        finalMesh.triangles = trianglesArray.ToArray(typeof(int)) as int[];
        finalMesh.normals = normalsArray.ToArray(typeof(Vector3)) as Vector3[];
        finalMesh.uv = uvArray.ToArray(typeof(Vector2)) as Vector2[];

        return finalMesh;
    }
    
    private Mesh generateSideTokenMesh(Vector3[] sideTokenVertices)
    {
        Mesh sideMesh = new Mesh();


        Vector3[] vertices;
        Vector3[] normals;
        Vector2[] uv;
        int[] tri;

        tri = new int[trianglesSideDimension];

        //  Lower left triangle.
        tri[0] = 0;
        tri[1] = 1;
        tri[2] = 3;

        // Upper right triangle.   
        tri[3] = 2;
        tri[4] = 3;
        tri[5] = 1;

        vertices = new Vector3[verticesSideDimension];
        vertices[0] = new Vector3(sideTokenVertices[0].x, sideTokenVertices[0].y, sideTokenVertices[0].z);
        vertices[1] = new Vector3(sideTokenVertices[1].x, sideTokenVertices[1].y, sideTokenVertices[1].z);
        vertices[2] = new Vector3(sideTokenVertices[2].x, sideTokenVertices[2].y, sideTokenVertices[2].z);
        vertices[3] = new Vector3(sideTokenVertices[3].x, sideTokenVertices[3].y, sideTokenVertices[3].z);

        normals = new Vector3[normalsSideDimension];

        Vector3 sideNormalVector = Vector3.Cross(sideTokenVertices[0] - sideTokenVertices[1], sideTokenVertices[1] - sideTokenVertices[2]).normalized;
        normals[0] = sideNormalVector;
        normals[1] = sideNormalVector;
        normals[2] = sideNormalVector;
        normals[3] = sideNormalVector;


        uv = new Vector2[uvSideDimension];

        uv[0] = new Vector2(1, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 1);

        sideMesh.vertices = vertices;
        sideMesh.triangles = tri;
        sideMesh.normals = normals;
        sideMesh.uv = uv;
        return sideMesh;
    }

    private Vector3 getHexVerticesVector(Vector3[]mHexagon, int vertexOfHexagon)
    {
        Vector3 centerHexagon = mHexagon[0];
        Vector3 normalVectorHexagon = mHexagon[1];
        Vector3 rotationalVectorHexagon = mHexagon[2];
        Vector3 vertexVector = /*Quaternion.AngleAxis(-60 * vertexOfHexagon, Vector3.up)*/ /*(Quaternion.Euler(0, -60 * vertexOfHexagon, 0)*/
            centerHexagon +  (Quaternion.AngleAxis(-60  * vertexOfHexagon, normalVectorHexagon) *
                               rotationalVectorHexagon);
        return new Vector3(vertexVector.x, vertexVector.y, vertexVector.z);
    }

    private void deleteFirstHexMesh()
    {

    }

    private Vector3[] getLastHex(Mesh hexMesh)
    {
        //Obtains three vertex from the las hexagon to calculate de center and the direction.
        Vector3[] allHexVertices = hexMesh.vertices;
        int lastVerticesIndex = allHexVertices.Length - 1;
        Vector3 vertexOne = allHexVertices[lastVerticesIndex - 1];
        Vector3 vertexTwo = allHexVertices[lastVerticesIndex - 0];
        Vector3 vertexThree = allHexVertices[lastVerticesIndex - 4];

        Debug.Log("vertexOne: " + vertexOne);
        Debug.Log("vertexTwo: " + vertexTwo);
        Debug.Log("vertexThree: " + vertexThree);

        Vector3 centerHexagon2 = vertexOne - vertexTwo + vertexThree;
        Vector3 normalVectorHexagon2 = Vector3.Cross(vertexOne - vertexTwo, vertexTwo - vertexThree).normalized;
        Vector3 rotationalVectorHexagon2 = vertexTwo - vertexThree;

        Debug.Log("centerHexagon2: " + centerHexagon2.ToString());
        Debug.Log("normalVectorHexagon2: " + normalVectorHexagon2.ToString());
        Debug.Log("rotationalVectorHexagon2: " + rotationalVectorHexagon2.ToString());

        for (int i = 0; i < 12; i++)
        {
            Debug.Log("vertex index " + i + " equals: " + allHexVertices[lastVerticesIndex - i]);
        }
        return generateHexInstance(centerHexagon2, normalVectorHexagon2, rotationalVectorHexagon2);

    }
}
