using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Samples.SharedGameObjectPoolSamples
{
    public class TimeCounter : MonoBehaviour
    {
        private CancellationTokenSource _cancellationTokenSource;
        public Action OnFinished { get; set; }


        // プールからレンタルされたらEnableされる
        private void OnEnable()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            _cancellationTokenSource = new CancellationTokenSource();

            WaitAsync(_cancellationTokenSource.Token).Forget();
        }

        // プールに返却されたらDisableされる
        private void OnDisable()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            OnFinished = null;
        }

        // 一定時間経ったら通知する
        private async UniTaskVoid WaitAsync(CancellationToken ct)
        {
            try
            {
                // ランダムに待つ
                await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(0.5f, 5f)), cancellationToken: ct);
            }
            finally
            {
                OnFinished?.Invoke();
            }
        }
    }
}