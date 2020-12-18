using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameplayLoopCombat1.classes
{
    public class Combat
    {
        public Character[] Participants { get; set; }
        public int Turn = 0;
        public bool Flee = false;
        public bool GameOver = false;
        public Random rand = new Random();

        public Combat(List<Character> _participants, bool generateEnemies = true)
        {
            Participants = _participants.OrderByDescending(c => c.Dexterity).ToArray();
            if (generateEnemies)
                GenerateEnemies();

            Console.WriteLine("Combat!");
            while (true)
            {
                if (Flee)
                {
                    Console.WriteLine("You flee!");
                    return;
                }

                if (GameOver)
                {
                    Console.WriteLine("You lose!");
                    return;
                }

                if(Participants.Where(p => p.Player == false && p.Health > 0).Count() == 0)
                {
                    Console.WriteLine("VICTORY!");
                    if(FindDrops.Search(Participants))
                    {
                        foreach(Character enemy in Participants.Where(p => p.Health <= 0 && !p.Player))
                        {
                            new Equipment(enemy);
                        }
                    }
                    return;
                }
                    
                NewTurn();
            }
            
        }

        public void NewTurn()
        {
            Turn++;
            for (int subturn = 0; subturn < Participants.Length; subturn++)
            {
                if(Participants[subturn].Health > 0)
                {
                    Console.WriteLine("Turn " + Turn + "." + subturn + " - " + Participants[subturn].Name);
                    IEnumerable<KeyValuePair<string, Ability>> castable = Abilities.AbilityList.Where(entry => entry.Value.MeetsRequirements(Participants[subturn]));
                    if (Participants[subturn].Player)
                        PlayerTurn(Participants[subturn], castable);
                    else
                    {
                        int npcChoice = rand.Next(0, castable.Count());
                        KeyValuePair<string, Ability>[] castableArr = castable.ToArray();
                        List<Character> playerTargets = Participants.Where(chars => chars.Player == true).ToList();

                        while (castableArr[npcChoice].Value.MaxTargets > playerTargets.Count())
                            playerTargets.Add(playerTargets.First());

                        AbilityResponse actionResult = castableArr[npcChoice].Value.Action(Participants[subturn], playerTargets.ToArray());
                        Console.WriteLine(Participants[subturn].Name + " uses " + castableArr[npcChoice].Key);
                        Console.WriteLine(actionResult.Message);
                    }

                    if (Flee)
                        return;
                    if(Participants.Where(p => p.Player && p.Health > 0).Count() == 0)
                    {
                        GameOver = true;
                        return;
                    }
                    Console.WriteLine("---continue---");
                    Console.ReadLine();
                }
            }
            
        }

        public void PlayerTurn(Character player, IEnumerable<KeyValuePair<string, Ability>> castable)
        {
            KeyValuePair<string, Ability>[] castableArr = castable.ToArray();
            int option = 0;
            Console.WriteLine("Your Turn! Type the number of your selection");
            Console.WriteLine(option + ". Flee!");
            foreach(KeyValuePair<string, Ability> ability in castableArr)
            {
                option++;
                Console.WriteLine(option + "." + ability.Key + " - " + ability.Value.Description);
            }

            int choice = -1;
            while(choice > option || choice < 0) 
            {
                Int32.TryParse(Console.ReadLine(), out choice);
            }

            if(choice == 0)
            {
                Flee = true;
                return;
            }

            Console.WriteLine("Who would you like to target with " + castableArr[choice - 1].Key + "?");
            int targetOption = 0;
            foreach (Character character in Participants)
            {
                
                Console.WriteLine(targetOption + "." + character.Name + " - Health: " + character.Health);
                targetOption++;
            }

            int targetChoice = -1;
            List<Character> targets = new List<Character>();
            while (targetChoice > targetOption || targetChoice < 0 || targets.Count() < castableArr[choice - 1].Value.MaxTargets)
            {
                Int32.TryParse(Console.ReadLine(), out targetChoice);
                if(targetChoice < targetOption && targetChoice >= 0)
                {
                    targets.Add(Participants[targetChoice]);
                    Console.WriteLine("Added target " + Participants[targetChoice].Name);
                }
            }

            AbilityResponse actionResult = castableArr[choice - 1].Value.Action(player, targets.ToArray());
            Console.WriteLine(actionResult.Message);
        }

        public void GenerateEnemies()
        {
            IEnumerable<Character> players = Participants.Where(p => p.Player);
            int ability = players.Sum(p => p.AbilityLevel);
            int enemyAbility = (Int32)(Party.Difficulty* ability);
            int enemyCount = rand.Next(1, players.Count()+2);

            List<Character> participantsL = Participants.ToList();

            for(int n = 0; enemyCount > n; n++)
            {
                int budget = enemyAbility / enemyCount;
                int statPick = rand.Next(0, 3);
                int str = (Int32)Math.Ceiling(budget * 0.3); //30% to str
                int dex = 0;
                int intel = 0;
                switch(statPick)
                {
                    case 0:
                        dex = (Int32)Math.Ceiling(budget * 0.5); //50% to main ability
                        str += (Int32)Math.Ceiling(budget * 0.1);
                        intel = (Int32)Math.Ceiling(budget * 0.1);
                        break;
                    case 1:
                        dex = (Int32)Math.Ceiling(budget * 0.1); 
                        str += (Int32)Math.Ceiling(budget * 0.5);//50% to main ability
                        intel = (Int32)Math.Ceiling(budget * 0.1);
                        break;
                    case 2:
                        dex = (Int32)Math.Ceiling(budget * 0.1);
                        str += (Int32)Math.Ceiling(budget * 0.1);
                        intel = (Int32)Math.Ceiling(budget * 0.5);//50% to main ability
                        break;

                }

                int adjChoice = rand.Next(0, Enum.GetValues(typeof(Adjectives)).Length);
                int aniChoice = rand.Next(0, Enum.GetValues(typeof(Animals)).Length);

                participantsL.Add(new Character((Adjectives)adjChoice + " " + (Animals)aniChoice, dex, str, intel));
                
            }

            Participants = participantsL.OrderByDescending(c => c.Dexterity).ToArray();

        }

        public enum Animals
        {
            Aardvark,Abyssinian,Adelie_Penguin,Affenpinscher,Afghan_Hound,African_Bush_Elephant,African_Civet,African_Clawed_Frog,African_Forest_Elephant,African_Palm_Civet,African_Penguin,African_Tree_Toad,African_Wild_Dog,Ainu_Dog,Airedale_Terrier,Akbash,Akita,Alaskan_Malamute,Albatross,Aldabra_Giant_Tortoise,Alligator,Alpine_Dachsbracke,American_Bulldog,American_Cocker_Spaniel,American_Coonhound,American_Eskimo_Dog,American_Foxhound,American_Pit_Bull_Terrier,American_Staffordshire_Terrier,American_Water_Spaniel,Anatolian_Shepherd_Dog,Angelfish,Ant,Anteater,Antelope,Appenzeller_Dog,Arctic_Fox,Arctic_Hare,Arctic_Wolf,Armadillo,Asian_Elephant,Asian_Giant_Hornet,Asian_Palm_Civet,Asiatic_Black_Bear,Australian_Cattle_Dog,Australian_Kelpie_Dog,Australian_Mist,Australian_Shepherd,Australian_Terrier,Avocet,Axolotl,Aye_Aye,Baboon,Bactrian_Camel,Badger,Balinese,Banded_Palm_Civet,Bandicoot,Barb,Barn_Owl,Barnacle,Barracuda,Basenji_Dog,Basking_Shark,Basset_Hound,Bat,Bavarian_Mountain_Hound,Beagle,Bear,Bearded_Collie,Bearded_Dragon,Beaver,Bedlington_Terrier,Beetle,Bengal_Tiger,Bernese_Mountain_Dog,Bichon_Frise,Binturong,Bird,Birds_Of_Paradise,Birman,Bison,Black_Bear,Black_Rhinoceros,Black_Russian_Terrier,Black_Widow_Spider,Bloodhound,Blue_Lacy_Dog,Blue_Whale,Bluetick_Coonhound,Bobcat,Bolognese_Dog,Bombay,Bongo,Bonobo,Booby,Border_Collie,Border_Terrier,Bornean_Orangutan,Borneo_Elephant,Boston_Terrier,Bottle_Nosed_Dolphin,Boxer_Dog,Boykin_Spaniel,Brazilian_Terrier,Brown_Bear,Budgerigar,Buffalo,Bull_Mastiff,Bull_Shark,Bull_Terrier,Bulldog,Bullfrog,Bumble_Bee,Burmese,Burrowing_Frog,Butterfly,Butterfly_Fish,Caiman,Caiman_Lizard,Cairn_Terrier,Camel,Canaan_Dog,Capybara,Caracal,Carolina_Dog,Cassowary,Cat,Caterpillar,Catfish,Cavalier_King_Charles_Spaniel,Centipede,Cesky_Fousek,Chameleon,Chamois,Cheetah,Chesapeake_Bay_Retriever,Chicken,Chihuahua,Chimpanzee,Chinchilla,Chinese_Crested_Dog,Chinook,Chinstrap_Penguin,Chipmunk,Chow_Chow,Cichlid,Clouded_Leopard,Clown_Fish,Clumber_Spaniel,Coati,Cockroach,Collared_Peccary,Collie,Common_Buzzard,Common_Frog,Common_Loon,Common_Toad,Coral,Cottontop_Tamarin,Cougar,Cow,Coyote,Crab,CrabEating_Macaque,Crane,Crested_Penguin,Crocodile,Cross_River_Gorilla,Curly_Coated_Retriever,Cuscus,Cuttlefish,Dachshund,Dalmatian,Darwins_Frog,Deer,Desert_Tortoise,Deutsche_Bracke,Dhole,Dingo,Discus,Doberman_Pinscher,Dodo,Dog,Dogo_Argentino,Dogue_De_Bordeaux,Dolphin,Donkey,Dormouse,Dragonfly,Drever,Duck,Dugong,Dunker,Dusky_Dolphin,Dwarf_Crocodile,Eagle,Earwig,Eastern_Gorilla,Eastern_Lowland_Gorilla,Echidna,Edible_Frog,Egyptian_Mau,Electric_Eel,Elephant,Elephant_Seal,Elephant_Shrew,Emperor_Penguin,Emperor_Tamarin,Emu,English_Cocker_Spaniel,English_Shepherd,English_Springer_Spaniel,Entlebucher_Mountain_Dog,Epagneul_Pont_Audemer,Eskimo_Dog,Estrela_Mountain_Dog,Falcon,Fennec_Fox,Ferret,Field_Spaniel,Fin_Whale,Finnish_Spitz,FireBellied_Toad,Fish,Fishing_Cat,Flamingo,Flat_Coat_Retriever,Flounder,Fly,Flying_Squirrel,Fossa,Fox,Fox_Terrier,French_Bulldog,Frigatebird,Frilled_Lizard,Frog,Fur_Seal,Galapagos_Penguin,Galapagos_Tortoise,Gar,Gecko,Gentoo_Penguin,Geoffroys_Tamarin,Gerbil,German_Pinscher,German_Shepherd,Gharial,Giant_African_Land_Snail,Giant_Clam,Giant_Panda_Bear,Giant_Schnauzer,Gibbon,Gila_Monster,Giraffe,Glass_Lizard,Glow_Worm,Goat,Golden_Lion_Tamarin,Golden_Oriole,Golden_Retriever,Goose,Gopher,Gorilla,Grasshopper,Great_Dane,Great_White_Shark,Greater_Swiss_Mountain_Dog,Green_BeeEater,Greenland_Dog,Grey_Mouse_Lemur,Grey_Reef_Shark,Grey_Seal,Greyhound,Grizzly_Bear,Grouse,Guinea_Fowl,Guinea_Pig,Guppy,Hammerhead_Shark,Hamster,Hare,Harrier,Havanese,Hedgehog,Hercules_Beetle,Hermit_Crab,Heron,Highland_Cattle,Himalayan,Hippopotamus,Honey_Bee,Horn_Shark,Horned_Frog,Horse,Horseshoe_Crab,Howler_Monkey,Human,Humboldt_Penguin,Hummingbird,Humpback_Whale,Hyena,Ibis,Ibizan_Hound,Iguana,Impala,Indian_Elephant,Indian_Palm_Squirrel,Indian_Rhinoceros,Indian_Star_Tortoise,Indochinese_Tiger,Indri,Insect,Irish_Setter,Irish_WolfHound,Jack_Russel,Jackal,Jaguar,Japanese_Chin,Japanese_Macaque,Javan_Rhinoceros,Javanese,Jellyfish,Kakapo,Kangaroo,Keel_Billed_Toucan,Killer_Whale,King_Crab,King_Penguin,Kingfisher,Kiwi,Koala,Komodo_Dragon,Kudu,Labradoodle,Labrador_Retriever,Ladybird,LeafTailed_Gecko,Lemming,Lemur,Leopard,Leopard_Cat,Leopard_Seal,Leopard_Tortoise,Liger,Lion,Lionfish,Little_Penguin,Lizard,Llama,Lobster,LongEared_Owl,Lynx,Macaroni_Penguin,Macaw,Magellanic_Penguin,Magpie,Maine_Coon,Malayan_Civet,Malayan_Tiger,Maltese,Manatee,Mandrill,Manta_Ray,Marine_Toad,Markhor,Marsh_Frog,Masked_Palm_Civet,Mastiff,Mayfly,Meerkat,Millipede,Minke_Whale,Mole,Molly,Mongoose,Mongrel,Monitor_Lizard,Monkey,Monte_Iberia_Eleuth,Moorhen,Moose,Moray_Eel,Moth,Mountain_Gorilla,Mountain_Lion,Mouse,Mule,Neanderthal,Neapolitan_Mastiff,Newfoundland,Newt,Nightingale,Norfolk_Terrier,Norwegian_Forest,Numbat,Nurse_Shark,Ocelot,Octopus,Okapi,Old_English_Sheepdog,Olm,Opossum,Orangutan,Ostrich,Otter,Oyster,Pademelon,Panther,Parrot,Patas_Monkey,Peacock,Pekingese,Pelican,Penguin,Persian,Pheasant,Pied_Tamarin,Pig,Pika,Pike,Pink_Fairy_Armadillo,Piranha,Platypus,Pointer,Poison_Dart_Frog,Polar_Bear,Pond_Skater,Poodle,Pool_Frog,Porcupine,Possum,Prawn,Proboscis_Monkey,Puffer_Fish,Puffin,Pug,Puma,Purple_Emperor,Puss_Moth,Pygmy_Hippopotamus,Pygmy_Marmoset,Quail,Quetzal,Quokka,Quoll,Rabbit,Raccoon,Raccoon_Dog,Radiated_Tortoise,Ragdoll,Rat,Rattlesnake,Red_Knee_Tarantula,Red_Panda,Red_Wolf,Redhanded_Tamarin,Reindeer,Rhinoceros,River_Dolphin,River_Turtle,Robin,Rock_Hyrax,Rockhopper_Penguin,Roseate_Spoonbill,Rottweiler,Royal_Penguin,Russian_Blue,SabreToothed_Tiger,Saint_Bernard,Salamander,Sand_Lizard,Saola,Scorpion,Scorpion_Fish,Sea_Dragon,Sea_Lion,Sea_Otter,Sea_Slug,Sea_Squirt,Sea_Turtle,Sea_Urchin,Seahorse,Seal,Serval,Sheep,Shih_Tzu,Shrimp,Siamese,Siamese_Fighting_Fish,Siberian,Siberian_Husky,Siberian_Tiger,Silver_Dollar,Skunk,Sloth,Slow_Worm,Snail,Snake,Snapping_Turtle,Snowshoe,Snowy_Owl,Somali,South_China_Tiger,Spadefoot_Toad,Sparrow,Spectacled_Bear,Sperm_Whale,Spider_Monkey,Spiny_Dogfish,Sponge,Squid,Squirrel,Squirrel_Monkey,Sri_Lankan_Elephant,Staffordshire_Bull_Terrier,Stag_Beetle,Starfish,Stellers_Sea_Cow,Stick_Insect,Stingray,Stoat,Striped_Rocket_Frog,Sumatran_Elephant,Sumatran_Orangutan,Sumatran_Rhinoceros,Sumatran_Tiger,Sun_Bear,Swan,Tang,Tapanuli_Orangutan,Tapir,Tarsier,Tasmanian_Devil,Tawny_Owl,Termite,Tetra,Thorny_Devil,Tibetan_Mastiff,Tiffany,Tiger,Tiger_Salamander,Tiger_Shark,Tortoise,Toucan,Tree_Frog,Tropicbird,Tuatara,Turkey,Turkish_Angora,Uakari,Uguisu,Umbrellabird,Vampire_Bat,Vervet_Monkey,Vulture,Wallaby,Walrus,Warthog,Wasp,Water_Buffalo,Water_Dragon,Water_Vole,Weasel,Welsh_Corgi,West_Highland_Terrier,Western_Gorilla,Western_Lowland_Gorilla,Whale_Shark,Whippet,White_Faced_Capuchin,White_Rhinoceros,White_Tiger,Wild_Boar,Wildebeest,Wolf,Wolverine,Wombat,Woodlouse,Woodpecker,Woolly_Mammoth,Woolly_Monkey,Wrasse,XRay_Tetra,Yak,YellowEyed_Penguin,Yorkshire_Terrier,Zebra,Zebra_Shark,Zebu,Zonkey,Zorse
        }

        public enum Adjectives
        {
            Giant, Tiny, Red, Green, Yellow, Blue, Brown, Large, Small, Winged, Flying, Burrowing, Floating, Dim, Ugly, Ravenous, Murderous, Terrified, Hostile, Outraged
        }
    }
}
