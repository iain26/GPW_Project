using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour {

    public List<GameObject> hair = new List<GameObject>();
    GameObject hairObject;
    int hairIndex;

    public List<GameObject> skin = new List<GameObject>();
    GameObject skinObject;
    int skinIndex;

    public List<GameObject> shirt = new List<GameObject>();
    GameObject shirtObject;
    int shirtIndex;

    int typeIndex = 0;

    public bool userControlled = false;

    //public bool showBackOfCard = true;

    // Use this for initialization
    void Start () {

        skinObject = gameObject.transform.GetChild(1).gameObject;
        for (int i = 0; i < skinObject.transform.childCount; i++)
        {
            skin.Add(skinObject.transform.GetChild(i).gameObject);
        }
        skinIndex = Random.Range(0, skin.Count);
        skin[skinIndex].SetActive(true);

        hairObject = gameObject.transform.GetChild(2).gameObject;
        for (int i = 0; i < hairObject.transform.childCount; i++)
        {
            hair.Add(hairObject.transform.GetChild(i).gameObject);
        }
        hairIndex = Random.Range(0, hair.Count);
        hair[hairIndex].SetActive(true);

        shirtObject = gameObject.transform.GetChild(3).gameObject;
        for (int i = 0; i < shirtObject.transform.childCount; i++)
        {
            shirt.Add(shirtObject.transform.GetChild(i).gameObject);
        }
        shirtIndex = Random.Range(0, shirt.Count);
        shirt[shirtIndex].SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (userControlled)
        {
            switch (typeIndex)
            {
                case 0:
                    Select(hair, hairIndex, "Hair");
                    break;
                case 1:
                    Select(skin, skinIndex, "Skin");
                    break;
                case 2:
                    Select(shirt, shirtIndex, "Shirt");
                    break;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                typeIndex--;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                typeIndex++;
            }
            if (typeIndex > 2)
                typeIndex = 0;
            if (typeIndex < 0)
                typeIndex = 2;
        }

        if (hair[hairIndex].name.Contains("Cyan"))
            GetComponent<Card>().hairColour = "Cyan";
        if (hair[hairIndex].name.Contains("Green"))
            GetComponent<Card>().hairColour = "Green";
        if (hair[hairIndex].name.Contains("Pink"))
            GetComponent<Card>().hairColour = "Pink";

        if (shirt[shirtIndex].name.Contains("Blue"))
            GetComponent<Card>().shirtColour = "Blue";
        if (shirt[shirtIndex].name.Contains("Orange"))
            GetComponent<Card>().shirtColour = "Orange";
        if (shirt[shirtIndex].name.Contains("Yellow"))
            GetComponent<Card>().shirtColour = "Yellow";
    }

    void Select(List<GameObject> visual, int index , string type)
    {
        //if (showBackOfCard)
        //    transform.GetChild(4).gameObject.SetActive(true);
        //else
        //    transform.GetChild(4).gameObject.SetActive(false);

        for (int i = 0; i < visual.Count; i++)
        {
            visual[i].SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            index++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            index--;
        }
        if (index >= visual.Count)
            index = 0;
        if (index < 0)
            index = visual.Count - 1;
        visual[index].SetActive(true);
        switch (type)
        {
            case "Skin":
                skinIndex = index;
                break;
            case "Shirt":
                shirtIndex = index;
                break;
            case "Hair":
                hairIndex = index;
                break;
        }
    }
}
