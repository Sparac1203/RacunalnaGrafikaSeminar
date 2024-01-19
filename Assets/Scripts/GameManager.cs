using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject chooseWorldButton;
    [SerializeField] private GameObject gameOver;

    [SerializeField] private MeshRenderer quadMeshRendererBackground;
    [SerializeField] private MeshRenderer quadMeshRendererGround;

    private int score;
    public int Score => score;

    private List<ActivePowerUp> activePowerUps = new List<ActivePowerUp>();
    private float powerUpDuration = 15f;

    private int currentWorld = 1;
    public Material world1Material;
    public Material world2Material;
    public Material ground1Material;
    public Material ground2Material;

    public Sprite[] world1Sprites;
    public Sprite[] world2Sprites;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            Application.targetFrameRate = 60;
            DontDestroyOnLoad(gameObject);
            Pause();
        }
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);
        chooseWorldButton.SetActive(false);


        Time.timeScale = 1f;
        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    public void GameOver()
    {
        playButton.SetActive(true);
        gameOver.SetActive(true);
        chooseWorldButton.SetActive(true);

        Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void PowerUp()
    {
        // List of available power-ups
        string[] powerUpOptions = { "DoubleScore", "GravityChange", "ExtraLife" };

        // Randomly select a power-up from the list
        string selectedPowerUp = powerUpOptions[Random.Range(0, powerUpOptions.Length)];
        Debug.Log("picked Power-Up: " + selectedPowerUp);

        // Apply the chosen power-up
        ApplyPowerUp(selectedPowerUp);

        // Add the new power-up to the active power-ups list
        activePowerUps.Add(new ActivePowerUp(selectedPowerUp, powerUpDuration));

    }

    private void Update()
    {
        // Update the timers for active power-ups
        for (int i = activePowerUps.Count - 1; i >= 0; i--)
        {
            activePowerUps[i].UpdateTimer(Time.deltaTime);

            // Check if the power-up duration has expired
            if (activePowerUps[i].IsExpired)
            {
                // Disable or remove the effects of the power-up
                DisablePowerUp(activePowerUps[i]);
                activePowerUps.RemoveAt(i);
            }
        }
    }

    private void DisablePowerUp(ActivePowerUp activePowerUp)
    {
        string powerUpType = activePowerUp.PowerUpType;

        switch (powerUpType)
        {
            case "DoubleScore":
                break;

            case "GravityChange":
                player.SetGravity(-9.81f);
                break;

            case "ExtraLife":
                // Example: Handle the end of the ExtraLife power-up
                break;

            default:
                Debug.LogWarning("Unknown power-up type: " + powerUpType);
                break;
        }

        // Update UI or perform other actions related to the end of the power-up
        UpdatePowerUpEndUI(powerUpType);
    }

    private void ApplyPowerUp(string powerUpType)
    {
        switch (powerUpType)
        {
            case "DoubleScore":
                score *= 2;
                scoreText.text = score.ToString();
                break;

            case "GravityChange":
                player.SetGravity(-12f);
                break;

            case "ExtraLife":
                // Find all existing Pipes in the scene
                Pipes[] existingPipes = FindObjectsOfType<Pipes>();

                foreach (Pipes pipes in existingPipes)
                {
                    Transform pipesTransform = pipes.transform;
                    GameObject topPipe = pipesTransform.Find("Top Pipe").gameObject;
                    GameObject bottomPipe = pipesTransform.Find("Bottom Pipe").gameObject;

                    if (topPipe != null && bottomPipe != null)
                    {
                        topPipe.tag = "Untagged";
                        bottomPipe.tag = "Untagged";

                        pipesTransform.parent = transform;
                    }
                    else
                    {
                        Debug.LogError("Failed to access Top Pipe or Bottom Pipe");
                    }
                }
                break;

            default:
                Debug.LogWarning("Unknown power-up type: " + powerUpType);
                break;
        }

        UpdatePowerUpStartUI(powerUpType);
    }

    private void UpdatePowerUpStartUI(string powerUpType)
    {
        // Example: Update UI to indicate the start of the active power-up
        Debug.Log("Activated Power-Up: " + powerUpType);
    }

    private void UpdatePowerUpEndUI(string powerUpType)
    {
        // Example: Update UI to indicate the end of the active power-up
        Debug.Log("Power-Up expired: " + powerUpType);
    }

    // Class to represent an active power-up with a timer
    private class ActivePowerUp
    {
        public string PowerUpType { get; private set; }
        public float Timer { get; private set; }

        public bool IsExpired => Timer <= 0f;

        public ActivePowerUp(string powerUpType, float duration)
        {
            PowerUpType = powerUpType;
            Timer = duration;
        }

        public void UpdateTimer(float deltaTime)
        {
            Timer -= deltaTime;
        }
    }

    public void ChangeWorld()
    {
        currentWorld = (currentWorld == 1) ? 2 : 1; // changing the current world

        if (currentWorld == 1)
        {
            quadMeshRendererBackground.material = world1Material;
            quadMeshRendererGround.material = ground1Material;
            player.SetBirdSprites(world1Sprites);
        }
        else if (currentWorld == 2)
        {
            quadMeshRendererBackground.material = world2Material;
            quadMeshRendererGround.material = ground2Material;
            player.SetBirdSprites(world2Sprites);
        }
    }

}
