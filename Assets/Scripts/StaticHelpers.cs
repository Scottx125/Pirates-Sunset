using System;
 namespace PirateGame.Helpers{
    public static class StaticHelpers
    {
            // Retrieves the sail enum value based on the int state of _sailState.
        public static float GetMobilityStateEnumValue(int speedMod){
            var value = (SpeedModifierEnum)speedMod;
            return (float)value / (Enum.GetValues(typeof(SpeedModifierEnum)).Length);
        }

        public static int GetMobilityStateEnumLength(){
            return Enum.GetValues(typeof(SpeedModifierEnum)).Length - 1;
        }

    }
 }
