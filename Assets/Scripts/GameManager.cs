using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoSingleton<GameManager>
{
    public bool godMode = false;
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float powerAmount = 1;
    public float powerRefillRate = 10;
    public float GameSpeed { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    public TextMeshProUGUI gameOverText;
    public Image powerFillImage;
    public Button retryButton;
    public ScreenRotate screenRotate;
    public ScreenMove screenMove;

    private Spawner[] spawners;

    private float score;

    private void Start()
    {
        spawners = FindObjectsOfType<Spawner>();
        NewGame();
    }

    public void NewGame()
    {
        score = 0f;
        GameSpeed = initialGameSpeed;
        enabled = true;

        Player.Instance.gameObject.SetActive(true);
        foreach (var spawner in spawners)
        {
            spawner.gameObject.SetActive(true);
        }

        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        UpdateHiscore();
    }

    public void GameOver()
    {
        if (!godMode)
        {
            var obstacles = FindObjectsOfType<Obstacle>();

            foreach (var obstacle in obstacles)
            {
                Destroy(obstacle.gameObject);
            }

            GameSpeed = 0f;
            enabled = false;

            Player.Instance.gameObject.SetActive(false);
            foreach (var spawner in spawners)
            {
                spawner.gameObject.SetActive(false);
            }
            gameOverText.gameObject.SetActive(true);
            retryButton.gameObject.SetActive(true);

            UpdateHiscore();
        }
    }

    private void Update()
    {
        GameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += GameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");

        if (!screenRotate.usingPower && !screenMove.usingPower)
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
