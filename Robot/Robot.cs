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

        public float[,] GetEndEffectorTransformationMatrixGraphicsConvention()
        {
            return MultiplyMatricesGraphics(
                GetJoint3TransformationMatrixGraphics(),
                MultiplyMatricesGraphics(
                    GetJoint2TransformationMatrixGraphics(),
                    MultiplyMatricesGraphics(
                        GetJoint1TransformationMatrixGraphics(),
                        GetBaseTransformationMatrixGraphics()
                    )
                )
            );
        }

        private float[,] GetBaseTransformationMatrixGraphics()
        {
            float tx = Position.X - StartPosition.X;
            float ty = Position.Y - StartPosition.Y;

            return new float[,]
            {
                { 1f, 0f, 0f },
                { 0f, 1f, 0f },
                { tx, ty, 1f }
            };
        }

        private float[,] GetJoint1TransformationMatrixGraphics()
        {
            float angleRad = ClawJoint1 * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(angleRad);
            float sin = (float)Math.Sin(angleRad);

            return new float[,]
            {
                { cos, sin, 0f },
                { -sin, cos, 0f },
                { 0f,   0f, 1f }
            };
        }

        private float[,] GetJoint2TransformationMatrixGraphics()
        {
            float angleRad = ClawJoint2 * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(angleRad);
            float sin = (float)Math.Sin(angleRad);

            return new float[,]
            {
                { cos, sin, 0f },
                { -sin, cos, 0f },
                { Segment1Length, 0f, 1f }
            };
        }

        private float[,] GetJoint3TransformationMatrixGraphics()
        {
            float angleRad = ClawJoint3 * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(angleRad);
            float sin = (float)Math.Sin(angleRad);

            return new float[,]
            {
                { cos, sin, 0f },
                { -sin, cos, 0f },
                { Segment2Length, 0f, 1f }
            };
        }

        // Matrix multiplication for standard convention
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

        // Matrix multiplication for graphics convention (reverse order)
        private float[,] MultiplyMatricesGraphics(float[,] a, float[,] b)
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
            float[,] gripperTransform = MultiplyMatrices(transform, GetGripperTransformationMatrix());

            return new PointF(gripperTransform[0, 2], gripperTransform[1, 2]);
        }

        public string GetTransformationMatrixInfo()
        {
            PointF endEffectorPos = CalculateEndEffectorPosition();
            float[,] matrixGraphics = GetEndEffectorTransformationMatrixGraphicsConvention();

            return string.Format(
                "End Effector Position:\nX: {0:F1}, Y: {1:F1}\n\n" +
                "Graphics Convention:\n" +
                "[ {11,7:F2} {12,7:F2} {13,7:F2} ]\n" +
                "[ {14,7:F2} {15,7:F2} {16,7:F2} ]\n" +
                "[ {17,7:F2} {18,7:F2} {19,7:F2} ]",
                endEffectorPos.X, endEffectorPos.Y,
                matrixGraphics[0, 0], matrixGraphics[0, 1], matrixGraphics[0, 2],
                matrixGraphics[1, 0], matrixGraphics[1, 1], matrixGraphics[1, 2],
                matrixGraphics[2, 0], matrixGraphics[2, 1], matrixGraphics[2, 2]
            );
        }

        private float[,] GetGripperTransformationMatrix()
        {
            return new float[,]
            {
                { 1f, 0f, Segment3Length },
                { 0f, 1f, 0f },
                { 0f, 0f, 1f }
            };
        }
    }
}
