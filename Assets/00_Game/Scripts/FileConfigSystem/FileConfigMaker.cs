using UnityEditor;
using UnityEngine;

public class FileConfigMaker
{
    [MenuItem("Assets/Create/_FileConfig/FromCSV", false, 0)]
    public static void CreateFileConfig()
    {
        foreach (var file in Selection.objects)
        {
            TextAsset textAsset = (TextAsset)file;
            string nameFile = textAsset.name;
            ScriptableObject scriptableObject = ScriptableObject.CreateInstance(nameFile); // nameFile is Class Name => must be Correct
            AssetDatabase.CreateAsset(scriptableObject, "Assets/Resources/Config/" +  textAsset.name + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            // Casting
            ConfigBase fileConfg =  (ConfigBase)scriptableObject;
            fileConfg.HandleDataFromTextAsset(textAsset);
            EditorUtility.SetDirty(fileConfg);
            
            Debug.Log(scriptableObject.name + "has been created at Assets/Resources/Config/" + nameFile + ".asset");

        }
    }
}
