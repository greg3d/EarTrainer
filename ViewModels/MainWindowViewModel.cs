using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarTrainer
{
    class MainWindowViewModel : BaseViewModel
    {

        private string inputFile;
        private string outputFile;

        public string InputFile
        {
            get { return inputFile; }
            set
            {
                inputFile = value;
                OnPropertyChanged("InputFile");
            }
        }

        public string OutputFile
        {
            get { return outputFile; }
            set
            {
                outputFile = value;
                OnPropertyChanged("OutputFile");
            }
        }

    }
}
