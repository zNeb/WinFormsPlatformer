using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ProjetFinalePlatformer
{
    public partial class Menu : Form
    {
        public Menu()
        {
            this.MouseDown += new MouseEventHandler(bouge_fenetre); //Fait que quand tu clique sur la barre ca bouge le fenetre.
            InitializeComponent();
        }
        //Code pour activer le mouvement du programme
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int LPAR);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;
        int intForce = 23;
        int intJeuxCommence = 0;
        private void bouge_fenetre(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void Menu_Load(object sender, EventArgs e)

        {
            FormBorderStyle = FormBorderStyle.None;
        }

        private void picJouer_Click(object sender, EventArgs e)
        {
            intJeuxCommence = 1;
        }
        private void TmrJoueur_Tick(object sender, EventArgs e)
        {
            intForce -= 1;
            picJoueur.Top -= intForce;
            if (picJoueur.Top + picJoueur.Height >= this.ClientRectangle.Height - 159)
            {
                picJoueur.Top = this.ClientRectangle.Height - 159 - picJoueur.Height;
                    intForce = 23;
                if (intJeuxCommence == 1)
                {
                    intJeuxCommence = 2;
                }
            }
            if (intJeuxCommence == 2)
            {
                picJoueur.Left += 9;
                picJouer.Enabled = false;

            }
            if (picJoueur.Bounds.IntersectsWith(picTop.Bounds))
            {
                    intForce = 15;
                picJoueur.Top = picTop.Location.Y - picJoueur.Height;
                
            }
            if (picJoueur.Left >= this.ClientRectangle.Width + 50)
            {
                //Change le forme et detruit le précédent pour sauver la memoire (code dans programme.cs)
                Program.intNiveau = 1; // Met le prochain niveau
                this.Close(); //Ferme ce forme
            }
        }

        private void PicFerme_Click(object sender, EventArgs e)
        {
            Application.Exit(); //Ferme le jeux
        }
    }
}
