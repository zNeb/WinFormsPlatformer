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
    public partial class Gagner : Form
    {
        public Gagner()
        {
            this.MouseDown += new MouseEventHandler(bouge_fenetre); //Fait que quand tu clique sur la barre ca bouge le fenetre.
            InitializeComponent();
        }
        private void ModeleNiveau_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;

        }

        #region bougeForme
        //Code pour activer le mouvement du programme avec la barre du haut
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int LPAR);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void bouge_fenetre(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        
        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;
        #endregion

        #region variablesGlobales
        bool droite = false; //Si vraie le joueur va a la droite
        bool gauche = false; //Si vraie le joueur va a la gauche
        bool saute = true; //Si vraie le joueur fait un saut quand il touche parterre
        bool dernierDirection = true; //True = droite, False = gauche
        bool parterre = false; //Si le joueur est parterre
        bool magmaGauche; //Animation pour la lave gauche
        bool magmaDroite; //Animation pour la lave droite
        int intForce = 22; //Force du saut du joueur
        int intT = 0; //variable pour les timers
        //Les images
        Image imgJoueurParterreDroite = Image.FromFile("assets/joueur_parterre_d.png");
        Image imgJoueurSauteDroite = Image.FromFile("assets/joueur_saute_d.png");
        Image imgJoueurParterreGauche = Image.FromFile("assets/joueur_parterre_g.png");
        Image imgJoueurSauteGauche = Image.FromFile("assets/joueur_saute_g.png");
        Image imgMagma1 = Image.FromFile("assets/magma_haut_gauche.png");
        Image imgMagma2 = Image.FromFile("assets/magma_haut_droite.png");
        #endregion

        #region methodes
        //Detecte et fait la collision
        private void collisionMagma(PictureBox picMagma)
        {
            //Recommence niveau si tu touche le magma
            if (picJoueur.Bounds.IntersectsWith(picMagma.Bounds))
            {
                picJoueur.Left = 128;
                picJoueur.Top = 450;
                intT = 0;
                tmrPerdu.Start();
                tmr1.Stop();
            }
        }
        private void magmaAnimGauche(PictureBox picMagma)
        {
            //Animation pour le Magma Gauche
            switch(magmaGauche)
            {
                case true:
                    picMagma.Image = imgMagma1;
                    magmaGauche = false;
                    break;
                case false:
                    picMagma.Image = imgMagma2;
                    magmaGauche = true;
                    break;
            }
        }
        private void magmaAnimDroite(PictureBox picMagma)
        {
            //Animation pour le magma droite
            switch (magmaDroite)
            {
                case true:
                    picMagma.Image = imgMagma2;
                    magmaDroite = false;
                    break;
                case false:
                    picMagma.Image = imgMagma1;
                    magmaDroite = true;
                    break;
            }
        }
        private void collisionBas(PictureBox picBas)
        {
            //Collison avec le bas d'une platforme
            if (picJoueur.Bounds.IntersectsWith(picBas.Bounds))
            {
                intForce = 0;
                picJoueur.Top = picBas.Bottom;
            }
        }
        private void collisionHaut(PictureBox picTop)
        {
            //Collison avec le Haut d'une platforme
            if (picJoueur.Bounds.IntersectsWith(picTop.Bounds))
            {
                if (saute == true)
                {
                    intForce = 22;
                    parterre = false;
                    if (dernierDirection == true)
                    {
                        picJoueur.Image = imgJoueurSauteDroite;
                    }
                    else
                    {
                        picJoueur.Image = imgJoueurSauteGauche;
                    }
                }
                else
                {
                    parterre = true;
                }
                picJoueur.Top = picTop.Location.Y - picJoueur.Height;
            }
        }
        private void collisionGauche(PictureBox picGauche)
        {
            //Collison avec le gauche d'une platforme
            {
                if (picJoueur.Bounds.IntersectsWith(picGauche.Bounds))
                {
                    droite = false;
                    picJoueur.Left = picGauche.Left - picJoueur.Width;
                }
            }
        }
        private void collisionDroite(PictureBox picDroite)
        {
            //Collison avec la droite d'une platforme
            if (picJoueur.Bounds.IntersectsWith(picDroite.Bounds))
            {
                gauche = false;
                picJoueur.Left = picDroite.Right;
            }
        }
        private void codeBase()
        {
            //Code de base pour le jeux
            //Mouvement
            if (droite == true)
            {
                picJoueur.Left += 7;
            }
            if (gauche == true)
            {
                picJoueur.Left -= 7;
            }
            //Collison avec la terre
            if (picJoueur.Left < 0 && tmrCommence.Enabled == false)
            {
                picJoueur.Left = 0;
            }
            if (picJoueur.Right > this.ClientRectangle.Width)
            {
                picJoueur.Left = this.ClientRectangle.Width - picJoueur.Width;
            }
        }
        #endregion

        #region Timers
        private void tmr1_Tick(object sender, EventArgs e)
        {
            //Fait le joueur tomber
            if (parterre == false)
            {
                intForce -= 1;
                picJoueur.Top -= intForce;
            }
            
            //Inclut les methodes du jeux
            codeBase();
            collisionHaut(picBase);
            collisionHaut(picBase2);
            collisionMagma(picMagma2);
            collisionMagma(picMagma1);
            picJoueur2.Top = picJoueur.Top;
            picJoueur3.Top = picJoueur.Top;
            picJoueur4.Top = picJoueur.Top;
            picJoueur5.Top = picJoueur.Top;
        }
        private void TmrCommence_Tick(object sender, EventArgs e)
        {
            //Fait le joueur entrer dans l'écran
            if (intT < 20)
            {
                //Faire rien lorsque le form load
                intT++;
            }
            else if (intT >= 20 && intT <= 50)
            {
                //Bouge le joueur a la bonne position
                picJoueur.Left += 7;
                intT++;
            }
            else
            {
                //Arrete le timer quand finis
                tmrCommence.Stop();
            }
        }

        private void TmrPerdu_Tick(object sender, EventArgs e)
        {
            //recommence le jeux quand tu perds
            if (intT == 0)
            {
                lblPerdu.Text = "3";
                intT++;
            }
            else if (intT == 1)
            {
                lblPerdu.Text = "2";
                intT++;
            }
            else if (intT == 2)
            {
                lblPerdu.Text = "1";
                intT++;
            }
            else if (intT == 3)
            {
                lblPerdu.Text = "Go!";
                intT++;
            }
            else if (intT == 4)
            {
                lblPerdu.Text = "";
                tmr1.Start();
                tmrPerdu.Stop();
            }
        }
        private void TmrMagma_Tick(object sender, EventArgs e)
        {
            //Fait les animations
            magmaAnimGauche(picMagma1);
            magmaAnimDroite(picMagma2);
        }
        #endregion

        #region clavier
        private void ModeleNiveau_KeyDown(object sender, KeyEventArgs e)
        {
            //Code de mouvement
            if (e.KeyCode == Keys.Right)
            {
                droite = true;
                dernierDirection = true;
                //Change la direction de l'image
                if (saute == true)
                {
                    picJoueur.Image = imgJoueurSauteDroite;
                } else
                {
                    picJoueur.Image = imgJoueurParterreDroite;
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                gauche = true;
                dernierDirection = false;
                //Change la direction de l'image
                if (saute == true)
                {
                    picJoueur.Image = imgJoueurSauteGauche;
                }
                else
                {
                    picJoueur.Image = imgJoueurParterreGauche;
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                intForce = 0;
                saute = false;
                picJoueur.Image = imgJoueurParterreDroite;
            }
        }
        private void ModeleNiveau_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                droite = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                gauche = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                saute = true;
                parterre = false;
            }
        }
        #endregion

        #region bouttons
        private void PicFerme_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}
