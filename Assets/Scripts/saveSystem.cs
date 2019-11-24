using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class saveSystem {
    public static void SaveData (Data Levels) {
        BinaryFormatter formatter = new BinaryFormatter ();
        string path = Path.Combine (Application.persistentDataPath, "gema.ry");
        FileStream stream = new FileStream (path, FileMode.Create);
        dataC data = new dataC (Levels);
        formatter.Serialize (stream, data);
        stream.Close ();
    }
    public static dataC LoadData () {
        string path = Path.Combine (Application.persistentDataPath, "gema.ry");
        if (File.Exists (path)) {
            BinaryFormatter formatter = new BinaryFormatter ();
            FileStream stream = new FileStream (path, FileMode.Open);
            dataC data = formatter.Deserialize (stream) as dataC;
            stream.Close ();
            return data;
        } else {
            Debug.Log ("save not found");
            return null;
        }
    }

}