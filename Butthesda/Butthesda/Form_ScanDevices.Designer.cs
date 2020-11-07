namespace Butthesda
{
    partial class Form_ScanDevices
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
            this.listBox_devices = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_done = new System.Windows.Forms.Button();
            this.button_scan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox_devices
            // 
            this.listBox_devices.CheckOnClick = true;
            this.listBox_devices.DisplayMember = "Name";
            this.listBox_devices.FormattingEnabled = true;
            this.listBox_devices.Location = new System.Drawing.Point(47, 72);
            this.listBox_devices.Name = "listBox_devices";
            this.listBox_devices.Size = new System.Drawing.Size(442, 199);
            this.listBox_devices.TabIndex = 0;
            this.listBox_devices.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ListBox_Devices_ItemSelect);
            this.listBox_devices.SelectedIndexChanged += new System.EventHandler(this.CheckedListBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select devices you want to use:";
            // 
            // button_done
            // 
            this.button_done.Location = new System.Drawing.Point(173, 295);
            this.button_done.Name = "button_done";
            this.button_done.Size = new System.Drawing.Size(75, 23);
            this.button_done.TabIndex = 2;
            this.button_done.Text = "Done";
            this.button_done.UseVisualStyleBackColor = true;
            this.button_done.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button_scan
            // 
            this.button_scan.Location = new System.Drawing.Point(47, 295);
            this.button_scan.Name = "button_scan";
            this.button_scan.Size = new System.Drawing.Size(75, 23);
            this.button_scan.TabIndex = 3;
            this.button_scan.Text = "Stop";
            this.button_scan.UseVisualStyleBackColor = true;
            this.button_scan.Click += new System.EventHandler(this.Button_Scan_Click);
            // 
            // Form_scanDevices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 330);
            this.Controls.Add(this.button_scan);
            this.Controls.Add(this.button_done);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox_devices);
            this.Name = "Form_scanDevices";
            this.Text = "From_scanDevices";
            this.Load += new System.EventHandler(this.Form_scanDevices_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox listBox_devices;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_done;
        private System.Windows.Forms.Button button_scan;
    }
}