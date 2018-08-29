using UnityEngine;

namespace Assets.Script
{
    public class Plane
    {
        public Vector3 i;
        public Vector3 j;
        public Vector3 k;
        public Vector3 firstVertexRight;
        public Vector3 firstVertexLeft;
        public Vector3 secondVertexRight;
        public Vector3 secondVertexLeft;

        public Plane(Vector3 firstVertexRight, Vector3 firstVertexLeft, Vector3 secondVertexRight,
            Vector3 secondVertexLeft)
        {
            this.firstVertexRight = firstVertexRight;
            this.firstVertexLeft = firstVertexLeft;
            this.secondVertexRight = secondVertexRight;
            this.secondVertexLeft = secondVertexLeft;

            this.i = (secondVertexLeft - firstVertexLeft);
            this.j = (firstVertexRight - firstVertexLeft);
            this.k = Vector3.Cross(i, j).normalized;

            Debug.Log("i,j: " + i + j);

        }

        public Vector3 CalculateCoordinateBase(Vector3 mVector)
        {
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
            Vector3 vectorTransformedCoordinates = new Vector3((float)transformedVector[0][0], (float)transformedVector[1][0], (float)transformedVector[2][0]);
            return vectorTransformedCoordinates;

        }

        public Vector3 CalculateOriginCoordinateBase(Vector3 mVector)
        {
            return CalculateCoordinateBase(mVector - firstVertexLeft);
        }

        public bool HasCrossedSuperiorSide(Vector3 mVector)
        {
            return CalculateOriginCoordinateBase(mVector).x > 1.0f;
        }

        public bool HasCrossedInferiorSide(Vector3 mVector)
        {
            return CalculateOriginCoordinateBase(mVector).x < 0.0f;
        }

        public bool HasCrossedLeftSide(Vector3 mVector)
        {
            return CalculateOriginCoordinateBase(mVector).y < 0.0f;
        }

        public bool HasCrossedRightSide(Vector3 mVector)
        {
            return CalculateOriginCoordinateBase(mVector).y > 1.0f;
        }
    }
}
