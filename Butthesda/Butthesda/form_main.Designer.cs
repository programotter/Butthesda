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
            this.button1 = new System.Windows.Forms.Button();
            this.listBox_devices = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_fallout4 = new System.Windows.Forms.RadioButton();
            this.button_skyrim_se = new System.Windows.Forms.RadioButton();
            this.button_skyrim = new System.Windows.Forms.RadioButton();
            this.textField_game_path = new System.Windows.Forms.TextBox();
            this.browse_game_path = new System.Windows.Forms.Button();
            this.button_start = new System.Windows.Forms.Button();
            this.button_default_path = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(391, 211);
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
            this.listBox_devices.Location = new System.Drawing.Point(140, 211);
            this.listBox_devices.Name = "listBox_devices";
            this.listBox_devices.Size = new System.Drawing.Size(245, 21);
            this.listBox_devices.TabIndex = 7;
            this.listBox_devices.ValueMember = "name";
            this.listBox_devices.SelectedIndexChanged += new System.EventHandler(this.ListBox_devices_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(140, 182);
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
            this.label1.Location = new System.Drawing.Point(30, 214);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Connected Devices:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_fallout4);
            this.groupBox1.Controls.Add(this.button_skyrim_se);
            this.groupBox1.Controls.Add(this.button_skyrim);
            this.groupBox1.Location = new System.Drawing.Point(140, 299);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Game";
            // 
            // button_fallout4
            // 
            this.button_fallout4.AutoSize = true;
            this.button_fallout4.Enabled = false;
            this.button_fallout4.Location = new System.Drawing.Point(7, 66);
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
            this.textField_game_path.Location = new System.Drawing.Point(140, 406);
            this.textField_game_path.Name = "textField_game_path";
            this.textField_game_path.Size = new System.Drawing.Size(410, 20);
            this.textField_game_path.TabIndex = 16;
            this.textField_game_path.TextChanged += new System.EventHandler(this.Game_path_TextChanged);
            // 
            // browse_game_path
            // 
            this.browse_game_path.Location = new System.Drawing.Point(556, 404);
            this.browse_game_path.Name = "browse_game_path";
            this.browse_game_path.Size = new System.Drawing.Size(75, 23);
            this.browse_game_path.TabIndex = 17;
            this.browse_game_path.Text = "Browse...";
            this.browse_game_path.UseVisualStyleBackColor = true;
            this.browse_game_path.Click += new System.EventHandler(this.Browse_game_path_Click);
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(715, 403);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 23);
            this.button_start.TabIndex = 18;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.Button_start_Click);
            // 
            // button_default_path
            // 
            this.button_default_path.Location = new System.Drawing.Point(347, 342);
            this.button_default_path.Name = "button_default_path";
            this.button_default_path.Size = new System.Drawing.Size(119, 23);
            this.button_default_path.TabIndex = 19;
            this.button_default_path.Text = "Auto find game path";
            this.button_default_path.UseVisualStyleBackColor = true;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_default_path);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.browse_game_path);
            this.Controls.Add(this.textField_game_path);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.listBox_devices);
            this.Controls.Add(this.button1);
            this.Name = "Form_Main";
            this.Text = "Form1";
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
    }
}

