using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SquareController : SingletonComponent<SquareController>
{
    [SerializeField] private Camera mainCamera;
    public Material lineRendererMaterial;
    public SquareContainer squareContainer;
    public GameObject squarePrefab, GameOverPanel;
    private ISquare _iSquareManager;
    List<GameObject> generatedSquaresList;
    public delegate void OnReset();
    public OnReset onReset;
    public delegate void OnGameBegin();
    public OnGameBegin onGameBegin;
    public delegate void OnTimeHighlight(Action callBack);
    public OnTimeHighlight onTimeHighlight;
    public delegate void OnPenatltyUpdate();
    public OnPenatltyUpdate onPenatltyUpdate;
    public delegate void OnSquareHint();
    public OnSquareHint onSquareHint;
    public delegate void OnUserUpdate();
    public OnSquareHint onUserUpdate;
    public delegate void OnLevelCleared();
    public OnLevelCleared onLevelCleared;
    public delegate void OnRestart();
    public OnRestart onRestart;
    public int PlayerIQScore { get; private set; }
    public float gameTime;
    public Vector2 screenBounds;
    public Color _timerTextColor;
    public GameObject EncouragingText;
    public Action onAction;
    public bool isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        onReset += ResetSquare;
        _timerTextColor = Color.white;
    }

    public void Game1()
    {

        PlayerIQScore = 0;
        generatedSquaresList = new List<GameObject>();
        switch (Constant.gameMode)
        {
            case GameMode.Game1: _iSquareManager = gameObject.AddComponent<Game1Square>(); break;
            case GameMode.Game2: _iSquareManager = gameObject.AddComponent<Game2>(); break;
            case GameMode.Game3: _iSquareManager = gameObject.AddComponent<Game3>(); break;
        }
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
       
        // _iSquareManager = gameObject.AddComponent<Game2>();
        //MoveToNextRound();
    }

    public void RestartGame()
    {
        isGameOver = false;
        _iSquareManager.OnRestartGame();
        onRestart();
    }

    public void OnGameOver()
    {
        SquareController.Instance.onLevelCleared?.Invoke();
        isGameOver = true;
        GameOverPanel.SetActive(true);
    }

    public void ResetIQ()
    {
        PlayerIQScore = 0;
        onUserUpdate();
    }

    public void UpdateScore(int points)
    {
        PlayerIQScore += points;
        onUserUpdate();
    }

    void ResetSquare()
    {
        foreach (var item in generatedSquaresList)
        {
            Destroy(item);
        }
        generatedSquaresList = new List<GameObject>();

        // _iSquareManager.RemoveLines();
        // callback.Invoke();
    }

    public void OnResetGame()
    {
        onReset();
    }

    public List<GameObject> GetSquareList
    {
        get
        {
            return generatedSquaresList;
        }
    }

    public ISquare GetManager()
    {
        return _iSquareManager;
    }

    public void AddSquareToList(GameObject squareSprite)
    {
        generatedSquaresList.Add(squareSprite);
    }

    public void RemoveObjFromList(GameObject obj)
    {
        generatedSquaresList.Remove(obj);
    }

  
    public GameObject IsPointerOverlappingAnySquare(Vector3 mousePos)
    {
        foreach (var prevSquare in generatedSquaresList)
        {
            if (prevSquare.GetComponent<SpriteRenderer>().bounds.Contains(mousePos))
            {
                return prevSquare;
            }
        }
        return null;
    }



    public Vector3 GenerateSpriteAtPos(GameObject currentSquareObj , bool isOverlapping)
    {
        // Debug.Log($"size of box is {sprite.bounds} width {Screen.width} heigh {Screen.height}");
        // Generate a random screen position

        Vector2 randomScreenPosition = new Vector2(UnityEngine.Random.Range(50, (Screen.width - 50)), UnityEngine.Random.Range(50, (Screen.height - 200)));
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(randomScreenPosition.x, randomScreenPosition.y, mainCamera.nearClipPlane));
        worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0);
        if(isOverlapping) return worldPosition;
        Vector3 currentSpriteSize = currentSquareObj.GetComponent<SpriteRenderer>().bounds.size;
        Rect currentSpriteRect = new Rect(worldPosition.x - currentSpriteSize.x / 2, worldPosition.y - currentSpriteSize.y / 2, currentSpriteSize.x, currentSpriteSize.y);
        foreach (var prevSquare in generatedSquaresList)
        {
            if (IsRectOverlappingSprite(currentSpriteRect, prevSquare.GetComponent<SpriteRenderer>()))
            {
                Debug.Log("IsBoxContained");
                return Vector3.zero;
            }
        }

        // Convert the random screen position to world position

        return worldPosition;
    }

    bool IsRectOverlappingSprite(Rect rect, SpriteRenderer spriteRenderer)
    {
        // Get the bounds of the sprite
        Bounds spriteBounds = spriteRenderer.bounds;

        // Convert the sprite's bounds to a Rect
        Rect spriteRect = new Rect(spriteBounds.min.x, spriteBounds.min.y, spriteBounds.size.x, spriteBounds.size.y);

        // Check if the rects overlap
        return rect.Overlaps(spriteRect);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AnimateEncouragingText()
    {
        if(!EncouragingText.activeInHierarchy)
            EncouragingText.SetActive(true);
    }
}