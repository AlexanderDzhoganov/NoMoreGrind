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

    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class NoMoreGrind : MonoBehaviour
    {
        private static float CostMultiplier = 0.1f;

        private static Rect windowRect = new Rect(256, 256, 320, 120);

        private static bool isVisible = false;

        private static bool patchApplied = false;

        public void Awake()
        {
            print("NoMoreGrind: Initialized");
            GameEvents.onGameStateSave.Add(SaveState);
            GameEvents.onGameStateLoad.Add(LoadState);
        }

        public void Start()
        {
            if (!patchApplied)
            {
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
                    UpgradeLevel[] upgradeLevels = (UpgradeLevel[])upgradeLevelsFields[0].GetValue(facility);
                    for (int i = 0; i < upgradeLevels.Length; i++)
                    {
                        UpgradeLevel level = upgradeLevels[i];
                        level.levelCost *= CostMultiplier;
                    }
                }

                patchApplied = true;
            }
        }

        public void LoadState(ConfigNode configNode)
        {
            if (configNode.HasValue("CostFactor"))
            {
                try
                {
                    CostMultiplier = float.Parse(configNode.GetValue("CostFactor"));
                }
                catch (Exception)
                {
                    CostMultiplier = 0.1f;
                }   
            }
        }

        public void SaveState(ConfigNode configNode)
        {
            if (configNode.HasValue("CostFactor"))
            {
                configNode.SetValue("CostFactor", CostMultiplier.ToString());
            }
            else
            {
                configNode.AddValue("CostFactor", CostMultiplier.ToString());
            }
        }

        void OnGUI()
        {
            var oldSkin = GUI.skin;
            GUI.skin = HighLogic.Skin;

            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.F11))
            {
                isVisible = true;
            }

            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.O))
            {
                isVisible = true;
            }

            if (isVisible)
            {
                windowRect = GUILayout.Window(0, windowRect, DoWindow, "No More Grind");
            }

            GUI.skin = oldSkin;
        }

        public void OnDestroy()
        {
        }

        private void DoWindow(int index)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("X", GUILayout.Width(16)))
            {
                isVisible = false;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Label("Cost factor");
            GUILayout.FlexibleSpace();

            CostMultiplier = GUILayout.HorizontalSlider(CostMultiplier, 0.05f, 2.0f, GUILayout.Width(256));

            GUILayout.Label(CostMultiplier.ToString("0.00"));
            GUILayout.EndHorizontal();
        }

    }

}
