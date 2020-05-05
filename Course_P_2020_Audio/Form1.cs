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

namespace Course_P_2020_Audio
{

    public partial class Form1 : Form
    {
        string rfname = @"C:\r.mp3";
        WaveOutEvent outputDevice;
        AudioFileReader audioFile;
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
            open.Filter = "mp3 files (*.mp3)|*.mp3|wav files (*.wav)|*.wav";
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
                waveViewer1.WaveStream = new WaveFileReader(rfname);
            }
            else
            {
                конвертироватьВWAVToolStripMenuItem.Enabled = true;
            }
            питчшифтингToolStripMenuItem.Enabled = true;
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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            outputDevice?.Pause();
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
            using (var reader = new Mp3FileReader(rfname))
            {
                WaveFileWriter.CreateWaveFile(filename, reader);
            }
            MessageBox.Show("Файл сохранен!");
        }

        private void питчшифтингToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var inPath = rfname;
            var semitone = Math.Pow(2, 1.0 / 12);
            var upOneTone = semitone * semitone;
            var downOneTone = 1.0 / upOneTone;
            using (var reader = new MediaFoundationReader(inPath))
            {
                var pitch = new SmbPitchShiftingSampleProvider(reader.ToSampleProvider());
                using (var device = new WaveOutEvent())
                {
                    pitch.PitchFactor = (float)downOneTone; // or downOneTone
                                                          // just playing the first 10 seconds of the file
                    SaveFileDialog save = new SaveFileDialog();
                    save.InitialDirectory = "C:\\";
                    save.Filter = "wav files (*.wav)|*.wav";
                    save.FilterIndex = 1;
                    save.Title = "Сохранить файл";
                    if (save.ShowDialog() == DialogResult.Cancel)
                        return;
                    string filename = save.FileName;
                    WaveFileWriter.CreateWaveFile(filename, pitch.ToWaveProvider16());
                    MessageBox.Show("Файл сохранен!");
                }
            }
        }
    }
}
