using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using jewelWheels;

public class jewelWheelsScript : MonoBehaviour
{
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable[] wheels;
    public KMSelectable resetButton;
    public KMSelectable submitButton;
    private List<KMSelectable> assignedWheels = new List<KMSelectable>();
    public Renderer outerWheel;
    public Renderer centralOrb;
    public Renderer compassColumn;
    public GameObject redLights;
    public GameObject greenLights;
    public GameObject yellowLights;
    public GameObject blueLights;
    public GameObject pinkLights;
    public GameObject whiteLights;
    public Renderer[] columns;
    public Renderer hinge;
    public Renderer[] treasure;

    //Gemstone textures
    public Texture[] gemstoneOptions;
    public Renderer[] gemstones1;
    public Renderer[] gemstones2;
    public Renderer[] gemstones3;
    public Renderer[] gemstones4;
    private List<Texture> chosenGemstones = new List<Texture>();
    private int gemstoneIndex;
    public int[] gemstoneCount;
    public List<Texture> mostAbundantGemstones = new List<Texture>();
    private int maxIndex = 0;
    private int serialDigit = 0;

    //Runes
    public string[] runeOptions1;
    public string[] runeOptions2;
    public string[] runeOptions3;
    public string[] runeOptions4;
    public TextMesh[] runes1;
    public TextMesh[] runes2;
    public TextMesh[] runes3;
    public TextMesh[] runes4;
    private int runeIndex;

    //Wheel animation
    private int outerOrientation = 0;
    private int wheelIndex = 0;
    private int wheelDistance = 0;
    private bool rotationLock = false;
    private int wheelATurns = 0;
    private int wheelBTurns = 0;
    private int wheelCTurns = 0;
    private int wheelDTurns = 0;
    private int totalRotations = 0;
    private int wheelDistanceA = 0;
    private int wheelDistanceB = 0;
    private int wheelDistanceC = 0;
    private int wheelDistanceD = 0;
    private int fullCircle = 0;
    private bool resetComplete = false;
    public string[] outerOrientationLog;
    public string[] orientations;

    //Priority Lists
    public List<Texture> priorityList1 = new List<Texture>();
    public List<Texture> priorityList2 = new List<Texture>();
    public List<Texture> priorityList3 = new List<Texture>();
    public List<Texture> priorityList4 = new List<Texture>();
    public List<Texture> priorityList5 = new List<Texture>();
    public List<Texture> priorityList6 = new List<Texture>();
    public List<Texture> priorityList7 = new List<Texture>();
    public List<Texture> priorityList8 = new List<Texture>();
    public List<Texture> priorityList9 = new List<Texture>();
    public List<Texture> priorityList10 = new List<Texture>();
    public List<Texture> priorityList11 = new List<Texture>();
    public List<Texture> priorityList12 = new List<Texture>();

    //Answers
    private List<Renderer> correctJewels = new List<Renderer>();
    private int targetOrientation = 0;
    public int[] jewelOrientation;
    public List<Renderer> orderedCorrectJewels = new List<Renderer>();

