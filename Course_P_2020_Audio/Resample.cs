using NAudio.Wave;
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
    public partial class Resample : Form
    {
        int outRate = 16000;//стандартная частота дискретизации
        string rfn;//перехват пути к исходному файлу
        /// <summary>
        /// Инициализация формы
        /// </summary>
        /// <param name="rfname"></param>
        public Resample(string rfname)
        {
            InitializeComponent();
            rfn = rfname;
        }
        /// <summary>
        /// Кнопка изменения частоты дискретизации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
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
               switch(listBox1.SelectedIndex)
                {
                    case 0:
                        outRate = 16000;
                        break;
                    case 1:
                        outRate = 32000;
                        break;
                    case 2:
                        outRate = 44100;
                        break;
                    case 3:
                        outRate = 48000;
                        break;
                    case 4:
                        outRate = 96000;
                        break;
                    case 5:
                        outRate = 192000;
                        break;
                }
                var inFile =rfn;
                var outFile = filename;
                using (var reader = new AudioFileReader(inFile))
                {
                    var resampler = new WdlResamplingSampleProvider(reader, outRate);
                    WaveFileWriter.CreateWaveFile16(outFile, resampler);
                }
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
