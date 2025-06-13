using AppliNicolas.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppliNicolas.Pages
{
    /// <summary>
    /// Logique d'interaction pour Connection.xaml
    /// </summary>
    public partial class Connection : UserControl
    {
        public Connection()
        {
            ((MainWindow)Application.Current.MainWindow).MenuPrincipale.Visibility = Visibility.Collapsed;

            InitializeComponent();

        }


        public void RemoveText(TextBox txtBox)
        {
            if (txtBox.Text == "Entrée votre texte ici...")
            {
                txtBox.Text = "";
            }
        }

        public static void AddText(TextBox txtBox)
        {
            if (string.IsNullOrWhiteSpace(txtBox.Text))
                txtBox.Text = "Entrée votre texte ici...";
        }

        private void TxtLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveText(TxtLogin);
        }

        private void TxtLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            AddText(TxtLogin);
        }
        private void TxtPass_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveText(TxtPassword);
        }

        private void TxtPass_LostFocus(object sender, RoutedEventArgs e)
        {
            AddText(TxtPassword);
        }


        private void SeConnecter_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text.Trim();
            string password = TxtPassword.Text.Trim();

            try
            {
                ConnexionBD.Instance.ConfigurerConnexion(login, password);

                Console.WriteLine(login + " : "+ password);

                Employe employe = Employe.Connexion(login, password);
                if (employe != null)
                {
                    MainWindow mw = (MainWindow)Application.Current.MainWindow;
                    mw.EmployeConnecte = employe;
                    mw.estResponsable = employe.EstResponsable;

                    mw.GestionVin = new GestionVin();

                    mw.NaviguerVers(new Acceuil());
                    mw.Selection_Menu_Item(mw.MI_Acceuil);
                    mw.MenuPrincipale.Visibility = Visibility.Visible;
                    mw.Connection();
                }
                else
                {
                    TxtErreur.Text = "Login ou mot de passe incorrect. (mot de passe incorrect)";
                    TxtPassword.Text = "";
                    AddText(TxtPassword);
                }
            }
            catch (Exception ex)
            {
                TxtErreur.Text = "Login ou mot de passe incorrect. (login inccorect)";
                TxtPassword.Text = "";
                AddText(TxtPassword);
            }
        }


        }
}
