using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Upgradeables;

using System.Reflection;

namespace NoMoreGrind
{

    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class NoMoreGrind : MonoBehaviour
    {
      
       
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

            var levelField =
                    new List<FieldInfo>(
                        fields.Where<FieldInfo>(
                            f => f.FieldType.Equals(typeof(UpgradeableObject.UpgradeLevel)))).First();

            foreach (UpgradeableFacility facility in GameObject.FindObjectsOfType<UpgradeableFacility>())
            {
                UpgradeableObject.UpgradeLevel level = (UpgradeableObject.UpgradeLevel)levelField.GetValue(facility);
                level.levelCost *= 0.1f;
            }
        }
         
        public void OnDestroy()
        {
            print("NoMoreGrind: Deinitialized");
        }

    }

}
