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
        private const string PLACEHOLDER_TEXT = "Entrée votre texte ici...";

        public Connection()
        {
            ((MainWindow)Application.Current.MainWindow).MenuPrincipale.Visibility = Visibility.Collapsed;
            InitializeComponent();
        }

        public void RemoveText(TextBox txtBox)
        {
            if (txtBox.Text == PLACEHOLDER_TEXT)
            {
                txtBox.Text = "";
            }
        }

        public static void AddText(TextBox txtBox)
        {
            if (string.IsNullOrWhiteSpace(txtBox.Text))
                txtBox.Text = PLACEHOLDER_TEXT;
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
            TenterConnexion();
        }

        private void TenterConnexion()
        {
            try
            {
                // Récupération et validation des données saisies
                string login = ObtenirLoginSaisi();
                string password = ObtenirMotDePasseSaisi();

                if (!ValiderDonneesSaisies(login, password))
                {
                    return;
                }

                // Tentative de connexion
                if (EffectuerConnexion(login, password))
                {
                    GererConnexionReussie();
                }
                else
                {
                    GererEchecConnexion("Login ou mot de passe incorrect.");
                }
            }
            catch (Exception ex)
            {
                GererErreurConnexion(ex);
            }
        }

        private string ObtenirLoginSaisi()
        {
            string login = TxtLogin.Text.Trim();
            return login == PLACEHOLDER_TEXT ? "" : login;
        }

        private string ObtenirMotDePasseSaisi()
        {
            string password = TxtPassword.Text.Trim();
            return password == PLACEHOLDER_TEXT ? "" : password;
        }

        private bool ValiderDonneesSaisies(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                TxtErreur.Text = "Veuillez saisir votre login.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                TxtErreur.Text = "Veuillez saisir votre mot de passe.";
                return false;
            }

            if (login.Length > 30)
            {
                TxtErreur.Text = "Le login ne peut pas dépasser 30 caractères.";
                return false;
            }

            if (password.Length > 100)
            {
                TxtErreur.Text = "Le mot de passe ne peut pas dépasser 100 caractères.";
                return false;
            }

            return true;
        }

        private bool EffectuerConnexion(string login, string password)
        {
            try
            {
                ConfigurerConnexionBD(login, password);
                Employe employe = RechercherEmploye(login, password);

                if (employe != null)
                {
                    ConfigurerSessionUtilisateur(employe);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la connexion : {ex.Message}");
            }
        }

        private void GererConnexionReussie()
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            mw.NaviguerVers(new Acceuil());
            mw.Selection_Menu_Item(mw.MI_Acceuil);
            mw.MenuPrincipale.Visibility = Visibility.Visible;
            mw.Connection();

            ViderChampsConnexion();
        }

        private void GererEchecConnexion(string message)
        {
            TxtErreur.Text = message;
            ReinitialiserMotDePasse();
        }

        private void GererErreurConnexion(Exception ex)
        {
            TxtErreur.Text = "Erreur de connexion. Vérifiez vos identifiants.";
            ReinitialiserMotDePasse();

            // Log de l'erreur pour debug
            Console.WriteLine($"Erreur de connexion: {ex.Message}");
        }

        private void ReinitialiserMotDePasse()
        {
            TxtPassword.Text = "";
            AddText(TxtPassword);
        }

        private void ViderChampsConnexion()
        {
            TxtLogin.Text = "";
            TxtPassword.Text = "";
            TxtErreur.Text = "";
            AddText(TxtLogin);
            AddText(TxtPassword);
        }

        // Méthodes dédiées pour les opérations de connexion
        private void ConfigurerConnexionBD(string login, string password)
        {
            try
            {
                ConnexionBD.Instance.ConfigurerConnexion(login, password);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur de configuration de la base de données : {ex.Message}");
            }
        }

        private Employe RechercherEmploye(string login, string password)
        {
            try
            {
                return Employe.Connexion(login, password);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la recherche de l'employé : {ex.Message}");
            }
        }

        private void ConfigurerSessionUtilisateur(Employe employe)
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            mw.EmployeConnecte = employe;
            mw.estResponsable = employe.EstResponsable;
        }
    }
}