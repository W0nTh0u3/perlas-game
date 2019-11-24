using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class selectDidYou : MonoBehaviour {

    public Text DidyouText;

    private Dictionary<string, string> didyouDict;
    // Start is called before the first frame update
    void Start () {
        didyouDict = new Dictionary<string, string> ();
        LoadDictionary ("allDidYou", didyouDict);
        pickRandomDidyou ();
    }

    private void pickRandomDidyou () {
        int randInt = Random.Range (0, didyouDict.Count);
        DidyouText.text = didyouDict.ElementAt (randInt).Key;
    }

    private void LoadDictionary (string dictFileName, Dictionary<string, string> outputDict) {
        TextAsset ta = Resources.Load (dictFileName) as TextAsset;
        JSONObject jsonObj = (JSONObject) JSON.Parse (ta.text);
        foreach (var key in jsonObj.GetKeys ()) { outputDict[key] = jsonObj[key]; }
    }
    // Update is called once per frame
}