using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Models;

public class FaceUpCountersView : MonoBehaviour
{
    //Need public UI elements here, which will actually display the model (DrawCountersModel, below) to the screen.
    //Eg. public GridLayoutGroup layout; and several prefabs for the different types of counters.

    private DrawCountersController myController;
    private GameObject sessionInfo;
    private DrawCounters drawCountersModel;

    public GameObject dragonCounterPrefab;
    public GameObject trollCounterPrefab;
    public GameObject cloudCounterPrefab;
    public GameObject cycleCounterPrefab;
    public GameObject unicornCounterPrefab;
    public GameObject pigCounterPrefab;
    public GameObject landObstaclePrefab;


    void Start(){
        myController = GameObject.Find("DrawCountersController").GetComponent<DrawCountersController>();
        sessionInfo = GameObject.Find("SessionInfo");
    }

    public void setAndSubscribeToModel(DrawCounters inputDrawCounters){
         drawCountersModel = inputDrawCounters;
         drawCountersModel.Updated += onModelUpdated;
         //onModelUpdated(null, null);
     }

    void onModelUpdated(object sender, EventArgs e) {
        //Here, needs to add counters to the GridLayoutGroup according to the model. Instantiated Counters must also have their "CounterViewHelper" component's "Guid" fields set appropriately.

        //First, destroy all children (mwahahah)
        while(this.transform.childCount > 0){
            Destroy(this.transform.GetChild(0));
        }

        //Now, loop through the counters of the model, instantiating appropriate counters each time.
        foreach(Counter c in drawCountersModel.faceUpCounters){
            switch(c){
                case TransportationCounter tc:
                {
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {  
                           GameObject instantiatedCounter = Instantiate(dragonCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            GameObject instantiatedCounter = Instantiate(cycleCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            GameObject instantiatedCounter = Instantiate(cloudCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            GameObject instantiatedCounter = Instantiate(trollCounterPrefab, this.transform);
                         instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            GameObject instantiatedCounter = Instantiate(pigCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            GameObject instantiatedCounter = Instantiate(unicornCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
                            break;
                        }
                        default: Debug.Log("Model transportation counter of type raft! This is not allowed!") ; break;
                    }
                    break;
                }
                case MagicSpellCounter msc:
                {
                    Debug.Log("Elfengold - Do later");
                    break;
                }
                case GoldCounter gc:
                {
                    Debug.Log("Elfengold - Do later");
                    break;
                }
                case ObstacleCounter obc:
                {
                    //*** Add sea obstacle later, during elfengold.
                    GameObject instantiatedCounter = Instantiate(landObstaclePrefab, this.transform);
                    instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
                    break;
                }
                default: Debug.Log("Counter is of undefined type!") ; break;
            }
        }
    }

    //Called by the Counter GridElements (those instantiated by this script, and which will be made children of the GridLayoutGroup). Will be used to identify which counter was clicked.
    public void CounterClicked(GameObject clickedCounter){
        if(drawCountersModel == null){
            Debug.Log("Yay!");
            return;
        }
        
        if( (drawCountersModel.currentPlayer.name == sessionInfo.GetComponent<SessionInfo>().getClient().clientCredentials.username) && Elfenroads.Model.game.currentPhase is DrawCounters){
            Debug.Log(clickedCounter);

            //Find the counter here

            myController.validateDrawCounter();

        }else{
            Debug.Log("Click invalid. Is it your turn? -> " + (drawCountersModel.currentPlayer.name == sessionInfo.GetComponent<SessionInfo>().getClient().clientCredentials.username));
            Debug.Log("If so, then it is because the currentPhase is " + Elfenroads.Model.game.currentPhase);
        }
    }

    //Called by some other button or object representing a random draw, which has an Event trigger leading here.
    public void RandomCounterClicked(){
        if(drawCountersModel == null){
            Debug.Log("Yay!");
            return;
        }
        if( (drawCountersModel.currentPlayer.name == sessionInfo.GetComponent<SessionInfo>().getClient().clientCredentials.username) && Elfenroads.Model.game.currentPhase is DrawCounters){
            myController.validateDrawRandomCounter();

        }else{
            Debug.Log("Click invalid. Is it your turn? -> " + (drawCountersModel.currentPlayer.name == sessionInfo.GetComponent<SessionInfo>().getClient().clientCredentials.username));
            Debug.Log("If so, then it is because the currentPhase is " + Elfenroads.Model.game.currentPhase);
        }
    }
}
