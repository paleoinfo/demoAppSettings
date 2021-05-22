using Newtonsoft.Json;
using RandomPicLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace demoAppsettings
{
    public partial class Form1 : Form
    {
        private AppsettingsInfo appSettingsData;
        private string PathFolderImage;
        /// <summary>Initializes a new instance of the <see cref="Form1" /> class.</summary>
        public Form1()
        {
            InitializeComponent();

            //===============================================================
            var JsonString = string.Empty;

            try
            {
                JsonString = System.IO.File.ReadAllText("appsettings.json");
                appSettingsData = JsonConvert.DeserializeObject<AppsettingsInfo>(JsonString);
            }
            catch (Exception)
            {
                MessageBox.Show($"errore nel file di configurazione appsettings.json");
                System.Environment.Exit(-1);
            }

            var ImgFolderPathFromAppsettings = appSettingsData.ImgFolderPath;
            PathFolderImage = System.IO.Path.Combine(Application.StartupPath, ImgFolderPathFromAppsettings);

            try
            {
                if (!System.IO.Directory.Exists(PathFolderImage))
                    System.IO.Directory.CreateDirectory(PathFolderImage);
            }
            catch (Exception)
            {
                MessageBox.Show($"errore nel file di configurazione\nImgFolderPath: {ImgFolderPathFromAppsettings}");
                System.Environment.Exit(-1);
            }


            //===============================================================

            this.BackColor = Color.FromName(appSettingsData.BackGroundColor);

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var topics = appSettingsData.Topics?.Count() == 0 ? new List<string>() { "Butterfly" } : appSettingsData.Topics;

            Image rndImg = await FlickrPics.getRandomPic(topics);

            pictureBox1.Image = rndImg;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
                return;

            var fileName = Path.Combine(PathFolderImage, $"IMG_{DateTime.Now.ToString("yyyyMMddHHmmss")}.png");

            try
            {
                pictureBox1.Image.Save(fileName);
                MessageBox.Show($"immagine salvata in:\n{fileName}", "salva", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show($"Errore salvataggio immagine :\n{fileName}", "salva", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        public struct AppsettingsInfo
        {
            public string BackGroundColor;
            public string ImgFolderPath;
            public string LastDateTime;
            public List<string> Topics;
        }
    }
}
