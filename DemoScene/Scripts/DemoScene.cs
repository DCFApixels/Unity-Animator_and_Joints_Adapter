using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DCFAPixels.DemoScene
{

    public class DemoScene : MonoBehaviour
    {
        private float defaultValue;
        private void Awake()
        {
            defaultValue = Time.fixedDeltaTime;
            Time.fixedDeltaTime = 0.01f;
        }

        private void OnDestroy()
        {
            Time.fixedDeltaTime = defaultValue;
        }
    }
}