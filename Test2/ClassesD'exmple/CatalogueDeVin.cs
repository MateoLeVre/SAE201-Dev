using AppliNicolas.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.ClassesD_exmple
{
    public static class CatalogueDeVin
    {
        public static List<Vin> ObtenirExemples()
        {
            return new List<Vin>
                {
                    new Vin("Esporao Reserva Tinto", "Bourgogne", TypeVin.Rouge, 2020, "/Images/vin.jpg", 12, 19.20,
                            "E305TP12", "Domaine Esporao",
                            "Ce vin rouge portugais d'exception provient du domaine renommé Esporao, situé dans la région de l'Alentejo...",
                            true),

                    new Vin("Château Margaux", "Margaux", TypeVin.Rouge, 2018, "/Images/vin.jpg", 8, 34.50,
                            "C421MG18", "Château Margaux",
                            "Un grand cru classique, élégant et raffiné, avec des notes de cassis, de cèdre et de truffe.",
                            false),

                    new Vin("Domaine de la Romanée-Conti", "Bourgogne", TypeVin.Rouge, 2021, "/Images/vin.jpg", 5, 120.00,
                            "DRC001", "Romanée-Conti",
                            "Un vin mythique aux arômes complexes et à la texture soyeuse, emblématique de la Bourgogne.",
                            true),

                    new Vin("Chablis Premier Cru", "Chablis", TypeVin.Blanc, 2020, "/Images/vin.jpg", 10, 25.00,
                            "CHB2020", "Domaine Laroche",
                            "Ce Chablis frais et minéral est parfait pour accompagner fruits de mer et poissons grillés.",
                            false),

                    new Vin("Rosé de Provence", "Provence", TypeVin.Rosé, 2020, "/Images/vin.jpg", 20, 12.90,
                            "RSP2020", "Maison Provence",
                            "Rosé léger et fruité, idéal pour l'été avec des arômes de fraise et de pamplemousse.",
                            false),

                    new Vin("Château Cheval Blanc", "Saint-Émilion", TypeVin.Rouge, 2020, "/Images/vin.jpg", 6, 98.00,
                            "CCB2020", "Cheval Blanc",
                            "Un grand Saint-Émilion, intense et harmonieux, avec une belle profondeur aromatique.",
                            true),
                };
                    }


        public static List<Vin> TrouverSimilaires(Vin vinPrincipal)
        {
            return ObtenirExemples().Where(vin => vin != vinPrincipal && vinPrincipal.EstSimilaireA(vin)).ToList();
        }
    }
}
