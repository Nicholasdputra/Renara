using System.Collections;
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
    public Sprite[] amySprites;
    public Image amyImage;
    public Coroutine amyTalkingCoroutine;
    void Awake()
    {
        //Determine which plant to display here later
        // plant = plantList.plant[0];
        currentStep = 0;
        dialogueText = GameObject.FindGameObjectWithTag("DialogueBox").GetComponentInChildren<TextMeshProUGUI>();
        dialogueText.text = "Hmm... Let's see what we can do with this plant!";
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
            if (currentStep < plant.extractionSteps.Length
            && currentItemScript.tool.toolName.ToLower() == plant.extractionSteps[currentStep].ToLower())
            {
                amyImage.sprite = amySprites[0];
                if (amyTalkingCoroutine != null)
                {
                    // If amyTalkingCoroutine is not null, stop it before starting a new one
                    StopCoroutine(amyTalkingCoroutine);
                }
                amyTalkingCoroutine = StartCoroutine(AmyTalking());
                dialogueText.text = "";
                dialogueText.text = string.Empty;
                dialogueText.text = "That seems right!";
                currentStep++;

                //stop kl dah di extract
                if (currentStep == plant.extractionSteps.Length)
                {
                    dialogueText.text = "";
                    dialogueText.text = string.Empty;
                    dialogueText.text = "You've successfully extracted the plant!";
                    amyImage.sprite = amySprites[0];
                    if (amyTalkingCoroutine != null)
                    {
                        // If amyTalkingCoroutine is not null, stop it before starting a new one
                        StopCoroutine(amyTalkingCoroutine);
                    }
                    amyTalkingCoroutine = StartCoroutine(AmyTalking());
                    StartCoroutine(plantExtractionView.ClosePlantExtraction());
                    //exit this view
                }
            } //jic someone somehow drags stuff still after it's been extracted, hrsny tp nnt cmn tinggal di disable aj viewnya jdny sebenerny gausah
            else if (currentStep >= plant.extractionSteps.Length)
            {
                dialogueText.text = "You've successfully extracted the plant!";
            } //kl salah 
            else
            {
                Debug.Log("What we chose: " + currentItemScript.tool.toolName);
                Debug.Log("What it's supposed to be: " + plant.extractionSteps[currentStep]);
                amyImage.sprite = amySprites[0];
                if (amyTalkingCoroutine != null)
                {
                    // If amyTalkingCoroutine is not null, stop it before starting a new one
                    StopCoroutine(amyTalkingCoroutine);
                }
                amyTalkingCoroutine = StartCoroutine(AmyTalking());
                dialogueText.text = "";
                dialogueText.text = string.Empty;
                dialogueText.text = "That doesn't seem right... Let's try another tool!";
            }
        }
    }

    public IEnumerator AmyTalking()
    {
        float duration = 2f;
        float elapsed = 0f;
        float interval = 0.1f;
        bool mouthOpen = false;

        while (elapsed < duration)
        {
            mouthOpen = !mouthOpen;
            amyImage.sprite = amySprites[mouthOpen ? 1 : 0];
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        // Always end on mouth closed
        amyImage.sprite = amySprites[0];
    }
}
