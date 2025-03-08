using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlantDisplay : MonoBehaviour, IDropHandler
{
    public PlantSO plant;
    public TextMeshProUGUI dialogueText;
    // [SerializeField] private PlantSO[] plantList;
    public PlantDataSO plantList;
    public int currentStep;

    void Awake()
    {
        //Determine which plant to display here later
        plant = plantList.plantData[0];
        currentStep = 0;
        dialogueText = GameObject.FindGameObjectWithTag("DialogueBox").GetComponentInChildren<TextMeshProUGUI>();
        dialogueText.text = "";
        dialogueText.text = string.Empty;
        Debug.Log(plant.plantName);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        //kl ada yang di drag ke plant
        if (eventData.pointerDrag != null)
        {
            ItemDisplay currentItemScript = eventData.pointerDrag.GetComponent<ItemDisplay>();

            //kl yang di drag sesuai dengan extract processnya
            if(currentStep < plant.extractionSteps.Length 
            && currentItemScript.item.itemName == plant.extractionSteps[currentStep])
            {
                dialogueText.text = "That seems right!";
                currentStep++;

                //stop kl dah di extract
                if(currentStep == plant.extractionSteps.Length) 
                {
                    dialogueText.text = "You've successfully extracted the plant!";
                    //exit this view
                }
            } //jic someone somehow drags stuff still after it's been extracted, hrsny tp nnt cmn tinggal di disable aj viewnya jdny sebenerny gausah
            else if(currentStep >= plant.extractionSteps.Length){
                dialogueText.text = "why are you trying to break my code :( stop it";
            } //kl salah 
            else{
                dialogueText.text = "That doesn't seem right... Let's try another tool!";
            }
        }
    }
}
