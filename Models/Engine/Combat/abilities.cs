using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MerchantRPG.Models.Engine.Combat;

#region Notes
/*
 * To make a new ability add an entry to the ability list and an ability action of type AbilityResponse name(Character caster, Character[] targets)
 */
#endregion

namespace GameplayLoopCombat1.classes
{
    public static class Abilities
    {
        public static Dictionary<string, Ability> AbilityList = new Dictionary<string, Ability>()
        {
            {"Wait", new Ability(new Requirement(0,0,0,0), (a, b, c, d) => { return new AbilityResponse(); }, 0, 0, 0, "do nothing for your turn" )},

            //DEX
            {"Quick Attack", new Ability(new Requirement(2,0,0,0), QuickAttack, 1, 0, 1, "does dex * 1 dmg to up to 1 target" )},
            {"MultiShot", new Ability(new Requirement(5,0,0,0), MultiShot, 5, 3, 0.5, "does dex * 0.5 dmg to up to 5 targets" )},
            {"Throw Sand", new Ability(new Requirement(15,5,0,0), ThrowSand, 1, 3, 3, "increase the damage taken by 1 target by dex * 5% for 2 turns" )},
            {"Evasion", new Ability(new Requirement(7,0,0,0), Evasion, 1, 2, 2, "evade dex * 2% of attacks until the end of combabt" )},
            {"Evasive Action", new Ability(new Requirement(10,0,0,0), Evasion, 3, 3, 2, "3 targets evade dex * 2% of attacks until the end of combat" )},
            {"Achille's Heel", new Ability(new Requirement(12,0,0,0), DexterityAttack, 1, 5, 3, "does dex * 3 dmg to 1 target" )},
            {"Assassinate", new Ability(new Requirement(15,5,0,0), Assassinate, 1, 7, 1, "instantly kill any target with lower health and level than you" )},
            

            //STR
            {"Strong Attack", new Ability(new Requirement(0,2,0,0), StrongAttack, 1, 0, 1, "does str * 1 dmg to 1 target" )},
            {"Cleave", new Ability(new Requirement(0,5,0,0), Cleave, 3, 4, 0.7, "does str * 0.7 dmg to 3 targets" )},
            {"Bleed", new Ability(new Requirement(0,7,0,0), Bleed, 1, 2, 0.3, "does str * 0.3 dmg to 1 target for 6 turns" )},
            {"Block", new Ability(new Requirement(0,7,0,0), Block, 1, 0, 0.05, "use your body to block str * 5% of incoming damage to a target for 1 turn" )},
            {"Cleave Bleed", new Ability(new Requirement(0,10,0,0), Bleed, 3, 4, 0.3, "does str * 0.3 dmg to 3 target for 6 turns" )},
            {"Throw Enemy", new Ability(new Requirement(5,10,0,0), ThrowEnemy, 2, 4, 2, "throw one enemy at another, damaging both for str * 2 + dex" )},
            {"Overpower", new Ability(new Requirement(15,0,0,0), StrengthAttack, 1, 6, 3, "does str * 3 dmg to 1 targets" )},

            //INT
            {"Calculated Attack", new Ability(new Requirement(0,0,2,0), CalculatedAttack, 1, 0, 1, "does int * 1 dmg to 1 target" )},
            {"Fireball", new Ability(new Requirement(0,0,5,0), Fireball, 3, 4, 0.7, "does int * 0.7 dmg to 3 targets")},
            {"Heal", new Ability(new Requirement(0,0,3,0), Heal, 1, 2, 1, "heals for int * 1 to 1 target")},
            {"Group Heal", new Ability(new Requirement(0,0,8,0), Heal, 3, 4, 1, "heals for int * 1 to 3 targets")},
            {"Greater Fireball", new Ability(new Requirement(0,0,10,0), Fireball, 5, 4, 1, "does int * 1 dmg to 5 targets")},
            {"Greater Heal", new Ability(new Requirement(0,0,12,0), Heal, 1, 2, 2, "heals for int * 2 to 1 target")},
            {"Group Minor Stimulant", new Ability(new Requirement(3,3,10,0), Stimulant, 3, 2, 1, "buff 3 targets for +2 to all stats for 3 turns")},
            {"Stimulant", new Ability(new Requirement(3,3,10,0), Stimulant, 1, 2, 5, "buff 1 targets for +5 to all stats for 3 turns")},
            {"Lightning Strike", new Ability(new Requirement(0,0,11,0), IntelligenceAttack, 1, 4, 2, "does int * 2 dmg to 1 targets")},
        };

        #region Ability Actions

