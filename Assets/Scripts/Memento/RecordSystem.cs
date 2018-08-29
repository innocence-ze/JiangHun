using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordSystem {

    private RecordSaveData m_LastSaveData = null;

    private int m_Chapter = 1;
    private int m_Level = 1;
    private int m_EndlessStep = 0;
    private int m_HighScore = 0;

    public void SetEndless(int endlessStep)
    {
        m_EndlessStep = endlessStep;
    }

    public void SetCurrentCL(int chapter, int level, int highScore)
    {
        m_Chapter = chapter;
        m_Level = level;
        m_HighScore = highScore;
    }

    public void SetHighCL(int chapter, int level)
    {
        m_Chapter = chapter;
        m_Level = level;
    }

    public RecordSaveData CreatSaveCurrentCLData()
    {
        var SaveData = new RecordSaveData()
        {
            CurrentChapter = m_Chapter,
            CurrentLevel = m_Level,
            CurrentHighScore = Mathf.Max(m_HighScore, PlayerPrefs.GetInt("C" + m_Chapter + "L" + m_Level))
        };
        return SaveData;
    }

    public RecordSaveData CreatSaveEndlessData()
    {
        var SaveData = new RecordSaveData()
        {
            EndlessStep = Mathf.Max(m_EndlessStep, m_LastSaveData.EndlessStep)
        };
        return SaveData;
    }

    public RecordSaveData CreatSaveHighCLData()
    {
        var SaveData = new RecordSaveData()
        {
            Chapter = Mathf.Max(m_Chapter, m_LastSaveData.Chapter),
            Level = m_Chapter * 4 + m_Level > m_LastSaveData.Chapter * 4 + m_LastSaveData.Level ? m_Level : m_LastSaveData.Level,
        };
        return SaveData;
    }

    public void SetSaveData(RecordSaveData SaveData)
    {
        m_LastSaveData = SaveData;
    }   
}
