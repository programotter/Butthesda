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
    public partial class Form_ScanDevices : Form
    {

        private readonly ButtplugClient client;
        public Form_ScanDevices(ButtplugClient client)
        {
            this.client = client;

            InitializeComponent();
  
            //create handler that gets triggered when we do scan later
            void HandleDeviceAdded(object aObj, DeviceAddedEventArgs aArgs)
            {
                listbox_update(true, aArgs.Device);
            };

            client.DeviceAdded += HandleDeviceAdded;

            void HandleDeviceRemoved(object aObj, DeviceRemovedEventArgs aArgs)
            {
                listbox_update(false, aArgs.Device);
            };

            client.DeviceRemoved += HandleDeviceRemoved;

            void HandleStopScanning(object aObj, EventArgs aEvent)
            {
                stopping_scan = false;
                scanning = false;
                Console.WriteLine("finished scanning");
            };
            client.ScanningFinished += HandleStopScanning;

            foreach (ButtplugClientDevice buttplugClientDevice in client.Devices)
            {
                listbox_update(true, buttplugClientDevice);
            }
            

            scanning = true;
            stopping_scan = false;
            client.StartScanningAsync();


        }

        private delegate void SafeCallDelegate(bool add, ButtplugClientDevice buttplugClientDevice);

        private void listbox_update(bool add, ButtplugClientDevice buttplugClientDevice)
        {
            if (this.listBox_devices.InvokeRequired)
            {
                var d = new SafeCallDelegate(listbox_update);
                this.Invoke(d, new object[] { add, buttplugClientDevice });
            }
            else
            {
                if (add)
                {
                    Device device = new Device(buttplugClientDevice.Name, client, buttplugClientDevice);
                    this.listBox_devices.Items.Add(device);
                    Console.WriteLine(device.name);
                }
                else
                {
                    foreach(Device device in listBox_devices.Items){
                        if (device.device == buttplugClientDevice)
                        {
                            this.listBox_devices.Items.Add(device);
                        }
                    }
                }
                
            }
        }

        private void CheckedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Device.devices.Clear();
            for (int i = 0; i < listBox_devices.Items.Count; i++) {
                if (listBox_devices.GetItemChecked(i))
                {
                    Device.devices.Add((Device)listBox_devices.Items[i]);
                }
            }

            this.Close();
        }

        private void Form_scanDevices_Load(object sender, EventArgs e)
        {

        }

        private bool scanning;
        private bool stopping_scan;
        private void Button_Scan_Click(object sender, EventArgs e)
        {
            if (scanning && !stopping_scan)
            {
                stopping_scan = true;
                client.StopScanningAsync();
                
            }

            if (!scanning && !stopping_scan)
            {
                client.StartScanningAsync();
                scanning = true;
            };
           
        }

        private void ListBox_Devices_ItemSelect(object sender, ItemCheckEventArgs e)
        {
            
            Console.WriteLine();
            Device device = (Device)listBox_devices.Items[e.Index];

            if (e.NewValue == CheckState.Checked) {
                
                device.active = true;
            } else
            {
                device.active = false;
            }
        }
    }
}
