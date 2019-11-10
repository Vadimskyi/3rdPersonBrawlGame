using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Utils
{
    public class MonoObjectPool<T> where T: MonoBehaviour
    {
        private Queue<T> _pool;
        private T _prefab;

        public MonoObjectPool(
            T prefab,
            int defaultPoolSize = 10)
        {
            _prefab = prefab;
            _pool = new Queue<T>();
        }

        public T Rent()
        {
            if (_pool.Count > 0) return _pool.Dequeue();
            return CreateInstance();
        }

        public void Return(T item)
        {
            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
        }

        private T CreateInstance()
        {
            return Object.Instantiate(_prefab);
        }
    }
}
