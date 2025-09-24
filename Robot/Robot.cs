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

        public const float Segment1Length = 30f;
        public const float Segment2Length = 25f;
        public const float Segment3Length = 20f;

        public Robot() => Reset();

        //Сброс робота до стартовой позиции
        public void Reset()
        {
            ClawJoint1 = 0f;
            ClawJoint2 = 0f;
            ClawJoint3 = 0f;

            Position = new PointF(staringX, startingY);
            StartPosition = Position;
        }

        // Расчет комплексной матрицы клешни относительно робота
        public float[,] GetEndEffectorTransformationMatrix()
        {
            // Forward kinematics: cumulative transformations
            // Each joint affects all subsequent segments

            // Base to Joint1 transformation
            float[,] baseToJoint1 = MultiplyMatrices(
                RotationMatrix(ClawJoint1),
                TranslationMatrix(0, -Segment1Length)
            );

            // Joint1 to Joint2 transformation
            float[,] joint1ToJoint2 = MultiplyMatrices(
                RotationMatrix(ClawJoint2),
                TranslationMatrix(0, -Segment2Length)
            );

            // Joint2 to Joint3 transformation
            float[,] joint2ToJoint3 = MultiplyMatrices(
                RotationMatrix(ClawJoint3),
                TranslationMatrix(0, -Segment3Length)
            );

            // Combine all transformations: Base → Joint1 → Joint2 → Joint3 → EndEffector
            float[,] transform = MultiplyMatrices(baseToJoint1, joint1ToJoint2);
            transform = MultiplyMatrices(transform, joint2ToJoint3);

            return transform;
        }

        // Calculate positions of all joints for realistic drawing
        public RobotArmPositions CalculateArmPositions()
        {
            RobotArmPositions positions = new RobotArmPositions();

            // Base position (where the arm starts on the robot)
            positions.BasePosition = new PointF(Position.X, Position.Y - 30);

            // Joint1 position (relative to base)
            float angle1Rad = ClawJoint1 * (float)Math.PI / 180f;
            positions.Joint1Position = new PointF(
                positions.BasePosition.X + Segment1Length * (float)Math.Sin(angle1Rad),
                positions.BasePosition.Y - Segment1Length * (float)Math.Cos(angle1Rad)
            );

            // Joint2 position (relative to joint1, cumulative angles)
            float angle2Rad = (ClawJoint1 + ClawJoint2) * (float)Math.PI / 180f;
            positions.Joint2Position = new PointF(
                positions.Joint1Position.X + Segment2Length * (float)Math.Sin(angle2Rad),
                positions.Joint1Position.Y - Segment2Length * (float)Math.Cos(angle2Rad)
            );

            // Joint3 position (relative to joint2, cumulative angles)
            float angle3Rad = (ClawJoint1 + ClawJoint2 + ClawJoint3) * (float)Math.PI / 180f;
            positions.Joint3Position = new PointF(
                positions.Joint2Position.X + Segment3Length * (float)Math.Sin(angle3Rad),
                positions.Joint2Position.Y - Segment3Length * (float)Math.Cos(angle3Rad)
            );

            // End effector position (same as joint3 for gripper base)
            positions.EndEffectorPosition = positions.Joint3Position;

            return positions;
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

    public class RobotArmPositions
    {
        public PointF BasePosition { get; set; }
        public PointF Joint1Position { get; set; }
        public PointF Joint2Position { get; set; }
        public PointF Joint3Position { get; set; }
        public PointF EndEffectorPosition { get; set; }
    }
}
