using UnityEngine;

namespace DCFAPixels.DemoScene
{
    public class CamGun : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public float power = 10f;
        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Rigidbody rb = Instantiate(bulletPrefab, Camera.main.transform.position, Quaternion.identity, null).GetComponent<Rigidbody>();
                rb.AddForce(ray.direction * power, ForceMode.Impulse);
            }
        }
    }
}

