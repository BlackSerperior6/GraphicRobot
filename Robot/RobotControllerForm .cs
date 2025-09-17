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

            // Draw wheels
            graphics.FillEllipse(Brushes.Black, robot.Position.X - 20, robot.Position.Y + 20, 40, 15);
            graphics.FillEllipse(Brushes.Black, robot.Position.X - 20, robot.Position.Y - 35, 40, 15);

            // Draw robot body
            graphics.FillRectangle(Brushes.Gray, robot.Position.X - 30, robot.Position.Y - 30, 60, 60);

            // Draw claw base
            PointF clawBase = new PointF(robot.Position.X, robot.Position.Y - 30);
            graphics.FillRectangle(Brushes.DarkGray, clawBase.X - 10, clawBase.Y - 10, 20, 10);

            // Draw claw segments
            DrawClawSegment(graphics, clawBase, robot.ClawJoint1, 30, Brushes.SteelBlue);

            PointF joint2Pos = CalculateJointPosition(clawBase, robot.ClawJoint1, 30);
            DrawClawSegment(graphics, joint2Pos, robot.ClawJoint2, 25, Brushes.LightSteelBlue);

            PointF joint3Pos = CalculateJointPosition(joint2Pos, robot.ClawJoint2, 25);
            DrawClawSegment(graphics, joint3Pos, robot.ClawJoint3, 20, Brushes.Silver);

            // Draw gripper
            PointF gripperPos = CalculateJointPosition(joint3Pos, robot.ClawJoint3, 20);
            DrawGripper(graphics, gripperPos, robot.GripperOpen);

            pictureBox.Refresh();
        }

        private void DrawClawSegment(Graphics g, PointF start, float angle, float length, Brush brush)
        {
            float endX = start.X + length * (float)Math.Sin(angle * Math.PI / 180);
            float endY = start.Y - length * (float)Math.Cos(angle * Math.PI / 180);

            PointF end = new PointF(endX, endY);

            using (Pen pen = new(brush, 8))
                g.DrawLine(pen, start, end);

            g.FillEllipse(Brushes.Red, start.X - 5, start.Y - 5, 10, 10);
        }

        private PointF CalculateJointPosition(PointF start, float angle, float length) => new(
                start.X + length * (float)Math.Sin(angle * Math.PI / 180),
                start.Y - length * (float)Math.Cos(angle * Math.PI / 180)
            );

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

        private void DrawGripper(Graphics g, PointF position, bool isOpen)
        {
            float gripperWidth = isOpen ? 20 : 5;

            // Draw left gripper
            PointF leftStart = new PointF(position.X - 5, position.Y);
            PointF leftEnd = new PointF(position.X - gripperWidth, position.Y + 15);
            g.DrawLine(new Pen(Brushes.DarkGray, 4), leftStart, leftEnd);

            // Draw right gripper
            PointF rightStart = new PointF(position.X + 5, position.Y);
            PointF rightEnd = new PointF(position.X + gripperWidth, position.Y + 15);
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

        private void btnGripperToggle_Click(object sender, EventArgs e)
        {
            robot.GripperOpen = !robot.GripperOpen;
            btnGripperToggle.Text = robot.GripperOpen ? "Close Gripper" : "Open Gripper";
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
    }
}
