using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarTrainer
{
    public class Settingz : BaseViewModel
    {

        public string InputFile { get; set; } = "Выберите файл...";
        public string OutputFile { get; set; }
        public float Period { get; set; }
        public int Progress { get; set; }
        public bool StartEnabled { get; set; } = false;
        public bool ProcessEnabled { get; set; } = false;
        public bool IsProcessing { get; set; } = false;
        

    }
}
