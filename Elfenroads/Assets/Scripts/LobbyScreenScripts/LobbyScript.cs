using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class LobbyScript : MonoBehaviour
{

    public TMPro.TMP_Text infoText;
    public Button createSessionButton;
    public Button refreshButton;
    
    public GameObject tableRow;
    public GameObject tableRowPrefab;
    public GameObject launchButton;
    public GameObject joinButton;

    private Client thisClient;
    
    void Start()
    {
        //Create Client object and get the button.
        thisClient = Client.Instance();
        createSessionButton = GameObject.Find("CreateButton").GetComponent<Button>();
        refreshButton = GameObject.Find("RefreshButton").GetComponent<Button>();


        //Add onclick listener to button for "login" function.
        createSessionButton.onClick.AddListener(createSessionAttempt);
        refreshButton.onClick.AddListener(refreshSessions);
        
        thisClient.RefreshSuccessEvent += refreshSuccess;
        thisClient.RefreshFailureEvent += refreshFailure;
        thisClient.CreateSuccessEvent += createSuccessResult;
        thisClient.CreateFailureEvent += createFailure;

    }

    private void createSessionAttempt(){
        thisClient.createSession(); 
    }

    private void createFailure(string inputError){
        Debug.Log(inputError);
    }

    private void createSuccessResult(string result){
        thisClient.hasSessionCreated = true;
        Debug.Log(result);
    }

    private void refreshSessions(){
        thisClient.refreshSessions();
    }

    private void refreshFailure(string error){
        Debug.Log("Error in refresh: " + error);
    }

    private void refreshSuccess(string result){
        Debug.Log(result);

        var jsonString = result.Replace('"', '\"');
        //var jsonString = result;

        //After getting a bunch of sessions, we need to use the result string to create rows.
        //From the result string, we only need to store the session ID, launch state, host player and players somewhere - parameters will be the same for all games so those don't matter. (THOUGH LATER WILL NEED TO DEAL WITH SAVEFILES HERE)
        JObject myObj = JObject.Parse(jsonString); 
        JObject trueObj = JObject.Parse(myObj["sessions"].ToString()); 
        List<string> sessionIDs = new List<string>();
        foreach(JProperty prop in trueObj.Properties()){
            sessionIDs.Add(prop.Name);
        }
        List<Session> foundSessions = new List<Session>();
        foreach(string ID in sessionIDs){
            foundSessions.Add(new Session(ID, trueObj[ID]["creator"].ToString(), trueObj[ID]["players"].ToString(), trueObj[ID]["launched"].ToString()));
            if(trueObj[ID]["creator"].ToString() == thisClient.thisPlayer.getName()){
                thisClient.hasSessionCreated = true; //If our client is a host in one of the recieved session
            }
        }
        thisClient.sessions = foundSessions; //Set the client's new list of found sessions.
        displaySessions(foundSessions);
        
        //Call something here to visually update the rows of the table based on the client info (excluding launched sessions). These rows should include a "Launch" button if it is the current client's session, and a "join" button otherwise.

    }

    /*
    public void enableButton(onButton) {
        onButton.setActive(true)
    }

    public void disableButton(offButton) {
        offButton.setActive(false)
    }
    */
    
    private void displaySessions(List<Session> foundSessions) {
        foreach (Transform child in tableRow.transform) {
            Destroy(child.gameObject);
        }

        foreach(Session session in foundSessions) {
            //Make the new row.
            GameObject instantiatedRow = Instantiate(tableRowPrefab, tableRow.transform); //0 is hostname, 1 is ready players
            //Set the strings for "Hostname" and "readyPlayers"
            instantiatedRow.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = session.hostPlayerName;
            instantiatedRow.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = session.players.Count + "/6";

            //Based on session attributes, decide what button (if any) should be added.
            if (Client.Instance().thisPlayer.getName() == session.hostPlayerName && session.players.Count >= 2) {
                GameObject instantiatedButton = Instantiate(launchButton, instantiatedRow.transform);
            } else if (Client.Instance().thisPlayer.getName() == session.hostPlayerName && session.players.Count < 2) {

            } else {
                GameObject instantiatedButton = Instantiate(joinButton, instantiatedRow.transform);
            }
        }
    }

}
