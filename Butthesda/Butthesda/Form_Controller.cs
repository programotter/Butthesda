﻿using Buttplug.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Butthesda
{



    public partial class Form_Controller : Form
    {
		private readonly int size_x = 100;
		private readonly int size_y = 20;
        private readonly int offset_x = 100;
        private readonly int offset_y = 20;
        private readonly int offset_corner = 5;

        private Device device;
        private List<CheckBox_custom> checkBox_list = new List<CheckBox_custom>();

        public Form_Controller(Device device)
        {
            this.device = device;

            InitializeComponent();

            this.Text = "Controller settings (" + device.name+ ")";

            int y = offset_corner + 30;

            Point label_loc_eventType = new Point(offset_x+ offset_corner, y);
            foreach (string eventType in Enum.GetNames(typeof(Device.EventType)))
            {
                Label label = new Label();
                label.Text = eventType;
                label.Location = label_loc_eventType;
                label.Size = new Size(size_x, size_y);
                this.Controls.Add(label);

                label_loc_eventType.Offset(offset_x, 0);

            }

            //create checkboxs
            string[] bodyPartNames = Enum.GetNames(typeof(Device.BodyPart));
            Device.BodyPart[] bodyPartIds = (Device.BodyPart[])Enum.GetValues(typeof(Device.BodyPart));

            for (int i = 0; i < bodyPartNames.Length; i++)
            {
                String bodyPart = bodyPartNames[i];
                Device.BodyPart bodyPartId = bodyPartIds[i];

                y += offset_y;
                Point location = new Point(offset_corner, y);
                Label label = new Label();
                label.Text = bodyPart;
                label.Location = location;
                label.Size = new Size(size_x, size_y);
                this.Controls.Add(label);

                Device.EventType[] eventTypeIds = (Device.EventType[])Enum.GetValues(typeof(Device.EventType));

                for (int j = 0; j < eventTypeIds.Length; j++)
                {
                    Device.EventType eventTypeId = eventTypeIds[j];
					

					location.Offset(offset_x, 0);
                    CheckBox cb = new CheckBox();
                    cb.Location = location;
                    cb.Size = new Size(size_x, size_y);
					cb.Checked = device.HasType(bodyPartId, eventTypeId);
					this.Controls.Add(cb);

                    checkBox_list.Add(new CheckBox_custom(cb, bodyPartId, eventTypeId));

                }

            }

        }


        private void Button1_Click(object sender, EventArgs e)
        {
            foreach(CheckBox_custom checkbox in checkBox_list)
            {
                device.SetType(checkbox.bodyPart, checkbox.eventType, checkbox.checkBox.Checked);
            }
            this.Close();
        }

        private void Button_cancel_Click(object sender, EventArgs e)
        {
			this.Close();
        }

    }
}
