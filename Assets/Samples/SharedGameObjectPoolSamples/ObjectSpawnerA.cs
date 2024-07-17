using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using uPools;
using Random = UnityEngine.Random;

namespace Samples.SharedGameObjectPoolSamples
{
    public class ObjectSpawnerA : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabA;

        private void Start()
        {
            // SharedGameObjectPool は staticクラスのためインスタンス化不要
            // Prewarmで事前にPrefabの存在を登録する必要がある
            SharedGameObjectPool.Prewarm(_prefabA, 2);

            // PrefabAを生成し続ける
            CreatePrefabALoopAsync(destroyCancellationToken).Forget();
        }

        private async UniTaskVoid CreatePrefabALoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                // PrefabAをプールからレンタルする
                var a = SharedGameObjectPool.Rent(
                    _prefabA,
                    Random.insideUnitSphere + Vector3.up * 5,
                    Quaternion.identity);

                // 適当に利用して終わったら返却する
                UseAsync(a, ct).Forget();

                // 1秒待って再生成する
                await UniTask.Delay(1000, cancellationToken: ct);
            }
        }

        private async UniTaskVoid UseAsync(GameObject obj, CancellationToken ct)
        {
            // 1.5秒経ったら返却する
            await UniTask.Delay(1500, cancellationToken: ct);

            // 使い終わったら返却する
            SharedGameObjectPool.Return(obj);
        }
    }
}