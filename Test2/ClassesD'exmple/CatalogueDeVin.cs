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
            return new List<Vin>
    {
        new Vin("Château Margaux", 1, 0, 2018, 34.50, 1001, 3, "Un grand cru classique, élégant avec des notes de cassis et de truffe."),
        new Vin("Esporão Reserva", 2, 0, 2020, 19.20, 1002, 4, "Vin rouge portugais issu de l'Alentejo, puissant et fruité."),
        new Vin("Romanée-Conti", 1, 0, 2021, 120.00, 1003, 6, "Vin de Bourgogne prestigieux, complexe et soyeux."),
        new Vin("Chablis Premier Cru", 3, 1, 2020, 25.00, 1004, 8, "Chablis frais et minéral, parfait pour fruits de mer."),
        new Vin("Rosé de Provence", 4, 2, 2020, 12.90, 1005, 9, "Rosé fruité, idéal pour l'été, arômes de fraise et pamplemousse."),
        new Vin("Cheval Blanc", 5, 0, 2020, 98.00, 1006, 1, "Saint-Émilion harmonieux, intense avec profondeur aromatique."),
        new Vin("Domaine Tempier", 6, 2, 2019, 17.00, 1007, 2, "Rosé de Bandol de caractère, floral et structuré."),
        new Vin("Puligny-Montrachet", 7, 1, 2019, 42.50, 1008, 3, "Vin blanc élégant, riche et équilibré, notes de noisette."),
        new Vin("Côtes-du-Rhône Bio", 8, 0, 2022, 11.00, 1009, 5, "Vin rouge bio facile à boire, arômes de fruits rouges."),
        new Vin("Sancerre Blanc", 9, 1, 2021, 21.90, 1010, 7, "Blanc sec et expressif de Loire, idéal avec poissons.")
    };
        }



        public static List<Vin> TrouverSimilaires(Vin vinPrincipal)
        {
            List<Vin> similaires = ObtenirExemples()
                .Where(vin => vinPrincipal.CalculerDegreRessemblance(vin) >= 1)
                .OrderByDescending(vin => vinPrincipal.CalculerDegreRessemblance(vin))
                .Take(5)
                .ToList();

            similaires.RemoveAll(v => v.Nom == vinPrincipal.Nom);

            return similaires;
        }



    }
}
