using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes
{
    public class GestionVin
    {
        private ObservableCollection<Vin> lesVins;
        private ObservableCollection<Commande> lesCommandes;
        private ObservableCollection<Demande> lesDemandes;
        private ObservableCollection<Client> lesClients;

        public GestionVin()
        {
            this.LesVins = ChargerVins();
            this.LesCommandes = ChargerCommandes();
            this.LesDemandes = ChargerDemandes();
            this.LesClients = ChargerClients();
        }

        public ObservableCollection<Vin> LesVins
        {
            get { return lesVins; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("La collection de vins ne peut pas être null");
                }
                lesVins = value;
            }
        }

        public ObservableCollection<Commande> LesCommandes
        {
            get { return lesCommandes; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("La collection de commandes ne peut pas être null");
                }
                lesCommandes = value;
            }
        }

        public ObservableCollection<Demande> LesDemandes
        {
            get { return lesDemandes; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("La collection de demandes ne peut pas être null");
                }
                lesDemandes = value;
            }
        }

        public ObservableCollection<Client> LesClients
        {
            get { return lesClients; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("La collection de clients ne peut pas être null");
                }
                lesClients = value;
            }
        }

        // Méthodes dédiées pour charger les données depuis la BDD
        private ObservableCollection<Vin> ChargerVins()
        {
            try
            {
                return new ObservableCollection<Vin>(new Vin().RecupereVinDansBDD());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des vins : {ex.Message}");
            }
        }

        private ObservableCollection<Commande> ChargerCommandes()
        {
            try
            {
                return new ObservableCollection<Commande>(new Commande().RecupereCommandeDansBDD());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des commandes : {ex.Message}");
            }
        }

        private ObservableCollection<Demande> ChargerDemandes()
        {
            try
            {
                return new ObservableCollection<Demande>(new Demande().RecupereDemandeDansBDD());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des demandes : {ex.Message}");
            }
        }

        private ObservableCollection<Client> ChargerClients()
        {
            try
            {
                return new ObservableCollection<Client>(new Client().RecupereClientDansBDD());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors du chargement des clients : {ex.Message}");
            }
        }
    }
}