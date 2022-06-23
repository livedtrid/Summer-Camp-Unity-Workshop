using UnityEngine;

namespace MyLads.Runtime.Scripts.Utils
{
    /// <summary>
    /// Will parent this transform to the gameobject with the supplied tag, otherwise will parent itself to the scene root.
    /// </summary>
    public class AutoParent : MonoBehaviour
    {
        [SerializeField] private string gameObjectTag;

        void Awake()
        {
            var t = GameObject.FindWithTag(gameObjectTag).transform;

            transform.SetParent(t, false);
        }
    }
}
