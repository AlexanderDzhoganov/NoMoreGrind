using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NoMoreGrind
{
    class GUI
    {

        public delegate void OnValueChanged(float costFactor);
        public OnValueChanged onValueChanged = null;

        private Rect windowRect = new Rect(256, 256, 320, 240);

        private bool isVisible = false;

        private float costFactor = 0.1f;

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

            float oldCostFactor = costFactor;
            costFactor = GUILayout.HorizontalSlider(costFactor, 0.05f, 2.0f);

            if (costFactor != oldCostFactor && onValueChanged != null)
            {
                onValueChanged(costFactor);
            }

            GUILayout.Label(costFactor.ToString("0.00"));
            GUILayout.EndHorizontal();
        }

        public void SaveConfig(ConfigNode node)
        {
            node.AddValue("CostFactor", costFactor.ToString());
        }

        public void LoadConfig(ConfigNode node)
        {
            try
            {
                costFactor = float.Parse(node.GetValue("CostFactor"));
            }
            catch (Exception)
            {
                costFactor = 0.1f;
            }
        }

        public float GetCostFactor()
        {
            return costFactor;
        }

        public void OnGUI()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt) && Input.GetKey(KeyCode.F11))
            {
                isVisible = true;
            }

            if (isVisible)
            {
                windowRect = GUILayout.Window(0, windowRect, DoWindow, "No More Grind");
            }
        }

    }

}
