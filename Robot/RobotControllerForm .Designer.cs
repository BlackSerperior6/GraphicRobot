namespace Robot
{
    partial class RobotControllerForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TrackBar trackBarJoint1;
        private System.Windows.Forms.TrackBar trackBarJoint2;
        private System.Windows.Forms.TrackBar trackBarJoint3;
        private System.Windows.Forms.Label lblJoint1;
        private System.Windows.Forms.Label lblJoint2;
        private System.Windows.Forms.Label lblJoint3;
        private System.Windows.Forms.Button btnMoveLeft;
        private System.Windows.Forms.Button btnMoveRight;
        private System.Windows.Forms.Button btnGripperToggle;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblTransformationMatrix;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pictureBox = new PictureBox();
            trackBarJoint1 = new TrackBar();
            trackBarJoint2 = new TrackBar();
            trackBarJoint3 = new TrackBar();
            lblJoint1 = new Label();
            lblJoint2 = new Label();
            lblJoint3 = new Label();
            btnMoveLeft = new Button();
            btnMoveRight = new Button();
            btnReset = new Button();
            btnGripperToggle = new Button();
            lblTransformationMatrix = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarJoint1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarJoint2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarJoint3).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.BackColor = Color.White;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.Location = new Point(12, 12);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(400, 300);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            // 
            // trackBarJoint1
            // 
            trackBarJoint1.Location = new Point(430, 30);
            trackBarJoint1.Maximum = 180;
            trackBarJoint1.Minimum = -180;
            trackBarJoint1.Name = "trackBarJoint1";
            trackBarJoint1.Size = new Size(200, 45);
            trackBarJoint1.TabIndex = 1;
            trackBarJoint1.Scroll += trackBarJoint1_Scroll;
            // 
            // trackBarJoint2
            // 
            trackBarJoint2.Location = new Point(430, 80);
            trackBarJoint2.Maximum = 180;
            trackBarJoint2.Minimum = -180;
            trackBarJoint2.Name = "trackBarJoint2";
            trackBarJoint2.Size = new Size(200, 45);
            trackBarJoint2.TabIndex = 2;
            trackBarJoint2.Scroll += trackBarJoint2_Scroll;
            // 
            // trackBarJoint3
            // 
            trackBarJoint3.Location = new Point(430, 130);
            trackBarJoint3.Maximum = 180;
            trackBarJoint3.Minimum = -180;
            trackBarJoint3.Name = "trackBarJoint3";
            trackBarJoint3.Size = new Size(200, 45);
            trackBarJoint3.TabIndex = 3;
            trackBarJoint3.Scroll += trackBarJoint3_Scroll;
            // 
            // lblJoint1
            // 
            lblJoint1.AutoSize = true;
            lblJoint1.Location = new Point(430, 12);
            lblJoint1.Name = "lblJoint1";
            lblJoint1.Size = new Size(58, 15);
            lblJoint1.TabIndex = 4;
            lblJoint1.Text = "Joint 1: 0°";
            // 
            // lblJoint2
            // 
            lblJoint2.AutoSize = true;
            lblJoint2.Location = new Point(430, 65);
            lblJoint2.Name = "lblJoint2";
            lblJoint2.Size = new Size(58, 15);
            lblJoint2.TabIndex = 5;
            lblJoint2.Text = "Joint 2: 0°";
            // 
            // lblJoint3
            // 
            lblJoint3.AutoSize = true;
            lblJoint3.Location = new Point(430, 115);
            lblJoint3.Name = "lblJoint3";
            lblJoint3.Size = new Size(58, 15);
            lblJoint3.TabIndex = 6;
            lblJoint3.Text = "Joint 3: 0°";
            // 
            // btnMoveLeft
            // 
            btnMoveLeft.Location = new Point(454, 198);
            btnMoveLeft.Name = "btnMoveLeft";
            btnMoveLeft.Size = new Size(60, 30);
            btnMoveLeft.TabIndex = 7;
            btnMoveLeft.Text = "←";
            btnMoveLeft.UseVisualStyleBackColor = true;
            btnMoveLeft.Click += btnMoveLeft_Click;
            // 
            // btnMoveRight
            // 
            btnMoveRight.Location = new Point(551, 198);
            btnMoveRight.Name = "btnMoveRight";
            btnMoveRight.Size = new Size(60, 30);
            btnMoveRight.TabIndex = 8;
            btnMoveRight.Text = "→";
            btnMoveRight.UseVisualStyleBackColor = true;
            btnMoveRight.Click += btnMoveRight_Click;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(494, 234);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(90, 30);
            btnReset.TabIndex = 12;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnGripperToggle
            // 
            btnGripperToggle.Location = new Point(494, 270);
            btnGripperToggle.Name = "btnGripperToggle";
            btnGripperToggle.Size = new Size(100, 26);
            btnGripperToggle.TabIndex = 13;
            btnGripperToggle.Text = "Close Gripper";
            btnGripperToggle.UseVisualStyleBackColor = true;
            btnGripperToggle.Click += btnGripperToggle_Click;
            // 
            // lblTransformationMatrix
            // 
            lblTransformationMatrix.AutoSize = true;
            lblTransformationMatrix.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTransformationMatrix.Location = new Point(12, 315);
            lblTransformationMatrix.Name = "lblTransformationMatrix";
            lblTransformationMatrix.Size = new Size(168, 56);
            lblTransformationMatrix.TabIndex = 13;
            lblTransformationMatrix.Text = "Transformation Matrix:\n[  1.00   0.00   0.00 ]\n[  0.00   1.00   0.00 ]\n[  0.00   0.00   1.00 ]";
            // 
            // RobotControllerForm
            // 
            ClientSize = new Size(616, 385);
            Controls.Add(btnGripperToggle);
            Controls.Add(btnReset);
            Controls.Add(btnMoveRight);
            Controls.Add(btnMoveLeft);
            Controls.Add(lblJoint3);
            Controls.Add(lblJoint2);
            Controls.Add(lblJoint1);
            Controls.Add(trackBarJoint3);
            Controls.Add(trackBarJoint2);
            Controls.Add(trackBarJoint1);
            Controls.Add(pictureBox);
            Controls.Add(lblTransformationMatrix);
            Name = "RobotControllerForm";
            Text = "Robot Controller";
            Load += RobotControllerForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarJoint1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarJoint2).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarJoint3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}