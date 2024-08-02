using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoom : MonoBehaviourPunCallbacks
{
    public Transform content; // Assign the Content GameObject of the Scroll View here
    public Text roomNameTemplate; // Assign the Text template here
    public Button joinRoomButton;

    private string selectedRoomName;

    private void Start()
    {
        joinRoomButton.interactable = false; // Disable join button initially
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject); // Clear previous list
            Debug.Log("OnRoomListUpdate is running");
        }

        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList)
                continue;

            Text roomNameText = Instantiate(roomNameTemplate, content);
            roomNameText.text = room.Name;
            roomNameText.gameObject.SetActive(true); // Ensure the text is visible

            // Assign a click event to select the room
            roomNameText.GetComponent<Button>().onClick.AddListener(() => OnRoomSelected(room.Name));
            Debug.Log("OnRoomListUpdate is running 2");
        }
    }

    public void OnRoomSelected(string roomName)
    {
        selectedRoomName = roomName;
        joinRoomButton.interactable = true; // Enable join button
    }

    public void OnJoinRoomButtonClicked()
    {
        if (!string.IsNullOrEmpty(selectedRoomName))
        {
            PhotonNetwork.JoinRoom(selectedRoomName);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        // Load the game scene or handle post-room-joining logic
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
        // Handle failure case, like displaying a message to the user
    }
}
