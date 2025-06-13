using AppliNicolas.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AppliNicolas.Pages
{
    public partial class RechercheVin : UserControl
    {
        private List<Vin> tousLesVins;
        private ObservableCollection<Vin> vinsFiltres;
        private List<CritereFiltre> filtresActifs;
        private bool triDecroissant = false;
        private string colonneTri = "Nom";

        public List<Vin> TousLesVins
        {
            get { return tousLesVins; }
            private set { tousLesVins = value ?? new List<Vin>(); }
        }

        public ObservableCollection<Vin> VinsFiltres
        {
            get { return vinsFiltres; }
            private set { vinsFiltres = value ?? new ObservableCollection<Vin>(); }
        }

        public RechercheVin()
        {
            InitializeComponent();

            try
            {
                InitialiserFiltres();
                ChargerDonnees();
                ConfigurerInterface();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des vins : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                InitialiserListesVides();
            }
        }

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                FiltrerVins();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtre_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is CheckBox cb && Enum.TryParse(cb.Tag?.ToString(), out CritereFiltre critere))
                {
                    if (!filtresActifs.Contains(critere))
                        filtresActifs.Add(critere);

                    FiltrerVins();
                    MettreAJourTexteBoutonFiltre();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'application du filtre : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Filtre_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is CheckBox cb && Enum.TryParse(cb.Tag?.ToString(), out CritereFiltre critere))
                {
                    if (filtresActifs.Contains(critere))
                        filtresActifs.Remove(critere);

                    FiltrerVins();
                    MettreAJourTexteBoutonFiltre();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression du filtre : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnFiltres_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (filtresActifs.Count > 0)
                {
                    ReinitialiserFiltres();
                    btnFiltres.IsChecked = false;
                    return;
                }

                popupFiltres.IsOpen = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la gestion des filtres : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnFiltres_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFiltres.IsOpen = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la fermeture des filtres : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VoirFiche_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vin vin = (sender as Button)?.Tag as Vin;
                if (vin == null)
                {
                    MessageBox.Show("Impossible de charger les informations de ce vin.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                NaviguerVersFicheVin(vin);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture de la fiche vin : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Rafraichir_click(object sender, RoutedEventArgs e)
        {
            try
            {
                RafraichirDonnees();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du rafraîchissement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbTriColonne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbTriColonne.SelectedItem is ComboBoxItem item && item.Tag is string tag)
                {
                    colonneTri = tag;
                    TrierVins();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du changement de tri : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnTriOrdre_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                triDecroissant = true;
                btnTriOrdre.Content = "↓";
                TrierVins();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du changement d'ordre de tri : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnTriOrdre_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                triDecroissant = false;
                btnTriOrdre.Content = "↑";
                TrierVins();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du changement d'ordre de tri : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void InitialiserFiltres()
        {
            filtresActifs = new List<CritereFiltre>();
        }

        private void ChargerDonnees()
        {
            try
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                if (mainWindow?.GestionVin?.LesVins == null)
                {
                    throw new Exception("Impossible d'accéder aux données des vins");
                }

                TousLesVins = mainWindow.GestionVin.LesVins
                    .OrderBy(v => v.Nom)
                    .ToList();

                VinsFiltres = new ObservableCollection<Vin>(TousLesVins);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des données : {ex.Message}");
            }
        }

        private void ConfigurerInterface()
        {
            DataContext = this;
            IC_Vins.ItemsSource = VinsFiltres;
        }

        private void InitialiserListesVides()
        {
            TousLesVins = new List<Vin>();
            VinsFiltres = new ObservableCollection<Vin>();
            InitialiserFiltres();
            ConfigurerInterface();
        }

        private void MettreAJourTexteBoutonFiltre()
        {
            int nbFiltres = filtresActifs.Count;
            if (nbFiltres == 0)
                btnFiltres.Content = "Filtres ▼";
            else
                btnFiltres.Content = $"Filtres ({nbFiltres}) ✕";
        }

        private void ReinitialiserFiltres()
        {
            try
            {
                foreach (var cb in wrapFiltres.Children.OfType<CheckBox>())
                    cb.IsChecked = false;

                filtresActifs.Clear();
                FiltrerVins();
                MettreAJourTexteBoutonFiltre();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la réinitialisation des filtres : {ex.Message}");
            }
        }

        private void FiltrerVins()
        {
            try
            {
                string texte = TxtRecherche?.Text?.ToLower()?.Trim() ?? "";
                IEnumerable<Vin> resultat = TousLesVins;

                if (!string.IsNullOrWhiteSpace(texte))
                {
                    resultat = resultat.Where(v => VerifierCorrespondanceFiltre(v, texte));
                }

                VinsFiltres = new ObservableCollection<Vin>(resultat.OrderBy(v => v.Nom));
                IC_Vins.ItemsSource = VinsFiltres;
                TrierVins();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du filtrage des vins : {ex.Message}");
            }
        }

        private bool VerifierCorrespondanceFiltre(Vin vin, string texte)
        {
            if (vin == null) return false;

            bool match = false;

            if (filtresActifs.Count == 0)
            {
                match |= vin.Nom?.ToLower().Contains(texte) == true;
                match |= vin.TypeVin?.ToLower().Contains(texte) == true;
                match |= vin.AppelationVin?.ToLower().Contains(texte) == true;
                match |= vin.Millesime.ToString().Contains(texte);
                match |= vin.Prix.ToString().Contains(texte);
                match |= vin.Detail?.ToLower().Contains(texte) == true;
            }
            else
            {
                foreach (var filtre in filtresActifs)
                {
                    switch (filtre)
                    {
                        case CritereFiltre.Nom:
                            match |= vin.Nom?.ToLower().Contains(texte) == true;
                            break;
                        case CritereFiltre.Type:
                            match |= vin.TypeVin?.ToLower().Contains(texte) == true;
                            break;
                        case CritereFiltre.Appellation:
                            match |= vin.AppelationVin?.ToLower().Contains(texte) == true;
                            break;
                        case CritereFiltre.Millesime:
                            match |= vin.Millesime.ToString().Contains(texte);
                            break;
                    }
                }
            }

            return match;
        }

        private void TrierVins()
        {
            try
            {
                if (VinsFiltres == null || VinsFiltres.Count == 0)
                    return;

                IOrderedEnumerable<Vin> trie;

                switch (colonneTri)
                {
                    case "Nom":
                        trie = triDecroissant ?
                            VinsFiltres.OrderByDescending(v => v.Nom ?? "") :
                            VinsFiltres.OrderBy(v => v.Nom ?? "");
                        break;
                    case "Type":
                        trie = triDecroissant ?
                            VinsFiltres.OrderByDescending(v => v.TypeVin ?? "") :
                            VinsFiltres.OrderBy(v => v.TypeVin ?? "");
                        break;
                    case "Appellation":
                        trie = triDecroissant ?
                            VinsFiltres.OrderByDescending(v => v.AppelationVin ?? "") :
                            VinsFiltres.OrderBy(v => v.AppelationVin ?? "");
                        break;
                    case "Millesime":
                        trie = triDecroissant ?
                            VinsFiltres.OrderByDescending(v => v.Millesime) :
                            VinsFiltres.OrderBy(v => v.Millesime);
                        break;
                    case "Prix":
                        trie = triDecroissant ?
                            VinsFiltres.OrderByDescending(v => v.Prix) :
                            VinsFiltres.OrderBy(v => v.Prix);
                        break;
                    default:
                        trie = VinsFiltres.OrderBy(v => v.Nom ?? "");
                        break;
                }

                VinsFiltres = new ObservableCollection<Vin>(trie);
                IC_Vins.ItemsSource = VinsFiltres;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du tri des vins : {ex.Message}");
            }
        }

        private void NaviguerVersFicheVin(Vin vin)
        {
            try
            {
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheVin(vin));
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la navigation vers la fiche vin : {ex.Message}");
            }
        }

        private void RafraichirDonnees()
        {
            try
            {
                TousLesVins = ChargerVinsDepuisBase();
                VinsFiltres = new ObservableCollection<Vin>(TousLesVins);
                IC_Vins.ItemsSource = VinsFiltres;
                TxtRecherche.Text = "";
                ReinitialiserFiltres();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du rafraîchissement des données : {ex.Message}");
            }
        }

        // Méthode dédiée pour la requête SQL
        private List<Vin> ChargerVinsDepuisBase()
        {
            try
            {
                return new Vin().RecupereVinDansBDD()
                    .OrderBy(v => v.Nom)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des vins depuis la base : {ex.Message}");
            }
        }
    }

    public enum CritereFiltre
    {
        Aucun,
        Nom,
        Type,
        Appellation,
        Millesime
    }
}