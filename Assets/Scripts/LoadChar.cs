using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadChar : MonoBehaviour
{
    public GameObject[] CharPrefabs;
    public Transform spawnPoint;
    //public TMP_Text label;

    // Start is called before the first frame update
    void Start()
    {
        int selectedChar = PlayerPrefs.GetInt("selectedCharacter");
        GameObject preFab = CharPrefabs[selectedChar];
        GameObject clone = Instantiate(preFab, spawnPoint.position, Quaternion.identity);
        //label.text = preFab.name;
    }

}
