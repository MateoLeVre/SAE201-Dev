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
            string login = TxtLogin.Text;
            string password = TxtPassword.Text;


            if (login == "admin" && password == "admin")
            {
                
                ((MainWindow)Application.Current.MainWindow).estResponsable = true;

            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).estResponsable = false;
            }
            ((MainWindow)Application.Current.MainWindow).NaviguerVers(new Acceuil());

            ((MainWindow)Application.Current.MainWindow).Selection_Menu_Item(((MainWindow)Application.Current.MainWindow).MI_Acceuil);

            ((MainWindow)Application.Current.MainWindow).MenuPrincipale.Visibility = Visibility.Visible;

            ((MainWindow)Application.Current.MainWindow).Connection();
        }

        private void SeConnecterRapide_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).estResponsable = true;

            ((MainWindow)Application.Current.MainWindow).NaviguerVers(new Acceuil());

            ((MainWindow)Application.Current.MainWindow).Selection_Menu_Item(((MainWindow)Application.Current.MainWindow).MI_Acceuil);

            ((MainWindow)Application.Current.MainWindow).MenuPrincipale.Visibility = Visibility.Visible;

            ((MainWindow)Application.Current.MainWindow).Connection();
        }

        }
}
