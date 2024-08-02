using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviourPunCallbacks
{
    public Transform content; // Assign the content panel of the scroll view
    public GameObject roomButtonPrefab; // Assign the prefab for a room button

    private Dictionary<string, RoomInfo> roomListings = new Dictionary<string, RoomInfo>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Clear out-of-date listings
        foreach (RoomInfo room in roomList)
        {
            if (roomListings.ContainsKey(room.Name))
            {
                roomListings.Remove(room.Name);
            }

            if (room.PlayerCount > 0 && !roomListings.ContainsKey(room.Name))
            {
                roomListings.Add(room.Name, room);
            }
        }

        // Update UI
        UpdateRoomListUI();
    }

    private void UpdateRoomListUI()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<string, RoomInfo> entry in roomListings)
        {
            GameObject roomButton = Instantiate(roomButtonPrefab, content);
            roomButton.GetComponentInChildren<Text>().text = entry.Key;

            roomButton.GetComponent<Button>().onClick.AddListener(() => {
                PhotonNetwork.JoinRoom(entry.Key);
            });
        }
    }
}
