using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject freezeBuf;
    [SerializeField] private GameObject destroyBuf;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Text[] scoreText;
    [SerializeField] private Text bestScoreText;

    [Range(0f, 100f)]
    [SerializeField] private int monsterProbability;
    [Range(0f, 100f)]
    [SerializeField] private int freezeBufProbability;
    [Range(0f, 100f)]
    [SerializeField] private int destroyBufProbability;

    private int score;
    private int bestScore;
    [SerializeField] private float level;
    private bool spawnTime;
    private int monsterCount;

    private void Start()
    {
        mainMenu.StartGame += StartGame;
        losePanel.SetActive(false);
        StartCoroutine(ScoreUpdateCoroutine());
    }

    private void StartGame()
    {
        PlayerPrefs.SetInt("MonsterCount", 0);
        
        PlayerPrefs.SetInt("Score", 0);
        foreach (Text score in scoreText)
            score.text = $"0";
        
        level = 1f;
        PlayerPrefs.SetFloat("Level", level);

        spawnTime = true;

        StartCoroutine(SpawnCoroutine());
        StartCoroutine(LevelUpCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        while(spawnTime == true)
        {
            SpawnObjects();
            spawnTime = false;
            yield return new WaitForSeconds(2/level);
            spawnTime = true;
        }
    }

    IEnumerator LevelUpCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(5);
            level += 0.1f;
            PlayerPrefs.SetFloat("Level", level);
            if (level > 8)
                level = 8;
        }
    }

    IEnumerator ScoreUpdateCoroutine()
    {
        while (true)
        {
            var currentScore = PlayerPrefs.GetInt("Score");
            foreach(Text score in scoreText)
                score.text = $"{currentScore}";
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SpawnObjects()
    {
        var randomProbability = Random.Range(0, 100);
        if (randomProbability <= monsterProbability && randomProbability > freezeBufProbability)
        {
            spawnManager.Spawn(monster);

            monsterCount = PlayerPrefs.GetInt("MonsterCount");
            monsterCount++;
            PlayerPrefs.SetInt("MonsterCount", monsterCount);

            if (monsterCount == 10)
            {
                LoseGame();
            }
        }
        else if (randomProbability <= freezeBufProbability && randomProbability > destroyBufProbability)
        {
            spawnManager.Spawn(freezeBuf);
            var bufs = GameObject.FindObjectsOfType<Bufs>();
            foreach (Bufs buf in bufs)
            {
                buf.freezSpawn += FreezSpawn;
            }
        }
        if (randomProbability <= destroyBufProbability)
            spawnManager.Spawn(destroyBuf);
        
    }

    private void FreezSpawn()
    {
        StartCoroutine(FreezeSpawnCoroutine());
    }

    IEnumerator FreezeSpawnCoroutine()
    {
        StopCoroutine(SpawnCoroutine());
        yield return new WaitForSeconds(3);
        StartCoroutine(SpawnCoroutine());
    }

    private void LoseGame()
    {
        menuPanel.SetActive(true);
        losePanel.SetActive(true);
        var currentScore = PlayerPrefs.GetInt("Score");
        if (currentScore > bestScore)
            bestScore = currentScore;
        bestScoreText.text = $"{bestScore}";
        foreach (Text score in scoreText)
            score.text = $"{currentScore}";

        StopAllCoroutines();
        DestroyAllMonsters();
    }

    private void DestroyAllMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach(GameObject monster in monsters)
        {
            Destroy(monster);
        }
    }
    private void OnDestroy()
    {
        mainMenu.StartGame -= StartGame;
        var bufs = GameObject.FindObjectsOfType<Bufs>();
        foreach (Bufs buf in bufs)
        {
            buf.freezSpawn -= FreezSpawn;
        }
    }
}
