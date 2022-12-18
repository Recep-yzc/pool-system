using Interface;
using System.Collections;
using UnityEngine;

public class TestItem : MonoBehaviour, IPoolItem
{
    public GameObject modelHolder;

    public bool IsAvailableForSpawn { get; set; }

    public void SetPosition(Vector3 value)
    {
        transform.position = value;
    }

    private void Awake()
    {
        Stop();
    }

    public void Play()
    {
        modelHolder.SetActive(true);
        IsAvailableForSpawn = false;
        StartCoroutine(delay());

        IEnumerator delay()
        {
            yield return new WaitForSeconds(1);
            Stop();
        }
    }

    public void Stop()
    {
        modelHolder.SetActive(false);
        IsAvailableForSpawn = true;
    }

}
