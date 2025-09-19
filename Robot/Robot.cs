namespace Robot
{
    internal class Robot
    {
        private const float staringX = 200f;
        private const float startingY = 260f;

        public PointF Position { get; set; }
        public PointF StartPosition { get; private set; }

        public float ClawJoint1 { get; set; }
        public float ClawJoint2 { get; set; }
        public float ClawJoint3 { get; set; }

        public bool GripperOpen { get; set; }

        public const float Segment1Length = 30f;
        public const float Segment2Length = 25f;
        public const float Segment3Length = 20f;

        public Robot() => Reset();

        public void Reset()
        {
            ClawJoint1 = 0f;
            ClawJoint2 = 0f;
            ClawJoint3 = 0f;
            GripperOpen = true;

            Position = new PointF(staringX, startingY);
            StartPosition = Position;
        }

        public float[,] GetEndEffectorTransformationMatrix()
        {
            // Calculate the cumulative transformation from base to end effector
            // This includes base translation and all joint rotations

            // Start with identity matrix
            float[,] transform = IdentityMatrix();

            // Apply base translation (robot movement)
            transform = MultiplyMatrices(transform, TranslationMatrix(Position.X - StartPosition.X, Position.Y - StartPosition.Y));

            // Apply joint rotations and translations in sequence
            transform = MultiplyMatrices(transform, RotationMatrix(ClawJoint1));
            transform = MultiplyMatrices(transform, TranslationMatrix(Segment1Length, 0));

            transform = MultiplyMatrices(transform, RotationMatrix(ClawJoint2));
            transform = MultiplyMatrices(transform, TranslationMatrix(Segment2Length, 0));

            transform = MultiplyMatrices(transform, RotationMatrix(ClawJoint3));
            transform = MultiplyMatrices(transform, TranslationMatrix(Segment3Length, 0));

            return transform;
        }

        private float[,] IdentityMatrix()
        {
            return new float[,]
            {
                { 1f, 0f, 0f },
                { 0f, 1f, 0f },
                { 0f, 0f, 1f }
            };
        }

        private float[,] TranslationMatrix(float tx, float ty)
        {
            return new float[,]
            {
                { 1f, 0f, tx },
                { 0f, 1f, ty },
                { 0f, 0f, 1f }
            };
        }

        private float[,] RotationMatrix(float angleDegrees)
        {
            float angleRad = angleDegrees * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(angleRad);
            float sin = (float)Math.Sin(angleRad);

            return new float[,]
            {
                { cos, -sin, 0f },
                { sin,  cos, 0f },
                { 0f,   0f,  1f }
            };
        }

        private float[,] MultiplyMatrices(float[,] a, float[,] b)
        {
            int rowsA = a.GetLength(0);
            int colsA = a.GetLength(1);
            int colsB = b.GetLength(1);

            float[,] result = new float[rowsA, colsB];

            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    float sum = 0f;
                    for (int k = 0; k < colsA; k++)
                    {
                        sum += a[i, k] * b[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return result;
        }

        public PointF CalculateEndEffectorPosition()
        {
            float[,] transform = GetEndEffectorTransformationMatrix();

            // The position is in the translation components (first two elements of third column)
            return new PointF(transform[0, 2], transform[1, 2]);
        }

        public string GetForwardKinematicsInfo()
        {
            PointF endEffectorPos = CalculateEndEffectorPosition();
            float[,] matrix = GetEndEffectorTransformationMatrix();

            return string.Format(
                "End Effector Position:\nX: {0:F1}, Y: {1:F1}\n\n" +
                "Transformation Matrix:\n" +
                "[ {2,7:F2} {3,7:F2} {4,7:F2} ]\n" +
                "[ {5,7:F2} {6,7:F2} {7,7:F2} ]\n" +
                "[ {8,7:F2} {9,7:F2} {10,7:F2} ]",
                endEffectorPos.X, endEffectorPos.Y,
                matrix[0, 0], matrix[0, 1], matrix[0, 2],
                matrix[1, 0], matrix[1, 1], matrix[1, 2],
                matrix[2, 0], matrix[2, 1], matrix[2, 2]
            );
        }

        // Helper method to get just the transformation matrix string
        public string GetTransformationMatrixString()
        {
            float[,] matrix = GetEndEffectorTransformationMatrix();

            return string.Format(
                "[ {0,7:F2} {1,7:F2} {2,7:F2} ]\n" +
                "[ {3,7:F2} {4,7:F2} {5,7:F2} ]\n" +
                "[ {6,7:F2} {7,7:F2} {8,7:F2} ]",
                matrix[0, 0], matrix[0, 1], matrix[0, 2],
                matrix[1, 0], matrix[1, 1], matrix[1, 2],
                matrix[2, 0], matrix[2, 1], matrix[2, 2]
            );
        }
    }
}
