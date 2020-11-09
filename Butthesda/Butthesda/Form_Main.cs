using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Buttplug.Client;
using Buttplug.Core;
using Buttplug.Client.Connectors.WebsocketConnector;
using Microsoft.Win32;
using System.IO;
using System.Threading;

namespace Butthesda
{

    public partial class Form_Main : Form
    {

        private ButtplugClient client;

        public Form_Main()
        {
			Start_Buttplug_Server();
			InitializeComponent();

            foreach(String game in Games.List())
            {
                if (Games.Running(game))
                {
                    if (game == Games.Skyrim.Executable_Name)
                    {
                        button_skyrim.Select();
                    }
                    else if (game == Games.SkyrimSe.Executable_Name)
                    {
                        button_skyrim_se.Select();
                    }
                    else
                    {
                        button_fallout4.Select();
                    }
                }
            }

        }

		static void OnDeviceAdded(object o, DeviceAddedEventArgs args)
		{
			Console.WriteLine($"Device ${args.Device.Name} connected");
		}

		private async void Start_Buttplug_Server()
		{
			var connector = new ButtplugEmbeddedConnector("Example Server");
			//var connector = new ButtplugWebsocketConnector(new Uri("ws://localhost:12345/buttplug"));
			client = new ButtplugClient("Example Client", connector);
			client.DeviceAdded += OnDeviceAdded;
			try
			{
				await client.ConnectAsync();
			}
			catch (ButtplugClientConnectorException ex)
			{
				Console.WriteLine(
					"Can't connect to Buttplug Server, exiting!" +
					$"Message: {ex.InnerException.Message}");
			}
			catch (ButtplugHandshakeException ex)
			{
				Console.WriteLine(
					"Handshake with Buttplug Server, exiting!" +
					$"Message: {ex.InnerException.Message}");
			}
			Console.WriteLine("Connected!");
		}


        private void Button3_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected){
                MessageBox.Show("Connect to Intiface first", "Intiface not connected!",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            

            var f = new Form_ScanDevices(client);
            f.ShowDialog();

            //Display new list of devices that was selected in the closed dialog
            listBox_devices.Items.Clear();
            foreach( Device device in Device.devices)
            {
                listBox_devices.Items.Add(device);
            }
			try {listBox_devices.SelectedIndex = 0;}catch{}
            
        }
		private void Label1_Click_2(object sender, EventArgs e)
		{

		}


		private void Form_main_Load(object sender, EventArgs e)
		{

		}

		private void ListBox_devices_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {

            if (listBox_devices.SelectedIndex < 0)
            {
                MessageBox.Show("Select a device you want to configure first", "Nothing selected", MessageBoxButtons.OK);
                return;
            }
            
            Device device = (Device)listBox_devices.SelectedItem;

			if (device == null)
            {
                MessageBox.Show("Select a device first", "No device selected!",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else{
                var f = new Form_Controller(device);
                f.ShowDialog();
            }
            
        }


        private void Button_default_path_Click(object sender, EventArgs e)
        {
            Set_GamePath();
        }

        private void Set_GamePath()
		{
			string keyName = "";

            if (Game_Name == Games.Skyrim.Executable_Name)
			{
                keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Bethesda Softworks\Skyrim";
                
            }else if (Game_Name == Games.SkyrimSe.Executable_Name)
			{
                keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Bethesda Softworks\Skyrim Special Edition";
            }
            else if (Game_Name == Games.Fallout4.Executable_Name)
            {
                keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Bethesda Softworks\Fallout4";
            }
			string path = (string)Registry.GetValue(keyName, "Installed Path", "");


			textField_game_path.Text = path;
        }

        private void Save_GamePath()
		{
            //save path
            if (Game_Name == Games.Skyrim.Executable_Name)
            {
                Properties.Settings.Default.GamePath_Skyrim = textField_game_path.Text;
            }
            else if (Game_Name == Games.SkyrimSe.Executable_Name)
            {
                Properties.Settings.Default.GamePath_SkyrimSe = textField_game_path.Text;
            }
            else if (Game_Name == Games.Fallout4.Executable_Name)
            {
                Properties.Settings.Default.GamePath_Fallout4 = textField_game_path.Text;
            }
            Properties.Settings.Default.Save();
        }

        private void Load_GamePath()
		{
            //load saved game_path
            if (Game_Name == Games.Skyrim.Executable_Name)
            {
                textField_game_path.Text = Properties.Settings.Default.GamePath_Skyrim;
            }
            else if (Game_Name == Games.SkyrimSe.Executable_Name)
            {
                textField_game_path.Text = Properties.Settings.Default.GamePath_SkyrimSe;
            }
            else if (Game_Name == Games.Fallout4.Executable_Name)
            {
                textField_game_path.Text = Properties.Settings.Default.GamePath_Fallout4;
            }

        }

        private bool waiting_for_game_start = false;
        private void Button_start_Click(object sender, EventArgs e)
        {
			if (waiting_for_game_start)
			{
                button_start.Text = "Start";
                waiting_for_game_start = false;

				if (Games.Running(Game_Name))
				{
                    bool requestRestart = true;
					while (requestRestart)
					{
                        var f = new Form_EventFileReader(Game_Name, textField_game_path.Text);
                        f.ShowDialog();
                        requestRestart = f.RequestRestart;
                    }

                }
                return;
			}

            if (String.IsNullOrEmpty(Game_Name))
            {
                MessageBox.Show("Select what game you want to play first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (!Directory.Exists(textField_game_path.Text))
            {
                MessageBox.Show("Selected folder doesn't exist", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!Directory.Exists(textField_game_path.Text + @"\FunScripts"))
            {
                MessageBox.Show("Selected folder is not the mod folder", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!File.Exists(textField_game_path.Text + @"\FunScripts\link.txt"))
            {
                MessageBox.Show("link file doesnt exist", "warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            waiting_for_game_start = true;
            button_start.Text = "Cancel \"waiting for game to start\"";

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                while (!Games.Running(Game_Name) && waiting_for_game_start)
                {
                    Thread.Sleep(100);
                }

				if (waiting_for_game_start)
				{
                    this.Invoke((MethodInvoker)delegate { Button_start_Click(sender, e); });
                }

            }).Start();

			if (!Games.Running(Game_Name))
			{
                File.WriteAllText(textField_game_path.Text + @"\FunScripts\link.txt", string.Empty);
            }

        }

        public static string game_path = "";
        private void Game_path_TextChanged(object sender, EventArgs e)
        {
            
            Save_GamePath();

        }

        private void Browse_game_path_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textField_game_path.Text = fbd.SelectedPath;
                }
            }
        }

        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.WriteLine("closingg");
            Application.Exit();
        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            Application.Exit();
        }
        public static string Game_Name { get; private set; }
        private void Game_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton.Checked)
            {

                

                //update game_name
                if (button_skyrim == radioButton)
                {
                    Game_Name = Games.Skyrim.Executable_Name;
                }else if (button_skyrim_se == radioButton)
                {
                    Game_Name = Games.SkyrimSe.Executable_Name;
                }
                else
                {
                    Game_Name = Games.Fallout4.Executable_Name;
                }

                Load_GamePath();

            }
        }


	}
}
