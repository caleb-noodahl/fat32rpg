using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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
            {"Quick Attack", new Ability(new Requirement(2,1,1,0), QuickAttack, 1, "does dex * 1 dmg to up to 1 target" )},
            {"MultiShot", new Ability(new Requirement(5,1,1,0), MultiShot, 5, "does dex * 0.5 dmg to up to 5 targets" )},

            //STR
            {"Strong Attack", new Ability(new Requirement(1,2,1,0), StrongAttack, 1, "does str * 1 dmg to up to 1 target" )},
            {"Cleave", new Ability(new Requirement(1,5,1,0), Cleave, 3, "does str * 0.7 dmg to up to 3 targets" )},

            //INT
            {"Calculated Attack", new Ability(new Requirement(1,1,2,0), CalculatedAttack, 1, "does int * 1 dmg to up to 1 target" )},
            {"Fireball", new Ability(new Requirement(1,1,5,0), Fireball, 3, "does int * 0.7 dmg to up tp 3 targets")},
            {"Heal", new Ability(new Requirement(1,1,3,0), Heal, 1, "heals for int * 1 to 1 target")}
        };

        #region Ability Actions

        //DEX
        public static AbilityResponse QuickAttack(Character caster, Character[] targets)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList["Quick Attack"].MeetsRequirements(caster);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for Quick Attack are" + AbilityList["Quick Attack"].PrintReqs();
                return resp;
            }

            //Ability Actions
            for (int tCount = 0; tCount < AbilityList["Quick Attack"].MaxTargets; tCount++)
            {
                double dmg = caster.Dexterity;
                targets[tCount].Health -= dmg;
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        public static AbilityResponse MultiShot(Character caster, Character[] targets)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList["MultiShot"].MeetsRequirements(caster);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for MultiShot are" + AbilityList["MultiShot"].PrintReqs();
                return resp;
            }

            //Ability Actions
            for (int tCount = 0; tCount < AbilityList["MultiShot"].MaxTargets; tCount++)
            {
                double dmg = caster.Dexterity * 0.5;
                targets[tCount].Health -= dmg;
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        //STR
        public static AbilityResponse StrongAttack(Character caster, Character[] targets)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList["Strong Attack"].MeetsRequirements(caster);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for Strong Attack are" + AbilityList["Strong Attack"].PrintReqs();
                return resp;
            }

            //Ability Actions 
            for (int tCount = 0; tCount < AbilityList["Strong Attack"].MaxTargets; tCount++)
            {
                double dmg = caster.Strength;
                targets[tCount].Health -= dmg;
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        public static AbilityResponse Cleave(Character caster, Character[] targets)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList["Cleave"].MeetsRequirements(caster);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for Cleave are" + AbilityList["Cleave"].PrintReqs();
                return resp;
            }

            //Ability Actions
            for (int tCount = 0; tCount < AbilityList["Cleave"].MaxTargets; tCount++)
            {
                double dmg = caster.Strength * 0.7;
                targets[tCount].Health -= dmg;
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        //INT
        public static AbilityResponse CalculatedAttack(Character caster, Character[] targets)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList["Calculated Attack"].MeetsRequirements(caster);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for Calculated Attack are" + AbilityList["Calculated Attack"].PrintReqs();
                return resp;
            }

            //Ability Actions 
            for (int tCount = 0; tCount < AbilityList["Calculated Attack"].MaxTargets; tCount++)
            {
                double dmg = caster.Intelligence;
                targets[tCount].Health -= dmg;
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        public static AbilityResponse Fireball(Character caster, Character[] targets)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList["Fireball"].MeetsRequirements(caster);
            if(!resp.Usable)
            {
                resp.Message = "The requirements for Fireball are" + AbilityList["Fireball"].PrintReqs();
                return resp;
            }

            //Ability Actions
            for (int tCount = 0; tCount < AbilityList["Fireball"].MaxTargets; tCount++)
            {
                double dmg = caster.Intelligence * 0.7;
                targets[tCount].Health -= dmg;
                resp.Message += " " + targets[tCount].Name + " hit for " + dmg;
            }

            return resp;
        }

        public static AbilityResponse Heal(Character caster, Character[] targets)
        {
            AbilityResponse resp = new AbilityResponse();

            //Requirements
            resp.Usable = AbilityList["Heal"].MeetsRequirements(caster);
            if (!resp.Usable)
            {
                resp.Message = "The requirements for Heal are" + AbilityList["Heal"].PrintReqs();
                return resp;
            }

            //Ability Actions - does int * 1 heal to 1 target
            for (int tCount = 0; tCount < AbilityList["Heal"].MaxTargets; tCount++)
            {
                double heal = caster.Intelligence;
                targets[tCount].Health += heal;
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
        public delegate AbilityResponse AbilityAction(Character caster, Character[] targets);
        public AbilityAction Action { get; set; }
        public int MaxTargets { get; set; }
        public string Description { get; set; }

        public bool MeetsRequirements(Character caster)
        {
            if(
                caster.Dexterity < Reqs.Dexterity ||
                caster.Strength < Reqs.Strength ||
                caster.Intelligence < Reqs.Intelligence ||
                caster.Health < Reqs.Health
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

        public Ability(Requirement _reqs, AbilityAction _action, int _maxTargets, string _description)
        {
            Reqs = _reqs;
            Action = _action;
            MaxTargets = _maxTargets;
            Description = _description;
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
