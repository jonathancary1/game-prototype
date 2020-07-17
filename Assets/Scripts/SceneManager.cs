using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// SceneManager manages instantiating all in-game objects and menus.
/// All interactions between different objects are handled through SceneManager.
/// </summary>
public class SceneManager : MonoBehaviour
{
    public enum State { TitleScreen, InGame }

    public AudioSource ButtonSound;
    public AudioSource HealthSound;

    public CameraManager CameraManager;
    public CanvasManager CanvasManager;
    public CharacterManager CharacterManager;

    private List<EnemyManager> enemies = new List<EnemyManager>();

    private float health;
    private float time;

    private State state;

    private void Start()
    {
        SetState(State.TitleScreen);
    }

    private void Update()
    {
        switch (state)
        {
            case State.TitleScreen:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Quit();
                }
                else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                {
                    Play();
                }
                break;
            case State.InGame:
                if (health == 0.0f || Input.GetKeyDown(KeyCode.Escape))
                {
                    SetState(State.TitleScreen);
                }
                else
                {
                    GameLogic();
                    SetScene();
                }
                break;
        }
    }

    public void Play()
    {
        ButtonSound.Play();
        SetState(State.InGame);
    }

    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// GameLogic detects collisions, updates player health, and spawns enemies.
    /// This is only called when state is equal to State.InGame.
    /// </summary>
    private void GameLogic()
    {
        time += Time.deltaTime;

        if (enemies.Count < (int)(time + 3.0f) / 5)
        {
            SpawnEnemy();
        }

        if (enemies.Any(enemy => CharacterManager.CollidesWith(enemy.Path, enemy.Size)))
        {
            health = Mathf.Clamp01(health - Time.deltaTime);

            if (!HealthSound.isPlaying)
            {
                HealthSound.Play();
            }
        }
    }

    /// <summary>
    /// SetScene updates CharacterManager, CameraManager, and CanvasManager.
    /// CharacterManager must be set first, since its state is used by CameraManager.
    /// </summary>
    private void SetScene()
    {
        CharacterManager.SetCharacter();
        CameraManager.SetCamera();
        CanvasManager.SetHealthBar(health);
        CanvasManager.SetTimer(time);
    }

    /// <summary>
    /// SetState sets the SceneState to either TitleScreen or InGame.
    /// </summary>
    private void SetState(State state)
    {
        this.state = state;

        switch (state)
        {
            case State.TitleScreen:
                CharacterManager.SetIdle();
                CharacterManager.SetVisible(false);
                CameraManager.Pitch = 90.0f;
                CameraManager.Yaw = 0.0f;
                CameraManager.SetCamera();
                DestroyEnemies();
                CanvasManager.SetButtonsVisible(true);
                CanvasManager.SetHealthBarVisible(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                CameraManager.AllowInput = false;
                CharacterManager.AllowInput = false;
                break;
            case State.InGame:
                health = 1.0f;
                time = 0.0f;
                CharacterManager.SetVisible(true);
                CameraManager.Pitch = 30.0f;
                CameraManager.Yaw = 45.0f;
                CameraManager.SetCamera();
                CanvasManager.SetButtonsVisible(false);
                CanvasManager.SetHealthBarVisible(true);
                CanvasManager.SetHealthBar(health);
                CanvasManager.SetTimer(time);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                CameraManager.AllowInput = true;
                CharacterManager.AllowInput = true;
                break;
        }
    }

    private void SpawnEnemy()
    {
        GameObject instance = Instantiate(Resources.Load<GameObject>("Enemy"));

        Vector3 position = CharacterManager.Position;
        float x = position.x + Utilities.RandomSign() * Random.Range(8.0f, 16.0f);
        float y = position.z + Utilities.RandomSign() * Random.Range(8.0f, 16.0f);

        EnemyManager enemy = instance.GetComponent<EnemyManager>();
        enemy.SetPosition(new Vector2(x - (x % enemy.Size), y - (y % enemy.Size)));
        enemy.CharacterManager = CharacterManager;

        enemies.Add(enemy);
    }

    private void DestroyEnemies()
    {
        foreach (EnemyManager enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        
        enemies.Clear();
    }
}
