
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DCFAPixels.DemoScene
{
    public class BonesDrawer : MonoBehaviour
    {
#if UNITY_EDITOR
        public Color color = Color.black;
        public bool isEnabled = true;
        public float scale = 1f;

        private void OnDrawGizmos()
        {
            if (isEnabled)
                DrawChildred(transform);
        }

        private void DrawChildred(Transform parent)
        {
            Vector3 parentPos = parent.position;
            int iMax = parent.childCount;
            Gizmos.color = color;
            Transform[] selected = Selection.transforms;
            for (int i = 0; i < iMax; i++)
            {
                Transform child = parent.GetChild(i);
                Gizmos.DrawLine(parentPos, child.position);
                Gizmos.DrawSphere(child.position, (selected.Any(o => o == child)) ? 0.065f * scale : 0.035f * scale);
                DrawChildred(child);
            }
        }
#endif
    }
}