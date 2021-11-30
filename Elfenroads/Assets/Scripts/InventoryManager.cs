using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject CounterManagerObserver;
    private List<GameObject> townPieceList = new List<GameObject>();
    private GameObject TownPieceCounter;
    // Start is called before the first frame update
    void Start()
    {
        TownPieceCounter.GetComponent<TextMeshProUGUI>().SetText("1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTownPieceCounter(GameObject tpCounter){
        this.TownPieceCounter = tpCounter;
    }

    public void addTownPiece(GameObject townPiece){
        this.townPieceList.Add(townPiece);
        TownPieceCounter.GetComponent<TextMeshProUGUI>().SetText(countTownPiece().ToString());
    }

    public int countTownPiece(){
        return townPieceList.Count;
    }
}