        //DEX
        public static AbilityResponse QuickAttack(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for Quick Attack are" + AbilityList["Quick Attack"].PrintReqs();
                return resp;
            }

            //Ability Actions
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double dmg = caster.Dexterity * AbilityList[abilityName].DmgMod;
                dmg = targets[tCount].DoDamage(dmg);
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        public static AbilityResponse MultiShot(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList["MultiShot"].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for MultiShot are" + AbilityList["MultiShot"].PrintReqs();
                return resp;
            }

            //Ability Actions
            int targetCount = targets.Length;
            if (targetCount > AbilityList["MultiShot"].MaxTargets)
                targetCount = AbilityList["MultiShot"].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double dmg = caster.Dexterity * AbilityList[abilityName].DmgMod;
                dmg = targets[tCount].DoDamage(dmg);
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        public static AbilityResponse Evasion(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for Evasion are" + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double evasion = caster.Dexterity * AbilityList[abilityName].DmgMod;
                StatusEffect status = new StatusEffect(abilityName, "Evading " + evasion + "% of attacks")
                {
                    MissPercent = evasion
                };
                
                targets[tCount].Effects.Add(status);
                resp.Message += " " + targets[tCount].Name + " is evading " + evasion + "% of attacks";
            }

            return resp;
        }

        public static AbilityResponse ThrowSand(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for " + abilityName + "  are" + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double dmgMod = caster.Dexterity * AbilityList[abilityName].DmgMod;
                StatusEffect status = new StatusEffect(abilityName, "Increases damage taken by " + dmgMod + "%")
                {
                     DamageModifier = dmgMod,
                     Turns = 2
                };

                targets[tCount].Effects.Add(status);
                resp.Message += " " + targets[tCount].Name + " is taking " + dmgMod + "% extra damage";
            }

