using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    float fSatisfaction = 0.5f;
    public float fMoney = 25f;
    float fPopulation = 1f;
    float fFood = 10f;
    float fNumBuilding = 0f;

    Image satisfactionIm;
    Text moneyT;
    Text populationT;
    Text foodT;

    public float speed = 1f;

    public bool sampling = false;

    GameObject samplingObject;

    bool waited = true;

	// Use this for initialization
	void Start () {
        IntialiseObjects();
    }

    private void OnEnable()
    {
        Selection.onSample += StatChange;
        Restoration.onPurchase += StatChange;
    }

    private void OnDisable()
    {
        Selection.onSample -= StatChange;
        Restoration.onPurchase -= StatChange;
    }

    void StatChange(string stat, float change)
    {
        switch (stat)
        {
            case "Building":
                fNumBuilding += change;
                break;
            case "Money":
                fMoney += change;
                break;
            case "Food":
                fFood += change;
                break;
            case "Population":
                fPopulation += change;
                break;
            case "Satisfaction":
                fSatisfaction += change;
                break;
        }
    }

    void IntialiseObjects()
    {
        samplingObject = GameObject.Find("SamplingPanel");

        satisfactionIm = GameObject.Find("HappinessMeter").GetComponent<Image>();
        moneyT = GameObject.Find("Money").GetComponent<Text>();
        populationT = GameObject.Find("Population").GetComponent<Text>();
        foodT = GameObject.Find("Food").GetComponent<Text>();
    }

    public void Sampling()
    {
        if (!sampling)
        {
            sampling = true;
        }
        else
        {
            sampling = false;
        }

    }

    void SetMeters()
    {
        satisfactionIm.fillAmount = fSatisfaction;
        int iMoney = (int)fMoney;
        moneyT.text = "Money: " + iMoney.ToString();
        populationT.text = "Population: " + fPopulation.ToString();
        int iFood = (int)fFood;
        foodT.text = "Food: " + iFood.ToString();
    }

    void CalculateSatisfaction()
    {
        if (fFood > fPopulation)
        {
            fSatisfaction += 0.01f * Time.deltaTime;
        }
        else
        {
            fSatisfaction -= 0.01f * Time.deltaTime;
        }
        if(fSatisfaction > 1f)
        {
            fSatisfaction = 1f;
        }
        if (fSatisfaction <= 0f)
        {
            Application.LoadLevel("Lose");
        }
        //Debug.Log(fSatisfaction);
    }

    IEnumerator TakeAwayFood()
    {
        if (waited)
        {
            waited = false;
            yield return new WaitForSeconds(10f);
            if (fSatisfaction > 0f)
            {
                if (fSatisfaction > 0.5f)
                {
                    if (fSatisfaction > 0.7f)
                    {
                        fFood -= 4 * fPopulation;
                    }
                    else
                    {
                        fFood -= 3 * fPopulation;
                    }
                }
                else
                {
                    fFood -= 2 * fPopulation;
                }
            }
            waited = true;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        StartCoroutine(TakeAwayFood());
        //fFood -= (speed * Random.Range(0.5f, 0.7f)) * Time.deltaTime * fPopulation;
        fFood += (speed */* Random.Range(0.5f, 0.7f)*/ 0.3f) * Time.deltaTime * fNumBuilding;
        Debug.Log(((speed * 0.6f) * Time.deltaTime * fPopulation) - (speed */* Random.Range(0.5f, 0.7f)*/ 0.5f) * Time.deltaTime * fNumBuilding);
        if (fFood <= 0)
        {
            fFood = 0f;
        }

        //fMoney += (speed * 0.75f) * Time.deltaTime * fPopulation;
        //fMoney -= (speed * 0.45f) * Time.deltaTime * fNumBuilding;
        CalculateSatisfaction();
        SetMeters();
        samplingObject.SetActive(sampling);
    }
}
