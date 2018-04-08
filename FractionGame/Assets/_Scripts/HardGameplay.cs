﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HardGameplay : MonoBehaviour {

    public GameObject time;
    public GameObject Finish;
    public Text score;

    private int level; // 0: easy, 1: medium, 2: hard
    private bool relax = false; // relax false means challenge
    private List<GameObject> questions = new List<GameObject>();
    private List<float> numbers = new List<float>();
    private List<int> numerators = new List<int>();
    private List<int> denumerators = new List<int>();
    private List<GameObject> img = new List<GameObject>();
    private List<GameObject> imgDisabled = new List<GameObject>();
    private List<GameObject> imgChosen = new List<GameObject>();
    private List<GameObject> finishObject = new List<GameObject>();

    private int selected = -1;

    void Start () {
        PlayerPrefs.SetInt("selected", -1);

        level = PlayerPrefs.GetInt("level");

        if (PlayerPrefs.GetInt("mode") == 0)
        {
            relax = true;
        }

        // if relax then hide time
        if (relax)
        {
            time.SetActive(false);
        }
        else
        {
            time.SetActive(true);
        }

        transform.gameObject.SetActive(true);
        foreach (RectTransform t in transform)
        {
            questions.Add(t.gameObject);
        }

        for (int i = 0; i < questions.Count; i++)
        {
            List<GameObject> items = new List<GameObject>();
            foreach (RectTransform t in questions[i].transform)
            {
                items.Add(t.gameObject);
            }
            items[0].SetActive(false);
            imgDisabled.Add(items[0]); // add ImgDisabled to list

            items[1].SetActive(false);
            imgChosen.Add(items[1]); // add ImgChosen to list

            img.Add(items[2]); // add Img to list

            List<GameObject> subitems = new List<GameObject>();
            foreach (RectTransform t in items[3].transform)
            {
                subitems.Add(t.gameObject);
            }

            // initialize fraction value and images
            if (i == 0)
            {
                subitems[0].GetComponent<InputField>().text = "".ToString();
                subitems[2].GetComponent<InputField>().text = "".ToString();

                numbers.Add((float)4 / (float)8);
                numerators.Add(-1);
                denumerators.Add(-1);
            }
            else if (i == 1)
            {
                subitems[0].GetComponent<InputField>().text = "".ToString();
                subitems[2].GetComponent<InputField>().text = "".ToString();

                numbers.Add((float)10 / (float)12);
                numerators.Add(-1);
                denumerators.Add(-1);
            }
            else if (i == 2)
            {
                subitems[0].GetComponent<InputField>().text = "".ToString();
                subitems[2].GetComponent<InputField>().text = "".ToString();

                numbers.Add((float)1 / (float)5);
                numerators.Add(-1);
                denumerators.Add(-1);
            }
            else if (i == 3)
            {
                subitems[0].GetComponent<InputField>().text = "".ToString();
                subitems[2].GetComponent<InputField>().text = "".ToString();

                numbers.Add((float)2 / (float)5);
                numerators.Add(1);
                denumerators.Add(-1);
            }
            else if (i == 4)
            {
                subitems[0].GetComponent<InputField>().text = "".ToString();
                subitems[2].GetComponent<InputField>().text = "".ToString();

                numbers.Add((float)5 / (float)6);
                numerators.Add(-1);
                denumerators.Add(-1);
            }
            else if (i == 5)
            {
                subitems[0].GetComponent<InputField>().text = "".ToString();
                subitems[2].GetComponent<InputField>().text = "".ToString();

                numbers.Add((float)2 / (float)10);
                numerators.Add(-1);
                denumerators.Add(-1);
            }
            else if (i == 6)
            {
                subitems[0].GetComponent<InputField>().text = "".ToString();
                subitems[2].GetComponent<InputField>().text = "".ToString();

                numbers.Add((float)4 / (float)10);
                numerators.Add(-1);
                denumerators.Add(-1);
            }
            else // (i == 7)
            {
                subitems[0].GetComponent<InputField>().text = "".ToString();
                subitems[2].GetComponent<InputField>().text = "".ToString();

                numbers.Add((float)3 / (float)6);
                numerators.Add(-1);
                denumerators.Add(-1);
            }
        }

        for (int i = 0; i < 8; i++)
        {
            PlayerPrefs.SetInt(i.ToString(), 0);
        }

    }
	
	// Update is called once per frame
	void Update () {
        int prefs = PlayerPrefs.GetInt("selected");

        if (prefs > -1 && prefs == selected) // unselect an image
        {
            img[selected].SetActive(true);
            imgChosen[selected].SetActive(false);
            imgDisabled[selected].SetActive(false);

            // reset selected
            selected = -1;

            // reset shared preference
            PlayerPrefs.SetInt("selected", -1);
        }
        else if (prefs > -1 && prefs != selected)
        {
            if (selected == -1)
            {
                selected = prefs;
            }
            else
            {
                int score_value;
                float selected_value = (float)numerators[selected] / (float)denumerators[selected];
                float prefs_value = (float)numerators[prefs] / (float)denumerators[prefs];
                if (numbers[selected] == numbers[prefs] && numbers[selected] == selected_value && numbers[prefs] == prefs_value)
                {
                    score_value = (Int32.Parse(score.text) + 10);
                    PlayerPrefs.SetInt(selected.ToString(), 1); // 1 means has been select and cannot be select anymore
                    PlayerPrefs.SetInt(prefs.ToString(), 1); // 1 means has been select and cannot be select anymore

                    // turn on disable mode for images
                    img[selected].SetActive(false);
                    imgChosen[selected].SetActive(false);
                    imgDisabled[selected].SetActive(true);

                    img[prefs].SetActive(false);
                    imgChosen[prefs].SetActive(false);
                    imgDisabled[prefs].SetActive(true);

                    // check if all has been disabled
                    bool allDisabled = true;
                    for (int i = 0; i < 8 && allDisabled; i++)
                    {
                        if (PlayerPrefs.GetInt(i.ToString()) == 0)
                        {
                            allDisabled = false;
                        }
                    }

                    // go to next level
                    if (allDisabled)
                    {
                        foreach (RectTransform t in Finish.transform)
                        {
                            finishObject.Add(t.gameObject);
                        }
                        finishObject[5].GetComponent<Text>().text = score_value.ToString();
                        Finish.SetActive(true);
                    }
                }
                else
                {
                    score_value = (Int32.Parse(score.text) - 10);

                    // turn on normal mode for image
                    img[selected].SetActive(true);
                    imgChosen[selected].SetActive(false);
                    imgDisabled[selected].SetActive(false);

                    img[prefs].SetActive(true);
                    imgChosen[prefs].SetActive(false);
                    imgDisabled[prefs].SetActive(false);
                }

                // print the score to UI
                score.text = score_value.ToString();

                // reset selected
                selected = -1;
            }

            // reset shared preference
            PlayerPrefs.SetInt("selected", -1);
        }
    }

    public void HandleButton(int idx)
    {
        // if has not been disabled
        if (PlayerPrefs.GetInt(idx.ToString()) == 0)
        {
            int imageTag = idx;

            // set image to choose mode
            img[imageTag].SetActive(false);
            imgDisabled[imageTag].SetActive(false);
            imgChosen[imageTag].SetActive(true);

            // set index of chosen image to shared preference
            PlayerPrefs.SetInt("selected", idx);
        }
    }

    public void UpdateNum(GameObject gameObject)
    {
        int tag = gameObject.tag[0] - '0';
        if (tag >= 0 && tag < 8)
        {
            if (gameObject.name == "Num")
            {
                numerators[tag] = Int32.Parse(gameObject.GetComponent<InputField>().text);
                Debug.Log("num:" + numerators[tag]);
            }
            else
            {
                denumerators[tag] = Int32.Parse(gameObject.GetComponent<InputField>().text);
                Debug.Log("denum:" + denumerators[tag]);
            }
        }
    }
}