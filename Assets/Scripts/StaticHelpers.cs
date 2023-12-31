using System;
using System.Collections.Generic;
using UnityEngine;

namespace PirateGame.Helpers{
    public static class StaticHelpers
    {
            // Retrieves the sail enum value based on the int state of _sailState.
        public static float GetMobilityStateEnumValue(int speedMod){
            var value = (SpeedModifierEnum)speedMod;
            return (float)value / (Enum.GetValues(typeof(SpeedModifierEnum)).Length);
        }

        public static int GetMobilityStateEnumLength(){
            return Enum.GetValues(typeof(SpeedModifierEnum)).Length;
        }

        public static SoundOptionObject GetRequiredSoundObject(List<SoundOptionObject> soundObjects, SoundOptionEnums enumToSearchFor)
        {
            foreach (SoundOptionObject soundOptionObject in soundObjects)
            {
                if (soundOptionObject.GetSoundEnum == enumToSearchFor)
                {
                    return soundOptionObject;
                }
            }
            return null;
        }

        public static T GetEnumFromString<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        
        public static bool IsDefinedIgnoreCase(Type enumType, string value)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type provided is not an enum!");
            }

            string[] names = Enum.GetNames(enumType);

            foreach(string name in names)
            {
                if (string.Equals(name, value, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
 }
