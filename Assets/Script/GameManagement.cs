using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] float maxHealth = 150f;
    private float score = 0;

    [SerializeField] float scoreMultiplier = 1;

    //Prefabs
    [SerializeField] GameObject projectilePrefab, enemyPrefab;
    [SerializeField] GameObject[] utilityPrefab;
    [SerializeField] GameObject frozeTextPrefab;

    //Target
    [SerializeField] GameObject nest;
    [SerializeField] ParticleSystem nestHit;

    //Player Stats
    [SerializeField] float speed = 3f;
    [SerializeField] float dashStrength = 5f, dashCooldown = 3f, attackCooldown = 0.8f, playerAttackDamage = 10f;

    //Enemy Stats
    public float enemySpeed = 10f;
    [SerializeField] float enemySpeedUpTime = 20f, enemySpeedMultiplier = 1.5f;
    [SerializeField] float enemyAttackDamage = 5f;
    [SerializeField] float enemySpawnRate = 10f;

    //Utilites
    [SerializeField] float utilityLifeTime = 25f, utilitySpawnRate = 15f, frozeTime = 5f, multipleTime = 5f;
    public bool isFrozen = false;

    //UI elements
    [SerializeField] TextMeshProUGUI scoreText, multiplierText;
    [SerializeField] Slider healtbar;
    [SerializeField] GameObject GameOverMenu, pausedMenu, controls;
    [SerializeField] Canvas canvas;



    public void Start()
    {
       Time.timeScale = 1.0f;
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>().PlayMusic();
        Cursor.visible = false;
        health = maxHealth;
        scoreText.SetText("Score: 0");
        multiplierText.SetText("1x");
        InvokeRepeating("SpawnEnemy", 0, enemySpawnRate);
        InvokeRepeating("SpawnUtility", 3, utilitySpawnRate);
        StartCoroutine(EnemySpeedUp());
        healtbar = GameObject.Find("Health bar").GetComponent<Slider>();
        healtbar.maxValue = maxHealth;
        healtbar.value = maxHealth;
        health= maxHealth;
        nestHit.transform.position = nest.transform.position;
        canvas = FindObjectOfType<Canvas>();
    }
    private void Update()
    {

        if (!IsAlive())
        {
            GameOver();
        }
        if(Input.GetKey(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void DamagePlayer() { 
        health -= enemyAttackDamage;
        healtbar.value = health;
        nestHit.Play();
        //Debug.Log(health);
    }
    public void DamageEnemy(ref float enemyHealth)
    {
        enemyHealth -= playerAttackDamage;
        //Debug.Log(enemyHealth);
    }
    
    public bool IsAlive()
    {
        if(health > 0)
        {
            return true;
        }
        return false;
    }

    public void ShootProjectile(Transform player)
    {
        Instantiate(projectilePrefab, player);
    }
    public void KillEnemy()
    {
        score += scoreMultiplier * 10;
        scoreText.SetText("Score: {0}", score);
    }

    public void SpawnEnemy() {
        Vector3[] positions =
        {
            new Vector3(
            Random.Range(-20f, 20f),
            0,
            Random.Range(1f, 2f)),

            new Vector3(
            Random.Range(-11f, -10f),
            0,
            Random.Range(-11f, 0f)),

            new Vector3(
            Random.Range(10f, 11f),
            0,
            Random.Range(-11f, 0f))
        };
        Vector3 position = Vector3.zero;
        if (IsAlive() && (SceneManager.GetActiveScene().name == "Level 1" || SceneManager.GetActiveScene().name == "Level 3"))
        {
            Debug.Log("Spawn");
            position = positions[0];
        }
        else if(IsAlive() && SceneManager.GetActiveScene().name == "Level 2")
        {
            position = positions[Random.Range(0,3)];
        }
        GameObject enemy = Instantiate(enemyPrefab, GameObject.Find("Enemy").transform);
        enemy.transform.position = position;

    }
    public void SpawnUtility()
    {
        if (IsAlive())
        {
            int index = Random.Range(0, 13);

            Vector3 position = new Vector3(
                Random.Range(-5.6f, 5.6f),
                1.8f,
                Random.Range(-11f, -3f));
            GameObject utility = Instantiate(utilityPrefab[index],GameObject.Find("Multiplier").transform);
            utility.transform.position = position;
        }
    }

    private IEnumerator EnemySpeedUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(enemySpeedUpTime);
            if (enemySpeed < speed && !isFrozen)
            {
                enemySpeed *= enemySpeedMultiplier;
                Debug.Log(enemySpeed);
            }
        }
    }
    public IEnumerator MultiplierTimer(float m, GameObject utility)
    {
        scoreMultiplier *= m;
        Debug.Log("Multiplie set");
        SetMultiplierTextColor();
        multiplierText.SetText("{0}x", scoreMultiplier);
        yield return new WaitForSeconds(multipleTime);
        scoreMultiplier /= m;
        multiplierText.SetText("{0}x", scoreMultiplier);
        Debug.Log("Multiplie setback");
        SetMultiplierTextColor();
    }
    
    public IEnumerator FrozenTimer(GameObject utility)
    {
        SpawnFreezeTimer();
        float currentSpeed = enemySpeed;
        Debug.Log("Froze");
        enemySpeed = 0.1f;
        isFrozen = true;
        yield return new WaitForSeconds(frozeTime);
        isFrozen = false;
        Debug.Log("speed back");
        enemySpeed = currentSpeed;
    }

    public IEnumerator CountDownFreezeTime(TextMeshProUGUI text, GameObject textGameObject)
    {
        text.SetText(frozeTime.ToString());
        float timer = frozeTime;
        while (timer>0) { 
            yield return new WaitForSeconds(1f);
            timer--;
            text.SetText(timer.ToString());
        }
        Destroy(textGameObject);
    }

    private void SpawnFreezeTimer()
    {
        Instantiate(frozeTextPrefab, canvas.transform);
    }

    private void SetMultiplierTextColor()
    {
        if (scoreMultiplier >= 2 && scoreMultiplier<5)
        {
            multiplierText.color = new Color(1, 0.85f, 0.08f, 1);
        }
        else if (scoreMultiplier >= 5 && scoreMultiplier<10)
        {
            multiplierText.color = new Color(1, 0.6681073f, 0.08627448f, 1);
        }
        else if (scoreMultiplier >= 10)
        {
            multiplierText.color = new Color(1, 0.5005218f, 0.08627448f, 1);
        }
        else
        {
            multiplierText.color = Color.white;
        }
    }

    private void GameOver()
    {
        enemySpawnRate = 0;
        utilitySpawnRate = 0;
        GameOverMenu.SetActive(true);
        Cursor.visible = true;
    }
    public void CountDown(TextMeshProUGUI text, float time)
    {
        float currentTime = time;
        text.SetText(((int)time).ToString());
        while (currentTime > 0)
        {
            text.SetText((Mathf.RoundToInt(currentTime)).ToString());
            currentTime -= Time.deltaTime;
        }
    }

    public void TryAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PauseGame()
    {
        pausedMenu.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void GameContinue()
    {
        pausedMenu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>().StopMusic();
        Destroy(GameObject.FindGameObjectWithTag("Music"));
    }

    public float GetDashStrength() { return dashStrength; }
    public float GetDashCooldown() { return dashCooldown; }
    public float GetAttackCooldown()  { return attackCooldown; }
    public float GetSpeed() { return speed; }
    public float GetUtilityLifetime() { return utilityLifeTime;  }
    public float GetFrozeTime() {  return frozeTime; }

}
