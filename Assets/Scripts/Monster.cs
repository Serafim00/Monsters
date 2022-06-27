using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : MonoBehaviour, IPointerClickHandler {

    [SerializeField] private float speedPingPong;
    [SerializeField] private float dist;
    private float hp;
    private int randomX;
    private int randomZ;

    private void Start()
    {
        var level = PlayerPrefs.GetFloat("Level");
        hp = (int)level;
        randomX = Random.Range(10, 100) / 10;
        randomZ = Random.Range(10, 100) / 10;
        StartCoroutine(PingPongCoroutine());
    }

    IEnumerator PingPongCoroutine()
    {
        while (true)
        {
            float posX = Mathf.Sin(Time.time * randomX) + randomX/2;
            float posY = Mathf.Sin(Time.time * speedPingPong) * dist;
            float posZ = Mathf.Sin(Time.time * randomZ) + randomZ;
            Vector3 pos = transform.localPosition;
            transform.localPosition = new Vector3(posX, posY, posZ);
            yield return new WaitForSeconds(0.025f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        HitMonster();
    }

    private void KillMonster()
    {
        var monsterCount = PlayerPrefs.GetInt("MonsterCount");
        monsterCount--;
        Destroy(gameObject);
        var score = PlayerPrefs.GetInt("Score");
        score++;
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("MonsterCount", monsterCount);
    }

    private void HitMonster()
    {
        var level = PlayerPrefs.GetFloat("Level");
        hp = (int)level; 
        hp -= 1f;
        if(hp <= 0)
            KillMonster();
    }
}
