using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypingReportScript : MonoBehaviour
{
    [SerializeField] LabManager labManager;
    string colorString = "<color=#888888>";
    string tempString;
    [SerializeField] TMP_Text reportText;
    [SerializeField] ReportSO reportSO;
    [SerializeField] int sentenceIndex = 0;
    [SerializeField] int letterIndex = 0;
    [SerializeField] bool waitingForDot = false;
    bool shownHint = false;
    bool isFinished = false;
    bool ischeckingInput = true;
    public void StartReport()
    {
        sentenceIndex = 0;
        letterIndex = 0;
        waitingForDot = false;
        isFinished = false;
        reportText.text = tempString = FormatTitle(reportSO.title);
        //there are no string before the first prompt so temp string is empty
        //write the first prompt w the color tag
        reportText.text = tempString + colorString + reportSO.sentences[sentenceIndex].promt + " (Type the grayed out words)";
        // reportText.transform.localPosition = Vector3.zero;
        shownHint = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFinished){
            if(Input.GetKeyDown(KeyCode.Return)){
                EndReport();
            }
        }else{
            if(Input.anyKeyDown && ischeckingInput){
                CheckInput();
            }
        }
    }

    string FormatTitle(string plantName){
        return "<font=\"ReportHeader\"><align=\"center\"><size=110>Laporan " + plantName + "</size></align></font>\n\n";
    }

    void CheckInput(){
        // Debug.Log("Checking Input");
        // if(!waitingForDot){
        //     Debug.Log("Comparing" + Input.inputString + " with " + reportSO.sentences[sentenceIndex].promt[letterIndex]);
        // }
        //waiting for dot means the word is alrdy complete j waiting on the dot at the end of the sentence
        if(waitingForDot && Input.GetKeyDown(KeyCode.Return)){
            ischeckingInput = false;
            StartCoroutine(TypeSentence());
            return;
        }else if(!waitingForDot && Input.inputString.ToLower() == reportSO.sentences[sentenceIndex].promt[letterIndex].ToString().ToLower()){
            //else, go to the next letter
            letterIndex++;
            reportText.text = tempString;
            //print out how many letters we have typed
            for(int i = 0; i<letterIndex; i++){
                reportText.text += reportSO.sentences[sentenceIndex].promt[i];
            }
            //then put the color tag
            reportText.text += colorString;
            for(int i = letterIndex; i<reportSO.sentences[sentenceIndex].promt.Length; i++){
                reportText.text += reportSO.sentences[sentenceIndex].promt[i];
            }
            //then the texts we havent typed 
            //reportText.text += ".";
            if(letterIndex >= reportSO.sentences[sentenceIndex].promt.Length){
                //word is done, wait for dot
                waitingForDot = true;
                if(!shownHint){
                    shownHint = true;
                    reportText.text += "(Enter)";
                }
                // Debug.Log("Waiting for Dot");
            }
        }
    }

    IEnumerator TypeSentence(){
        //write the text before the current sentence
        reportText.text = tempString;

        //write the current sentence
        for(int i = 0; i < reportSO.sentences[sentenceIndex].fullSentence.Length; i++){
            // Debug.Log("Typing sentence " + (sentenceIndex + 1));
            reportText.text += reportSO.sentences[sentenceIndex].fullSentence[i];
            yield return new WaitForSeconds(0.025f);
        }
        reportText.text += " ";
        yield return new WaitForSeconds(0.025f);
        NextSentence();
    }

    void NextSentence(){
        letterIndex = 0;
        waitingForDot = false;
        sentenceIndex++;
        tempString = reportText.text;
        if(sentenceIndex >= reportSO.sentences.Length){
            isFinished = true;
            reportText.text += "\n\nPress Enter to continue...";
            return;
        }
        //reportText.text += colorString + reportSO.sentences[sentenceIndex].promt + ".";
        reportText.text += colorString + reportSO.sentences[sentenceIndex].promt;
        ischeckingInput = true;
    }

    void EndReport(){
        Debug.Log("End of Report");
        gameObject.SetActive(false);
        labManager.ShowExtractedMaterials(true);
    }   
}
