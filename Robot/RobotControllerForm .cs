namespace Robot
{
    public partial class RobotControllerForm : Form
    {
        private const float rightBoundary = 370f;
        private const float leftBoundary = 30f;

        private const float rightOffset = 10f;
        private const float leftOffset = 10f;

        private Robot robot;
        private Graphics graphics;
        private Bitmap canvas;

        public RobotControllerForm()
        {
            InitializeComponent();
            InitializeRobot();
            InitializeCanvas();
        }

        private void InitializeRobot() => robot = new Robot();

        private void InitializeCanvas()
        {
            canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            graphics = Graphics.FromImage(canvas);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pictureBox.Image = canvas;
        }

        private void DrawRobot()
        {
            graphics.Clear(Color.White);

            RobotArmPositions positions = robot.CalculateArmPositions();

            graphics.FillEllipse(Brushes.Black, robot.Position.X - 20, robot.Position.Y + 20, 40, 15);
            graphics.FillEllipse(Brushes.Black, robot.Position.X - 20, robot.Position.Y - 35, 40, 15);

            graphics.FillRectangle(Brushes.Gray, robot.Position.X - 30, robot.Position.Y - 30, 60, 60);

            graphics.FillRectangle(Brushes.DarkGray, positions.BasePosition.X - 10, positions.BasePosition.Y - 10, 20, 10);

            DrawArmSegment(graphics, positions.BasePosition, positions.Joint1Position, Brushes.SteelBlue, "J1");
            DrawArmSegment(graphics, positions.Joint1Position, positions.Joint2Position, Brushes.LightSteelBlue, "J2");
            DrawArmSegment(graphics, positions.Joint2Position, positions.Joint3Position, Brushes.Silver, "J3");

            DrawGripper(graphics, positions.EndEffectorPosition);

            pictureBox.Refresh();

            UpdateTransformationMatrixDisplay();
        }

        private void DrawArmSegment(Graphics g, PointF start, PointF end, Brush brush, string label)
        {
            using (Pen pen = new Pen(brush, 8))
                g.DrawLine(pen, start, end);

            g.FillEllipse(Brushes.Red, start.X - 5, start.Y - 5, 10, 10);

            g.DrawString(label, SystemFonts.DefaultFont, Brushes.Black, start.X + 8, start.Y - 15);
        }

        private void UpdateJointTrackBars()
        {
            trackBarJoint1.Value = (int)robot.ClawJoint1;
            trackBarJoint2.Value = (int)robot.ClawJoint2;
            trackBarJoint3.Value = (int)robot.ClawJoint3;

            lblJoint1.Text = $"Joint 1: {robot.ClawJoint1}°";
            lblJoint2.Text = $"Joint 2: {robot.ClawJoint2}°";
            lblJoint3.Text = $"Joint 3: {robot.ClawJoint3}°";
        }

        private void trackBarJoint1_Scroll(object sender, EventArgs e)
        {
            robot.ClawJoint1 = trackBarJoint1.Value;
            UpdateJointLabels();
            DrawRobot();
        }

        private void trackBarJoint2_Scroll(object sender, EventArgs e)
        {
            robot.ClawJoint2 = trackBarJoint2.Value;
            UpdateJointLabels();
            DrawRobot();
        }

        private void trackBarJoint3_Scroll(object sender, EventArgs e)
        {
            robot.ClawJoint3 = trackBarJoint3.Value;
            UpdateJointLabels();
            DrawRobot();
        }

        private void UpdateJointLabels()
        {
            lblJoint1.Text = $"Joint 1: {robot.ClawJoint1}°";
            lblJoint2.Text = $"Joint 2: {robot.ClawJoint2}°";
            lblJoint3.Text = $"Joint 3: {robot.ClawJoint3}°";
        }

        private void DrawGripper(Graphics g, PointF position)
        {
            PointF leftStart = new PointF(position.X - 5, position.Y);
            PointF leftEnd = new PointF(position.X - 20f, position.Y + 15);
            g.DrawLine(new Pen(Brushes.DarkGray, 4), leftStart, leftEnd);

            PointF rightStart = new PointF(position.X + 5, position.Y);
            PointF rightEnd = new PointF(position.X + 20f, position.Y + 15);
            g.DrawLine(new Pen(Brushes.DarkGray, 4), rightStart, rightEnd);
        }

        private void btnMoveLeft_Click(object sender, EventArgs e)
        {
            float newX = robot.Position.X - leftOffset;

            if (newX < leftBoundary)
                return;

            robot.Position = new PointF(newX, robot.Position.Y);
            DrawRobot();
        }

        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            float newX = robot.Position.X + rightOffset;

            if (newX > rightBoundary)
                return;

            robot.Position = new PointF(newX, robot.Position.Y);
            DrawRobot();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            robot.Reset();
            UpdateJointTrackBars();
            DrawRobot();
        }

        private void RobotControllerForm_Load(object sender, EventArgs e)
        {
            UpdateJointTrackBars();
            DrawRobot();
        }

        private void UpdateTransformationMatrixDisplay() 
            => lblTransformationMatrix.Text = robot.GetTransformationMatrixString();
    }
}
