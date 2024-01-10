using UnityEngine;
using UnityEngine.UI;

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

    private int currentWorld = 1; 
    public Material world1Material;
    public Material world2Material;
    public Material ground1Material;
    public Material ground2Material;

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



    public void ChangeWorld()
    {
        currentWorld = (currentWorld == 1) ? 2 : 1; //changing the current world
        if (currentWorld == 1)
        {
            quadMeshRendererBackground.material = world1Material;
            quadMeshRendererGround.material = ground1Material;
        }
        else if (currentWorld == 2)
        {
            quadMeshRendererBackground.material = world2Material;
            quadMeshRendererGround.material = ground2Material;
        }
    }

}
