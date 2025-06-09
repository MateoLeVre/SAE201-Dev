using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes
{
    public class Commande
    {

        private DetailCommande detail;

        private int numCommande;

        private int numEmploye;

        private DateTime dateCommande;

        private string etatCommande;

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

        public int NumCommande
        {
            get { return numCommande; }
            set { numCommande = value; }
        }
        public int NumEmploye
        {
            get { return numEmploye; }
            set { numEmploye = value; }
        }

        public DateTime DateCommande
        {
            get { return dateCommande; }
            set { dateCommande = value; }
        }

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
