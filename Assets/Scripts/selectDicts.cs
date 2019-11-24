using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class selectDicts : MonoBehaviour {
    
    public Text QuoteText;

    private Dictionary<string, string> quoteDict;
    // Start is called before the first frame update
    void Start() {
        quoteDict = new Dictionary<string,string> ();
        LoadDictionary ("allDictions", quoteDict);
        pickRandomQuote();
    }

    private void pickRandomQuote() {
        int randInt = Random.Range (0, quoteDict.Count);
        QuoteText.text = quoteDict.ElementAt (randInt).Key;
    }

    private void LoadDictionary (string dictFileName, Dictionary<string, string> outputDict) {
        TextAsset ta = Resources.Load (dictFileName) as TextAsset;
        JSONObject jsonObj = (JSONObject) JSON.Parse (ta.text);
        foreach (var key in jsonObj.GetKeys ()) { outputDict[key] = jsonObj[key]; }
    }
    // Update is called once per frame
}
