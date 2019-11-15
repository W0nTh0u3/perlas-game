using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class selectDidYouKnowImage : MonoBehaviour {
    public Sprite[] dyks;
    // Start is called before the first frame update
    void Start () {
        Object[] dykImages = Resources.LoadAll ("didYouKnow/", typeof (Sprite));
        dyks = new Sprite[dykImages.Length];
        for (int x = 0; x < dykImages.Length; x++) {
            dyks[x] = (Sprite) dykImages[x];
        }
        PickDidYouKnow ();
    }
    void PickDidYouKnow () {
        int randInt = Random.Range (0, dyks.Length - 1);
        gameObject.GetComponent<Image> ().sprite = dyks[randInt];
    }
}