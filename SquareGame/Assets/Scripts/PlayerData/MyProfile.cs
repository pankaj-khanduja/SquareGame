using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SquareOne;
public class MyProfile : MonoBehaviour
{
    public static MyProfile Instance = null;
    [HideInInspector]
    public PlayerData _PlayerData;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        _PlayerData = gameObject.AddComponent<PlayerData>();
        LoadPlayerData();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadPlayerData()
    {
        _PlayerData.LoadPlayerData(Constant.PlayerLoginID, PlayerDataLoaded);
    }

    public void PlayerDataLoaded()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
