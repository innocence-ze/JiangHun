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

    public void SetCL(int chapter, int level, int highScore)
    {
        m_Chapter = chapter;
        m_Level = level;
        m_HighScore = highScore;
    }

    public RecordSaveData CreatSaveCLData()
    {
        var SaveData = new RecordSaveData()
        {
            Chapter = Mathf.Max(m_Chapter, m_LastSaveData.Chapter),
            Level = m_Chapter * 5 + m_Level > m_LastSaveData.Chapter * 5 + m_LastSaveData.Level ? m_Level : m_LastSaveData.Level,
            CurrentChapter= m_Chapter,
            CurrentLevel = m_Level,
            CurrentHighScore = Mathf.Max(m_HighScore,m_LastSaveData.CurrentHighScore)/*,*/
            //EndlessStep = m_LastSaveData.EndlessStep
        };
        return SaveData;
    }

    public RecordSaveData CreatSaveEndlessData()
    {
        var SaveData = new RecordSaveData()
        {
            EndlessStep = Mathf.Max(m_EndlessStep, m_LastSaveData.EndlessStep)/*,*/
            //Chapter = m_LastSaveData.Chapter,
            //Level = m_LastSaveData.Level,
            //CurrentChapter = m_LastSaveData.CurrentChapter,
            //CurrentLevel = m_LastSaveData.CurrentLevel,
            //CurrentHighScore = m_LastSaveData.CurrentHighScore
        };
        return SaveData;
    }

    public void SetSaveData(RecordSaveData SaveData)
    {
        m_LastSaveData = SaveData;
    }   
}
