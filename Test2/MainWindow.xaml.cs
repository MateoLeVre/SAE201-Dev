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

namespace Test2
{
    public partial class MainWindow : Window
    {
        public bool IsResponsable { get; set; } = true; // à remplacer dynamiquement selon le login
        
        public string RoleUtilisateur => IsResponsable ? "Responsable" : "Vendeur";

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this; // permet le binding de IsResponsable
        }

        private void Accueil_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Recherche_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Commandes_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Profil_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Demandes_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CommandesFournisseur_Click(object sender, RoutedEventArgs e)
        {
        }

        private void APropos_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Application Nicolas Vins\nVersion 1.0", "À propos");
        }

        private void Deconnexion_Click(object sender, RoutedEventArgs e)
        {
            // Logique de déconnexion
            MessageBox.Show("Déconnecté.");
        }

        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}