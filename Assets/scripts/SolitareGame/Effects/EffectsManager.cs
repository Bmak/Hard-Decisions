
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class EffectsManager
{
    private static bool _isInited = false;

    private static readonly Dictionary<string, BaseEffect> _effects = new Dictionary<string, BaseEffect>();

    public static Action EndMoveAction = () => { };

    public static BaseEffect GetEffect(string key)
    {
        if (!_isInited)
        {
            Init();
        }

        BaseEffect e;
        if (_effects.TryGetValue(key, out e))
        {
            return e;
        }
        Debug.LogError("Can't Find Effect with key: " + key);
        return null;
    }

    private static void Init()
    {
        var eff = ReflectiveEnumerator.GetInheretedClasses<BaseEffect>();

        foreach (BaseEffect baseEffect in eff)
        {
            _effects[baseEffect.Key] = baseEffect;

            //Debug.Log(baseEffect.GetType());
        }

        _isInited = true;
    }

}


public static class ReflectiveEnumerator
{
    static ReflectiveEnumerator() { }

    public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class, IComparable<T>
    {
        List<T> objects = new List<T>();
        foreach (Type type in
            Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
        {
            objects.Add((T)Activator.CreateInstance(type, constructorArgs));
        }
        objects.Sort();
        return objects;
    }

    public static IEnumerable<T> GetInheretedClasses<T>(params object[] constructorArgs) where T : class
    {
        IEnumerable<T> exporters = typeof(T)
            .Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract)
            .Select(t => (T)Activator.CreateInstance(t));

        return exporters;
    }
}