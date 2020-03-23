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
using System.ComponentModel;
using System.Threading;

namespace EarTrainer
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        public Settingz settings;
        public string file;
        public IProgress<int> prgrs;
        public MainWindow()
        {
            settings = new Settingz();
            settings.Period = 10;

            this.DataContext = settings;
            InitializeComponent();

            settings.PropertyChanged += Settings_PropertyChanged;
            prgrs = new Progress<int>(s => pbar.Value = s);

            settings.StartEnabled = false;
            settings.ProcessEnabled = false;
            settings.IsProcessing = false;
            progBarMessage.Visibility = Visibility.Hidden;

            

            
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Period")
            {
                settings.OutputFile = file + "__" + settings.Period.ToString() + "ms.wav";

                if (settings.Period <= 0)
                {
                    settings.Period = 1;
                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {            
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите входной файл";
            ofd.Filter = "Wave файлы (*.wav)|*.wav|MP3 файлы (*.mp3)|*.mp3";

            if (ofd.ShowDialog() == true)
            {
                settings.InputFile = ofd.FileName;
                var name = settings.InputFile;

                if (name.EndsWith(".wav") || name.EndsWith(".mp3") || name.EndsWith(".WAV") || name.EndsWith(".MP3"))
                {
                    name = name.Substring(0, name.Length - 4);
                }

                
                file = name;

                settings.OutputFile = file + "__" + settings.Period.ToString() + "ms.wav";


                settings.StartEnabled = true;
                settings.ProcessEnabled = true;
                /*
                if (outputDevice == null)
                {
                    outputDevice = new WaveOutEvent();
                    outputDevice.PlaybackStopped += OnPlayBackStopped;
                }*/



            }
        }

        private void OnPlayBackStopped(object sender, StoppedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            settings.IsProcessing = true;
            progBarMessage.Visibility = Visibility.Visible;

            settings.StartEnabled = false;
            settings.ProcessEnabled = false;

            var calculate = Task.Factory.StartNew(() =>
            {
                prgrs.Report(0);

                audioFile = new AudioFileReader(settings.InputFile);
                audioFile.ToStereo();

                var n = audioFile.Length / (audioFile.WaveFormat.BitsPerSample / 8) / audioFile.WaveFormat.Channels;

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
                
                //string outpath = @"E:\states.dat";
                string outwav = settings.OutputFile;

                float FS = 44100;

                prgrs.Report(10);
                Thread.Sleep(200);

                using (WaveFileWriter outWavFile = new WaveFileWriter(outwav, new WaveFormat((int)FS, 16, 2)))
                {
                    
                    float period = settings.Period; // ms
                    double fullPath = period;

                    int bb = 0;
                    int i = 0;
                    bool mode = false;

                    do
                    {

                        if (i % 1000 == 0)
                        {
                            prgrs.Report((int) (i / (float)n * 100 * 0.9f + 10f));
                        }

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
                        }
                        else
                        {
                            rightVal = 0;
                        }

                        //writer.Write(leftVal);
                        //writer.Write(rightVal);

                        outWavFile.WriteSample(leftVal);
                        outWavFile.WriteSample(rightVal);

                        i++;
                        
                        

                    } while (bb > 0);


                    prgrs.Report(95);
                    //writer.Flush();
                    //writer.Close();
                    outWavFile.Flush();
                    prgrs.Report(100);

                    outWavFile.Close();

                }

                //outputDevice.Init(audioFile);
                

                

                

            });

            calculate.GetAwaiter().OnCompleted(() =>
            {
                settings.IsProcessing = false;
                progBarMessage.Visibility = Visibility.Hidden;

                settings.StartEnabled = true;
                settings.ProcessEnabled = true;
                MessageBox.Show("Готово!");

                prgrs.Report(0);
            });




            
        }

        private void minus1_Click(object sender, RoutedEventArgs e)
        {
            settings.Period -= 1;
        }

        private void plus1_Click(object sender, RoutedEventArgs e)
        {
            settings.Period += 1;
        }

        private void minus10_Click(object sender, RoutedEventArgs e)
        {
            settings.Period -= 10;

        }

        private void plus10_Click(object sender, RoutedEventArgs e)
        {
            settings.Period += 10;
        }

    }
}
