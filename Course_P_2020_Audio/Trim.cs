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

namespace Course_P_2020_Audio
{
    public partial class Trim : Form
    {
        string rfn;//перехват пути к исходному аудиофайлу
        int totaltm;//общая длительность аудиозаписи
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rfname"></param>
        /// <param name="tm"></param>
        public Trim(string rfname, int tm)
        {
            InitializeComponent();
            rfn = rfname;
            totaltm = tm;
            numericUpDown1.Maximum = tm;
            numericUpDown2.Maximum = tm;
        }
        /// <summary>
        /// Кнопка обрезки аудиофайла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > numericUpDown2.Value)
                MessageBox.Show("Начальная позиция позже финальной!", "Ошибка", MessageBoxButtons.OK);
            else
            {
                if ((numericUpDown1.Value + numericUpDown2.Value) >= totaltm)
                    MessageBox.Show("Длина обрезок больше длины аудиозаписи!", "Ошибка", MessageBoxButtons.OK);
                else
                {
                    SaveFileDialog save = new SaveFileDialog();
                    save.InitialDirectory = "C:\\";
                    save.Filter = "wav files (*.wav)|*.wav";
                    save.FilterIndex = 1;
                    save.Title = "Сохранить файл";
                    if (save.ShowDialog() == DialogResult.Cancel)
                        return;
                    string filename = save.FileName;
                    TimeSpan cfs = new TimeSpan(0, 0, Convert.ToInt32(numericUpDown1.Value));
                    TimeSpan cfe = new TimeSpan(0, 0, Convert.ToInt32(numericUpDown2.Value));
                    WavFileUtils.TrimWavFile(rfn, filename, cfs, cfe);
                    MessageBox.Show("Файл сохранен!");
                    this.Close();
                }
            }
        }
    }
}
public static class WavFileUtils
{
    public static void TrimWavFile(string inPath, string outPath, TimeSpan cutFromStart, TimeSpan cutFromEnd)
    {
        using (WaveFileReader reader = new WaveFileReader(inPath))
        {
            using (WaveFileWriter writer = new WaveFileWriter(outPath, reader.WaveFormat))
            {
                int bytesPerMillisecond = reader.WaveFormat.AverageBytesPerSecond / 1000;

                int startPos = (int)cutFromStart.TotalMilliseconds * bytesPerMillisecond;
                startPos = startPos - startPos % reader.WaveFormat.BlockAlign;

                int endBytes = (int)cutFromEnd.TotalMilliseconds * bytesPerMillisecond;
                endBytes = endBytes - endBytes % reader.WaveFormat.BlockAlign;
                int endPos = (int)reader.Length - endBytes;

                TrimWavFile(reader, writer, startPos, endPos);
            }
        }
    }

    private static void TrimWavFile(WaveFileReader reader, WaveFileWriter writer, int startPos, int endPos)
    {
        reader.Position = startPos;
        byte[] buffer = new byte[1024];
        while (reader.Position < endPos)
        {
            int bytesRequired = (int)(endPos - reader.Position);
            if (bytesRequired > 0)
            {
                int bytesToRead = Math.Min(bytesRequired, buffer.Length);
                int bytesRead = reader.Read(buffer, 0, bytesToRead);
                if (bytesRead > 0)
                {
                    writer.WriteData(buffer, 0, bytesRead);
                }
            }
        }
    }
}