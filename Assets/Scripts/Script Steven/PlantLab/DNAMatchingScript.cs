using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DNAMatchingScript : MonoBehaviour, IEndDragHandler
{
    public int numberOfDNA;
    //When the sprite is done this will be changed to sprite
    [Header("Red, Green, Blue, Yellow")]
    [SerializeField] Sprite[] dnaSprites = new Sprite[4];
    [SerializeField] GameObject dnaMatchPanel;
    [SerializeField] Transform targetDNAPanel;
    [SerializeField] Transform content;
    [SerializeField] GameObject dnaPrefab;
    int correctTile;
    float tileWidth = 240f; // width of each DNA tile including spacing

    void OnValueChange(Vector2 value)
    {
        //OnValueChange is called everytime the scroll rect is moved
        //put tick sound effect everytime it hits a new tile
    }

    public void StartDNAExtraction(){
        tileWidth = 240f;
        //make the scroll rect draggable
        GetComponent<ScrollRect>().horizontal = true;
        correctTile = -1;
        //each dna tile is 100 with 10 px spacing. The horizontal group "has" 10 px paddingn on each side
        // meaning ths first tile needs to be 110 + 10 px away, and the last tile needs to be 10 + 100 + 10 (one block + edge space) px away
        //...idk it just works pokoknya gt lah :sob:
        content.GetComponent<RectTransform>().sizeDelta = new Vector2((tileWidth*(numberOfDNA-3))+ 1400f, 0);

        // 3856
        GenerateTargetDNA();
    }

    [ContextMenu("Generate DNA")]
    public void GenerateTargetDNA(){
        
        //reset UI
        foreach(Transform dna in targetDNAPanel){
            if(dna.name != "DNAStrandSprite"){
                Destroy(dna.gameObject);
            }
        }

        foreach(Transform dna in content){
            if(dna.name != "DNAStrandSprite"){
                Destroy(dna.gameObject);
            }
        }

        //generate 4(maybe we can change this too) random acgt as target
        //generate random acgt as one strip
        //check if the strip has the target
        //if not, add the target to random index from 0-(total-3) cuz range is inclusive
        
        string targetDNA = "";
        string fullDNA = "";
        for (int i = 0; i < 4; i++)
        {
            targetDNA += Random.Range(0, 4);
            GameObject tempObject = Instantiate(dnaPrefab, targetDNAPanel);
            //if we want to do the opposite thinggy you can just change the color of the target from here
            tempObject.GetComponent<Image>().sprite = dnaSprites[3 - (targetDNA[i] - '0')];
            tempObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 180);
        }
        // Debug.Log("Target DNA: " + targetDNA);

        //create 15 random DNA string
        for(int i = 0; i < numberOfDNA; i++){
            fullDNA += Random.Range(0, 4);
        }

        //if the fullDNA doesnt contain the targetDNA, add it to random index and remove the excess (4 chars)
        if(!fullDNA.Contains(targetDNA)){
            correctTile = Random.Range(1,numberOfDNA-3);
            fullDNA = fullDNA.Insert(correctTile, targetDNA);
            //remove 4 last characters
            fullDNA = fullDNA.Substring(0, fullDNA.Length - 4);
            // Debug.Log("len" + fullDNA.Length);
            // Debug.Log("Dna length " + fullDNA.Length);
            // correctTile--;;
            // Debug.Log("New DNA: " + fullDNA);
            Debug.Log("Correct Tile: " + correctTile);
        }else{
            //else, find the index of the targetDNA
            //very rare edge case
            Debug.Log("yoo rare case where dna alrdy exists very cool!");
            correctTile = fullDNA.IndexOf(targetDNA);
            // Debug.Log("Correct Tile: " + correctTile);
        }

        foreach(char c in fullDNA){
            GameObject tempObject = Instantiate(dnaPrefab, content);
            tempObject.GetComponent<Image>().sprite = dnaSprites[c - '0'];
        }
        
        //set random position
        content.localPosition = new Vector3(Random.Range(0, numberOfDNA) * 110f, content.localPosition.y, content.localPosition.z);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("End Drag");
        StopAllCoroutines();
        StartCoroutine(ScrollRectSnap());
    }

    IEnumerator ScrollRectSnap()
    {
        //snap to the nearest tile, the -40 is for liniency so it can snap forward
        int tile = (int)((content.localPosition.x)/(tileWidth));
        if(content.localPosition.x >= 0){
            //too far to the front, just let unity snap it back
            //but we still need to process the tile
            tile = 0;
        }else if(content.localPosition.x <= -(tileWidth * (numberOfDNA - 3))){
            Debug.Log("Too far to the back, snapping to last tile");
            Debug.Log("local pos: " + content.localPosition.x);
            Debug.Log("trest: " + (content.GetComponent<RectTransform>().sizeDelta.x-(4*tileWidth)));
            //...dont ask why its 670 idk either
            //negative cuz all of our tiles are negative
            //too far to the back, just let unity snap it back
            //but we still need to process the tile
            tile = numberOfDNA - 3;
        }else{
            Debug.Log("Tile: " + tile);
            //middle tile, we manually snap it
            float target = tile * tileWidth;
            // Debug.Log("Tile: " + tile + " Target: " + target);
            float t = 0;
            while(t < 0.5){
                t += Time.deltaTime;
                content.localPosition = new Vector3(Mathf.SmoothStep(content.localPosition.x, target, t/0.5f), content.localPosition.y, content.localPosition.z);
                yield return null;
            }
        }
        //then just submits whatever tile it landed on
        SubmitDNA(tile);
    }
    void SubmitDNA(int tile)
    {
        if(correctTile == -1){
            Debug.LogError("The player shouldnt have been able to move DNA without DNA Target existing");
            return;
        }

        Debug.Log("Tile: " + tile + " Correct Tile: " + correctTile);
        if(Mathf.Abs(tile) == correctTile){
            Debug.Log("Correct!");
            float target = tile * tileWidth;
            content.localPosition = new Vector3(target, content.localPosition.y, content.localPosition.z);
            GetComponent<ScrollRect>().horizontal = false;
            dnaMatchPanel.GetComponent<Animator>().SetTrigger("DNAMatch");
            //stop players from scrolling the rect 
        }
    }
}