    //Solved animation
    private int columnDistance = 0;
    private bool striked = false;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved = false;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        resetButton.OnInteract += delegate () { onReset(); return false; };
        submitButton.OnInteract += delegate () { onSubmit(); return false; };
        foreach (KMSelectable wheel in wheels)
        {
            KMSelectable trueWheel = wheel;
            wheel.OnInteract += delegate () { onWheel(trueWheel); return false; };
        }
    }

    void Start()
    {
        redLights.SetActive(false);
        greenLights.SetActive(false);
        yellowLights.SetActive(false);
        blueLights.SetActive(false);
        pinkLights.SetActive(false);
        whiteLights.SetActive(true);
        GetGemstones();
        GetRunes();
        GetOuterOrientation();
        GetTreasure();
        GetCorrectJewels();
        GetTargetOrientation();
        GetWheelCodes();
        GetOrderedJewels();
        GetCorrectJewelStartOrientation();
    }

    void GetGemstones()
    {
        foreach (Renderer gemstone in gemstones1)
        {
            gemstoneIndex = UnityEngine.Random.Range(0,8);
            while (chosenGemstones.Contains(gemstoneOptions[gemstoneIndex]))
            {
                gemstoneIndex = UnityEngine.Random.Range(0,8);
            }
            gemstone.material.mainTexture = gemstoneOptions[gemstoneIndex];
            gemstoneCount[gemstoneIndex]++;
            chosenGemstones.Add(gemstoneOptions[gemstoneIndex]);
        }
        Debug.LogFormat("[Jewel Wheels #{0}] The first wheel (outermost) gemstones are {1}, {2}, {3} & {4}.", moduleId, chosenGemstones[0].name, chosenGemstones[1].name, chosenGemstones[2].name, chosenGemstones[3].name);
        chosenGemstones.Clear();

        foreach (Renderer gemstone in gemstones2)
        {
            gemstoneIndex = UnityEngine.Random.Range(0,8);
            while (chosenGemstones.Contains(gemstoneOptions[gemstoneIndex]))
            {
                gemstoneIndex = UnityEngine.Random.Range(0,8);
            }
            gemstone.material.mainTexture = gemstoneOptions[gemstoneIndex];
            gemstoneCount[gemstoneIndex]++;
            chosenGemstones.Add(gemstoneOptions[gemstoneIndex]);
        }
        Debug.LogFormat("[Jewel Wheels #{0}] The second wheel (second outermost) gemstones are {1}, {2}, {3} & {4}.", moduleId, chosenGemstones[0].name, chosenGemstones[1].name, chosenGemstones[2].name, chosenGemstones[3].name);
        chosenGemstones.Clear();

        foreach (Renderer gemstone in gemstones3)
        {
            gemstoneIndex = UnityEngine.Random.Range(0,8);
            while (chosenGemstones.Contains(gemstoneOptions[gemstoneIndex]))
            {
                gemstoneIndex = UnityEngine.Random.Range(0,8);
            }
            gemstone.material.mainTexture = gemstoneOptions[gemstoneIndex];
            gemstoneCount[gemstoneIndex]++;
            chosenGemstones.Add(gemstoneOptions[gemstoneIndex]);
        }
        Debug.LogFormat("[Jewel Wheels #{0}] The third wheel (second innermost) gemstones are {1}, {2}, {3} & {4}.", moduleId, chosenGemstones[0].name, chosenGemstones[1].name, chosenGemstones[2].name, chosenGemstones[3].name);
        chosenGemstones.Clear();

        foreach (Renderer gemstone in gemstones4)
        {
            gemstoneIndex = UnityEngine.Random.Range(0,8);
            while (chosenGemstones.Contains(gemstoneOptions[gemstoneIndex]))
            {
                gemstoneIndex = UnityEngine.Random.Range(0,8);
            }
            gemstone.material.mainTexture = gemstoneOptions[gemstoneIndex];
            gemstoneCount[gemstoneIndex]++;
            chosenGemstones.Add(gemstoneOptions[gemstoneIndex]);
        }
        Debug.LogFormat("[Jewel Wheels #{0}] The fourth wheel (innermost) gemstones are {1}, {2}, {3} & {4}.", moduleId, chosenGemstones[0].name, chosenGemstones[1].name, chosenGemstones[2].name, chosenGemstones[3].name);
        chosenGemstones.Clear();

        if (gemstoneCount.Where((x) => x.Equals(4)).Count() == 1)
        {
            int maxValue = gemstoneCount.Max();
            int maxIndex = gemstoneCount.ToList().IndexOf(maxValue);
            mostAbundantGemstones.Add(gemstoneOptions[maxIndex]);
            Debug.LogFormat("[Jewel Wheels #{0}] There are four {1} gemstones. These are most abundant.", moduleId, mostAbundantGemstones[0].name);
        }
        else if (gemstoneCount.Where((x) => x.Equals(4)).Count() > 1)
        {
            foreach (int count in gemstoneCount)
            {
                if (count == 4)
                {
                    mostAbundantGemstones.Add(gemstoneOptions[maxIndex]);
                }
                maxIndex++;
            }
            foreach (Texture entry in mostAbundantGemstones)
            {
                Debug.LogFormat("[Jewel Wheels #{0}] There are four {1} gemstones. These are most abundant.", moduleId, entry.name);
            }
        }
        else if (gemstoneCount.Where((x) => x.Equals(3)).Count() == 1)
        {
            int maxValue = gemstoneCount.Max();
            maxIndex = gemstoneCount.ToList().IndexOf(maxValue);
            mostAbundantGemstones.Add(gemstoneOptions[maxIndex]);
            Debug.LogFormat("[Jewel Wheels #{0}] There are three {1} gemstones. These are most abundant.", moduleId, mostAbundantGemstones[0].name);
        }
        else if (gemstoneCount.Where((x) => x.Equals(3)).Count() > 1)
        {
            foreach (int count in gemstoneCount)
            {
                if (count == 3)
                {
                    mostAbundantGemstones.Add(gemstoneOptions[maxIndex]);
                }
                maxIndex++;
            }
            foreach (Texture entry in mostAbundantGemstones)
            {
                Debug.LogFormat("[Jewel Wheels #{0}] There are three {1} gemstones. These are most abundant.", moduleId, entry.name);
            }
        }
        else
        {
            mostAbundantGemstones.AddRange(gemstoneOptions); // All of the gems have been chosen, so there's no reason for this list to be different from the overall gemstoneOptions
            Debug.LogFormat("[Jewel Wheels #{0}] There are two of every gemstone.", moduleId);
        }
    }

    char TranslateRune(string rune)
    {
        var runeTranslationFrom = "abgdezhqiklmnxoprstufcyw";
        var runeTranslationTo = "αβγδεζηθικλμνξοπρστυφχψω";
        return runeTranslationTo[runeTranslationFrom.IndexOf(rune[0])];
    }

    void GetRunes()
    {
        foreach (TextMesh rune in runes1)
        {
            runeIndex = UnityEngine.Random.Range(0,6);
            rune.text = runeOptions1[runeIndex];
        }
        Debug.LogFormat("[Jewel Wheels #{0}] The first wheel runes are {1} & {2}.", moduleId, TranslateRune(runes1[0].text), TranslateRune(runes1[1].text));
        foreach (TextMesh rune in runes2)
        {
            runeIndex = UnityEngine.Random.Range(0,6);
            rune.text = runeOptions2[runeIndex];
        }
        Debug.LogFormat("[Jewel Wheels #{0}] The second wheel runes are {1} & {2}.", moduleId, TranslateRune(runes2[0].text), TranslateRune(runes2[1].text));
        foreach (TextMesh rune in runes3)
        {
            runeIndex = UnityEngine.Random.Range(0,6);
            rune.text = runeOptions3[runeIndex];
        }
        Debug.LogFormat("[Jewel Wheels #{0}] The third wheel runes are {1} & {2}.", moduleId, TranslateRune(runes3[0].text), TranslateRune(runes3[1].text));
        foreach (TextMesh rune in runes4)
        {
            runeIndex = UnityEngine.Random.Range(0,6);
            rune.text = runeOptions4[runeIndex];
        }
        Debug.LogFormat("[Jewel Wheels #{0}] The fourth wheel runes are {1} & {2}.", moduleId, TranslateRune(runes4[0].text), TranslateRune(runes4[1].text));
    }

    void GetOuterOrientation()
    {
        outerOrientation = UnityEngine.Random.Range(0,4);
        if (outerOrientation == 0)
        {
            outerWheel.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f) * outerWheel.transform.localRotation;;
            compassColumn.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f) * compassColumn.transform.localRotation;;
        }
        else if (outerOrientation == 1)
        {
            outerWheel.transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f) * outerWheel.transform.localRotation;;
            compassColumn.transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f) * compassColumn.transform.localRotation;;
        }
        else if (outerOrientation == 2)
        {
            outerWheel.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f) * outerWheel.transform.localRotation;;
            compassColumn.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f) * compassColumn.transform.localRotation;;
        }
        else if (outerOrientation == 3)
        {
            outerWheel.transform.localRotation = Quaternion.Euler(0.0f, 270.0f, 0.0f) * outerWheel.transform.localRotation;;
            compassColumn.transform.localRotation = Quaternion.Euler(0.0f, 270.0f, 0.0f) * compassColumn.transform.localRotation;;
        }
        Debug.LogFormat("[Jewel Wheels #{0}] North is {1}.", moduleId, outerOrientationLog[outerOrientation]);
    }

    public void GetTreasure()
    {
        foreach (Renderer jewel in treasure)
        {
            int treasureIndex = UnityEngine.Random.Range(0,8);
            jewel.material.mainTexture = gemstoneOptions[treasureIndex];
        }
    }

    public void GetCorrectJewels()
    {
        if (runes1[0].text == "a")
        {
            if (runes1[1].text == "a")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList1.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "b")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList9.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "g")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList3.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "d")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList11.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "e")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList6.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "z")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList7.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes1[0].text == "b")
        {
            if (runes1[1].text == "a")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList9.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "b")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList5.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "g")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList6.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "d")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList1.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "e")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList9.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "z")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList2.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes1[0].text == "g")
        {
            if (runes1[1].text == "a")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList3.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "b")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList6.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "g")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList2.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "d")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList7.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "e")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList10.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "z")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList8.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes1[0].text == "d")
        {
            if (runes1[1].text == "a")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList11.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "b")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList1.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "g")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList7.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "d")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList12.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "e")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList4.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "z")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList5.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes1[0].text == "e")
        {
            if (runes1[1].text == "a")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList6.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "b")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList9.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "g")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList10.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "d")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList4.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "e")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList10.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "z")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList12.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes1[0].text == "z")
        {
            if (runes1[1].text == "a")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList7.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "b")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList2.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "g")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList8.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "d")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList5.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "e")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList12.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes1[1].text == "z")
            {
                gemstones1 = gemstones1.OrderBy(renderer => priorityList4.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        correctJewels.Add(gemstones1[0]);


        if (runes2[0].text == "h")
        {
            if (runes2[1].text == "h")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList5.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "q")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList8.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "i")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList3.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "k")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList9.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "l")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList7.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "m")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList4.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes2[0].text == "q")
        {
            if (runes2[1].text == "h")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList8.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "q")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList2.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "i")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList7.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "k")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList1.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "l")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList5.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "m")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList10.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes2[0].text == "i")
        {
            if (runes2[1].text == "h")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList3.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "q")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList7.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "i")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList11.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "k")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList6.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "l")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList12.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "m")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList2.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes2[0].text == "k")
        {
            if (runes2[1].text == "h")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList9.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "q")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList1.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "i")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList6.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "k")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList4.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "l")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList3.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "m")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList8.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes2[0].text == "l")
        {
            if (runes2[1].text == "h")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList7.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "q")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList5.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "i")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList12.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "k")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList3.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "l")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList11.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "m")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList12.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes2[0].text == "m")
        {
            if (runes2[1].text == "h")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList4.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "q")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList10.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "i")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList2.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "k")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList8.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "l")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList12.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes2[1].text == "m")
            {
                gemstones2 = gemstones2.OrderBy(renderer => priorityList9.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        correctJewels.Add(gemstones2[0]);


        if (runes3[0].text == "n")
        {
            if (runes3[1].text == "n")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList12.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "x")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList5.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "o")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList10.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "p")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList1.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "r")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList5.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "s")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList3.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes3[0].text == "x")
        {
            if (runes3[1].text == "n")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList5.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "x")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList2.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "o")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList6.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "p")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList5.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "r")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList11.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "s")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList8.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes3[0].text == "o")
        {
            if (runes3[1].text == "n")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList10.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "x")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList6.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "o")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList8.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "p")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList3.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "r")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList12.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "s")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList2.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes3[0].text == "p")
        {
            if (runes3[1].text == "n")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList1.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "x")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList5.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "o")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList3.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "p")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList11.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "r")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList1.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "s")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList10.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes3[0].text == "r")
        {
            if (runes3[1].text == "n")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList5.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "x")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList11.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "o")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList12.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "p")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList1.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "r")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList4.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "s")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList9.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes3[0].text == "s")
        {
            if (runes3[1].text == "n")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList3.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "x")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList8.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "o")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList2.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "p")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList10.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "r")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList9.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes3[1].text == "s")
            {
                gemstones3 = gemstones3.OrderBy(renderer => priorityList6.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        correctJewels.Add(gemstones3[0]);


        if (runes4[0].text == "t")
        {
            if (runes4[1].text == "t")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList9.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "u")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList4.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "f")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList1.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "c")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList10.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "y")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList6.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "w")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList2.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes4[0].text == "u")
        {
            if (runes4[1].text == "t")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList4.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "u")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList3.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "f")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList7.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "c")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList4.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "y")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList12.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "w")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList8.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes4[0].text == "f")
        {
            if (runes4[1].text == "t")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList1.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "u")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList7.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "f")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList8.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "c")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList11.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "y")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList9.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "w")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList3.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes4[0].text == "c")
        {
            if (runes4[1].text == "t")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList10.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "u")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList4.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "f")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList11.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "c")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList1.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "y")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList10.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "w")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList6.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes4[0].text == "y")
        {
            if (runes4[1].text == "t")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList6.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "u")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList12.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "f")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList9.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "c")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList10.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "y")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList5.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "w")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList11.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        else if (runes4[0].text == "w")
        {
            if (runes4[1].text == "t")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList2.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "u")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList8.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "f")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList3.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "c")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList6.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "y")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList11.IndexOf(renderer.material.mainTexture)).ToArray();
            }
            else if (runes4[1].text == "w")
            {
                gemstones4 = gemstones4.OrderBy(renderer => priorityList7.IndexOf(renderer.material.mainTexture)).ToArray();
            }
        }
        correctJewels.Add(gemstones4[0]);
        Debug.LogFormat("[Jewel Wheels #{0}] The correct jewel of the first wheel (outermost) is {1}.", moduleId, correctJewels[0].material.mainTexture.name);
        Debug.LogFormat("[Jewel Wheels #{0}] The correct jewel of the second wheel (second outermost) is {1}.", moduleId, correctJewels[1].material.mainTexture.name);
        Debug.LogFormat("[Jewel Wheels #{0}] The correct jewel of the third wheel (second innermost) is {1}.", moduleId, correctJewels[2].material.mainTexture.name);
        Debug.LogFormat("[Jewel Wheels #{0}] The correct jewel of the fourth wheel (innermost) is {1}.", moduleId, correctJewels[3].material.mainTexture.name);
    }

    public void GetTargetOrientation()
    {
        if (mostAbundantGemstones.Count() == 1)
        {

        }
        else
        {
            serialDigit = Bomb.GetSerialNumberNumbers().Last();
            if (serialDigit == 0)
            {
                mostAbundantGemstones = mostAbundantGemstones.OrderBy(x => priorityList10.IndexOf(x)).ToList();
            }
            else if (serialDigit == 1)
            {
                mostAbundantGemstones = mostAbundantGemstones.OrderBy(x => priorityList1.IndexOf(x)).ToList();
            }
            else if (serialDigit == 2)
            {
                mostAbundantGemstones = mostAbundantGemstones.OrderBy(x => priorityList2.IndexOf(x)).ToList();
            }
            else if (serialDigit == 3)
            {
                mostAbundantGemstones = mostAbundantGemstones.OrderBy(x => priorityList3.IndexOf(x)).ToList();
            }
            else if (serialDigit == 4)
            {
                mostAbundantGemstones = mostAbundantGemstones.OrderBy(x => priorityList4.IndexOf(x)).ToList();
            }
            else if (serialDigit == 5)
            {
                mostAbundantGemstones = mostAbundantGemstones.OrderBy(x => priorityList5.IndexOf(x)).ToList();
            }
            else if (serialDigit == 6)
            {
                mostAbundantGemstones = mostAbundantGemstones.OrderBy(x => priorityList6.IndexOf(x)).ToList();
            }
            else if (serialDigit == 7)
            {
                mostAbundantGemstones = mostAbundantGemstones.OrderBy(x => priorityList7.IndexOf(x)).ToList();
            }
            else if (serialDigit == 8)
            {
                mostAbundantGemstones = mostAbundantGemstones.OrderBy(x => priorityList8.IndexOf(x)).ToList();
            }
            else if (serialDigit == 9)
            {
                mostAbundantGemstones = mostAbundantGemstones.OrderBy(x => priorityList9.IndexOf(x)).ToList();
            }
        }
        if (mostAbundantGemstones[0] == gemstoneOptions[2] || mostAbundantGemstones[0] == gemstoneOptions[4])
        {
            targetOrientation = 0;
        }
        else if (mostAbundantGemstones[0] == gemstoneOptions[0] || mostAbundantGemstones[0] == gemstoneOptions[1])
        {
            targetOrientation = 1;
        }
        else if (mostAbundantGemstones[0] == gemstoneOptions[3] || mostAbundantGemstones[0] == gemstoneOptions[6])
        {
            targetOrientation = 2;
        }
        else if (mostAbundantGemstones[0] == gemstoneOptions[5] || mostAbundantGemstones[0] == gemstoneOptions[7])
        {
            targetOrientation = 3;
        }
        Debug.LogFormat("[Jewel Wheels #{0}] The target orientation is {1}.", moduleId, orientations[targetOrientation]);
    }

    public void GetWheelCodes()
    {
        foreach (KMSelectable wheel in wheels)
        {
            int index = UnityEngine.Random.Range(0, assignedWheels.Count + 1);
            assignedWheels.Insert(index, wheel);
        }
        Debug.LogFormat("[Jewel Wheels #{0}] Wheel A is the {1}. Wheel B is the {2}. Wheel C is the {3}. Wheel D is the {4}.", moduleId, assignedWheels[0].name, assignedWheels[1].name, assignedWheels[2].name, assignedWheels[3].name);
    }

    public void GetOrderedJewels()
    {
        if (assignedWheels[0] == wheels[0])
        {
            orderedCorrectJewels.Add(correctJewels[0]);
        }
        else if (assignedWheels[0] == wheels[1])
        {
            orderedCorrectJewels.Add(correctJewels[1]);
        }
        else if (assignedWheels[0] == wheels[2])
        {
            orderedCorrectJewels.Add(correctJewels[2]);
        }
        else if (assignedWheels[0] == wheels[3])
        {
            orderedCorrectJewels.Add(correctJewels[3]);
        }

        if (assignedWheels[1] == wheels[0])
        {
            orderedCorrectJewels.Add(correctJewels[0]);
        }
        else if (assignedWheels[1] == wheels[1])
        {
            orderedCorrectJewels.Add(correctJewels[1]);
        }
        else if (assignedWheels[1] == wheels[2])
        {
            orderedCorrectJewels.Add(correctJewels[2]);
        }
        else if (assignedWheels[1] == wheels[3])
        {
            orderedCorrectJewels.Add(correctJewels[3]);
        }

        if (assignedWheels[2] == wheels[0])
        {
            orderedCorrectJewels.Add(correctJewels[0]);
        }
        else if (assignedWheels[2] == wheels[1])
        {
            orderedCorrectJewels.Add(correctJewels[1]);
        }
        else if (assignedWheels[2] == wheels[2])
        {
            orderedCorrectJewels.Add(correctJewels[2]);
        }
        else if (assignedWheels[2] == wheels[3])
        {
            orderedCorrectJewels.Add(correctJewels[3]);
        }

        if (assignedWheels[3] == wheels[0])
        {
            orderedCorrectJewels.Add(correctJewels[0]);
        }
        else if (assignedWheels[3] == wheels[1])
        {
            orderedCorrectJewels.Add(correctJewels[1]);
        }
        else if (assignedWheels[3] == wheels[2])
        {
            orderedCorrectJewels.Add(correctJewels[2]);
        }
        else if (assignedWheels[3] == wheels[3])
        {
            orderedCorrectJewels.Add(correctJewels[3]);
        }
    }

    public void GetCorrectJewelStartOrientation()
    {
        if (orderedCorrectJewels[0].name.Contains("N"))
        {
            jewelOrientation[0] = 0;
        }
        else if (orderedCorrectJewels[0].name.Contains("E"))
        {
            jewelOrientation[0] = 1;
        }
        else if (orderedCorrectJewels[0].name.Contains("S"))
        {
            jewelOrientation[0] = 2;
        }
        else if (orderedCorrectJewels[0].name.Contains("W"))
        {
            jewelOrientation[0] = 3;
        }

        if (orderedCorrectJewels[1].name.Contains("N"))
        {
            jewelOrientation[1] = 0;
        }
        else if (orderedCorrectJewels[1].name.Contains("E"))
        {
            jewelOrientation[1] = 1;
        }
        else if (orderedCorrectJewels[1].name.Contains("S"))
        {
            jewelOrientation[1] = 2;
        }
        else if (orderedCorrectJewels[1].name.Contains("W"))
        {
            jewelOrientation[1] = 3;
        }

        if (orderedCorrectJewels[2].name.Contains("N"))
        {
            jewelOrientation[2] = 0;
        }
        else if (orderedCorrectJewels[2].name.Contains("E"))
        {
            jewelOrientation[2] = 1;
        }
        else if (orderedCorrectJewels[2].name.Contains("S"))
        {
            jewelOrientation[2] = 2;
        }
        else if (orderedCorrectJewels[2].name.Contains("W"))
        {
            jewelOrientation[2] = 3;
        }

        if (orderedCorrectJewels[3].name.Contains("N"))
        {
            jewelOrientation[3] = 0;
        }
        else if (orderedCorrectJewels[3].name.Contains("E"))
        {
            jewelOrientation[3] = 1;
        }
        else if (orderedCorrectJewels[3].name.Contains("S"))
        {
            jewelOrientation[3] = 2;
        }
        else if (orderedCorrectJewels[3].name.Contains("W"))
        {
            jewelOrientation[3] = 3;
        }
        Debug.LogFormat("[Jewel Wheels #{0}] The correct jewel of wheel A is {1} and is facing {2}.", moduleId, orderedCorrectJewels[0].material.mainTexture.name, orientations[jewelOrientation[0]]);
        Debug.LogFormat("[Jewel Wheels #{0}] The correct jewel of wheel B is {1} and is facing {2}.", moduleId, orderedCorrectJewels[1].material.mainTexture.name, orientations[jewelOrientation[1]]);
        Debug.LogFormat("[Jewel Wheels #{0}] The correct jewel of wheel C is {1} and is facing {2}.", moduleId, orderedCorrectJewels[2].material.mainTexture.name, orientations[jewelOrientation[2]]);
        Debug.LogFormat("[Jewel Wheels #{0}] The correct jewel of wheel D is {1} and is facing {2}.", moduleId, orderedCorrectJewels[3].material.mainTexture.name, orientations[jewelOrientation[3]]);
    }

    public void onWheel (KMSelectable wheel)
    {
        if (moduleSolved || rotationLock)
        {
            return;
        }
        else
        {
            rotationLock = true;
            if (wheel == assignedWheels[0])
            {
                wheelIndex = 0;
                wheelATurns++;
                jewelOrientation[0] = (jewelOrientation[0]+ 5) % 4;
            }
            else if (wheel == assignedWheels[1])
            {
                wheelIndex = 1;
                wheelBTurns++;
                wheelATurns += 3;
                jewelOrientation[1] = (jewelOrientation[1]+ 5) % 4;
                jewelOrientation[0] = (jewelOrientation[0]+ 3) % 4;
            }
            else if (wheel == assignedWheels[2])
            {
                wheelIndex = 2;
                wheelCTurns++;
                wheelBTurns += 3;
                jewelOrientation[2] = (jewelOrientation[2]+ 5) % 4;
                jewelOrientation[1] = (jewelOrientation[1]+ 3) % 4;
            }
            else if (wheel == assignedWheels[3])
            {
                wheelIndex = 3;
                wheelDTurns++;
                wheelCTurns += 3;
                jewelOrientation[3] = (jewelOrientation[3]+ 5) % 4;
                jewelOrientation[2] = (jewelOrientation[2]+ 3) % 4;
            }
            StartCoroutine(wheelAnimation());
        }
    }

    private IEnumerator wheelAnimation()
    {
        whiteLights.SetActive(false);
        yellowLights.SetActive(true);
        Audio.PlaySoundAtTransform("wheelTurn", transform);
        while (wheelDistance != -12)
        {
            yield return new WaitForSeconds(0.06f);
            assignedWheels[wheelIndex].transform.localRotation = Quaternion.Euler(0.0f, -0.5f, 0.0f) * assignedWheels[wheelIndex].transform.localRotation;;
            if (wheelIndex == 0)
            {

            }
            else
            {
                assignedWheels[(wheelIndex + 3) % 4].transform.localRotation = Quaternion.Euler(0.0f, 0.5f, 0.0f) * assignedWheels[(wheelIndex + 3) % 4].transform.localRotation;;
            }
            centralOrb.transform.localRotation = Quaternion.Euler(0.0f, 0.5f, 0.0f) * centralOrb.transform.localRotation;;
            wheelDistance -= 1;
        }
        wheelDistance = -6;
        yield return new WaitForSeconds(0.6f);
        while (wheelDistance != 90)
        {
            yield return new WaitForSeconds(0.034f);
            assignedWheels[wheelIndex].transform.localRotation = Quaternion.Euler(0.0f, 1.0f, 0.0f) * assignedWheels[wheelIndex].transform.localRotation;;
            if (wheelIndex == 0)
            {

            }
            else
            {
                assignedWheels[(wheelIndex + 3) % 4].transform.localRotation = Quaternion.Euler(0.0f, -1.0f, 0.0f) * assignedWheels[(wheelIndex + 3) % 4].transform.localRotation;;
            }
            centralOrb.transform.localRotation = Quaternion.Euler(0.0f, -1.0f, 0.0f) * centralOrb.transform.localRotation;;
            wheelDistance += 1;
        }
        wheelDistance = 0;
        totalRotations++;
        yellowLights.SetActive(false);
        StartCoroutine(wheelShuffle());
    }

    private IEnumerator wheelShuffle()
    {
        if (totalRotations % 13 == 0 || striked)
        {
            if (striked)
            {
                striked = false;
            }
            else
            {
                pinkLights.SetActive(true);
                whiteLights.SetActive(false);
            }
            yield return new WaitForSeconds(0.3f);
            Audio.PlaySoundAtTransform("wholeTurn", transform);
            int direction = UnityEngine.Random.Range(0,2);
            if (direction == 0)
            {
                while (wheelDistance != 90)
                {
                    yield return new WaitForSeconds(0.034f);
                    outerWheel.transform.localRotation = Quaternion.Euler(0.0f, 1.0f, 0.0f) * outerWheel.transform.localRotation;;
                    compassColumn.transform.localRotation = Quaternion.Euler(0.0f, 1.0f, 0.0f) * compassColumn.transform.localRotation;;
                    assignedWheels[0].transform.localRotation = Quaternion.Euler(0.0f, -2.0f, 0.0f) * assignedWheels[0].transform.localRotation;;
                    assignedWheels[1].transform.localRotation = Quaternion.Euler(0.0f, 1.0f, 0.0f) * assignedWheels[1].transform.localRotation;;
                    assignedWheels[2].transform.localRotation = Quaternion.Euler(0.0f, -2.0f, 0.0f) * assignedWheels[2].transform.localRotation;;
                    assignedWheels[3].transform.localRotation = Quaternion.Euler(0.0f, 1.0f, 0.0f) * assignedWheels[3].transform.localRotation;;
                    centralOrb.transform.localRotation = Quaternion.Euler(0.0f, -5.0f, 0.0f) * centralOrb.transform.localRotation;;
                    wheelDistance += 1;
                }
                wheelATurns += 2;
                jewelOrientation[0] = (jewelOrientation[0]+ 2) % 4;
                wheelBTurns++;
                jewelOrientation[1] = (jewelOrientation[1]+ 1) % 4;
                wheelCTurns += 2;
                jewelOrientation[2] = (jewelOrientation[2]+ 2) % 4;
                wheelDTurns++;
                jewelOrientation[3] = (jewelOrientation[3]+ 1) % 4;
                outerOrientation = (outerOrientation + 1) % 4;
                Debug.LogFormat("[Jewel Wheels #{0}] North is {1}.", moduleId, outerOrientationLog[outerOrientation]);
            }
            else
            {
                while (wheelDistance != 90)
                {
                    yield return new WaitForSeconds(0.034f);
                    outerWheel.transform.localRotation = Quaternion.Euler(0.0f, -1.0f, 0.0f) * outerWheel.transform.localRotation;;
                    compassColumn.transform.localRotation = Quaternion.Euler(0.0f, -1.0f, 0.0f) * compassColumn.transform.localRotation;;
                    assignedWheels[0].transform.localRotation = Quaternion.Euler(0.0f, 2.0f, 0.0f) * assignedWheels[0].transform.localRotation;;
                    assignedWheels[1].transform.localRotation = Quaternion.Euler(0.0f, -1.0f, 0.0f) * assignedWheels[1].transform.localRotation;;
                    assignedWheels[2].transform.localRotation = Quaternion.Euler(0.0f, 2.0f, 0.0f) * assignedWheels[2].transform.localRotation;;
                    assignedWheels[3].transform.localRotation = Quaternion.Euler(0.0f, -1.0f, 0.0f) * assignedWheels[3].transform.localRotation;;
                    centralOrb.transform.localRotation = Quaternion.Euler(0.0f, 5.0f, 0.0f) * centralOrb.transform.localRotation;;
                    wheelDistance += 1;
                }
                wheelATurns += 2;
                jewelOrientation[0] = (jewelOrientation[0]+ 2) % 4;
                wheelBTurns += 3;
                jewelOrientation[1] = (jewelOrientation[1]+ 3) % 4;
                wheelCTurns += 2;
                jewelOrientation[2] = (jewelOrientation[2]+ 2) % 4;
                wheelDTurns += 3;
                jewelOrientation[3] = (jewelOrientation[3]+ 3) % 4;
                outerOrientation = (outerOrientation + 3) % 4;
                Debug.LogFormat("[Jewel Wheels #{0}] North is {1}.", moduleId, outerOrientationLog[outerOrientation]);
                Debug.LogFormat("[Jewel Wheels #{0}] The correct jewel of wheel A is {1} and is facing {2}.", moduleId, orderedCorrectJewels[0].material.mainTexture.name, orientations[jewelOrientation[0]]);
                Debug.LogFormat("[Jewel Wheels #{0}] The correct jewel of wheel B is {1} and is facing {2}.", moduleId, orderedCorrectJewels[1].material.mainTexture.name, orientations[jewelOrientation[1]]);
                Debug.LogFormat("[Jewel Wheels #{0}] The correct jewel of wheel C is {1} and is facing {2}.", moduleId, orderedCorrectJewels[2].material.mainTexture.name, orientations[jewelOrientation[2]]);
                Debug.LogFormat("[Jewel Wheels #{0}] The correct jewel of wheel D is {1} and is facing {2}.", moduleId, orderedCorrectJewels[3].material.mainTexture.name, orientations[jewelOrientation[3]]);
            }
        }
        wheelDistance = 0;
        rotationLock = false;
        pinkLights.SetActive(false);
        redLights.SetActive(false);
        whiteLights.SetActive(true);
    }

    public void onReset()
    {
        if (moduleSolved || rotationLock || ((wheelATurns % 4) + (wheelBTurns % 4) + (wheelCTurns % 4) + (wheelDTurns % 4)) == 0)
        {
            return;
        }
        else
        {
            rotationLock = true;
            Audio.PlaySoundAtTransform("reset", transform);
            GetCorrectJewelStartOrientation();
            StartCoroutine(resetAnimationA());
            StartCoroutine(resetAnimationB());
            StartCoroutine(resetAnimationC());
            StartCoroutine(resetAnimationD());
            StartCoroutine(mainResetAnimation());
            StartCoroutine(animationsComplete());
        }
    }

    private IEnumerator resetAnimationA()
    {
        while (wheelATurns % 4 > 0)
        {
            while (wheelDistanceA != 90)
            {
                yield return new WaitForSeconds(0.034f);
                assignedWheels[0].transform.localRotation = Quaternion.Euler(0.0f, -1.0f, 0.0f) * assignedWheels[0].transform.localRotation;;
                wheelDistanceA += 1;
            }
            wheelDistanceA = 0;
            wheelATurns += 3;
        }
        wheelATurns = 0;
    }

    private IEnumerator resetAnimationB()
    {
        while (wheelBTurns % 4 > 0)
        {
            while (wheelDistanceB != 90)
            {
                yield return new WaitForSeconds(0.034f);
                assignedWheels[1].transform.localRotation = Quaternion.Euler(0.0f, 1.0f, 0.0f) * assignedWheels[1].transform.localRotation;;
                wheelDistanceB += 1;
            }
            wheelDistanceB = 0;
            wheelBTurns += 1;
        }
        wheelBTurns = 0;
    }

    private IEnumerator resetAnimationC()
    {
        while (wheelCTurns % 4 > 0)
        {
            while (wheelDistanceC != 90)
            {
                yield return new WaitForSeconds(0.034f);
                assignedWheels[2].transform.localRotation = Quaternion.Euler(0.0f, -1.0f, 0.0f) * assignedWheels[2].transform.localRotation;;
                wheelDistanceC += 1;
            }
            wheelDistanceC = 0;
            wheelCTurns += 3;
        }
        wheelCTurns = 0;
    }

    private IEnumerator resetAnimationD()
    {
        while (wheelDTurns % 4 > 0)
        {
            while (wheelDistanceD != 90)
            {
                yield return new WaitForSeconds(0.034f);
                assignedWheels[3].transform.localRotation = Quaternion.Euler(0.0f, 1.0f, 0.0f) * assignedWheels[3].transform.localRotation;;
                wheelDistanceD += 1;
            }
            wheelDistanceD = 0;
            wheelDTurns += 1;
        }
        wheelDTurns = 0;
    }

    private IEnumerator mainResetAnimation()
    {
        whiteLights.SetActive(false);
        blueLights.SetActive(true);
        while (fullCircle != 300)
        {
            yield return new WaitForSeconds(0.034f);
            outerWheel.transform.localRotation = Quaternion.Euler(0.0f, -1.2f, 0.0f) * outerWheel.transform.localRotation;;
            compassColumn.transform.localRotation = Quaternion.Euler(0.0f, 1.2f, 0.0f) * compassColumn.transform.localRotation;;
            centralOrb.transform.localRotation = Quaternion.Euler(0.0f, 5.0f, 0.0f) * centralOrb.transform.localRotation;;
            fullCircle += 1;
        }
        fullCircle = 0;
        resetComplete = true;
        blueLights.SetActive(false);
        whiteLights.SetActive(true);
    }

    private IEnumerator animationsComplete()
    {
        yield return new WaitUntil(() => resetComplete == true);
        resetComplete = false;
        totalRotations = 0;
        rotationLock = false;
    }

    public void onSubmit()
    {
        if (moduleSolved || rotationLock)
        {
            return;
        }
        else if (jewelOrientation[0] == targetOrientation && jewelOrientation[1] == targetOrientation && jewelOrientation[2] == targetOrientation && jewelOrientation[3] == targetOrientation)
        {
            rotationLock = true;
            yellowLights.SetActive(true);
            whiteLights.SetActive(false);
            StartCoroutine(moduleSolvedAnimationOne());
            StartCoroutine(moduleSolvedAnimationTwo());
            StartCoroutine(moduleSolvedAnimationSix());
        }
        else
        {
            rotationLock = true;
            StartCoroutine(strikeCoroutine());
        }
    }

    private IEnumerator strikeCoroutine()
    {
        redLights.SetActive(true);
        whiteLights.SetActive(false);
        GetComponent<KMBombModule>().HandleStrike();
        Debug.LogFormat("[Jewel Wheels #{0}] Strike! You did not orient the gemstones correctly.", moduleId);
        yield return new WaitForSeconds(2f);
        striked = true;
        StartCoroutine(wheelShuffle());
    }

    private IEnumerator moduleSolvedAnimationOne()
    {
        Audio.PlaySoundAtTransform("steam", transform);
        while (columnDistance != 21)
        {
            yield return new WaitForSeconds(0.025f);
            columns[0].transform.localPosition = columns[0].transform.localPosition + Vector3.up * -0.02f;
            columnDistance++;
        }
        StartCoroutine(moduleSolvedAnimationThree());
        columnDistance = 0;
        Audio.PlaySoundAtTransform("steam", transform);
        Audio.PlaySoundAtTransform("reset", transform);
        while (columnDistance != 42)
        {
            yield return new WaitForSeconds(0.025f);
            columns[1].transform.localPosition = columns[1].transform.localPosition + Vector3.up * -0.02f;
            columnDistance++;
        }
        StartCoroutine(moduleSolvedAnimationFour());
        columnDistance = 0;
        Audio.PlaySoundAtTransform("steam", transform);
        while (columnDistance != 63)
        {
            yield return new WaitForSeconds(0.025f);
            columns[2].transform.localPosition = columns[2].transform.localPosition + Vector3.up * -0.02f;
            columnDistance++;
        }
        StartCoroutine(moduleSolvedAnimationFive());
        columnDistance = 0;
        Audio.PlaySoundAtTransform("steam", transform);
        while (columnDistance != 84)
        {
            yield return new WaitForSeconds(0.025f);
            columns[3].transform.localPosition = columns[3].transform.localPosition + Vector3.up * -0.02f;
            columnDistance++;
        }
        columnDistance = 0;
        Audio.PlaySoundAtTransform("wholeTurn", transform);
        Audio.PlaySoundAtTransform("wheelTurn", transform);
        while (columnDistance != 101)
        {
            yield return new WaitForSeconds(0.025f);
            columns[4].transform.localPosition = columns[4].transform.localPosition + Vector3.up * -0.02f;
            columnDistance++;
        }
    }

    private IEnumerator moduleSolvedAnimationTwo()
    {
        int fullCircleA = 0;
        Audio.PlaySoundAtTransform("wheelTurn", transform);
        while (fullCircleA != 360)
        {
            yield return new WaitForSeconds(0.025f);
            columns[0].transform.localRotation = Quaternion.Euler(0.0f, -1f, 0.0f) * columns[0].transform.localRotation;;
            fullCircleA += 1;
        }
        fullCircleA = 0;
    }

    private IEnumerator moduleSolvedAnimationThree()
    {
        int fullCircleB = 0;
        Audio.PlaySoundAtTransform("wheelTurn", transform);
        while (fullCircleB != 360)
        {
            yield return new WaitForSeconds(0.025f);
            columns[1].transform.localRotation = Quaternion.Euler(0.0f, 1.5f, 0.0f) * columns[1].transform.localRotation;;
            fullCircleB += 1;
        }
        fullCircleB = 0;
    }

    private IEnumerator moduleSolvedAnimationFour()
    {
        int fullCircleC = 0;
        Audio.PlaySoundAtTransform("wheelTurn", transform);
        while (fullCircleC != 270)
        {
            yield return new WaitForSeconds(0.025f);
            columns[2].transform.localRotation = Quaternion.Euler(0.0f, -2f, 0.0f) * columns[2].transform.localRotation;;
            fullCircleC += 1;
        }
        fullCircleC = 0;
    }

    private IEnumerator moduleSolvedAnimationFive()
    {
        int fullCircleD = 0;
        Audio.PlaySoundAtTransform("wheelTurn", transform);
        while (fullCircleD != 180)
        {
            yield return new WaitForSeconds(0.025f);
            columns[3].transform.localRotation = Quaternion.Euler(0.0f, 2.5f, 0.0f) * columns[3].transform.localRotation;;
            fullCircleD += 1;
        }
        fullCircleD = 0;
    }

    private IEnumerator moduleSolvedAnimationSix()
    {
        int fullCircleE = 0;
        Audio.PlaySoundAtTransform("wholeTurn", transform);
        Audio.PlaySoundAtTransform("wheelTurn", transform);
        while (fullCircleE != 400)
        {
            yield return new WaitForSeconds(0.025f);
            centralOrb.transform.localRotation = Quaternion.Euler(0.0f, -5.0f, 0.0f) * centralOrb.transform.localRotation;;
            fullCircleE += 1;
        }
        fullCircleE = 0;
        yellowLights.SetActive(false);
        greenLights.SetActive(true);
        moduleSolved = true;
        GetComponent<KMBombModule>().HandlePass();
        Audio.PlaySoundAtTransform("unlock", transform);
        Debug.LogFormat("[Jewel Wheels #{0}] Module disarmed.", moduleId);
        int doorOpen = 0;
        while (doorOpen != 60)
        {
            yield return new WaitForSeconds(0.01f);
            hinge.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.5f) * hinge.transform.localRotation;;
            doorOpen += 1;
        }
        while (doorOpen != 260)
        {
            yield return new WaitForSeconds(0.01f);
            hinge.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.25f) * hinge.transform.localRotation;;
            doorOpen += 1;
        }
        while (doorOpen != 340)
        {
            yield return new WaitForSeconds(0.01f);
            hinge.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.125f) * hinge.transform.localRotation;;
            doorOpen += 1;
        }
	}

