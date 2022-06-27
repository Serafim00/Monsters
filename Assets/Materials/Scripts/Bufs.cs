using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bufs : MonoBehaviour, IPointerClickHandler
{
    private enum BufsItem
    {
        freeze,
        destroyAll,
    };

    [SerializeField] private BufsItem bufsItem = BufsItem.freeze;
    [SerializeField] private float speedPingPong;
    [SerializeField] private float dist;

    public Action freezSpawn;

    private void Start()
    {
        StartCoroutine(PingPongCoroutine());
    }

    IEnumerator PingPongCoroutine()
    {
        while (true)
        {
            float posY = Mathf.Sin(Time.time * speedPingPong) * dist;
            Vector3 pos = transform.localPosition;
            transform.localPosition = new Vector3(pos.x, posY, pos.z);
            yield return new WaitForSeconds(0.025f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (bufsItem == BufsItem.freeze)
            FreezeSpawn();
        else if (bufsItem == BufsItem.destroyAll)
            KillAllMonster();
    }

    private void KillAllMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        var score = PlayerPrefs.GetInt("Score");
        score += monsters.Length;

        foreach (GameObject monster in monsters)
        {
            Destroy(monster);
        }

        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("MonsterCount", 0);

        Destroy(gameObject);
    }

    private void FreezeSpawn()
    {
        freezSpawn?.Invoke();
        Destroy(gameObject);
    }
}
