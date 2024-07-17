using UnityEngine;
using uPools;

namespace Samples.CustomSample
{
    // GameObjectをプールするクラスをカスタマイズしてみる
    public sealed class CustomGameObjectPool : ObjectPoolBase<GameObject>
    {
        private readonly GameObject _prefab;

        public CustomGameObjectPool(GameObject prefab)
        {
            _prefab = prefab;
        }

        protected override GameObject CreateInstance()
        {
            var obj = Object.Instantiate(_prefab);
            // ここで初期化処理を行ったりできる
            return obj;
        }

        protected override void OnDestroy(GameObject instance)
        {
            Object.Destroy(instance);
        }

        protected override void OnRent(GameObject instance)
        {
            // ここでレンタル時の処理を行ったりできる
            instance.SetActive(true);
        }

        protected override void OnReturn(GameObject instance)
        {
            // ここで返却時の処理を行ったりできる
            instance.SetActive(false);
        }
    }
}