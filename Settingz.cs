using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarTrainer
{
    public class Settingz : BaseViewModel
    {

        public string InputFile { get; set; }
        public string OutputFile { get; set; }
        public float Period { get; set; }

        public int Progress { get; set; }
        
    }
}
