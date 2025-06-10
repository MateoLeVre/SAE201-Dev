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
            this.LesVins = new ObservableCollection<Vin>(new Vin().RecupereVinDansBDD());
            this.LesCommandes = new ObservableCollection<Commande>(new Commande().RecupereCommandeDansBDD());
            this.LesDemandes = new ObservableCollection<Demande>(new Demande().RecupereDemandeDansBDD());
            this.LesClients = new ObservableCollection<Client>(new Client().RecupereClientDansBDD());
        }

        public ObservableCollection<Vin> LesVins
        {
            get
            {
                return lesVins;
            }

            set
            {
                lesVins = value;
            }
        }

        public ObservableCollection<Commande> LesCommandes
        {
            get
            {
                return lesCommandes;
            }

            set
            {
                lesCommandes = value;
            }
        }

        public ObservableCollection<Demande> LesDemandes
        {
            get
            {
                return this.lesDemandes;
            }

            set
            {
                this.lesDemandes = value;
            }
        }

        public ObservableCollection<Client> LesClients
        {
            get
            {
                return this.lesClients;
            }

            set
            {
                this.lesClients = value;
            }
        }
    }
}
