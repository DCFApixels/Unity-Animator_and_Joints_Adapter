using UnityEngine;

namespace DCFAPixels.DemoScene
{
    public class CamGunBullet : MonoBehaviour
    {
        public float lifetime = 1f;
        private float time;
        public AnimationCurve _scaleCurve;
        private float _startScale;

        private void Start()
        {
            time = lifetime;
            _startScale = transform.localScale.x;
        }
        private void Update()
        {
            time -= Time.deltaTime;
            if (time <= 0)
                Destroy(gameObject);

            transform.localScale = Vector3.one * _startScale * _scaleCurve.Evaluate(Mathf.Min(time * 2f, 1f));
        }
    }
}