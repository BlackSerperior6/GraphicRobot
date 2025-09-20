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

        //Сброс робота до стартовой позиции
        public void Reset()
        {
            ClawJoint1 = 0f;
            ClawJoint2 = 0f;
            ClawJoint3 = 0f;
            GripperOpen = true;

            Position = new PointF(staringX, startingY);
            StartPosition = Position;
        }

        // Расчет комплексной матрицы клешни относительно робота
        public float[,] CalculateGrippersTransformationMatrix()
        {
            float[,] transform = TranslationMatrix(Position.X - StartPosition.X,
                Position.Y - StartPosition.Y);

            transform = MultiplyMatrices(transform, RotationMatrix(ClawJoint1));
            transform = MultiplyMatrices(transform, TranslationMatrix(0, -Segment1Length));

            transform = MultiplyMatrices(transform, RotationMatrix(ClawJoint2));
            transform = MultiplyMatrices(transform, TranslationMatrix(0, -Segment2Length));

            transform = MultiplyMatrices(transform, RotationMatrix(ClawJoint3));
            transform = MultiplyMatrices(transform, TranslationMatrix(0, -Segment3Length));

            return transform;
        }

        //Метод для формирования матрицы перемещения
        private float[,] TranslationMatrix(float tx, float ty)
        {
            return new float[,]
            {
                { 1f, 0f, 0 },
                { 0f, 1f, 0 },
                { tx, ty, 1f }
            };
        }

        //Метод для формирования матрицы вращения
        private float[,] RotationMatrix(float angleDegrees)
        {
            float angleRad = angleDegrees * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(angleRad);
            float sin = (float)Math.Sin(angleRad);

            return new float[,]
            {
                { cos, sin, 0f },
                { -sin, cos, 0f },
                { 0f,   0f,  1f }
            };
        }

        //Метод для умножения матриц
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
                        sum += a[i, k] * b[k, j];

                    result[i, j] = sum;
                }
            }

            return result;
        }

        //Метод для перевода матрицы в текстовый формат
        public string GetMatrixString()
        {
            float[,] matrix = CalculateGrippersTransformationMatrix();

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
