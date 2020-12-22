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
            //DEX
            {"Quick Attack", new Ability(new Requirement(2,1,1,0), QuickAttack, 1, 0, 1, "does dex * 1 dmg to up to 1 target" )},
            {"MultiShot", new Ability(new Requirement(5,1,1,0), MultiShot, 5, 3, 0.5, "does dex * 0.5 dmg to up to 5 targets" )},
            {"Evasion", new Ability(new Requirement(7,1,1,0), Evasion, 1, 2, 2, "evade dex * 2% of attacks until the end of combabt" )},
            {"Evasive Action", new Ability(new Requirement(10,1,1,0), Evasion, 3, 3, 2, "3 targets evade dex * 2% of attacks until the end of combabt" )},            

            //STR
            {"Strong Attack", new Ability(new Requirement(1,2,1,0), StrongAttack, 1, 0, 1, "does str * 1 dmg to up to 1 target" )},
            {"Cleave", new Ability(new Requirement(1,5,1,0), Cleave, 3, 4, 0.7, "does str * 0.7 dmg to up to 3 targets" )},
            {"Bleed", new Ability(new Requirement(1,7,1,0), Bleed, 1, 2, 0.3, "does str * 0.3 dmg to 1 target for 6 turns" )},
            {"Cleave Bleed", new Ability(new Requirement(1,10,1,0), Bleed, 3, 4, 0.3, "does str * 0.3 dmg to 3 target for 6 turns" )},

            //INT
            {"Calculated Attack", new Ability(new Requirement(1,1,2,0), CalculatedAttack, 1, 0, 1, "does int * 1 dmg to up to 1 target" )},
            {"Fireball", new Ability(new Requirement(1,1,5,0), Fireball, 3, 4, 0.7, "does int * 0.7 dmg to up tp 3 targets")},
            {"Heal", new Ability(new Requirement(1,1,3,0), Heal, 1, 2, 1, "heals for int * 1 to 1 target")},
            {"Group Heal", new Ability(new Requirement(1,1,8,0), Heal, 3, 4, 1, "heals for int * 1 to 3 targets")},
            {"Greater Fireball", new Ability(new Requirement(1,1,10,0), Fireball, 5, 4, 1, "does int * 1 dmg to up tp 5 targets")},
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
                resp.Message = "The requirements for Heal are" + AbilityList[abilityName].PrintReqs();
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
