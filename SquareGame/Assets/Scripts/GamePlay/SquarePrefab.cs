using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SquareOne;
public struct SquareData
{
    public int number {get; private set;}
    public Color squareColor;
    public bool isSelected , isPenaltySquare;
    public Vector3 direction;

    public SquareData(int number, Color squareColor)
    {
        this.number = number;
        this.squareColor = squareColor;
        this.isSelected = false;
        this.isPenaltySquare = false;
        this.direction =  (Vector3.zero);
    }
}
public class SquarePrefab : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite , outlineSprite;
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] GameObject _ParticleEffect , _RedParticleEfffect , _CorrectRetainSpriteParticle;
    public GameObject RedAlertPanel;
    public SquareData squareData;
    int numberOflines = 0;
    bool isMove = false;
    float speed = 2;

    Vector2 targetPosition;

    private void OnEnable() {
        SquareController.Instance.onSquareHint += ShowHint;
    }

    private void OnDisable() {
        if(SquareController.Instance != null)
            SquareController.Instance.onSquareHint -= ShowHint;        
    }
    public void InIt(int index)
    {
        Square squareInfo = SquareController.Instance.squareContainer.Square[index];
        squareData = new SquareData(squareInfo.squareNumber, squareInfo.squareColor);
        BactToOriginal(squareData.squareColor);
    }

    public void OnResetSquare()
    {
        if (squareData.isSelected) return;
        textMesh.text = "";
        outlineSprite.color = sprite.color = new Color(112.0f / 255.0f , 128.0f / 255.0f , 144.0f / 255.0f, 1);
        squareData.isSelected = false;
    }

    public void PenaltySquare()
    {
        textMesh.text = "";
        outlineSprite.color = sprite.color = Color.red;
        squareData.isPenaltySquare = true;
        Invoke("DestroySquare", Random.Range(10,15));
        if(SquareController.Instance.GetSquareList.Count > 5)
        {
            if (Random.Range(0, 2) == 0) SquareMoveBy();
        }
    }

   

    private void Update()
    {
        if (!isMove) return;
            transform.Translate(squareData.direction * speed * Time.deltaTime);


        //Horizontal boundaries
        if (transform.position.x > SquareController.Instance.screenBounds.x - 0.5f || transform.position.x < -SquareController.Instance.screenBounds.x + 0.5f)
        {
            squareData.direction.x = -squareData.direction.x; // Reverse the x direction
        }

        // Vertical boundaries
        if (transform.position.y > SquareController.Instance.screenBounds.y - 0.5f || transform.position.y <  -SquareController.Instance.screenBounds.y + 0.5f)
        {
            squareData.direction.y = -squareData.direction.y; // Reverse the y direction
        }

        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void SquareMoveBy()
    {
        int indexX = Random.Range(0, 2);
        int indexY = Random.Range(0, 2);
        squareData.direction = new Vector3(Constant.directionMode[indexX], Constant.directionMode[indexY], 0); // Diagonal movement
        //targetPosition = SquareController.Instance.GenerateSpriteAtPos(this.gameObject, true);
        isMove = true;
    }

    public void DestroySquare()
    {
        isMove = false;
        SquareController.Instance.RemoveObjFromList(this.gameObject);
        if(!squareData.isPenaltySquare)
        {
            DisableSprite(_ParticleEffect);
        }
        else
        {
            DisableSprite(_RedParticleEfffect);
        }
        
    }

    public void ShowRedAlert()
    {
        Instantiate(RedAlertPanel);
    }



    void DisableSprite(GameObject obj)
    {
        obj.SetActive(true);
        outlineSprite.enabled =  sprite.enabled = false;
        if(gameObject.GetComponent<BoxCollider2D>())
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        Invoke("DestroyAfterParticle", 2);

    }

    void DestroyAfterParticle()
    {
        Destroy(gameObject);
    }



    public void CheckForUndo()
    {
        numberOflines--;
        if (numberOflines <= 0)
        {
            numberOflines = 0;
        }
        if(squareData.number == 1)
        {
            squareData.isSelected = false;
            
        }

    }

    void BactToOriginal(Color color)
    {
        outlineSprite.color = sprite.color = color;
        textMesh.text = squareData.number.ToString();
    }

    public void ShowHint()
    {
        if (squareData.isSelected) return;
        BactToOriginal(squareData.squareColor);
        Invoke("OnResetSquare", 1);
    }

    public void OnUserResponse(bool status)
    {
        if (status) {numberOflines++; BactToOriginal(squareData.squareColor);outlineSprite.color =Color.green; }
        else SquareController.Instance.onSquareHint();
       
        if (squareData.isSelected) return;
        // if(!status) outlineSprite.color =Color.red;
        squareData.isSelected = status;
        if(squareData.isSelected)
        {
            try
            {
                _CorrectRetainSpriteParticle.SetActive(true);
            }
            catch(System.Exception ex)
            {

            }
            
        }

    }

}
