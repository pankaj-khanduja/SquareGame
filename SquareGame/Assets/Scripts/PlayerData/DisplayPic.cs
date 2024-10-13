
using UnityEngine;
using UnityEngine.UI;
public class DisplayPic : MonoBehaviour
{

    Image userPicImage;
    public bool isLocalUser;
    // Start is called before the first frame update
    void Start()
    {
        userPicImage = GetComponent<Image>();
        if(isLocalUser)
        {
            MyProfile.Instance.OnPlayerDataLoaded += OnPlayerDataLoaded;
        }
        OnPlayerDataLoaded();

    }

    private void OnDisable()
    {
        if (MyProfile.Instance != null)
            MyProfile.Instance.OnPlayerDataLoaded -= OnPlayerDataLoaded;
    }

    void OnPlayerDataLoaded()
    {
        if (MyProfile.Instance == null) return;
        Texture2D tex = MyProfile.Instance._PlayerData._PlayerDataContainer.userPic;
        if (tex == null) return;
        Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        userPicImage.sprite = newSprite;
    }

   
}
