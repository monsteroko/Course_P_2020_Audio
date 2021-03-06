﻿using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course_P_2020_Audio
{
    public partial class Pathsh : Form
    {
        string rfn;//перехват пути исходного файла
        /// <summary>
        /// Инициализация формы
        /// </summary>
        /// <param name="rfname"></param>
        public Pathsh(string rfname)
        {
            InitializeComponent();
            rfn = rfname;
            listBox1.SelectedIndex = 0;
        }
        /// <summary>
        /// Питч-шифтинг по данным из формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            var inPath = rfn;
            var semitone = Math.Pow(2, 1.0 / 12);
            var upOneTone = semitone * semitone;
            var downOneTone = 1.0 / upOneTone;
            using (var reader = new MediaFoundationReader(inPath))
            {
                var pitch = new SmbPitchShiftingSampleProvider(reader.ToSampleProvider());
                using (var device = new WaveOutEvent())
                {
                    if(listBox1.SelectedIndex==0)
                    {
                        pitch.PitchFactor = (float)upOneTone * trackBar1.Value * (float)0.25;
                    }
                    else
                    {
                        pitch.PitchFactor = (float)downOneTone * trackBar1.Value * (float)0.042;
                    }
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
                        WaveFileWriter.CreateWaveFile(filename, pitch.ToWaveProvider16());
                        MessageBox.Show("Файл сохранен!");
                        this.Close();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Не выбран путь сохранения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
