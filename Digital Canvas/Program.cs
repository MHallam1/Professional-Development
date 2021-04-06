using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digital_Canvas
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Canvas());

            Console.WriteLine("Test");
            Console.WriteLine("meme meme");
            Console.WriteLine("meme meme");
            Console.WriteLine("meme 3444444444444444444");
        }
    }
}
