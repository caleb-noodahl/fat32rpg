using MerchantRPG.Models.Engine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameplayLoopCombat1.classes
{
    public class Equipment : DSI
    {
        public string Description { get; set; }
        public Guid ID { get; set; } = new Guid();
        public Equipment(int dex, int str, int intel, string description)
        {
            Dexterity = dex;
            Strength = str;
            Intelligence = intel;
            Description = description;
        }

        public Equipment(ItemType item)
        {
            Random rand = new Random();
            Dexterity = rand.Next(0, Party.Lead.AbilityLevel / 10);
            Strength = rand.Next(0, Party.Lead.AbilityLevel / 10);
            Intelligence = rand.Next(0, Party.Lead.AbilityLevel / 10);

            if (AbilityLevel == 0)
            {
                int pityPoint = rand.Next(0, 3);
                switch (pityPoint)
                {
                    case 0:
                        Dexterity++;
                        break;
                    case 1:
                        Strength++;
                        break;
                    case 2:
                        Intelligence++;
                        break;
                }
            }

            int attrRand = rand.Next(0, 100);
            if (attrRand < 11)
            {
                int negDexRand = rand.Next(0, 100);
                if (negDexRand > 50)
                    Strength += (int)Math.Ceiling(((double)Dexterity / 2));
                else
                    Intelligence += (int)Math.Ceiling(((double)Dexterity / 2));

                Dexterity *= -1;
            }
            else if (attrRand < 22)
            {
                int negStrRand = rand.Next(0, 100);
                if (negStrRand > 50)
                    Dexterity += (int)Math.Ceiling(((double)Strength / 2));
                else
                    Intelligence += (int)Math.Ceiling(((double)Strength / 2));

                Strength *= -1;
            }
            else if (attrRand < 33)
            {
                int negIntRand = rand.Next(0, 100);
                if (negIntRand > 50)
                    Dexterity += (int)Math.Ceiling(((double)Intelligence / 2));
                else
                    Strength += (int)Math.Ceiling(((double)Intelligence / 2));

                Intelligence *= -1;
            }

            string adj = Names.Adjective(rand.Next(0, Enum.GetValues(typeof(Names.Adjectives)).Length));
            if (AbilityLevel < 0)
                adj = "Cursed " + adj;

            switch (item)
            {
                case ItemType.Armor:
                    Description = adj + " " + Names.Metals[rand.Next(0, Names.Metals.Length)] + " " + Names.Armor[rand.Next(0, Names.Armor.Length)] + " of " + PrimaryStat;
                    break;
                case ItemType.Weapons:
                    Description = adj + " " + Names.Metals[rand.Next(0, Names.Metals.Length)] + " " + Names.Weapons[rand.Next(0, Names.Weapons.Length)] + " of " + PrimaryStat;
                    break;
            }
                
        }

        public Equipment(InventoryItem ii)
        {
            this.Description = ii.Name;
            this.Strength = ii.Stat == Stats.Strength ? Strength + ii.StatModifier : this.Strength;
            this.Dexterity = ii.Stat == Stats.Dexterity ? Dexterity + ii.StatModifier : this.Dexterity;
            this.Intelligence = ii.Stat == Stats.Intelligencge ? ii.StatModifier : this.Intelligence;
        }

        public Equipment(Character owner)
        {
            Random rand = new Random();

            Dexterity = Convert.ToInt32(owner.Dexterity * (rand.Next(0, 20) / 100));
            Strength = Convert.ToInt32(owner.Strength * (rand.Next(0, 20) / 100));
            Intelligence = Convert.ToInt32(owner.Intelligence * (rand.Next(0, 20) / 100));

            if(AbilityLevel == 0)
            {
                int pityPoint = rand.Next(0, 3);
                switch(pityPoint)
                {
                    case 0:
                        Dexterity++;
                        break;
                    case 1:
                        Strength++;
                        break;
                    case 2:
                        Intelligence++;
                        break;
                }
            }

            int attrRand = rand.Next(0, 100);
            if (attrRand < 11)
            {
                int negDexRand = rand.Next(0, 100);
                if (negDexRand > 50)
                    Strength += (int)Math.Ceiling(((double)Dexterity / 2));
                else
                    Intelligence += (int)Math.Ceiling(((double)Dexterity / 2));

                Dexterity *= -1;
            }
            else if (attrRand < 22)
            {
                int negStrRand = rand.Next(0, 100);
                if (negStrRand > 50)
                    Dexterity += (int)Math.Ceiling(((double)Strength / 2));
                else
                    Intelligence += (int)Math.Ceiling(((double)Strength / 2));

                Strength *= -1;
            }
            else if (attrRand < 33)
            {
                int negIntRand = rand.Next(0, 100);
                if (negIntRand > 50)
                    Dexterity += (int)Math.Ceiling(((double)Intelligence / 2));
                else
                    Strength += (int)Math.Ceiling(((double)Intelligence / 2));

                Intelligence *= -1;
            }

            string adj = Names.Adjective(rand.Next(0, Enum.GetValues(typeof(Names.Adjectives)).Length));
            ItemType t = ItemType.Weapons;
            if(rand.Next(0, 2) == 0)
            {
                Description = adj + " " + Names.Metals[rand.Next(0, Names.Metals.Length)] + " " + Names.Weapons[rand.Next(0, Names.Weapons.Length)] + " of " + PrimaryStat;
                t = ItemType.Weapons;
            }
            else
            {
                Description = adj + " " + Names.Metals[rand.Next(0, Names.Metals.Length)] + " " + Names.Armor[rand.Next(0, Names.Armor.Length)] + " of " + PrimaryStat;
                t = ItemType.Armor;
            }

            if (AbilityLevel < 0)
                adj = "Cursed " + adj;



            Enum.TryParse(PrimaryStat, out Stats _Stat); ;
            

            if (Party.State.AddToInventory(
                new InventoryItem()
                {
                    Equip = this,
                    Name = Description,
                    Value = AbilityLevel * rand.Next(50, 100),
                    Rating = AbilityLevel,
                    Weight = (Math.Abs(AbilityLevel) + 1) * rand.Next(2, 10),
                    Type = t,
                    Stat = _Stat,
                    StatModifier = PrimaryVal
                }))
                DistributeEquipment(owner);


        }

        public void DistributeEquipment(Character owner, bool shop = false)
        {
            string opening = "You've found: " + Description;
            if (!shop)
                opening += " on " + owner.Name + " corpse.";
            Console.WriteLine(opening);
            Console.WriteLine("Dex: " + Dexterity + " - Str: " + Strength + " - Int: " + Intelligence);
            Console.WriteLine("Do you wish to equip it? y/n");
            string doEquip = Console.ReadLine();
            if(doEquip.ToLower() == "y")
            {
                Console.WriteLine("Equip to whom?");
                int characterIndex = 0;
                Character[] party = Party.Members.ToArray();
                foreach (Character player in party)
                {
                    Console.WriteLine( characterIndex + ". " + player.Name);
                    characterIndex++;
                }
                int choice = -1;
                while (choice > characterIndex || choice < 0)
                {
                    Int32.TryParse(Console.ReadLine(), out choice);
                }
                party[choice].AddEquipment(this);
                Console.WriteLine("You have equipped " + Description + " to " + party[choice].Name);
                Console.WriteLine("---continue---");
                Console.ReadLine();
            }
        }
    }

    public static class FindDrops
    {
        public static bool Search (Character[] participants)
        {
            //living player party ability level / killed things ability level
            var rand = new Random();
            double score = participants.Where(p => p.Health > 0 && p.Player).Sum(p => p.AbilityLevel) / participants.Where(p => p.Health <= 0 && !p.Player).Sum(p => p.AbilityLevel);
            int dropScore = rand.Next(0, 100);
            return (score * 100 > dropScore);
        }
    }
    
}
