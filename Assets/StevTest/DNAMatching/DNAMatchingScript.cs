using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollViewScript : MonoBehaviour, IEndDragHandler
{
    public int numberOfDNA;
    //When the sprite is done this will be changed to sprite
    Color[] dnaColors = new Color[]{
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow
    };
    [SerializeField] Transform targetDNAPanel;
    [SerializeField] Transform content;
    [SerializeField] GameObject dnaPrefab;
    int correctTile;
    // Start is called before the first frame update
    void Start()
    {
        correctTile = -1;
        //each dna tile is 100 with 10 px spacing. The horizontal group "has" 10 px paddingn on each side
        // meaning ths first tile needs to be 110 + 10 px away, and the last tile needs to be 100 + 10 px away
        //...idk it just works pokoknya gt lah :sob:
        content.GetComponent<RectTransform>().sizeDelta = new Vector2((110*numberOfDNA) + 230, 0);
        GenerateTargetDNA();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnValueChange(Vector2 value)
    {
        //put tick sound effect everytime it hits a new tile

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
        for(int i = 0; i < 4; i++){
            targetDNA += Random.Range(0, 4);
            GameObject tempObject = Instantiate(dnaPrefab, targetDNAPanel);
            //if we want to do the opposite thinggy you can just change the color of the target from here
            tempObject.GetComponent<Image>().color = dnaColors[targetDNA[i] - '0'];
        }
        Debug.Log("Target DNA: " + targetDNA);

        //create 15 random DNA string
        for(int i = 0; i < numberOfDNA; i++){
            fullDNA += Random.Range(0, 4);
        }

        //if the fullDNA doesnt contain the targetDNA, add it to random index and remove the excess (4 chars)
        if(!fullDNA.Contains(targetDNA)){
            correctTile = Random.Range(0,numberOfDNA-3);
            fullDNA = fullDNA.Insert(correctTile, targetDNA);
            //remove 4 last characters
            fullDNA = fullDNA.Substring(0, fullDNA.Length - 4);
            Debug.Log("len" + fullDNA.Length);
            // Debug.Log("Dna length " + fullDNA.Length);
            // correctTile--;;
            Debug.Log("New DNA: " + fullDNA);
            Debug.Log("Correct Tile: " + correctTile);
        }else{
            //else, find the index of the targetDNA
            //very rare edge case
            Debug.Log("yoo rare case where dna alrdy exists very cool!");
            correctTile = fullDNA.IndexOf(targetDNA);
            Debug.Log("Correct Tile: " + correctTile);
        }

        foreach(char c in fullDNA){
            GameObject tempObject = Instantiate(dnaPrefab, content);
            tempObject.GetComponent<Image>().color = dnaColors[c - '0'];
        }
        
        //set random position
        content.localPosition = new Vector3(Random.Range(0, numberOfDNA) * 110f, content.localPosition.y, content.localPosition.z);
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
            Debug.Log("This is the final output for this minigame, call ur next functions from here");
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        StopAllCoroutines();
        StartCoroutine(ScrollRectSnap());
    }

    IEnumerator ScrollRectSnap()
    {
        //snap to the nearest tile, the -40 is for liniency so it can snap forward
        int tile = (int)((content.localPosition.x - 40)/110);
        if(content.localPosition.x >= 0){
            //too far to the front, just let unity snap it back
            //but we still need to process the tile
            tile = 0;
        }else if(content.localPosition.x <= (content.GetComponent<RectTransform>().sizeDelta.x-670) * -1){
            //...dont ask why its 670 idk either
            //negative cuz all of our tiles are negative
            //too far to the back, just let unity snap it back
            //but we still need to process the tile
            tile = 11;
        }else{
            //middle tile, we manually snap it
            float target = tile * 110;
            Debug.Log("Tile: " + tile + " Target: " + target);
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
}
