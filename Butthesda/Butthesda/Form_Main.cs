using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Threading;
using Buttplug;

namespace Butthesda
{

    public partial class Form_Main : Form
    {

        private ButtplugClient client;

        public Form_Main()
        {
            ConnectEmbedded();
            InitializeComponent();

            foreach(Game game in Game.List())
            {
                if (game.Running())
                {
                    if (game == Game.Skyrim)
                    {
                        button_skyrim.Select();
                    }
                    else if (game == Game.SkyrimSe)
                    {
                        button_skyrim_se.Select();
                    }
                    else if (game == Game.SkyrimVr)
                    {
                        button_skyrim_vr.Select();
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

        private void ConnectEmbedded()
        {

            
            // First off, we'll set up our Embedded Connector.
            var connector = new ButtplugEmbeddedConnectorOptions();

            // If we want to change anything after making the options object,
            // we can just access the members. We'll explain more about this
            // in a later chapter.
            connector.ServerName = "New Server Name";

            client = new ButtplugClient("Example Client");

            // Connecting using an embedded connection should never fail.
            client.ConnectAsync(connector);
        }


        private void Button3_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(client.);
            if (client == null){
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
            textField_game_path.Text = game.Install_Dir();
        }


        private void Save_GamePath()
		{
            //save path
            if (game == Game.Skyrim)
            {
                Properties.Settings.Default.GamePath_Skyrim = textField_game_path.Text;
            }
            else if (game == Game.SkyrimSe)
            {
                Properties.Settings.Default.GamePath_SkyrimSe = textField_game_path.Text;
            }
            else if (game == Game.SkyrimVr)
            {
                Properties.Settings.Default.GamePath_SkyrimVr = textField_game_path.Text;
            }
            else if (game == Game.Fallout4)
            {
                Properties.Settings.Default.GamePath_Fallout4 = textField_game_path.Text;
            }
            Properties.Settings.Default.Save();
        }

        private void Load_GamePath()
		{
            //load saved game_path
            if (game == Game.Skyrim)
            {
                textField_game_path.Text = Properties.Settings.Default.GamePath_Skyrim;
            }
            else if (game == Game.SkyrimSe)
            {
                textField_game_path.Text = Properties.Settings.Default.GamePath_SkyrimSe;
            }
            else if (game == Game.SkyrimVr)
            {
                textField_game_path.Text = Properties.Settings.Default.GamePath_SkyrimVr;
            }
            else if (game == Game.Fallout4)
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

				if (game.Running())
				{

					if (game.IsGameRunningAsAdmin() && !Program.IsRunningAsAdmin())
					{
                        MessageBox.Show("The game is running as admin so this program should be as well", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Program.RestartAsAdmin();
                        return;
                    }

                    var f = new Form_EventFileReader(game.Executable_Name, textField_game_path.Text);
                    f.Init();
                    f.ShowDialog();
                }
                return;
			}

            if (game == null)
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
                Thread.CurrentThread.Name = "Wait for game to start";
                /* run your code here */
                while (!game.Running() && waiting_for_game_start)
                {
                    Thread.Sleep(100);
                }

				if (waiting_for_game_start)
				{
                    this.Invoke((MethodInvoker)delegate { Button_start_Click(sender, e); });
                }

            }).Start();

			if (!game.Running())
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
        public static Game game { get; private set; }
        private void Game_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton.Checked)
            {

                

                //update game_name
                if (button_skyrim == radioButton)
                {
                    game = Game.Skyrim;
                }else if (button_skyrim_se == radioButton)
                {
                    game = Game.SkyrimSe;
                }
                else if (button_skyrim_vr == radioButton)
                {
                    game = Game.SkyrimVr;
                }
                else
                {
                    game = Game.Fallout4;
                }

                Load_GamePath();

            }
        }



        private void label3_Click(object sender, EventArgs e)
		{

		}

		private void label8_Click(object sender, EventArgs e)
		{

		}
	}
}
