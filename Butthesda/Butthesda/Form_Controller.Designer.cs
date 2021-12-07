namespace Butthesda
{
    partial class Form_Controller
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
			this.label13 = new System.Windows.Forms.Label();
			this.button_save = new System.Windows.Forms.Button();
			this.button_cancel = new System.Windows.Forms.Button();
			this.label_min = new System.Windows.Forms.Label();
			this.label_max = new System.Windows.Forms.Label();
			this.numericUpDown_min = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown_max = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_min)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_max)).BeginInit();
			this.SuspendLayout();
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(12, 9);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(241, 13);
			this.label13.TabIndex = 46;
			this.label13.Text = "Select effects that this controller needs to react to";
			// 
			// button_save
			// 
			this.button_save.Location = new System.Drawing.Point(505, 539);
			this.button_save.Name = "button_save";
			this.button_save.Size = new System.Drawing.Size(75, 23);
			this.button_save.TabIndex = 47;
			this.button_save.Text = "Save";
			this.button_save.UseVisualStyleBackColor = true;
			this.button_save.Click += new System.EventHandler(this.Button1_Click);
			// 
			// button_cancel
			// 
			this.button_cancel.Location = new System.Drawing.Point(424, 539);
			this.button_cancel.Name = "button_cancel";
			this.button_cancel.Size = new System.Drawing.Size(75, 23);
			this.button_cancel.TabIndex = 48;
			this.button_cancel.Text = "Cancel";
			this.button_cancel.UseVisualStyleBackColor = true;
			this.button_cancel.Click += new System.EventHandler(this.Button_cancel_Click);
			// 
			// label_min
			// 
			this.label_min.AutoSize = true;
			this.label_min.Location = new System.Drawing.Point(27, 383);
			this.label_min.Name = "label_min";
			this.label_min.Size = new System.Drawing.Size(23, 13);
			this.label_min.TabIndex = 50;
			this.label_min.Text = "min";
			this.label_min.Click += new System.EventHandler(this.label1_Click);
			// 
			// label_max
			// 
			this.label_max.AutoSize = true;
			this.label_max.Location = new System.Drawing.Point(27, 409);
			this.label_max.Name = "label_max";
			this.label_max.Size = new System.Drawing.Size(26, 13);
			this.label_max.TabIndex = 51;
			this.label_max.Text = "max";
			// 
			// numericUpDown_min
			// 
			this.numericUpDown_min.Location = new System.Drawing.Point(59, 383);
			this.numericUpDown_min.Name = "numericUpDown_min";
			this.numericUpDown_min.Size = new System.Drawing.Size(56, 20);
			this.numericUpDown_min.TabIndex = 53;
			// 
			// numericUpDown_max
			// 
			this.numericUpDown_max.Location = new System.Drawing.Point(59, 409);
			this.numericUpDown_max.Name = "numericUpDown_max";
			this.numericUpDown_max.Size = new System.Drawing.Size(56, 20);
			this.numericUpDown_max.TabIndex = 54;
			this.numericUpDown_max.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numericUpDown_max.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
			// 
			// Form_Controller
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(592, 574);
			this.Controls.Add(this.numericUpDown_max);
			this.Controls.Add(this.numericUpDown_min);
			this.Controls.Add(this.label_max);
			this.Controls.Add(this.label_min);
			this.Controls.Add(this.button_cancel);
			this.Controls.Add(this.button_save);
			this.Controls.Add(this.label13);
			this.Name = "Form_Controller";
			this.Text = "Controller settings";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_min)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_max)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_cancel;
		private System.Windows.Forms.Label label_min;
		private System.Windows.Forms.Label label_max;
		private System.Windows.Forms.NumericUpDown numericUpDown_min;
		private System.Windows.Forms.NumericUpDown numericUpDown_max;
	}
}