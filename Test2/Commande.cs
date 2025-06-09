using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas
{
    public class Commande
    {
        public Commande(int numCommande, DetailCommande detail, DateTime dateCommande, string etatCommande)
        {
            NumCommande = numCommande;
			NumEmploye = detail.demande.NumEmploye;
            DatCommande = dateCommande;
            EtatCommande = etatCommande;
            PrixTotal = detail.PrixCommande;
        }

		private DetailCommande detail;

        private int numCommande;
		public int NumCommande
		{
			get { return numCommande; }
			set { numCommande = value; }
		}

		public int NumEmploye
		{
			get { return detail.demande.NumEmploye; }
			set { detail.demande.NumEmploye = value; }
		}

		private DateTime dateCommande;
		public DateTime DatCommande
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

		public double PrixTotal
		{
			get { return detail.PrixCommande; }
			set { detail.PrixCommande = value; }
		}

        public override bool Equals(object? obj)
        {
            return obj is Commande commande &&
                   NumCommande == commande.NumCommande;
        }
    }
}
