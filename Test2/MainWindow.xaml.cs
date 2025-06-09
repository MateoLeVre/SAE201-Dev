using AppliNicolas.Classes;
using AppliNicolas.ClassesD_exmple;
using AppliNicolas.Pages;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppliNicolas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool estResponsable { get; set; }

        public string role_Utilisateur { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            double ecranLargeur = SystemParameters.PrimaryScreenWidth;
            double ecranHauteur = SystemParameters.PrimaryScreenHeight;

            this.Width = ecranLargeur;
            this.Height = ecranHauteur;

            estResponsable = false;
            if (!estResponsable)
            {
                role_Utilisateur = "Vendeur";
                MI_Commande.Visibility = Visibility.Collapsed;
            }
            else
            {
                role_Utilisateur = "Responsable";
            }

            Selection_Menu_Item(MI_Acceuil);

            MenuPrincipale.DataContext = this;

        }


        public void Selection_Menu_Item(MenuItem mi_choisi)
        {
            //On change le style que si le menu n'est pas celui sélectionné
            if (mi_choisi.Style != (Style)this.FindResource("StyleMenuItemActif"))
            {
                //Mettre tout les menu item en style de base

                MI_Commande.Style = (Style)this.FindResource("StyleMenuItem");
                MI_Acceuil.Style = (Style)this.FindResource("StyleMenuItem");
                MI_Vin.Style = (Style)this.FindResource("StyleMenuItem");
                MI_Demande.Style = (Style)this.FindResource("StyleMenuItem");
                MI_Client.Style = (Style)this.FindResource("StyleMenuItem");
                MI_Deconnection.Style = (Style)this.FindResource("StyleMenuItem");

                //Mettre le menu item actif en style actif
                mi_choisi.Style = (Style)this.FindResource("StyleMenuItemActif");
            }
        }

        private void MI_Acceuil_Click(object sender, RoutedEventArgs e)
        {
            Selection_Menu_Item(MI_Acceuil);
        }

        private void MI_Vin_Click(object sender, RoutedEventArgs e)
        {
            Selection_Menu_Item(MI_Vin); 
            FicheVin fiche = new FicheVin(CatalogueDeVin.ObtenirExemples()[0]);
            MainContent.Content = fiche;
        }

        private void MI_Demande_Click(object sender, RoutedEventArgs e)
        {
            Selection_Menu_Item(MI_Demande);
        }

        private void MI_Client_Click(object sender, RoutedEventArgs e)
        {
            Selection_Menu_Item(MI_Client);
        }

        private void MI_Deconnection_Click(object sender, RoutedEventArgs e)
        {
            Selection_Menu_Item(MI_Deconnection);
        }

        private void MI_Commande_Click(object sender, RoutedEventArgs e)
        {
            Selection_Menu_Item(MI_Commande);
        }
    }
}