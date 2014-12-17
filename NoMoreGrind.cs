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

    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class NoMoreGrind : MonoBehaviour
    {
        private static float CostMultiplier = 0.1f;

        public void Awake()
        {
            print("NoMoreGrind: Initialized");
        }

        public void Start()
        {
            List<FieldInfo> fields = new List<FieldInfo>
            (
                typeof(UpgradeableFacility).GetFields
                (
                    BindingFlags.NonPublic | BindingFlags.Instance
                )
            );
            Debug.Log("Found " + fields.Count + " priv fields in UpgradableFacility");

            List<FieldInfo> upgradeLevelsFields =
                    (new List<FieldInfo>(
                        fields.Where<FieldInfo>(
                            f => f.FieldType.Equals(typeof(UpgradeLevel[])))));
            Debug.Log("Found " + upgradeLevelsFields.Count + " UpgradeLevels fields");

            Debug.LogError("### NoMoreGrind ###");
            foreach (UpgradeableFacility facility in GameObject.FindObjectsOfType<UpgradeableFacility>())
            {
                Debug.LogWarning(facility.name + " has upgrade-cose: " + facility.GetUpgradeCost());

                UpgradeLevel[] upgradeLevels = (UpgradeLevel[])upgradeLevelsFields[0].GetValue(facility);
                for (int i = 0; i < upgradeLevels.Length; i++)
                {
                    UpgradeLevel level = upgradeLevels[i];
                    Debug.Log("Level " + i + " costs " + level.levelCost);
                    level.levelCost *= CostMultiplier;
                    Debug.Log("> set to: " + level.levelCost);
                }
            }
            Debug.LogError("### NoMoreGrind ###");
        }

        public void LoadState(ConfigNode configNode)
        {
            gui.LoadConfig(configNode);
        }

        public void SaveState(ConfigNode configNode)
        {
            gui.SaveConfig(configNode);    
        }

        void OnGUI()
        {
            gui.OnGUI();
        }

        private GUI gui = new GUI();

        public void OnDestroy()
        {
            print("NoMoreGrind: Deinitialized");
        }

    }

}
