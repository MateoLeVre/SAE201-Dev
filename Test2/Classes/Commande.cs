using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes
{
    public class Commande
    {
        public Commande(int numCommande, DetailCommande detail, DateTime dateCommande, string etatCommande)
        {
            NumCommande = numCommande;
            NumEmploye = detail.demande.NumEmploye;
            DateCommande = dateCommande;
            EtatCommande = etatCommande;
        }

        public Commande(int numCommande, int numEmploye, DateTime dateCommande, string etatCommande)
        {
            NumCommande = numCommande;
            NumEmploye = numEmploye;
            DateCommande = dateCommande;
            EtatCommande = etatCommande;
        }

        private DetailCommande detail;

        private int numCommande;
        public int NumCommande
        {
            get { return numCommande; }
            set { numCommande = value; }
        }

        private int numEmploye;
        public int NumEmploye
        {
            get { return numEmploye; }
            set { numEmploye = value; }
        }

        private DateTime dateCommande;
        public DateTime DateCommande
        {
            get { return dateCommande; }
            set { dateCommande = value; }
        }

        private string etatCommande;
        public string EtatCommande
        {
            get { return etatCommande; }
            set { etatCommande = value; }
        }

        public override bool Equals(object? obj)
        {
            return obj is Commande commande &&
                   NumCommande == commande.NumCommande;
        }
    }
}
