using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetFinalePlatformer
{
    static class Program
    {
        public static int intNiveau { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            intNiveau = 0;

            Application.Run(new Menu());

            if (intNiveau == 0)
            {
                Application.Run(new ModeleNiveau());
            }
            if (intNiveau == 1)
            {
                Application.Run(new Niveau1());
            }
            if (intNiveau == 2)
            {
                Application.Run(new Niveau2());
            }
            if (intNiveau == 3)
            {
                Application.Run(new Niveau3());
            }
            if (intNiveau == 4)
            {
                Application.Run(new Niveau4());
            }
            if (intNiveau == 5)
            {
               Application.Run(new Niveau5());
            }
            if (intNiveau == 6)
            {
                Application.Run(new Gagner());
            }
        }
    }
}
