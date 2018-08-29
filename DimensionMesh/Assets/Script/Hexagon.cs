using UnityEngine;

namespace Assets.Script
{
    public class Hexagon
    {
        public Vector3 centerHexagon;
        public Vector3 normalVectorHexagon;
        public Vector3 rotationalVectorHexagon;
        private static readonly Vector3 referenceRotationalVector = new Vector3(1, 0, 0);


        public Hexagon(Vector3 centerHexagon, Vector3 normalVectorHexagon, float radioDimension, float initialAngle)
        {
            this.centerHexagon = centerHexagon;
            this.normalVectorHexagon = normalVectorHexagon.normalized;
            this.rotationalVectorHexagon = CalculateRotationalVectorHexagon(radioDimension, initialAngle);
        }

        private Vector3 CalculateRotationalVectorHexagon(float radioDimension, float initialAngle)
        {
            Vector3 rotationalBuilder = Vector3.Cross(normalVectorHexagon, referenceRotationalVector);
            rotationalBuilder = rotationalBuilder.normalized;
            rotationalBuilder = RotateVector(rotationalBuilder, initialAngle, normalVectorHexagon);
            rotationalBuilder = rotationalBuilder * radioDimension;
            return rotationalBuilder;
        }


        public static Hexagon NewInstance(Vector3 centerHexagon, Vector3 normalVectorHexagon, float radioDimension, float initialAngle)
        {
            return new Hexagon(centerHexagon, normalVectorHexagon, radioDimension, initialAngle);
        }

        public static Hexagon NewInstance(Hexagon referenceHexagon, float distanceFromRefCenter, Vector3 normalVectorHexagon, float radioDimension, float initialAngle)
        {
            Vector3 centerHexagon = CalculateCenterHexagon(referenceHexagon, distanceFromRefCenter, normalVectorHexagon);
            return new Hexagon(centerHexagon, normalVectorHexagon, radioDimension, initialAngle);
        }

        private static Vector3 CalculateCenterHexagon(Hexagon referenceHexagon, float distanceFromRefCenter, Vector3 normalVectorHexagon)
        {
            normalVectorHexagon = normalVectorHexagon.normalized;
            Vector3 centerHexagon = referenceHexagon.centerHexagon + normalVectorHexagon * distanceFromRefCenter;
            return centerHexagon;
        }

        /**
         * First vertices is index 0 and last vertices is index 5.
         */
        public Vector3 GetHexVerticesVector(int vertexOfHexagon)
        {
            return GetHexVerticesVector(this, vertexOfHexagon);
        }

        public static Vector3 GetHexVerticesVector(Hexagon mHexagon, int vertexOfHexagon)
        {
            Vector3 vertexVector = mHexagon.centerHexagon + (Quaternion.AngleAxis(-60 * vertexOfHexagon, mHexagon.normalVectorHexagon) *
                                                             mHexagon.rotationalVectorHexagon);
            return new Vector3(vertexVector.x, vertexVector.y, vertexVector.z);
        }

        public static Vector3 RotateVector(Vector3 vectorToRotate, float angles, Vector3 refVector)
        {
            return (Quaternion.AngleAxis(angles, refVector) *
                    vectorToRotate);
        }
    }
}
