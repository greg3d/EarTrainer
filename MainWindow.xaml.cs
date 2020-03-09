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

                    audioFile.ToMono();
                    audioFile.ToStereo();

                    MessageBox.Show(audioFile.WaveFormat.SampleRate.ToString());


                    outputDevice.Init(audioFile);
                }
                


                outputDevice.Play();

                

            }
        }

        private void OnPlayBackStopped(object sender, StoppedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
