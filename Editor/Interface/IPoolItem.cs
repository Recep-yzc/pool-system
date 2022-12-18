using UnityEngine;

namespace Interface
{
    public interface IPoolItem
    {
        public bool IsAvailableForSpawn { get; set; }
    }
}
