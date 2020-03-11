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
        private WaveFileWriter outWavFile;

        public string outPutFile;

        public MainWindow()
        {
            InitializeComponent();
            //this.DataContext = new MainWindowViewModel();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите входной файл";
            ofd.Filter = "Wave файлы (*.wav)|*.wav|MP3 файлы (*.mp3)|*.mp3";

            if (ofd.ShowDialog() == true)
            {
                inputTextBox.Text = ofd.FileName;
                
                if (outputDevice == null)
                {
                    outputDevice = new WaveOutEvent();
                    outputDevice.PlaybackStopped += OnPlayBackStopped;
                }

                if (audioFile == null)
                {
                    audioFile = new AudioFileReader(inputTextBox.Text);
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
                    string outwav = @"E:\states.wav";

                    float FS = 44100;

                    outWavFile = new WaveFileWriter(outwav, new WaveFormat((int)FS,16,2));
                    
                    using (BinaryWriter writer = new BinaryWriter(File.Open(outpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read)))
                    {
                        
                        float period = 10f; // ms
                        double fullPath = period;

                        int bb = 0;
                        int i = 0;
                        bool mode = false;

                        do
                        {

                            float[] samples = new float[2];
                            bb = wave.Read(samples, 0, samples.Length);
                            var leftVal = samples[0];
                            var rightVal = samples[1];

                            var curPath = Math.Round(i * 1000f / FS);

                            if (curPath >= fullPath)
                            {
                                mode = !mode;
                                fullPath = fullPath + period;
                            }

                            if (mode == true)
                            {
                                leftVal = 0;
                            } else
                            {
                                rightVal = 0;
                            }
                            
                            writer.Write(leftVal);
                            writer.Write(rightVal);

                            outWavFile.WriteSample(leftVal);
                            outWavFile.WriteSample(rightVal);

                            i++;

                        } while (bb > 0);

                        writer.Flush();
                        writer.Close();
                        outWavFile.Flush();
                        outWavFile.Close();

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
