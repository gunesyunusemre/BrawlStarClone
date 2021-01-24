using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NetworkedPrefab
{
    public GameObject Prefab;
    public string Path;

    public NetworkedPrefab(GameObject obj, string _path)
    {
        Prefab = obj;
        Path = ReturnPrefabPathModified(_path);
        //Assets/Resources/File.prefab 
        //Resources/File
    }

    private string ReturnPrefabPathModified(string _Path)
    {
        int extensionLength = System.IO.Path.GetExtension(_Path).Length;
        int additionalLength = 10;
        int startIndex = _Path.ToLower().IndexOf("resources");

        if (startIndex == -1)
        {
            return string.Empty;
        }
        else
            return _Path.Substring(startIndex + additionalLength, _Path.Length - (additionalLength + startIndex + extensionLength));
    }

}
