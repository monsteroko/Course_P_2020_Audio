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
    public partial class Mix : Form
    {
        string rfn;//перехват пути исходного аудиофайла
        /// <summary>
        /// Инициализация формы
        /// </summary>
        /// <param name="rf"></param>
        public Mix(string rf)
        {
            InitializeComponent();
            rfn = rf;
        }
        /// <summary>
        /// Кнопка, в которой открывается второй файл и сохраняется соединенный файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string rfname = @"C:\r.wav";
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = "C:\\";
            open.Filter = "wav files (*.wav)|*.wav";
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
                using (var reader1 = new AudioFileReader(rfn))
                using (var reader2 = new AudioFileReader(rfname))
                {
                    reader1.Volume = trackBar1.Value * 0.1f;
                    reader2.Volume = trackBar2.Value * 0.1f;
                    var mixer = new MixingSampleProvider(new[] { reader1, reader2 });
                    WaveFileWriter.CreateWaveFile16(filename, mixer);
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
