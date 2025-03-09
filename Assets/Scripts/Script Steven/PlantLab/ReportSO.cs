using UnityEngine;

[CreateAssetMenu(fileName = "ReportSO", menuName = "ScriptableObjects/ReportSO")]
public class ReportSO : ScriptableObject
{
    public Sentences[] sentences;
}

[System.Serializable]
public class Sentences{
    public string promt;
    public string fullSentence;
}