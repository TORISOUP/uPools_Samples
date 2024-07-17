using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using uPools;
using Random = UnityEngine.Random;

namespace Samples.SharedGameObjectPoolSamples
{
    public class ObjectSpawnerB : MonoBehaviour
    {
        [SerializeField] private TimeCounter _timeCounterPrefab;

        private void Start()
        {
            // SharedGameObjectPool は staticクラスのためインスタンス化不要

            // 登録するのものは「GameObject」である必要がある
            SharedGameObjectPool.Prewarm(_timeCounterPrefab.gameObject, 2);

            // 生成し続ける
            CreatePrefabBLoopAsync(destroyCancellationToken).Forget();
            
            SharedGameObjectPool.Return(new GameObject());
        }
        
        private async UniTaskVoid CreatePrefabBLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                // プールからレンタルする
                // 型推論される
                TimeCounter timeCounter = SharedGameObjectPool.Rent(
                    _timeCounterPrefab,
                    Random.insideUnitSphere + Vector3.up * 5,
                    Quaternion.identity);

                timeCounter.OnFinished += () =>
                {
                    // PrefabBの処理が終わったら返却する
                    SharedGameObjectPool.Return(timeCounter.gameObject);
                };

                // 1秒待ってまた生成する
                await UniTask.Delay(1000, cancellationToken: ct);
            }
        }
    }
}