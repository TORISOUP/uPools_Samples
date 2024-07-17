using UnityEngine;
using uPools;

namespace Samples.ObjectPoolSample
{
    public sealed class ObjectPoolUseSample : MonoBehaviour
    {
        private ObjectPool<MyObject> _objectPool;

        private void Awake()
        {
            Debug.Log("-- Initialize --");

            // プールを作成する
            _objectPool = new ObjectPool<MyObject>(
                // 新規生成時に実行される
                createFunc: () => new MyObject(),
                // プールからレンタルされたときに実行される
                onRent: obj => { Debug.Log($"onRent: MyObject[{obj.MyId}]がプールからレンタルされました"); },
                // プールに返却されたときに実行される
                onReturn: obj => { Debug.Log($"onReturn: MyObject[{obj.MyId}]がプールに返却されました"); },
                // オブジェクトがプールから破棄されたときに実行される
                onDestroy: obj =>
                {
                    Debug.Log($"onDestroy: MyObject[{obj.MyId}]がプールから破棄されました");
                    obj.Dispose();
                });

            Debug.Log("-- Prewarm = 2 --");

            // 事前に生成しておくオブジェクト数を指定
            _objectPool.Prewarm(2);

            Debug.Log("-- Rent & Return --");
            {
                var obj1 = _objectPool.Rent();
                var obj2 = _objectPool.Rent();

                _objectPool.Return(obj1);
                _objectPool.Return(obj2);
            }

            Debug.Log("-- Clear pool --");
            _objectPool.Clear();
            
            Debug.Log("-- Rent & Return --");
            {
                // プールが空になっているので新しく生成される
                var obj1 = _objectPool.Rent();
                _objectPool.Return(obj1);
            }
            
            Debug.Log("-- Dispose --");
            _objectPool.Dispose();
        }
    }
}