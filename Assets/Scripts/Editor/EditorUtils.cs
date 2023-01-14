using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorUtils : MonoBehaviour {
    [MenuItem("Tools/Add Selected To Mars", true, 1)]
    public static bool CheckAddToMars() {
        return Selection.activeGameObject != null;
    }

    [MenuItem("Tools/Add Selected To Mars", false, 1)]
    public static void AddToMars() {
        GameManager gm = GetManager();
        BaseBoardSpace[] b = Selection.activeGameObject.transform.GetComponentsInChildren<BaseBoardSpace>();
        ArrayUtility.AddRange(ref gm.mars, b);

        Debug.Log(string.Format("Added {0} items to Mars.", b.Length));
    }

    [MenuItem("Tools/Add Selected To Uranus", true, 2)]
    public static bool CheckAddToUranus() {
        return Selection.activeGameObject != null;
    }

    [MenuItem("Tools/Add Selected To Uranus", false, 2)]
    public static void AddToUranus() {
        GameManager gm = GetManager();
        BaseBoardSpace[] b = Selection.activeGameObject.transform.GetComponentsInChildren<BaseBoardSpace>();
        ArrayUtility.AddRange(ref gm.uranus, b);

        Debug.Log(string.Format("Added {0} items to Uranus.", b.Length));
    }

    [MenuItem("Tools/Clear Mars", false, 3)]
    public static void ClearMars() {
        ArrayUtility.Clear(ref GetManager().mars);
    }

    [MenuItem("Tools/Clear Uranus", false, 4)]
    public static void ClearUranus() {
        ArrayUtility.Clear(ref GetManager().uranus);
    }

    public static GameManager GetManager() {
        return FindObjectOfType<GameManager>();
    }
}