#pragma warning disable 414
    private string TwitchHelpMessage = "Turn the wheels with !{0} turn 3 [Turn wheel 3 (range is 1-4)]. Turn the wheels more than once with !{0} turn 1 2. [Turn wheel 1 twice. Wheels range from 1-4, Turns range from 1-3]. Reset the wheels with !{0} reset. Submit with !{0} submit.";
#pragma warning restore 414

	private IEnumerator ProcessTwitchCommand(string inputCommand)
	{
		if (inputCommand.Equals("reset", StringComparison.InvariantCultureIgnoreCase))
		{
			yield return null;

			while (rotationLock)
				yield return "trycancel your reset command for jewel wheel was cancelled.";
			
			yield return new KMSelectable[] {resetButton};
			yield break;
		}

		if (inputCommand.Equals("submit", StringComparison.InvariantCultureIgnoreCase))
		{
			yield return null;

			while (rotationLock)
				yield return "trycancel your solve command for jewel wheel was cancelled.";
			
			yield return new KMSelectable[] { submitButton };
			yield return null;
			yield return "solve";
			yield break;
		}

		Match match = Regex.Match(inputCommand, "^turn ([1-4])( [1-3])?$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
		if (match.Success)
		{
			yield return null;

			var wheel = int.Parse(match.Groups[1].Value) - 1;
			int times;
			if (!int.TryParse(match.Groups[2].Value, out times))
				times = 1;

			while (rotationLock)
				yield return "trycancel Your wheel turn command for jewel wheel was cancelled with " + times + " turns of wheel " + wheel + " remaining.";

			while (true)
			{
				yield return new KMSelectable[] {wheels[wheel]};
				times -= 1;
				if (times == 0)
					break;
				while (rotationLock)
					yield return "trycancel Your wheel turn command for jewel wheel was cancelled with " + times + " turns of wheel " + wheel + " remaining.";
				if (totalRotations % 13 == 0)
					break;
			}
		}
	}

	private IEnumerator TwitchHandleForcedSolve()
	{
		yield return null;
		while (!yellowLights.activeSelf && !moduleSolved)
		{
			if (jewelOrientation.All(x => x == targetOrientation))
			{
				submitButton.OnInteract();
			}
			else if (jewelOrientation[3] != targetOrientation)
			{
				assignedWheels[3].OnInteract();
			}
			else if (jewelOrientation[2] != targetOrientation)
			{
				assignedWheels[2].OnInteract();
			}
			else if (jewelOrientation[1] != targetOrientation)
			{
				assignedWheels[1].OnInteract();
			}
			else if (jewelOrientation[0] != targetOrientation)
			{
				assignedWheels[0].OnInteract();
			}

			while (rotationLock)
				yield return true;
		}

	}
}
