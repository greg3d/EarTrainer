using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace EarTrainer
{
    public class TesterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var divider = 1f;
            var adder = 0f;

            var val = (float)value;

            // 0.01 0.1
            if (val >= 0.01 && val <= 0.1)
            {
                divider = 100f;
                adder = 0f;
            }
            // 0.15:0.05:1  
            if (val > 0.1 && val <= 1)
            {
                divider = 20f;
                adder = 8f;
            }

            //1.5   20
            if (val > 1 && val <= 20)
            {
                divider = 2f;
                adder = 26;
            }

            return Math.Round(val * divider + adder); 

            //return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var divider = 1f;
            var adder = 0f;

            var val = Math.Round((double)value);

            // 0.01 0.1
            if (val >= 1 && val <= 10)
            {
                divider = 100f;
                adder = 0f;
            }
            // 0.15:0.05:1  
            if (val > 10 && val <= 28)
            {
                divider = 20f;
                adder = 8f;
            }

            //1.5   20
            if (val > 28 && val <= 66)
            {
                divider = 2f;
                adder = 26;
            }

            //MessageBox.Show(value.ToString());
            return (float)Math.Round((val - adder) / divider , 2);
        }
    }

    [Magic]
    public class Settingz : BaseViewModel
    {
       

        public string InputFile { get; set; } = "Выберите файл...";
        public string OutputFile { get; set; }

        public float Period { get; set; }
        public bool StartEnabled { get; set; } = false;
        public bool ProcessEnabled { get; set; } = false;
        public bool IsProcessing { get; set; } = false;

    }
}
