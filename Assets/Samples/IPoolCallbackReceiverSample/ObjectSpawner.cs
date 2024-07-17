using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using uPools;
using Random = UnityEngine.Random;

namespace Samples.IPoolCallbackReceiverSample
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private TimeCounter _timeCounterPrefab;

        private void Start()
        {
            SharedGameObjectPool.Prewarm(_timeCounterPrefab.gameObject, 2);
            CreatePrefabBLoopAsync(destroyCancellationToken).Forget();
        }
        
        private async UniTaskVoid CreatePrefabBLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                // プールからレンタルする
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