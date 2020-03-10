using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio.Wave;
using System.IO;

namespace EarTrainer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();


            ofd.InitialDirectory = "c:\\";
            ofd.Filter = "Wave файлы (*.wav)|*.wav|MP3 файлы (*.mp3)|*.mp3";
            //ofd.FilterIndex = 2;
            //ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true)
            {
                var inPath = ofd.FileName;



                if (outputDevice == null)
                {
                    outputDevice = new WaveOutEvent();
                    outputDevice.PlaybackStopped += OnPlayBackStopped;
                }

                if (audioFile == null)
                {
                    audioFile = new AudioFileReader(inPath);
                    audioFile.ToStereo();

                    var outFormat = new WaveFormat(44100, 2);
                    ISampleProvider wave;

                    using (var resampler = new MediaFoundationResampler(audioFile, outFormat))
                    {
                        wave = resampler.ToSampleProvider();
                    }

                    //MessageBox.Show(audioFile.WaveFormat.Channels.ToString());

                    //audioFile.ToMono();
                    //audioFile.ToStereo();

                    var bits = wave.WaveFormat.BitsPerSample;
                    var Fs = wave.WaveFormat.SampleRate;
                    var channels = wave.WaveFormat.Channels;
                    

                    MessageBox.Show(bits.ToString());
                    MessageBox.Show(Fs.ToString());
                    MessageBox.Show(channels.ToString());

                    string outpath = @"E:\states.dat";

                    var streamOut = new MemoryStream();
                    using (BinaryWriter writer = new BinaryWriter(File.Open(outpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read)))
                    {
                        int FS = 44100;
                        float period = 5; // ms
                        float periodTicks = period / 1000 * 44100;
                        float fullPath = 0f;

                        int bb = 0;
                        do
                        {
                            float[] samples = new float[1];

                            bb = wave.Read(samples, 0, samples.Length);

                            var value = samples[0];
                            writer.Write(samples[0]);
                        } while (bb > 0);

                        /*
                        byte[] buf = new byte[4096];
                        int bytesRead = 0;

                        do
                        {
                            bytesRead = wave.Read(buf, 0, buf.Length);
                            writer.Write(buf, 0, bytesRead);
                        } while (bytesRead > 0);
                        */
                    }

                    //outputDevice.Init(audioFile);
                }



                //outputDevice.Play();

                MessageBox.Show("Done!");

            }
        }

        private void OnPlayBackStopped(object sender, StoppedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
