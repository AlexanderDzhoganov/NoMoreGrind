using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Upgradeables;
using UpgradeLevel = Upgradeables.UpgradeableObject.UpgradeLevel;

using System.Reflection;

namespace NoMoreGrind
{
    public enum Facility
    {
        VehicleAssemblyBuilding,
        TrackingStation,
        SpaceplaneHangar,
        Runway,
        ResearchAndDevelopment,
        MissionControl,
        LaunchPad,
        AstronautComplex,
        Administration
    }

    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class NoMoreGrind : MonoBehaviour
    {
        private static bool patchApplied = false;

        public void Awake()
        {
            print("NoMoreGrind: Initialized");
        }
        
        private static readonly string ConfigPath = Path.Combine(Path.Combine(Path.Combine(KSPUtil.ApplicationRootPath, "GameData"), "NoMoreGrind"), "config.txt");

        public void Start()
        {
            if (!patchApplied)
            {
                Dictionary<Facility, float> priceMultipliers = new Dictionary<Facility, float>();

                try
                {
                    priceMultipliers = ReadConfig(ConfigPath);
                }
                catch (Exception)
                {
                    print("NoMoreGrind: Malformed config.txt");
                    throw;
                }

                List<FieldInfo> fields = new List<FieldInfo>
                (
                    typeof(UpgradeableFacility).GetFields
                    (
                        BindingFlags.NonPublic | BindingFlags.Instance
                    )
                );

                List<FieldInfo> upgradeLevelsFields =
                        (new List<FieldInfo>(
                            fields.Where<FieldInfo>(
                                f => f.FieldType.Equals(typeof(UpgradeLevel[])))));
                
                foreach (UpgradeableFacility facility in GameObject.FindObjectsOfType<UpgradeableFacility>())
                {
                    Facility facilityType = (Facility)Enum.Parse(typeof(Facility), facility.name);
                    if (priceMultipliers.ContainsKey(facilityType))
                    {
                        UpgradeLevel[] upgradeLevels = (UpgradeLevel[])upgradeLevelsFields[0].GetValue(facility);
                        for (int i = 0; i < upgradeLevels.Length; i++)
                        {
                            UpgradeLevel level = upgradeLevels[i];
                            level.levelCost *= priceMultipliers[facilityType];
                        }
                    }
                }

                patchApplied = true;
            }
        }

        public static Dictionary<Facility, float> ReadConfig(string path)
        {
            Dictionary<Facility, float> priceMultipliers = new Dictionary<Facility, float>();

            var text = File.ReadAllText(path).Split('\n');
            foreach (var line in text)
            {
                var items = line.Split('=');
                var facility = items[0].Trim();
                var price = items[1].Trim();
                priceMultipliers.Add((Facility)Enum.Parse(typeof(Facility), facility), float.Parse(price));
            }

            return priceMultipliers;
        }

    }

}
