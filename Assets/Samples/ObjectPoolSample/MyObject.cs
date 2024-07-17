using System;
using UnityEngine;
using uPools;

namespace Samples.ObjectPoolSample
{
    // プールされるオブジェクト
    public sealed class MyObject : IPoolCallbackReceiver, IDisposable
    {
        private static int InstanceCount = 0;

        public int MyId { get; }

        public MyObject()
        {
            MyId = InstanceCount++;
            Debug.Log($"{nameof(MyObject)}[{MyId}]が生成されました");
        }
        
        public void OnRent()
        {
            Debug.Log($"{nameof(MyObject)}[{MyId}]がレンタルされました");
        }

        public void OnReturn()
        {
            Debug.Log($"{nameof(MyObject)}[{MyId}]が返却されました");
        }

        public void Dispose()
        {
            Debug.Log($"{nameof(MyObject)}[{MyId}]がDispose()されました");
        }
    }
}