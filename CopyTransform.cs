using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DCFAPixels
{
    [AddComponentMenu("Physics/CopyTransform")]
    [ExecuteAlways]
    public class CopyTransform : MonoBehaviour
    {
        [SerializeField]
        private Transform _from;

        public void SetFrom(Transform from)
        {
            _from = from;
            Init();
        }
        public void SetFrom(Transform from, Quaternion startRotation)
        {
            _from = from;
            Init();
            _startRotation = startRotation;
        }

        private ConfigurableJoint _joint;
        private Quaternion _startRotation = Quaternion.identity;

        private void Init()
        {
            _joint = GetComponent<ConfigurableJoint>();
            _startRotation = _from.localRotation;

            if (_joint != null)
            {
                UpdateMethod = UpdateWithJoint;
                return;
            }

            UpdateMethod = UpdateDefault;
        }

        private void Awake()
        {
            if (_from != null)
                Init();
        }

        private System.Action UpdateMethod;
        private void LateUpdate()
        {
            if (_from == null)
                return;
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying && _from != null)
            {
                UpdateDefault();
                return;
            }
#endif
            UpdateMethod();
        }

        private void UpdateWithJoint()
        {
            transform.localScale = _from.localScale;
            transform.localPosition = _from.localPosition;
            SetJointRotation(_joint, _from.localRotation, _startRotation);
        }
        private void UpdateDefault()
        {
            transform.localScale = _from.localScale;
            transform.localPosition = _from.localPosition;
            transform.localRotation = _from.localRotation;
        }


        static void SetJointRotation(ConfigurableJoint joint, Quaternion targetRotation, Quaternion startRotation)
        {
            var right = joint.axis;
            var forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
            var up = Vector3.Cross(forward, right).normalized;

            Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);
            Quaternion resultRotation = Quaternion.Inverse(targetRotation) * startRotation * worldToJointSpace;

            joint.targetRotation = resultRotation;
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Generate Animator-Joins", false, 30)]
        private static void AutoFills()
        {
            Transform[] selectedObjects = Selection.transforms;
            if (selectedObjects.Length <= 0)
            {
                Debug.LogWarning("No GameObject is selected");
                return;
            }
            foreach (var selectedObject in selectedObjects)
            {
                AutoFill(selectedObject);
            }
        }

        private static void AutoFill(Transform selectedObject)
        {
            Transform target = selectedObject.GetComponentInParent<Animator>()?.transform;
            if (target == null)
            {
                Animator[] animators = selectedObject.GetComponentsInChildren<Animator>();
                if (animators.Length == 1)
                {
                    target = animators[0].transform;
                    goto exit;
                }
                if (animators.Length > 1)
                    Debug.LogWarning($"The selected {selectedObject.name} contains several Animators inside");
                else
                    Debug.LogWarning($"The selected {selectedObject.name} does not contain an animator");
                return;
            }
            exit:


            Transform clone = new GameObject().transform;
            clone.parent = target.parent;
            clone.name = target.name + " (Animator)";

            clone.localScale = target.localScale;
            clone.position = target.position;
            clone.rotation = target.rotation;

            CopyChilde(target, clone);

            foreach (var item in clone.GetComponentsInChildren<Joint>())
                DestroyImmediate(item);

            foreach (var item in clone.GetComponentsInChildren<Rigidbody>())
                DestroyImmediate(item);

            Animator targetAnimator = target.GetComponent<Animator>();
            clone.gameObject.AddComponent<Animator>().runtimeAnimatorController = targetAnimator.runtimeAnimatorController;
            DestroyImmediate(targetAnimator);
        }

        private static void CopyChilde(Transform from, Transform to)
        {
            int length = from.childCount;
            for (int i = 0; i < length; i++)
            {
                Transform origin = from.GetChild(i);
                if (origin.TryGetComponent<Renderer>(out _))
                    continue;
                Transform clone = new GameObject().transform;
                clone.parent = to;
                clone.localScale = origin.localScale;
                clone.position = origin.position;
                clone.rotation = origin.rotation;
                clone.name = origin.name;
                origin.gameObject.AddComponent<CopyTransform>().SetFrom(clone);
                CopyChilde(origin, clone);
            }
        }
#endif
    }
}