namespace Butthesda
{
    partial class Form_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
			this.button1 = new System.Windows.Forms.Button();
			this.listBox_devices = new System.Windows.Forms.ComboBox();
			this.button3 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.button_skyrim_vr = new System.Windows.Forms.RadioButton();
			this.button_fallout4 = new System.Windows.Forms.RadioButton();
			this.button_skyrim_se = new System.Windows.Forms.RadioButton();
			this.button_skyrim = new System.Windows.Forms.RadioButton();
			this.textField_game_path = new System.Windows.Forms.TextBox();
			this.browse_game_path = new System.Windows.Forms.Button();
			this.button_start = new System.Windows.Forms.Button();
			this.button_default_path = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(383, 90);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 6;
			this.button1.Text = "Config";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1_Click);
			// 
			// listBox_devices
			// 
			this.listBox_devices.DisplayMember = "name";
			this.listBox_devices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.listBox_devices.FormattingEnabled = true;
			this.listBox_devices.Location = new System.Drawing.Point(132, 91);
			this.listBox_devices.Name = "listBox_devices";
			this.listBox_devices.Size = new System.Drawing.Size(245, 21);
			this.listBox_devices.TabIndex = 7;
			this.listBox_devices.ValueMember = "name";
			this.listBox_devices.SelectedIndexChanged += new System.EventHandler(this.ListBox_devices_SelectedIndexChanged);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(25, 33);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 10;
			this.button3.Text = "Find devices";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.Button3_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(22, 94);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 13);
			this.label1.TabIndex = 14;
			this.label1.Text = "Connected Devices:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.button_skyrim_vr);
			this.groupBox1.Controls.Add(this.button_fallout4);
			this.groupBox1.Controls.Add(this.button_skyrim_se);
			this.groupBox1.Controls.Add(this.button_skyrim);
			this.groupBox1.Location = new System.Drawing.Point(23, 150);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 117);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Game";
			// 
			// button_skyrim_vr
			// 
			this.button_skyrim_vr.AutoSize = true;
			this.button_skyrim_vr.Location = new System.Drawing.Point(6, 66);
			this.button_skyrim_vr.Name = "button_skyrim_vr";
			this.button_skyrim_vr.Size = new System.Drawing.Size(114, 17);
			this.button_skyrim_vr.TabIndex = 3;
			this.button_skyrim_vr.Text = "Skyrim VR (testing)";
			this.button_skyrim_vr.UseVisualStyleBackColor = true;
			this.button_skyrim_vr.CheckedChanged += new System.EventHandler(this.Game_CheckedChanged);
			// 
			// button_fallout4
			// 
			this.button_fallout4.AutoSize = true;
			this.button_fallout4.Enabled = false;
			this.button_fallout4.Location = new System.Drawing.Point(6, 89);
			this.button_fallout4.Name = "button_fallout4";
			this.button_fallout4.Size = new System.Drawing.Size(65, 17);
			this.button_fallout4.TabIndex = 2;
			this.button_fallout4.Text = "Fallout 4";
			this.button_fallout4.UseVisualStyleBackColor = true;
			this.button_fallout4.CheckedChanged += new System.EventHandler(this.Game_CheckedChanged);
			// 
			// button_skyrim_se
			// 
			this.button_skyrim_se.AutoSize = true;
			this.button_skyrim_se.Location = new System.Drawing.Point(7, 43);
			this.button_skyrim_se.Name = "button_skyrim_se";
			this.button_skyrim_se.Size = new System.Drawing.Size(73, 17);
			this.button_skyrim_se.TabIndex = 1;
			this.button_skyrim_se.Text = "Skyrim SE";
			this.button_skyrim_se.UseVisualStyleBackColor = true;
			this.button_skyrim_se.CheckedChanged += new System.EventHandler(this.Game_CheckedChanged);
			// 
			// button_skyrim
			// 
			this.button_skyrim.AutoSize = true;
			this.button_skyrim.Location = new System.Drawing.Point(7, 20);
			this.button_skyrim.Name = "button_skyrim";
			this.button_skyrim.Size = new System.Drawing.Size(56, 17);
			this.button_skyrim.TabIndex = 0;
			this.button_skyrim.Text = "Skyrim";
			this.button_skyrim.UseVisualStyleBackColor = true;
			this.button_skyrim.CheckedChanged += new System.EventHandler(this.Game_CheckedChanged);
			// 
			// textField_game_path
			// 
			this.textField_game_path.Location = new System.Drawing.Point(24, 305);
			this.textField_game_path.Name = "textField_game_path";
			this.textField_game_path.Size = new System.Drawing.Size(410, 20);
			this.textField_game_path.TabIndex = 16;
			this.textField_game_path.TextChanged += new System.EventHandler(this.Game_path_TextChanged);
			// 
			// browse_game_path
			// 
			this.browse_game_path.Location = new System.Drawing.Point(23, 331);
			this.browse_game_path.Name = "browse_game_path";
			this.browse_game_path.Size = new System.Drawing.Size(75, 23);
			this.browse_game_path.TabIndex = 17;
			this.browse_game_path.Text = "Browse...";
			this.browse_game_path.UseVisualStyleBackColor = true;
			this.browse_game_path.Click += new System.EventHandler(this.Browse_game_path_Click);
			// 
			// button_start
			// 
			this.button_start.Location = new System.Drawing.Point(23, 403);
			this.button_start.Name = "button_start";
			this.button_start.Size = new System.Drawing.Size(200, 23);
			this.button_start.TabIndex = 18;
			this.button_start.Text = "Start";
			this.button_start.UseVisualStyleBackColor = true;
			this.button_start.Click += new System.EventHandler(this.Button_start_Click);
			// 
			// button_default_path
			// 
			this.button_default_path.Location = new System.Drawing.Point(104, 331);
			this.button_default_path.Name = "button_default_path";
			this.button_default_path.Size = new System.Drawing.Size(119, 23);
			this.button_default_path.TabIndex = 19;
			this.button_default_path.Text = "Auto find game path";
			this.button_default_path.UseVisualStyleBackColor = true;
			this.button_default_path.Click += new System.EventHandler(this.Button_default_path_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(20, 270);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(399, 13);
			this.label2.TabIndex = 20;
			this.label2.Text = "4. Install the Butthesda mod using a mod mangers or by placing it in the /data fo" +
    "lder";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(20, 289);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(405, 13);
			this.label3.TabIndex = 21;
			this.label3.Text = "5. Select the mod folder \"mods/butthesda\" or \"game/data\" if you installed it manu" +
    "aly";
			this.label3.Click += new System.EventHandler(this.label3_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(20, 17);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(376, 13);
			this.label4.TabIndex = 22;
			this.label4.Text = "1. Click \"Find device\" to start searching for compatible controllers and sex toys" +
    "";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(20, 75);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(479, 13);
			this.label5.TabIndex = 23;
			this.label5.Text = "2. Select one toy at the time and use the \"Config\" button to setup when that devi" +
    "ce needs to vibrate";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(20, 134);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(178, 13);
			this.label6.TabIndex = 24;
			this.label6.Text = "3. Select what game you are playing";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(20, 381);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(304, 13);
			this.label7.TabIndex = 25;
			this.label7.Text = "6. Click start, and the program will wait for you to start the game";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(22, 450);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(149, 13);
			this.label8.TabIndex = 26;
			this.label8.Text = "7. Run the game you selected";
			this.label8.Click += new System.EventHandler(this.label8_Click);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(344, 457);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(178, 13);
			this.label9.TabIndex = 27;
			this.label9.Text = "By Mr. Private \"www.loverslab.com\"";
			// 
			// Form_Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(521, 472);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button_default_path);
			this.Controls.Add(this.button_start);
			this.Controls.Add(this.browse_game_path);
			this.Controls.Add(this.textField_game_path);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.listBox_devices);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form_Main";
			this.Text = "Butthesda (ButtplugIO integration for Bethesda games)";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Main_FormClosed);
			this.Load += new System.EventHandler(this.Form_main_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox listBox_devices;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton button_fallout4;
        private System.Windows.Forms.RadioButton button_skyrim_se;
        private System.Windows.Forms.RadioButton button_skyrim;
        private System.Windows.Forms.TextBox textField_game_path;
        private System.Windows.Forms.Button browse_game_path;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_default_path;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.RadioButton button_skyrim_vr;
	}
}

