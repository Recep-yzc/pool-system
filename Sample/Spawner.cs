using Manager;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Property")]
    [SerializeField] private float spawnPerDuration = 1;

    #region private
    private float _currentDuration;
    #endregion


    private void Update()
    {
        _currentDuration += Time.deltaTime;

        if (_currentDuration >= spawnPerDuration)
        {
            _currentDuration = 0;
            CreateItem();
        }
    }

    private void CreateItem()
    {
        var testItem = PoolManager.Instance.GetPoolItem<TestItem>(typeof(TestItem));

        testItem.SetPosition(Random.insideUnitSphere * 3f);
        testItem.Play();
    }
}
