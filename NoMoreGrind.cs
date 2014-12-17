using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Upgradeables;

namespace KSPAdvancedFlyByWire
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
            foreach (UpgradeableFacility facility in GameObject.FindObjectsOfType<UpgradeableFacility>())
            {
            }
        }
         
        public void OnDestroy()
        {
            print("NoMoreGrind: Deinitialized");
        }

    }

}