            return resp;
        }

        public static AbilityResponse DexterityAttack(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for " + abilityName + " are " + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double dmg = caster.Dexterity * AbilityList[abilityName].DmgMod;
                dmg = targets[tCount].DoDamage(dmg);
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        public static AbilityResponse Assassinate(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList["Assassinate"].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for Assassinate are" + AbilityList["Assassinate"].PrintReqs();
                return resp;
            }

            //Ability Actions
            if(targets[0].Health > caster.Health || targets[0].AbilityLevel > caster.AbilityLevel)
            {
                resp.Message = targets[0].Name + " is more powerful and vital than you, your assassination attempt fails."; 
                return resp;
            }
            else
            {
                resp.Message = targets[0].Name + " falls to the ground, dead before " + caster.Name + "'s feet";
                targets[0].Health = 0;
                return resp;
            }

        }

        //STR
        public static AbilityResponse StrongAttack(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for Strong Attack are" + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions 
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double dmg = caster.Strength * AbilityList[abilityName].DmgMod;
                dmg = targets[tCount].DoDamage(dmg);
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        public static AbilityResponse StrengthAttack(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for " + abilityName + " are " + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double dmg = caster.Strength * AbilityList[abilityName].DmgMod;
                dmg = targets[tCount].DoDamage(dmg);
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        public static AbilityResponse Cleave(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for Cleave are" + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double dmg = caster.Strength * AbilityList[abilityName].DmgMod;
                dmg = targets[tCount].DoDamage(dmg);
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        public static AbilityResponse Bleed(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for Bleed are" + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double dmg = caster.Strength * AbilityList[abilityName].DmgMod;
                StatusEffect status = new StatusEffect(abilityName, "Bleed for " + dmg + " damage per turn")
                {
                    EffectAction = c => 
                    {
                        Console.WriteLine(c.Name + " bleeds for " + dmg);
                        c.DoDamage(dmg); 
                    }
                };

                targets[tCount].Effects.Add(status);
                resp.Message += " " + targets[tCount].Name + " is bleeding for " + dmg + " damage per turn";
            }

            return resp;
        }

        public static AbilityResponse Block(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for " + abilityName + " are" + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double block = caster.Strength * AbilityList[abilityName].DmgMod;
                if(block > 0.75)
                {
                    block = 0.75;
                }
                StatusEffect status = new StatusEffect(abilityName, "Blocking " + block + "% of incoming damage")
                {
                    DamageModifier = 1 - block,
                    Turns = 2
                };

                targets[tCount].Effects.Add(status);
                resp.Message += " " + targets[tCount].Name + " is blocking " + (block * 100) + "% of incoming damage";
            }

            return resp;
        }

        public static AbilityResponse ThrowEnemy(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for " + abilityName + " are" + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double dmg = caster.Strength * AbilityList[abilityName].DmgMod + caster.Dexterity;
                dmg = targets[tCount].DoDamage(dmg);
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        //INT
        public static AbilityResponse CalculatedAttack(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for Calculated Attack are" + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions 
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double dmg = caster.Intelligence * AbilityList[abilityName].DmgMod;
                dmg = targets[tCount].DoDamage(dmg);
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        public static AbilityResponse IntelligenceAttack(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for " + abilityName + " are " + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double dmg = caster.Intelligence * AbilityList[abilityName].DmgMod;
                dmg = targets[tCount].DoDamage(dmg);
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        public static AbilityResponse Fireball(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if(!resp.Usable)
            {
                resp.Message = "The requirements for Fireball are" + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double dmg = caster.Intelligence * AbilityList[abilityName].DmgMod;
                dmg = targets[tCount].DoDamage(dmg);
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        public static AbilityResponse Heal(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for Heal are " + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions - does int * 1 heal to 1 target
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                double heal = caster.Intelligence;
                targets[tCount].DoDamage(heal * -1 * AbilityList[abilityName].DmgMod);
                if (targets[tCount].Health > targets[tCount].HealthMax)
                    targets[tCount].Health = targets[tCount].HealthMax;
                resp.Message += " " + targets[tCount].Name + " healed for " + heal;
            }

            return resp;
        }

        public static AbilityResponse Stimulant(Character caster, Character[] targets, int turn, string abilityName)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList[abilityName].MeetsRequirements(caster, turn);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for " + abilityName + " are " + AbilityList[abilityName].PrintReqs();
                return resp;
            }

            //Ability Actions
            int targetCount = targets.Length;
            if (targetCount > AbilityList[abilityName].MaxTargets)
                targetCount = AbilityList[abilityName].MaxTargets;
            for (int tCount = 0; tCount < targetCount; tCount++)
            {
                StatusEffect status = new StatusEffect(abilityName, "Buffed for +" + (int)AbilityList[abilityName].DmgMod + " to all stats")
                {
                    Dexterity = (int)AbilityList[abilityName].DmgMod,
                    Strength = (int)AbilityList[abilityName].DmgMod,
                    Intelligence = (int)AbilityList[abilityName].DmgMod,
                    Turns = 3
                };

                targets[tCount].Effects.Add(status);
                resp.Message += " " + targets[tCount].Name + " is buffed for +" + (int)AbilityList[abilityName].DmgMod + " to all stats";
            }

            return resp;
        }

        #endregion
    }

    public class Ability
    {
        public Requirement Reqs { get; set; }
        public delegate AbilityResponse AbilityAction(Character caster, Character[] targets, int turn, string abilityName);
        private AbilityAction _Action;

        public AbilityAction Action 
        { 
            get 
            {
                return _Action;
            } 
            set 
            {
                _Action = (caster, targets, turn, abilityName) => 
                { 
                    AbilityResponse result = value(caster, targets, turn, abilityName);
                    caster.AddXP(Reqs);
                    return result;
                };
            } 
        }
        public int MaxTargets { get; set; }
        public string Description { get; set; }
        public int Cooldown { get; set; }
        public double DmgMod { get; set; }

        public Dictionary<Character, int> LastCast { get; set; } = new Dictionary<Character, int>();

        public bool MeetsRequirements(Character caster, int turn = 0)
        {
            if (
                caster.Dexterity < Reqs.Dexterity ||
                caster.Strength < Reqs.Strength ||
                caster.Intelligence < Reqs.Intelligence ||
                caster.Health < Reqs.Health ||
                (LastCast.ContainsKey(caster) && LastCast[caster] + Cooldown > turn)
                )
                    return false;
            
            return true;
        }

        public string PrintReqs()
        {
            string print = "";
            foreach (PropertyInfo prop in Reqs.GetType().GetProperties())
            {
                print += " " + prop.Name + "=" + prop.GetValue(Reqs, null);
            }
            return print;
        }

        public Ability(Requirement _reqs, AbilityAction _action, int _maxTargets, int _cooldown, double _dmgMod, string _description)
        {
            Reqs = _reqs;
            Action = _action;
            MaxTargets = _maxTargets;
            Description = _description;
            Cooldown = _cooldown;
            DmgMod = _dmgMod;
        }

    }

    public class Requirement : DSI
    {
        public int Health { get; set; }

        public Requirement(int _dex, int _str, int _int, int _health)
        {
            Dexterity = _dex;
            Strength = _str;
            Intelligence = _int;
            Health = _health;
        }

    }

    public class AbilityResponse
    {
        public bool Usable { get; set; }
        public string Message { get; set; }
    }
    
}
