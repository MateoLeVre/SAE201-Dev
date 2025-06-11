using AppliNicolas.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace AppliNicolas.ClassesD_exmple
{
    public static class CatalogueDeVin
    {
        public static List<Vin> ObtenirExemples()
        {
           return new Vin().RecupereVinDansBDD();
    
        }



        public static List<Vin> TrouverSimilaires(Vin vinPrincipal)
        {
            return ObtenirExemples()
                .Where(vin => vin.Nom != vinPrincipal.Nom &&
                              vinPrincipal.CalculerDegreRessemblance(vin) >= 1)
                .OrderByDescending(vin => vinPrincipal.CalculerDegreRessemblance(vin))
                .Take(5)
                .ToList();
        }




    }
}
