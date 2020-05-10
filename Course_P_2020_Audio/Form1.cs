using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave.SampleProviders;
using System.Threading;
using System.Diagnostics;

namespace Course_P_2020_Audio
{

    public partial class Form1 : Form
    {
        int tm = 0;
        bool isplaying = false;
        string audiolngth;
        string rfname = @"C:\r.wav";
        WaveOutEvent outputDevice;
        AudioFileReader audioFile;
        TimeSpan duration = new TimeSpan();
        public Form1()
        {
            InitializeComponent();
        }

        private void обАвтореToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Олейник О.К., 535а", "About", MessageBoxButtons.OK);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = "C:\\";
            open.Filter = "wav files (*.wav)|*.wav|mp3 files (*.mp3)|*.mp3";
            open.FilterIndex = 1;
            open.Title = "Открыть файл";
            try
            {
                if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rfname = open.FileName;
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Не выбран файл!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            label1.Text = "Открыт файл: " + rfname;
            if (rfname.IndexOf(".mp3") == -1)
            {
                конвертироватьВWAVToolStripMenuItem.Enabled = false;
                обрезатьToolStripMenuItem.Enabled = true;
                объединитьToolStripMenuItem.Enabled = true;
                конвертацияВМоноToolStripMenuItem.Enabled = true;
                конвертацияВСтереоToolStripMenuItem.Enabled = true;
                ресэмплингToolStripMenuItem.Enabled = true;
                waveViewer1.WaveStream = new WaveFileReader(rfname);
                WaveFileReader reader = new WaveFileReader(rfname);
                duration = reader.TotalTime;

            }
            else
            {
                конвертироватьВWAVToolStripMenuItem.Enabled = true;
                обрезатьToolStripMenuItem.Enabled = false;
                объединитьToolStripMenuItem.Enabled = false;
                конвертацияВМоноToolStripMenuItem.Enabled = false;
                конвертацияВСтереоToolStripMenuItem.Enabled = false;
                ресэмплингToolStripMenuItem.Enabled = false;
                Mp3FileReader reader = new Mp3FileReader(rfname);
                duration = reader.TotalTime;
                
            }
            питчшифтингToolStripMenuItem.Enabled = true;
            audiolngth = " / "+Convert.ToString((int)duration.TotalSeconds)+"    Секунд проиграно";
            label2.Visible = true;
            label2.Text = "0" + audiolngth;
            label1.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(rfname);
                outputDevice.Init(audioFile);
            }
            isplaying = true;
            outputDevice.Play();
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            outputDevice.Dispose();
            outputDevice = null;
            audioFile.Dispose();
            audioFile = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            outputDevice?.Stop();
            isplaying = false;
            tm = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            outputDevice?.Pause();
            isplaying = false;
        }

        private void конвертироватьВWAVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.InitialDirectory = "C:\\";
            save.Filter = "wav files (*.wav)|*.wav";
            save.FilterIndex = 1;
            save.Title = "Сохранить файл";
            if (save.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = save.FileName;
            if (filename != null)
            {
                using (var reader = new Mp3FileReader(rfname))
                {
                    WaveFileWriter.CreateWaveFile(filename, reader);
                }
                MessageBox.Show("Файл сохранен!");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Не выбран путь сохранения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void питчшифтингToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pathsh fm = new Pathsh(rfname);
            fm.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isplaying)
            {
                tm++;
            }
            label2.Text = tm.ToString() + audiolngth;
        }

        private void обрезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Trim trimfm = new Trim(rfname, (int)duration.TotalSeconds);
            trimfm.ShowDialog();
        }

        private void объединитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mix fm = new Mix(rfname);
            fm.ShowDialog();
        }

        private void конвертацияВМоноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.InitialDirectory = "C:\\";
            save.Filter = "wav files (*.wav)|*.wav";
            save.FilterIndex = 1;
            save.Title = "Сохранить файл";
            if (save.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = save.FileName;
            if (filename != null)
            {
                using (var inputReader = new AudioFileReader(rfname))
                {
                    var mono = new StereoToMonoSampleProvider(inputReader);
                    WaveFileWriter.CreateWaveFile16(filename, mono);
                }
                MessageBox.Show("Файл сохранен!");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Не выбран путь сохранения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void конвертацияВСтереоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.InitialDirectory = "C:\\";
            save.Filter = "wav files (*.wav)|*.wav";
            save.FilterIndex = 1;
            save.Title = "Сохранить файл";
            if (save.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = save.FileName;
            if (filename != null)
            {
                using (var inputReader = new AudioFileReader(rfname))
                {
                    var stereo = new MonoToStereoSampleProvider(inputReader);
                    WaveFileWriter.CreateWaveFile16(filename, stereo);
                }
                MessageBox.Show("Файл сохранен!");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Не выбран путь сохранения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ресэмплингToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Resample fm = new Resample(rfname);
            fm.ShowDialog();
        }
    }
}
