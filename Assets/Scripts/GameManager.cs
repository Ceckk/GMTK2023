using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoSingleton<GameManager>
{
    public bool godMode = false;
    public float initialGameSpeed = 5f;
    public float speed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float speedOffset = 0;
    public float powerAmount = 1;
    public float powerRefillRate = 10;
    public float GameSpeed { get => speed + speedOffset; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    public GameObject gameOver;
    public Image powerFillImage;
    public Button retryButton;
    public ScreenRotate screenRotate;
    public ScreenMove screenMove;
    public ScreenSlow screenSlow;
    public ScreenAutoRotate screenAutoRotate;
    public GameObject limits;
    public GameObject deadPlayer;

    private Spawner[] spawners;

    private float score;

    private void Start()
    {
        spawners = FindObjectsOfType<Spawner>();
        limits.transform.rotation = Quaternion.identity;
        limits.transform.position = Vector3.zero;
        score = 0f;
        speed = initialGameSpeed;
        powerAmount = 1;
        enabled = true;

        Player.Instance.gameObject.SetActive(true);
        deadPlayer.SetActive(false);

        foreach (var spawner in spawners)
        {
            spawner.gameObject.SetActive(false);
        }
        // screenRotate.enabled = true;
        screenMove.enabled = false;
        screenMove.ResetPositions();
        // screenSlow.enabled = true;
        // screenAutoRotate.enabled = true;

        gameOver.SetActive(false);
        retryButton.gameObject.SetActive(false);

        UpdateHiscore();
    }

    public void NewGame()
    {
        limits.transform.rotation = Quaternion.identity;
        limits.transform.position = Vector3.zero;
        score = 0f;
        speed = initialGameSpeed;
        powerAmount = 1;
        enabled = true;

        var obstacles = FindObjectsOfType<Obstacle>();
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        Player.Instance.gameObject.SetActive(true);
        deadPlayer.SetActive(false);

        foreach (var spawner in spawners)
        {
            spawner.gameObject.SetActive(true);
        }
        // screenRotate.enabled = true;
        screenMove.enabled = true;
        screenMove.ResetPositions();
        // screenSlow.enabled = true;
        // screenAutoRotate.enabled = true;

        gameOver.SetActive(false);
        retryButton.gameObject.SetActive(false);

        UpdateHiscore();
    }

    public void GameOver()
    {
        if (!godMode)
        {
            speed = 0f;
            enabled = false;

            Player.Instance.gameObject.SetActive(false);
            deadPlayer.SetActive(true);

            foreach (var spawner in spawners)
            {
                spawner.gameObject.SetActive(false);
            }
            // screenRotate.enabled = false;
            screenMove.enabled = false;
            // screenSlow.enabled = false;
            // screenAutoRotate.enabled = false;

            gameOver.SetActive(true);
            retryButton.gameObject.SetActive(true);

            UpdateHiscore();
        }
    }

    private void Update()
    {
        speed += gameSpeedIncrease * Time.deltaTime;
        score += speed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");

        if (!screenRotate.usingPower && !screenMove.usingPower && !screenSlow.usingPower)
        {
            powerAmount += powerRefillRate * Time.deltaTime;
        }

        powerAmount = Mathf.Clamp01(powerAmount);

        powerFillImage.fillAmount = powerAmount;
    }

    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }

}
