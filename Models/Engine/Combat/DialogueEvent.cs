using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerchantRPG.Models.Engine.Combat
{
    public class DialogueEvent
    {
        public string Text { get; set; }
        public Dictionary<string, DialogueEvent> Decision { get; set; }
        public bool Resolution { get; set; }
        public Action Action { get; set; }
    }

    public static class Dialogue
    {
        public static DialogueEvent CreateEvent()
        {
            
            return new DialogueEvent();
        }

        public enum DialogueTypes
        {
            Puzzle, Obstacle, Traveler, Accident, Threat
        }

        public enum ResolveTypes
        {
            CombatOrEquipment, PartyOrNot, ModifyOrNot, CombatOrNot, EquipmentOrNot
        }
    }
}
