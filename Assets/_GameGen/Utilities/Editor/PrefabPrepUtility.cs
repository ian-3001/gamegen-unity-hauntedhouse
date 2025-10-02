using System;
using UnityEditor;
using UnityEngine;

public class PrefabPrepUtility : EditorWindow
{
    public enum Pivot
    {
        Root,
        //Center,
        BottomCenter,
    }
    
    //private enum PivotSource { Renderer, Collider }
    
    [MenuItem("GameGen/Utilities/Prefab Prep Utility")]
    public static void Open()
    {
        PrefabPrepUtility window = GetWindow<PrefabPrepUtility>();
    }

    
    //TODO lots of features to add in the future
    //private GameObject _template;
    private Pivot _pivot = Pivot.Root;
    private bool _useObjectName = true;
    private string _newObjName = "";
    private bool _addId = false;
    private bool _usePrefix = true;
    private string _prefix = "pf_";
    
    private void OnGUI()
    {
        _useObjectName = EditorGUILayout.Toggle("Use Object Name", _useObjectName);

        if (!_useObjectName)
        {
            _newObjName = EditorGUILayout.TextField("New Obj Name", _newObjName);
        }

        _pivot = (Pivot)EditorGUILayout.EnumPopup(_pivot);
        
        if (GUILayout.Button("Set Up Prefabs"))
        {
            int index = 0;
            foreach (GameObject o in Selection.gameObjects)
            {
                SetUpPrefab(o, index++);
            }
        }
    }

    private void SetUpPrefab(GameObject rootObj, int id)
    {
        string prefix = _usePrefix ? _prefix : "";
        string rootName = _useObjectName ? rootObj.name : _newObjName;
        string suffix = _addId ? $"_{id}" : "";
        string finalName = $"{prefix}{rootName}{suffix}";
        
        
        GameObject newGo = new GameObject(finalName);
        Undo.RegisterCreatedObjectUndo(newGo, "New Root");
        Undo.MoveGameObjectToScene(newGo, rootObj.scene, "Move To Scene");

        Vector3 pivot = GetPivot(rootObj, _pivot);

        newGo.transform.position = pivot;
        rootObj.transform.SetParent(newGo.transform);
        
        Undo.RecordObject(rootObj, "Added Parent Pivot");
    }



    private Vector3 GetPivot(GameObject rootObj, Pivot pivot)
    {
        Vector3 result = rootObj.transform.position;

        if (pivot == Pivot.Root) { return result; }


        if (pivot == Pivot.BottomCenter)
        {
            result = GetBottomPoint(rootObj);
        }

        return result;
    }


    private Vector3 GetBottomPoint(GameObject rootObj)
    {
        Vector3 result = rootObj.transform.position;
        Renderer[] renderers = rootObj.GetComponentsInChildren<Renderer>();

        if (renderers.Length <= 0)
        {
            Debug.LogWarning($"No Renderers Found on: {rootObj}");
            return result;
        }

        Vector3 bottomPoint = result + Vector3.down * 1000;
        Vector3 closest = result;
        float closestDistance = Vector3.Distance(closest, bottomPoint);
        
        foreach (Renderer renderer in renderers)
        {
            Vector3 closestToBottom =  renderer.bounds.ClosestPoint(bottomPoint);
            float distance = Vector3.Distance(closestToBottom, bottomPoint);

            if (distance < closestDistance)
            {
                closest = closestToBottom;
                closestDistance = distance;
            }
        }

        return closest;
    }
    
}
