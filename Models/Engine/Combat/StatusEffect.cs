using GameplayLoopCombat1.classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantRPG.Models.Engine.Combat
{
    public class StatusEffect : DSI
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DamageModifier { get; set; }
        public Action<Character> EffectAction { get; set; } = x => { };
        public int Turns { get; set; }
        public double MissPercent;


        public StatusEffect(string _name, string _description, int _dex = 0, int _str = 0, int _int = 0, int _dmgMod = 1, int _turns = -1, double _missPercent = 0)
        {
            Name = _name;
            Description = _description;
            Dexterity = _dex;
            Strength = _str;
            Intelligence = _int;
            DamageModifier = _dmgMod;
            Turns = _turns;
            MissPercent = _missPercent;
        }
    }
}
