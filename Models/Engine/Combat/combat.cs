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
                int aniChoice = rand.Next(0, Animals.Length);

                participantsL.Add(new Character((Adjectives)adjChoice + " " + Animals[aniChoice], dex, str, intel));
                
            }

            Participants = participantsL.OrderByDescending(c => c.Dexterity).ToArray();

        }

        public string[] Animals = new string[]
        {
            "Aardvark","Abyssinian","Adelie Penguin","Affenpinscher","Afghan Hound","African Bush Elephant","African Civet","African Clawed Frog","African Forest Elephant","African Palm Civet","African Penguin","African Tree Toad","African Wild Dog","Ainu Dog","Airedale Terrier","Akbash","Akita","Alaskan Malamute","Albatross","Aldabra Giant Tortoise","Alligator","Alpine Dachsbracke","American Bulldog","American Cocker Spaniel","American Coonhound","American Eskimo Dog","American Foxhound","American Pit Bull Terrier","American Staffordshire Terrier","American Water Spaniel","Anatolian Shepherd Dog","Angelfish","Ant","Anteater","Antelope","Appenzeller Dog","Arctic Fox","Arctic Hare","Arctic Wolf","Armadillo","Asian Elephant","Asian Giant Hornet","Asian Palm Civet","Asiatic Black Bear","Australian Cattle Dog","Australian Kelpie Dog","Australian Mist","Australian Shepherd","Australian Terrier","Avocet","Axolotl","Aye Aye","Baboon","Bactrian Camel","Badger","Balinese","Banded Palm Civet","Bandicoot","Barb","Barn Owl","Barnacle","Barracuda","Basenji Dog","Basking Shark","Basset Hound","Bat","Bavarian Mountain Hound","Beagle","Bear","Bearded Collie","Bearded Dragon","Beaver","Bedlington Terrier","Beetle","Bengal Tiger","Bernese Mountain Dog","Bichon Frise","Binturong","Bird","Birds Of Paradise","Birman","Bison","Black Bear","Black Rhinoceros","Black Russian Terrier","Black Widow Spider","Bloodhound","Blue Lacy Dog","Blue Whale","Bluetick Coonhound","Bobcat","Bolognese Dog","Bombay","Bongo","Bonobo","Booby","Border Collie","Border Terrier","Bornean Orangutan","Borneo Elephant","Boston Terrier","Bottle Nosed Dolphin","Boxer Dog","Boykin Spaniel","Brazilian Terrier","Brown Bear","Budgerigar","Buffalo","Bull Mastiff","Bull Shark","Bull Terrier","Bulldog","Bullfrog","Bumble Bee","Burmese","Burrowing Frog","Butterfly","Butterfly Fish","Caiman","Caiman Lizard","Cairn Terrier","Camel","Canaan Dog","Capybara","Caracal","Carolina Dog","Cassowary","Cat","Caterpillar","Catfish","Cavalier King Charles Spaniel","Centipede","Cesky Fousek","Chameleon","Chamois","Cheetah","Chesapeake Bay Retriever","Chicken","Chihuahua","Chimpanzee","Chinchilla","Chinese Crested Dog","Chinook","Chinstrap Penguin","Chipmunk","Chow Chow","Cichlid","Clouded Leopard","Clown Fish","Clumber Spaniel","Coati","Cockroach","Collared Peccary","Collie","Common Buzzard","Common Frog","Common Loon","Common Toad","Coral","Cottontop Tamarin","Cougar","Cow","Coyote","Crab","CrabEating Macaque","Crane","Crested Penguin","Crocodile","Cross River Gorilla","Curly Coated Retriever","Cuscus","Cuttlefish","Dachshund","Dalmatian","Darwins Frog","Deer","Desert Tortoise","Deutsche Bracke","Dhole","Dingo","Discus","Doberman Pinscher","Dodo","Dog","Dogo Argentino","Dogue De Bordeaux","Dolphin","Donkey","Dormouse","Dragonfly","Drever","Duck","Dugong","Dunker","Dusky Dolphin","Dwarf Crocodile","Eagle","Earwig","Eastern Gorilla","Eastern Lowland Gorilla","Echidna","Edible Frog","Egyptian Mau","Electric Eel","Elephant","Elephant Seal","Elephant Shrew","Emperor Penguin","Emperor Tamarin","Emu","English Cocker Spaniel","English Shepherd","English Springer Spaniel","Entlebucher Mountain Dog","Epagneul Pont Audemer","Eskimo Dog","Estrela Mountain Dog","Falcon","Fennec Fox","Ferret","Field Spaniel","Fin Whale","Finnish Spitz","FireBellied Toad","Fish","Fishing Cat","Flamingo","Flat Coat Retriever","Flounder","Fly","Flying Squirrel","Fossa","Fox","Fox Terrier","French Bulldog","Frigatebird","Frilled Lizard","Frog","Fur Seal","Galapagos Penguin","Galapagos Tortoise","Gar","Gecko","Gentoo Penguin","Geoffroys Tamarin","Gerbil","German Pinscher","German Shepherd","Gharial","Giant African Land Snail","Giant Clam","Giant Panda Bear","Giant Schnauzer","Gibbon","Gila Monster","Giraffe","Glass Lizard","Glow Worm","Goat","Golden Lion Tamarin","Golden Oriole","Golden Retriever","Goose","Gopher","Gorilla","Grasshopper","Great Dane","Great White Shark","Greater Swiss Mountain Dog","Green BeeEater","Greenland Dog","Grey Mouse Lemur","Grey Reef Shark","Grey Seal","Greyhound","Grizzly Bear","Grouse","Guinea Fowl","Guinea Pig","Guppy","Hammerhead Shark","Hamster","Hare","Harrier","Havanese","Hedgehog","Hercules Beetle","Hermit Crab","Heron","Highland Cattle","Himalayan","Hippopotamus","Honey Bee","Horn Shark","Horned Frog","Horse","Horseshoe Crab","Howler Monkey","Human","Humboldt Penguin","Hummingbird","Humpback Whale","Hyena","Ibis","Ibizan Hound","Iguana","Impala","Indian Elephant","Indian Palm Squirrel","Indian Rhinoceros","Indian Star Tortoise","Indochinese Tiger","Indri","Insect","Irish Setter","Irish WolfHound","Jack Russel","Jackal","Jaguar","Japanese Chin","Japanese Macaque","Javan Rhinoceros","Javanese","Jellyfish","Kakapo","Kangaroo","Keel Billed Toucan","Killer Whale","King Crab","King Penguin","Kingfisher","Kiwi","Koala","Komodo Dragon","Kudu","Labradoodle","Labrador Retriever","Ladybird","LeafTailed Gecko","Lemming","Lemur","Leopard","Leopard Cat","Leopard Seal","Leopard Tortoise","Liger","Lion","Lionfish","Little Penguin","Lizard","Llama","Lobster","LongEared Owl","Lynx","Macaroni Penguin","Macaw","Magellanic Penguin","Magpie","Maine Coon","Malayan Civet","Malayan Tiger","Maltese","Manatee","Mandrill","Manta Ray","Marine Toad","Markhor","Marsh Frog","Masked Palm Civet","Mastiff","Mayfly","Meerkat","Millipede","Minke Whale","Mole","Molly","Mongoose","Mongrel","Monitor Lizard","Monkey","Monte Iberia Eleuth","Moorhen","Moose","Moray Eel","Moth","Mountain Gorilla","Mountain Lion","Mouse","Mule","Neanderthal","Neapolitan Mastiff","Newfoundland","Newt","Nightingale","Norfolk Terrier","Norwegian Forest","Numbat","Nurse Shark","Ocelot","Octopus","Okapi","Old English Sheepdog","Olm","Opossum","Orangutan","Ostrich","Otter","Oyster","Pademelon","Panther","Parrot","Patas Monkey","Peacock","Pekingese","Pelican","Penguin","Persian","Pheasant","Pied Tamarin","Pig","Pika","Pike","Pink Fairy Armadillo","Piranha","Platypus","Pointer","Poison Dart Frog","Polar Bear","Pond Skater","Poodle","Pool Frog","Porcupine","Possum","Prawn","Proboscis Monkey","Puffer Fish","Puffin","Pug","Puma","Purple Emperor","Puss Moth","Pygmy Hippopotamus","Pygmy Marmoset","Quail","Quetzal","Quokka","Quoll","Rabbit","Raccoon","Raccoon Dog","Radiated Tortoise","Ragdoll","Rat","Rattlesnake","Red Knee Tarantula","Red Panda","Red Wolf","Redhanded Tamarin","Reindeer","Rhinoceros","River Dolphin","River Turtle","Robin","Rock Hyrax","Rockhopper Penguin","Roseate Spoonbill","Rottweiler","Royal Penguin","Russian Blue","SabreToothed Tiger","Saint Bernard","Salamander","Sand Lizard","Saola","Scorpion","Scorpion Fish","Sea Dragon","Sea Lion","Sea Otter","Sea Slug","Sea Squirt","Sea Turtle","Sea Urchin","Seahorse","Seal","Serval","Sheep","Shih Tzu","Shrimp","Siamese","Siamese Fighting Fish","Siberian","Siberian Husky","Siberian Tiger","Silver Dollar","Skunk","Sloth","Slow Worm","Snail","Snake","Snapping Turtle","Snowshoe","Snowy Owl","Somali","South China Tiger","Spadefoot Toad","Sparrow","Spectacled Bear","Sperm Whale","Spider Monkey","Spiny Dogfish","Sponge","Squid","Squirrel","Squirrel Monkey","Sri Lankan Elephant","Staffordshire Bull Terrier","Stag Beetle","Starfish","Stellers Sea Cow","Stick Insect","Stingray","Stoat","Striped Rocket Frog","Sumatran Elephant","Sumatran Orangutan","Sumatran Rhinoceros","Sumatran Tiger","Sun Bear","Swan","Tang","Tapanuli Orangutan","Tapir","Tarsier","Tasmanian Devil","Tawny Owl","Termite","Tetra","Thorny Devil","Tibetan Mastiff","Tiffany","Tiger","Tiger Salamander","Tiger Shark","Tortoise","Toucan","Tree Frog","Tropicbird","Tuatara","Turkey","Turkish Angora","Uakari","Uguisu","Umbrellabird","Vampire Bat","Vervet Monkey","Vulture","Wallaby","Walrus","Warthog","Wasp","Water Buffalo","Water Dragon","Water Vole","Weasel","Welsh Corgi","West Highland Terrier","Western Gorilla","Western Lowland Gorilla","Whale Shark","Whippet","White Faced Capuchin","White Rhinoceros","White Tiger","Wild Boar","Wildebeest","Wolf","Wolverine","Wombat","Woodlouse","Woodpecker","Woolly Mammoth","Woolly Monkey","Wrasse","XRay Tetra","Yak","YellowEyed Penguin","Yorkshire Terrier","Zebra","Zebra Shark","Zebu","Zonkey","Zorse"
        };

        public enum Adjectives
        {
            Giant, Tiny, Red, Green, Yellow, Blue, Brown, Large, Small, Winged, Flying, Burrowing, Floating, Dim, Ugly, Ravenous, Murderous, Terrified, Hostile, Outraged
        }
    }
}
