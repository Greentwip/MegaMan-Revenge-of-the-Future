using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGame : MonoBehaviour
{

    public bool isDemo = true;

    public static CurrentGame Instance { set; get; }     //Allows other scripts to call functions from SoundManager.             

    public string CurrentLevel { get; set; }

    public string selectedDatafile = "a";

    public Checkpoint CurrentCheckpoint { set; get; }
    public int PlayerLives {

        set; get;
        /*set
        {
            PlayerPrefs.SetInt(ConstructLivesString(selectedDatafile), value);

            PlayerPrefs.Save();
        }
        get
        {
            return PlayerPrefs.GetInt(ConstructLivesString(selectedDatafile));
        }*/

    }
    public void IsBossDefeated(bool value, string level)
    {
        int result = 0;
        if (value == true)
        {
            result = 1;
        }

        if(PlayerPrefs.HasKey(ConstructStageString(selectedDatafile,
                                                level)))
        {
            PlayerPrefs.SetInt(ConstructStageString(selectedDatafile,
                                                    level), result);

            PlayerPrefs.Save();
        }
        else
        {
            throw new PlayerPrefsException("No key was found");
        }


    }
    public bool IsBossDefeated(string level)
    {
        if(PlayerPrefs.HasKey(ConstructStageString(selectedDatafile,
                                                    level)))
        {
            return PlayerPrefs.GetInt(ConstructStageString(selectedDatafile,
                                                            level)) == 1;
        } else
        {
            throw new PlayerPrefsException("No key was found");
        }
    }
    
    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (Instance == null)
        {
            //if not, set it to this.
            Instance = this;

        }
        //If instance already exists:
        else if (Instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);

    }

    private List<string> stageList = new List<string>();

    private void Start()
    {
        Load();
        /*stageList.Add("iceman");
        stageList.Add("sheriffman");
        stageList.Add("boomerman");
        stageList.Add("militaryman");
        stageList.Add("vineman");
        stageList.Add("windman");
        stageList.Add("fastman");
        stageList.Add("nightman");
        stageList.Add("drwilly");

        // verify the three data files and store default data accordingly
        ConstructInitialData("a");
        ConstructInitialData("b");
        ConstructInitialData("c");*/
    }

    private void ConstructInitialData(string dataFile)
    {
        if (!PlayerPrefs.HasKey(ConstructLivesString(dataFile)))
        {
            PlayerPrefs.SetInt(ConstructLivesString(dataFile), 3);
        }

        if (!PlayerPrefs.HasKey(ConstructSfxString(dataFile)))
        {
            PlayerPrefs.SetFloat(ConstructSfxString(dataFile), 100.0f);
        }

        if (!PlayerPrefs.HasKey(ConstructBgmString(dataFile)))
        {
            PlayerPrefs.SetFloat(ConstructBgmString(dataFile), 100.0f);
        }

        foreach(var stage in stageList)
        {
            if (!PlayerPrefs.HasKey(ConstructStageString(dataFile, stage)))
            {
                PlayerPrefs.SetInt(ConstructStageString(dataFile, stage), 0);
            }
        }

        PlayerPrefs.Save();
    }

    private string ConstructLivesString(string dataFile)
    {
        return dataFile + "_" + "lives";
    }

    private string ConstructSfxString(string dataFile)
    {
        return dataFile + "_" + "sfx_volume";
    }

    private string ConstructBgmString(string dataFile)
    {
        return dataFile + "_" + "bgm_volume";
    }

    private string ConstructStageString(string dataFile, string stage)
    {
        return dataFile + "_" + stage + "_" + "defeated";
    }

    public void Load()
    {
        this.CurrentCheckpoint = null;
        this.PlayerLives = 3;

    }

    public void Save()
    {
        this.CurrentCheckpoint = null;

    }

}

