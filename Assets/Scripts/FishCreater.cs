using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCreater : MonoBehaviour
{
    public List<GameObject> fishPrefabs1;
    public List<GameObject> fishPrefabs2;
    public List<GameObject> fishPrefabs3;// 물고기 프리팹들을 담을 리스트
    public float spawnInterval = 3f;
    public float spawnRangeMin = -5f;
    public float spawnRangeMax = 5f;
    public float prefabChangeTime = 30f;

    private bool isPrefabSet1 = true;
    private bool isPrefabSet2 = false;
    private bool isPrefabSet3 = false;

    public GameObject timerObject;
    private Timer timer_;

    private void Start()
    {
        timer_ = timerObject.GetComponent<Timer>();
        StartCoroutine(SpawnFishRoutine());
    }

    public IEnumerator SpawnFishRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            List<GameObject> fishPrefabs;
            if (isPrefabSet1)
            {
                fishPrefabs = fishPrefabs1;
            }
            else if (isPrefabSet2)
            {
                fishPrefabs = fishPrefabs2;
            }
            else
            {
                fishPrefabs = fishPrefabs3;
            }

            // 랜덤한 물고기 프리팹을 선택합니다.
            int randomIndex = Random.Range(0, fishPrefabs.Count);
            GameObject fishPrefab = fishPrefabs[randomIndex];

            // 물고기를 생성합니다.
            Vector3 spawnPosition = new Vector3(10f, Random.Range(spawnRangeMin, spawnRangeMax), 0f);
            GameObject fish = Instantiate(fishPrefab, spawnPosition, Quaternion.identity);
            Destroy(fish, 10f); // 일정 시간이 지난 후에 물고기를 삭제합니다.
        }
    }
    void Update()
    {
        if (timer_.timer == 60 || timer_.timer == 30)
        {
            isPrefabSet1 = false;
            isPrefabSet2 = false;
            isPrefabSet3 = false;

            // 해당 시간 범위에 따라 물고기 프리팹 세트를 변경합니다.
            if (timer_.timer == prefabChangeTime * 3)
            {
                isPrefabSet1 = true;
            }
            else if (timer_.timer == prefabChangeTime * 2)
            {
                isPrefabSet2 = true;
            }
            else
            {
                isPrefabSet3 = true;
            }
        }
    }
}
