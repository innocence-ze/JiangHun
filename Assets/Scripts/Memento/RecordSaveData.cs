using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordSaveData  {

    public int Chapter { get; set; }
    public int Level { get; set; }
    public int CurrentChapter { get; set; }
    public int CurrentLevel { get; set; }
    public int CurrentHighScore { get; set; }

    public int EndlessStep { get; set; }

    public RecordSaveData() { }

    public void SaveEndless()
    {
        PlayerPrefs.SetInt("EndlessStep", EndlessStep);
    }

    public void SaveCurrentCL()
    {       
        PlayerPrefs.SetInt("CurrentC", CurrentChapter);
        PlayerPrefs.SetInt("CurrentL", CurrentLevel);
        var CurrentCL = "C" + CurrentChapter + "L" + CurrentLevel;
        PlayerPrefs.SetInt(CurrentCL, CurrentHighScore);
    }

    public void SaveHighCL()
    {
        PlayerPrefs.SetInt("Chapter", Chapter);
        PlayerPrefs.SetInt("Level", Level);
    }

    public void LoadEndless()
    {
        EndlessStep = PlayerPrefs.GetInt("EndlessStep", 0);
    }

    public void LoadCurrentCL()
    {
        CurrentChapter = PlayerPrefs.GetInt("CurrentC", 1);
        CurrentLevel = PlayerPrefs.GetInt("CurrentL", 1);
        var CurrentCL = "C" + CurrentChapter + "L" + CurrentLevel;
        CurrentHighScore = PlayerPrefs.GetInt(CurrentCL,1);
    }

    public void LoadHighCL()
    {
        Chapter = PlayerPrefs.GetInt("Chapter", 1);
        Level = PlayerPrefs.GetInt("Level", 1);
    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
