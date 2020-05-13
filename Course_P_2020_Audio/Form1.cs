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
        int tm = 0;//кол-во секунд проигрывания аудио
        bool isplaying = false;//проигрывается ли аудиозапись
        string audiolngth;//длительность аудиозапись в секундах
        string rfname = @"C:\r.wav";//путь к открываемому аудиофайлу
        WaveOutEvent outputDevice;//устройство воспроизведения по умолчанию
        AudioFileReader audioFile;//инициализация аудиоридера
        TimeSpan duration = new TimeSpan();//класс для перевода в секунды длительности аудио
        /// <summary>
        /// Инициализация формы
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Окно "Об авторе"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void обАвтореToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Олейник О.К., 535а", "About", MessageBoxButtons.OK);
        }
        /// <summary>
        /// Выход из программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// Пункт открытия аудиофайла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAud();
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
            if (rfname.IndexOf(".mp3") == -1)//действия при открытии wav файла
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
            else//дейтсивия при открытии mp3 файла
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
        /// <summary>
        /// Запуск проигрывания открытого аудио
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Стоп проигрывания аудио
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            StopAud();
        }
        /// <summary>
        /// Пауза проигрывания аудио
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            outputDevice?.Pause();
            isplaying = false;
        }
        /// <summary>
        /// Пункт конвертирования .mp3 в .wav
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void конвертироватьВWAVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAud();
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
        /// <summary>
        /// Пункт питчшифтинга (повышение/понижение тональности)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void питчшифтингToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAud();
            Pathsh fm = new Pathsh(rfname);
            fm.ShowDialog();
        }
        /// <summary>
        /// Таймер для отображения времени проигрывания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isplaying)
            {
                tm++;
            }
            label2.Text = tm.ToString() + audiolngth;
        }
        /// <summary>
        /// Пункт обрезки аудио
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void обрезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAud();
            Trim trimfm = new Trim(rfname, (int)duration.TotalSeconds);
            trimfm.ShowDialog();
        }
        /// <summary>
        /// Пункт обьединения каналов аудио
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void объединитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAud();
            Mix fm = new Mix(rfname);
            fm.ShowDialog();
        }
        /// <summary>
        /// Пункт конвертации в моно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void конвертацияВМоноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAud();
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
        /// <summary>
        /// Пункт конвертации в стерео
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void конвертацияВСтереоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAud();
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
        /// <summary>
        /// Пункт ресемплинга аудио
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ресэмплингToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopAud();
            Resample fm = new Resample(rfname);
            fm.ShowDialog();
        }

        void StopAud()
        {
            outputDevice?.Stop();
            isplaying = false;
            tm = 0;
        }
    }
}
