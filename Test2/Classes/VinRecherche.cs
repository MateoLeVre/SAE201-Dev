using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes
{
    public class VinRecherche
    {
        public VinRecherche(Vin vin, DateTime heureRecherche)
        {
            Vin = vin;
            HeureRecherche = heureRecherche;
        }

        public Vin Vin { get; set; }
        public DateTime HeureRecherche { get; set; }
    }

}
