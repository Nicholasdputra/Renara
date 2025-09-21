using UnityEngine;

[CreateAssetMenu(fileName = "ReportSO", menuName = "ScriptableObjects/ReportSO")]
public class ReportSO : ScriptableObject
{
    public string title;
    public Sentences[] sentences;
}

[System.Serializable]
public class Sentences{
    public string promt;
    [TextArea(3, 10)]
    public string fullSentence;
}