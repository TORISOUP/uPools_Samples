using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using uPools;
using Random = UnityEngine.Random;

namespace Samples.IPoolCallbackReceiverSample
{
    // IPoolCallbackReceiverでプールからレンタルされたときと返却されたときの処理を実装する
    public class TimeCounter : MonoBehaviour, IPoolCallbackReceiver
    {
        private CancellationTokenSource _cancellationTokenSource;
        public Action OnFinished { get; set; }


        // プールからレンタルされたら呼び出される
        public void OnRent()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            _cancellationTokenSource = new CancellationTokenSource();

            WaitAsync(_cancellationTokenSource.Token).Forget();
        }

        // プールに返却されたら呼び出される
        public void OnReturn()
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

        // 完全に破棄された場合の処理も忘れずに
        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
}