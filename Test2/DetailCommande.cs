using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas
{
    public class DetailCommande
    {
        public DetailCommande(int numVin, int quantiteCommande, double prixCommande, int numDemande)
        {
            NumVin = numVin;
            QuantiteCommande = quantiteCommande;
            PrixCommande = prixCommande;
            NumDemande = numDemande;
        }

		private int numDemande;
		public int NumDemande
		{
			get { return numDemande; }
			set { numDemande = value; }
		}

		private int numVin;
		public int NumVin
		{
			get { return numVin; }
			set { numVin = value; }
		}

		private int quantiteCommande;
		public int QuantiteCommande
		{
			get { return quantiteCommande; }
			set { quantiteCommande = value; }
		}

		private double prixCommande;
        public double PrixCommande
		{
			get { return prixCommande; }
			set { prixCommande = value; }
		}

        public override bool Equals(object? obj)
        {
            return obj is DetailCommande commande &&
                   NumDemande == commande.NumDemande;
        }
    }
}
