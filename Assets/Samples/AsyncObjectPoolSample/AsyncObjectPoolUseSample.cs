using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using uPools;
using Random = UnityEngine.Random;

namespace Samples.AsyncObjectPoolSample
{
    public sealed class AsyncObjectPoolUseSample : MonoBehaviour
    {
        private AsyncObjectPool<MyObject> _asyncObjectPool;

        private async UniTaskVoid Start()
        {
            Debug.Log("-- Initialize --");

            // プールを作成する
            _asyncObjectPool = new AsyncObjectPool<MyObject>(
                // 新規生成時に実行される
                createFunc: async ct =>
                {
                    // 非同期処理があったとして
                    await UniTask.Delay(
                        TimeSpan.FromMilliseconds(Random.Range(100, 1000)),
                        cancellationToken: ct);

                    return new MyObject();
                },
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

            Debug.Log("-- PrewarmAsync = 2 --");

            // 事前にオブジェクトを非同期に生成
            await _asyncObjectPool.PrewarmAsync(3, destroyCancellationToken);

            Debug.Log("-- RentAsync & Return --");
            {
                var obj1 = await _asyncObjectPool.RentAsync(destroyCancellationToken);
                var obj2 = await _asyncObjectPool.RentAsync(destroyCancellationToken);

                _asyncObjectPool.Return(obj1);
                _asyncObjectPool.Return(obj2);
            }

            Debug.Log("-- Clear pool --");
            _asyncObjectPool.Clear();

            Debug.Log("-- Rent & Return --");
            {
                // プールが空になっているので新しく生成される
                var obj1 = await _asyncObjectPool.RentAsync(destroyCancellationToken);
                _asyncObjectPool.Return(obj1);
            }

            Debug.Log("-- Dispose --");
            _asyncObjectPool.Dispose();
        }
    }
}