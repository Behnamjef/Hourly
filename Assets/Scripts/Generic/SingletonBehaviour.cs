using UnityEngine;

namespace Hourly
{
    public class SingletonBehaviour<T> : CommonBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (!_instance)
                    _instance = FindObjectOfType<T>();
                if (!_instance)
                    _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                return _instance;
            }
        }

        private static T _instance;
    }
}