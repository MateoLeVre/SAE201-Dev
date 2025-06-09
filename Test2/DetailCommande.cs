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
        public DetailCommande(Demande demande)
        {
            NumVin = demande.NumVin;
            QuantiteCommande = demande.QuantiteDemande;
			PrixCommande = 0;
        }

		public Demande demande;

		public int NumVin
		{
			get { return demande.NumVin; }
			set { demande.NumVin = value; }
		}

		public int QuantiteCommande
		{
			get { return demande.QuantiteDemande; }
			set { demande.QuantiteDemande = value; }
		}

		private double prixCommande;
        public double PrixCommande
		{
            get { return prixCommande; }
            set { prixCommande = demande.QuantiteDemande * demande.vin.Prix; }
		}
    }
}
