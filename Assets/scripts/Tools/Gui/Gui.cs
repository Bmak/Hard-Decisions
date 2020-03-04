using System;
using System.Collections.Generic;
using UnityEngine;

public class Gui : MonoBehaviour
{
    private static Gui _instance;

    [SerializeField] private Canvas _canvas;

    private List<GameObject> _screens = new List<GameObject>();

    public Canvas Canvas { get { return _canvas; } }
    public List<GameObject> Screens { get { return _screens; } }


    private void Awake()
    {
        _instance = this;
    }

    public static T ShowScreen<T>() where T : Component
    {
        GameObject screen = GetScreen(typeof(T));
        _instance.Screens.Add(screen);

        return screen.GetComponent<T>();
    }

    private static GameObject GetScreen(Type type)
    {
        GameObject obj = Resources.Load("Screens/" + type.Name) as GameObject;
        return Instantiate(obj, _instance.Canvas.transform);
    }

    public static void Close(Component screen)
    {
        _instance.Screens.Remove(screen.gameObject);
        Destroy(screen.gameObject);
    }
}
