using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Risk
{
    class Map
    {
        // LISTS
        private List<Continent> continents;
        private List<Territory> territories;

        // CONTINENTS
        private Continent northamerica;
        private Continent southamerica;
        private Continent africa;
        private Continent europe;
        private Continent asia;
        private Continent australia;

        // NORTH AMERICA
        private Territory alaska;
        private Territory nunavut;
        private Territory greenland;
        private Territory alberta;
        private Territory ontario;
        private Territory quebec;
        private Territory california;
        private Territory florida;
        private Territory mexico;

        // SOUTH AMERICA
        private Territory venezuela;
        private Territory brazil;
        private Territory peru;
        private Territory argentina;

        // AFRICA
        private Territory algeria;
        private Territory egypt;
        private Territory ethiopia;
        private Territory congo;
        private Territory southafrica;
        private Territory madagascar;

        // EUROPE
        private Territory iberia;
        private Territory rome;
        private Territory britain;
        private Territory prussia;
        private Territory russia;
        private Territory iceland;
        private Territory scandinavia;

        // ASIA
        private Territory middleeast;
        private Territory india;
        private Territory siam;
        private Territory afghanistan;
        private Territory china;
        private Territory ural;
        private Territory mongolia;
        private Territory japan;
        private Territory siberia;
        private Territory irkutsk;
        private Territory yakutsk;
        private Territory kamchatka;

        // AUSTRALIA
        private Territory indonesia;
        private Territory newguinea;
        private Territory perth;
        private Territory sydney;

        // LISTS
        public List<Continent> Continents
        {
            get { return continents; }
        }

        public List<Territory> Territories
        {
            get { return territories; }
        }
        
        // CONTINENTS
        public Continent NorthAmerica
        {
            get { return northamerica; }
        }
        public Continent SouthAmerica
        {
            get { return southamerica; }
        }
        public Continent Africa
        {
            get { return africa; }
        }
        public Continent Europe
        {
            get { return europe; }
        }
        public Continent Asia
        {
            get { return asia; }
        }
        public Continent Australia
        {
            get { return australia; }
        }

        // NORTH AMERICA
        public Territory Alaska
        {
            get { return alaska; }
        }
        public Territory Nunavut
        {
            get { return nunavut; }
        }
        public Territory Greenland
        {
            get { return greenland; }
        }
        public Territory Alberta
        {
            get { return alberta; }
        }
        public Territory Ontario
        {
            get { return ontario; }
        }
        public Territory Quebec
        {
            get { return quebec; }
        }
        public Territory California
        {
            get { return california; }
        }
        public Territory Florida
        {
            get { return florida; }
        }
        public Territory Mexico
        {
            get { return mexico; }
        }

        // SOUTH AMERICA
        public Territory Venezuela
        {
            get { return venezuela; }
        }
        public Territory Peru
        {
            get { return peru; }
        }
        public Territory Brazil
        {
            get { return brazil; }
        }
        public Territory Argentina
        {
            get { return argentina; }
        }

        // AFRICA
        public Territory Algeria
        {
            get { return algeria; }
        }
        public Territory Egypt
        {
            get { return egypt; }
        }
        public Territory Ethiopia
        {
            get { return ethiopia; }
        }
        public Territory SouthAfrica
        {
            get { return southafrica; }
        }
        public Territory Madagascar
        {
            get { return madagascar; }
        }
        public Territory Congo
        {
            get { return congo; }
        }

        // EUROPE
        public Territory Iberia
        {
            get { return iberia; }
        }
        public Territory Rome
        {
            get { return rome; }
        }
        public Territory Britain
        {
            get { return britain; }
        }
        public Territory Prussia
        {
            get { return prussia; }
        }
        public Territory Russia
        {
            get { return russia; }
        }
        public Territory Iceland
        {
            get { return iceland; }
        }
        public Territory Scandinavia
        {
            get { return scandinavia; }
        }

        // ASIA
        public Territory MiddleEast
        {
            get { return middleeast; }
        }
        public Territory India
        {
            get { return india; }
        }
        public Territory Siam
        {
            get { return siam; }
        }
        public Territory Afghanistan
        {
            get { return afghanistan; }
        }
        public Territory China
        {
            get { return china; }
        }
        public Territory Ural
        {
            get { return ural; }
        }
        public Territory Mongolia
        {
            get { return mongolia; }
        }
        public Territory Japan
        {
            get { return japan; }
        }
        public Territory Siberia
        {
            get { return siberia; }
        }
        public Territory Irkutsk
        {
            get { return irkutsk; }
        }
        public Territory Yakutsk
        {
            get { return yakutsk; }
        }
        public Territory Kamchatka
        {
            get { return kamchatka; }
        }

        // AUSTRALIA
        public Territory Indonesia
        {
            get { return indonesia; }
        }
        public Territory NewGuinea
        {
            get { return newguinea; }
        }
        public Territory Perth
        {
            get { return perth; }
        }
        public Territory Sydney
        {
            get { return sydney; }
        }

        public Map()
        {
            // NORTH AMERICA
            alaska = new Territory("Alaska", northamerica);
            nunavut = new Territory("Nunavut", northamerica);
            greenland = new Territory("Greenland", northamerica);
            alberta = new Territory("Alberta", northamerica);
            ontario = new Territory("Ontario", northamerica);
            quebec = new Territory("Quebec", northamerica);
            california = new Territory("California", northamerica);
            florida = new Territory("Florida", northamerica);
            mexico = new Territory("Mexico", northamerica);
            // SOUTH AMERICA
            venezuela = new Territory("Venezuela", southamerica);
            brazil = new Territory("Brazil", southamerica);
            peru = new Territory("Peru", southamerica);
            argentina = new Territory("Argentina", southamerica);
            // AFRICA
            algeria = new Territory("Algeria", africa);
            egypt = new Territory("Egypt", africa);
            ethiopia = new Territory("Ethiopia", africa);
            congo = new Territory("Congo", africa);
            southafrica = new Territory("South Africa", africa);
            madagascar = new Territory("Madagascar", africa);
            // EUROPE
            iceland = new Territory("Iceland", europe);
            scandinavia = new Territory("Scandinavia", europe);
            britain = new Territory("Britain", europe);
            prussia = new Territory("Prussia", europe);
            russia = new Territory("Russia", europe);
            iberia = new Territory("Iberia", europe);
            rome = new Territory("Rome", europe);
            // ASIA
            middleeast = new Territory("Middle East", asia);
            india = new Territory("India", asia);
            siam = new Territory("Siam", asia);
            afghanistan = new Territory("Afghanistan", asia);
            china = new Territory("China", asia);
            ural = new Territory("Ural", asia);
            siberia = new Territory("Siberia", asia);
            irkutsk = new Territory("Irkutsk", asia);
            mongolia = new Territory("Mongolia", asia);
            japan = new Territory("Japan", asia);
            yakutsk = new Territory("Yakutsk", asia);
            kamchatka = new Territory("Kamchatka", asia);
            // AUSTRALIA
            indonesia = new Territory("Indonesia", australia);
            newguinea = new Territory("New Guinea", australia);
            perth = new Territory("Perth", australia);
            sydney = new Territory("Sydney", australia);
            // CONTINENTS
            northamerica = new Continent("North America", new List<Territory> { alaska, nunavut, greenland, alberta, ontario, quebec, california, florida, mexico });
            southamerica = new Continent("South America", new List<Territory> { venezuela, brazil, peru, argentina });
            africa = new Continent("Africa", new List<Territory> { algeria, egypt, ethiopia, congo, southafrica, madagascar });
            europe = new Continent("Europe", new List<Territory> { iceland, scandinavia, britain, prussia, iberia, rome, russia });
            asia = new Continent("Asia", new List<Territory> { middleeast, india, siam, afghanistan, china, ural, siberia, irkutsk, mongolia, japan, yakutsk, kamchatka });
            australia = new Continent("Australia", new List<Territory> { indonesia, newguinea, perth, sydney });

            // ADJACENT
            // NORTH AMERICA
            alaska.AdjacentTerritories = new List<Territory> { kamchatka, nunavut, alberta };
            nunavut.AdjacentTerritories = new List<Territory> { alaska, alberta, ontario, greenland };
            greenland.AdjacentTerritories = new List<Territory> { iceland, quebec, ontario, nunavut };
            alberta.AdjacentTerritories = new List<Territory> { alaska, nunavut, ontario, california };
            ontario.AdjacentTerritories = new List<Territory> { nunavut, alberta, california, florida, quebec, greenland };
            quebec.AdjacentTerritories = new List<Territory> { greenland, ontario, florida };
            california.AdjacentTerritories = new List<Territory> { alberta, ontario, florida, mexico };
            florida.AdjacentTerritories = new List<Territory> { mexico, california, ontario, quebec };
            mexico.AdjacentTerritories = new List<Territory> { california, florida, venezuela };
            // SOUTH AMERICA
            venezuela.AdjacentTerritories = new List<Territory> { mexico, brazil, peru };
            brazil.AdjacentTerritories = new List<Territory> { algeria, venezuela, peru, argentina };
            peru.AdjacentTerritories = new List<Territory> { venezuela, brazil, argentina };
            argentina.AdjacentTerritories = new List<Territory> { peru, brazil };
            // AFRICA
            algeria.AdjacentTerritories = new List<Territory> { brazil, iberia, rome, egypt, ethiopia, congo };
            egypt.AdjacentTerritories = new List<Territory> { rome, middleeast, ethiopia, algeria };
            ethiopia.AdjacentTerritories = new List<Territory> { egypt, middleeast, madagascar, congo, algeria };
            congo.AdjacentTerritories = new List<Territory> { algeria, ethiopia, southafrica };
            southafrica.AdjacentTerritories = new List<Territory> { madagascar, ethiopia, congo };
            // EUROPE
            iceland.AdjacentTerritories = new List<Territory> { greenland, britain, scandinavia };
            scandinavia.AdjacentTerritories = new List<Territory> { iceland, prussia, russia, britain };
            britain.AdjacentTerritories = new List<Territory> { iceland, scandinavia, prussia, iberia };
            prussia.AdjacentTerritories = new List<Territory> { scandinavia, russia, rome, iberia, britain, scandinavia };
            russia.AdjacentTerritories = new List<Territory> { scandinavia, prussia, rome, middleeast, afghanistan, ural };
            iberia.AdjacentTerritories = new List<Territory> { britain, prussia, rome, algeria };
            rome.AdjacentTerritories = new List<Territory> { iberia, prussia, russia, middleeast, algeria, egypt };
            // ASIA
            middleeast.AdjacentTerritories = new List<Territory> { russia, afghanistan, india, ethiopia, egypt, rome };
            india.AdjacentTerritories = new List<Territory> { middleeast, afghanistan, china, siam };
            siam.AdjacentTerritories = new List<Territory> { india, china, indonesia };
            afghanistan.AdjacentTerritories = new List<Territory> { russia, ural, china, india, middleeast };
            china.AdjacentTerritories = new List<Territory> { siam, india, afghanistan, ural, siberia, mongolia };
            ural.AdjacentTerritories = new List<Territory> { russia, afghanistan, china, siberia };
            siberia.AdjacentTerritories = new List<Territory> { ural, china, mongolia, irkutsk, yakutsk };
            irkutsk.AdjacentTerritories = new List<Territory> { siberia, yakutsk, kamchatka, mongolia };
            mongolia.AdjacentTerritories = new List<Territory> { china, siberia, irkutsk, kamchatka, japan };
            japan.AdjacentTerritories = new List<Territory> { mongolia, kamchatka };
            yakutsk.AdjacentTerritories = new List<Territory> { siberia, irkutsk, kamchatka };
            kamchatka.AdjacentTerritories = new List<Territory> { alaska, yakutsk, irkutsk, mongolia };
            // AUSTRALIA
            indonesia.AdjacentTerritories = new List<Territory> { siam, newguinea, perth };
            newguinea.AdjacentTerritories = new List<Territory> { indonesia, perth, sydney };
            perth.AdjacentTerritories = new List<Territory> { indonesia, newguinea, sydney };
            sydney.AdjacentTerritories = new List<Territory> { perth, newguinea };

            // LISTS
            continents = new List<Continent> { northamerica, southamerica, europe, asia, africa, australia };
            territories = new List<Territory> { alaska, nunavut, greenland, alberta, ontario, quebec, california, florida, mexico, venezuela, peru, brazil, argentina, algeria, egypt, ethiopia, congo, southafrica, madagascar, iceland, scandinavia, britain, prussia, iberia, rome, russia, ural, siberia, yakutsk, kamchatka, irkutsk, mongolia, japan, afghanistan, china, middleeast, india, siam, indonesia, newguinea, perth, sydney };
        }

        public bool OwnsContinent(Player p, Continent c)
        {
            foreach (Territory i in c.Territories)
            {
                if (i.Owner != p) return false;
            }
            return true;
        }
    }
}
