#region Creator

//Created by Recep Yazıcı 
//gamedeveloper.recep@gmail.com

#endregion

using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager
{
    public class PoolManager : MonoBehaviour
    {
      #region Singleton

        public static PoolManager Instance;

        private void Awake()
        {
            if (!Instance)
            {
                DontDestroyOnLoad(this.gameObject);
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            Spawn();
        }

        #endregion

        [Header("Pool List")]
        [SerializeField] private List<CreatablePoolItemProperty> creatablePoolItems = new();

        #region private

        private readonly Dictionary<Type, List<IPoolItem>> poolItemList = new();

        #endregion

        private void Spawn()
        {
            creatablePoolItems.ForEach(x => FirstCreatePoolItem(x));
        }

        public T GetPoolItem<T>(Type poolItemType)
        {
            IPoolItem iPoolItemResult = null;

            if (poolItemList.ContainsKey(poolItemType))
            {
                iPoolItemResult = poolItemList[poolItemType].FirstOrDefault(x => x.IsAvailableForSpawn);

                if (iPoolItemResult == null)
                {
                    iPoolItemResult = AfterCreatePoolItem(poolItemType);

                    Add(poolItemType, iPoolItemResult);
                }
            }

            return (T)iPoolItemResult;
        }

        private void Add<T>(Type poolItemType, T poolItem)
        {
            List<IPoolItem> newPoolItems;
            if (poolItemList.ContainsKey(poolItemType))
            {
                newPoolItems = poolItemList[poolItemType];
                newPoolItems.Add((IPoolItem)poolItem);
            }
            else
            {
                newPoolItems = new List<IPoolItem> { (IPoolItem)poolItem };
                poolItemList.Add(poolItemType, newPoolItems);
            }
        }

        private IPoolItem AfterCreatePoolItem(Type poolItemType)
        {
            var creatablePoolItemPropertyTemp =
                creatablePoolItems.FirstOrDefault(x => x.GetPoolItemType() == poolItemType);

            var prefab = creatablePoolItemPropertyTemp.Prefab;
            var parent = creatablePoolItemPropertyTemp.Parent;

            return PoolItemInstantiate(prefab, parent);
        }

        private void FirstCreatePoolItem(CreatablePoolItemProperty creatablePoolItemProperty)
        {
            var prefab = creatablePoolItemProperty.Prefab;
            var parent = creatablePoolItemProperty.Parent;
            var spawnCount = creatablePoolItemProperty.SpawnCount;
            var poolItemType = creatablePoolItemProperty.GetPoolItemType();

            for (int s = 0; s < spawnCount; s++)
            {
                var iPoolItemTemp = PoolItemInstantiate(prefab, parent);
                Add(poolItemType, iPoolItemTemp);
            }
        }

        private IPoolItem PoolItemInstantiate(GameObject prefab, Transform parent)
        {
            return Instantiate(prefab, parent).GetComponent<IPoolItem>();
        }
    }
}


 [Serializable]
    public class CreatablePoolItemProperty
    {
        [Space(5)] public GameObject Prefab;
        [Space(5)] public int SpawnCount;
        [Space(5)] public Transform Parent;

        public Type GetPoolItemType()
        {
            var poolItemType = Prefab.GetComponent<IPoolItem>()?.GetType();

            if (poolItemType == null)
                Debug.LogError("Prefab has not IPoolItem");

            return poolItemType;
        }
    }
