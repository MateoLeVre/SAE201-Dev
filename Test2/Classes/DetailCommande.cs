using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes
{
    public class DetailCommande
    {
        private int numCommande;
        private int numVin;
        private int quantite;
        private double prixDetail;

        //Pour le prix du vin il faut que je code le truc du vin// 

        public DetailCommande(int numCommande, int numVin, int quantite)
        {
            this.NumCommande = numCommande;
            this.NumVin = numVin;
            this.Quantite = quantite;
            PrixDetail = 0;
        }

        public int NumCommande
        {
            get
            {
                return numCommande;
            }

            set
            {
                numCommande = value;
            }
        }

        public int NumVin
        {
            get
            {
                return numVin;
            }

            set
            {
                numVin = value;
            }
        }

        public int Quantite
        {
            get
            {
                return quantite;
            }

            set
            {
                quantite = value;
            }
        }

        public double PrixDetail
        {
            get
            {
                return this.prixDetail;
            }

            set
            {
                this.prixDetail = value;
            }
        }
    }
}
