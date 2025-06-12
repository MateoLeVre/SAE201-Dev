using AppliNicolas.Classes;
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
        public List<Vin> TousLesVins { get; set; }
        public ObservableCollection<Vin> VinsFiltres { get; set; }

        // Liste dynamique des filtres actifs
        private List<CritereFiltre> filtresActifs = new List<CritereFiltre>();

        public RechercheVin()
        {
            InitializeComponent();

            TousLesVins = ((MainWindow)Application.Current.MainWindow)
                .GestionVin.LesVins
                .OrderBy(v => v.Nom)
                .ToList();

            VinsFiltres = new ObservableCollection<Vin>(TousLesVins);
            DataContext = this;
            IC_Vins.ItemsSource = VinsFiltres;
        }

        private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrerVins();
        }

        private void Filtre_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && Enum.TryParse(cb.Tag?.ToString(), out CritereFiltre critere))
            {
                if (!filtresActifs.Contains(critere))
                    filtresActifs.Add(critere);

                FiltrerVins();
                MettreAJourTexteBoutonFiltre(); // 👈
            }
        }


        private void Filtre_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && Enum.TryParse(cb.Tag?.ToString(), out CritereFiltre critere))
            {
                if (filtresActifs.Contains(critere))
                    filtresActifs.Remove(critere);

                FiltrerVins();
                MettreAJourTexteBoutonFiltre(); // 👈
            }
        }


        private void btnFiltres_Checked(object sender, RoutedEventArgs e)
        {
            // Si des filtres sont actifs : clique = réinitialise
            if (filtresActifs.Count > 0)
            {
                foreach (var cb in wrapFiltres.Children.OfType<CheckBox>())
                    cb.IsChecked = false;

                filtresActifs.Clear();
                FiltrerVins();
                MettreAJourTexteBoutonFiltre();

                btnFiltres.IsChecked = false; // referme popup
                return;
            }

            // Sinon : on ouvre le popup normalement
            popupFiltres.IsOpen = true;
        }


        private void btnFiltres_Unchecked(object sender, RoutedEventArgs e)
        {
            popupFiltres.IsOpen = false;
        }
        private void MettreAJourTexteBoutonFiltre()
        {
            int nbFiltres = filtresActifs.Count;
            if (nbFiltres == 0)
                btnFiltres.Content = "Filtres ▼";
            else
                btnFiltres.Content = $"Filtres ({nbFiltres}) ✕";
        }

        private void FiltrerVins()
        {
            string texte = TxtRecherche?.Text?.ToLower() ?? "";
            IEnumerable<Vin> resultat = TousLesVins;

            if (string.IsNullOrWhiteSpace(texte))
            {
                VinsFiltres = new ObservableCollection<Vin>(resultat);
                IC_Vins.ItemsSource = VinsFiltres;
                return;
            }

            resultat = resultat.Where(v =>
            {
                bool match = false;

                if (filtresActifs.Count == 0)
                {
                    // Pas de filtres cochés : rechercher dans tout
                    match |= v.Nom.ToLower().Contains(texte);
                    match |= v.TypeVin.ToLower().Contains(texte);
                    match |= v.AppelationVin.ToLower().Contains(texte);
                    match |= v.Millesime.ToString().Contains(texte);
                }
                else
                {
                    foreach (var filtre in filtresActifs)
                    {
                        switch (filtre)
                        {
                            case CritereFiltre.Nom:
                                match |= v.Nom.ToLower().Contains(texte);
                                break;
                            case CritereFiltre.Type:
                                match |= v.TypeVin.ToLower().Contains(texte);
                                break;
                            case CritereFiltre.Appellation:
                                match |= v.AppelationVin.ToLower().Contains(texte);
                                break;
                            case CritereFiltre.Millesime:
                                match |= v.Millesime.ToString().Contains(texte);
                                break;
                        }
                    }
                }

                return match;
            });

            VinsFiltres = new ObservableCollection<Vin>(resultat.OrderBy(v => v.Nom));
            IC_Vins.ItemsSource = VinsFiltres;
        }

        private void VoirFiche_Click(object sender, RoutedEventArgs e)
        {
            Vin vin = (sender as Button)?.Tag as Vin;
            if (vin != null)
            {
                ((MainWindow)Application.Current.MainWindow).NaviguerVers(new FicheVin(vin));
            }
        }

        private void Rafraichir_click(object sender, RoutedEventArgs e)
        {
            TousLesVins = new Vin().RecupereVinDansBDD()
                .OrderBy(v => v.Nom)
                .ToList();

            VinsFiltres = new ObservableCollection<Vin>(TousLesVins);
            IC_Vins.ItemsSource = VinsFiltres;
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
