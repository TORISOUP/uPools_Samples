using UnityEngine;
using uPools;

namespace Samples.GameObjectPoolSample
{
    public class GameObjectPoolUseSample : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        private GameObjectPool _pool;

        private void Start()
        {
            // プールの作成時にPrefabを指定する
            _pool = new GameObjectPool(_prefab);

            {
                // オブジェクトをレンタル
                var go = _pool.Rent();
                _pool.Return(go);
            }
            {
                // オブジェクトをレンタル
                // 親Transformを指定
                var go = _pool.Rent(transform);
                _pool.Return(go);
            }
            {
                // オブジェクトをレンタル
                // 親Transformや位置姿勢を指定
                var go = _pool.Rent(new Vector3(0, 10, 0), Quaternion.identity, transform);
                _pool.Return(go);
            }
            
            // プールをクリア（登録されているGameObjectもDestroyされる）
            _pool.Clear();
            
            // Disposeでも内部的にClear()が呼ばれる
            _pool.Dispose();
        }
    }
}