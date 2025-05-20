using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlantDisplay : MonoBehaviour, IDropHandler
{
    public GameObject overworldPlant;
    public PlantSO plant;
    public TextMeshProUGUI dialogueText;
    public PlantDataSO plantList;
    public int currentStep;
    public string plantName;
    public PlantExtraction plantExtractionView;

    void Awake()
    {
        //Determine which plant to display here later
        // plant = plantList.plant[0];
        currentStep = 0;
        dialogueText = GameObject.FindGameObjectWithTag("DialogueBox").GetComponentInChildren<TextMeshProUGUI>();
        dialogueText.text = "";
        dialogueText.text = string.Empty;
        // Debug.Log(plant.plantName);
        this.GetComponent<Image>().sprite = plant.plantImage;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Debug.Log("OnDrop");
        //kl ada yang di drag ke plant
        if (eventData.pointerDrag != null)
        {
            ToolDisplay currentItemScript = eventData.pointerDrag.GetComponent<ToolDisplay>();

            //kl yang di drag sesuai dengan extract processnya
            if(currentStep < plant.extractionSteps.Length 
            && currentItemScript.tool.toolName.ToLower() == plant.extractionSteps[currentStep].ToLower())
            {
                dialogueText.text = "That seems right!";
                currentStep++;

                //stop kl dah di extract
                if(currentStep == plant.extractionSteps.Length) 
                {
                    dialogueText.text = "You've successfully extracted the plant!";
                    StartCoroutine(plantExtractionView.ClosePlantExtraction());
                    //exit this view
                }
            } //jic someone somehow drags stuff still after it's been extracted, hrsny tp nnt cmn tinggal di disable aj viewnya jdny sebenerny gausah
            else if(currentStep >= plant.extractionSteps.Length){
                dialogueText.text = "why are you trying to break my code :( stop it";
            } //kl salah 
            else{
                Debug.Log("What we chose: " + currentItemScript.tool.toolName);
                Debug.Log("What it's supposed to be: " + plant.extractionSteps[currentStep]);
                dialogueText.text = "That doesn't seem right... Let's try another tool!";
            }
        }
    }
}
