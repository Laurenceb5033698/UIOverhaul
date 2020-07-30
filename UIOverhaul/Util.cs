using System.Reflection;
using UnityEngine;

namespace UIOverhaul
{
    public static class Util
    {
        //good for testing prefabs loaded by assetbundle
        public static void PrintGameObjectChildren(GameObject objToPrint)
        {
            var allComponents = objToPrint.GetComponentsInChildren<Transform>();
            UIOverhaul.Logger.LogMessage("prefab object count: " + allComponents.Length);

            foreach (Transform obj in allComponents)
            {
                //if (obj.name == "FullHealthText")
                UIOverhaul.Logger.LogMessage("prefab objname: " + obj.name);
            }
        }

        //Builds on AddComponent and allows an existing component to have it's values copied to a new component on a new GameObject
        public static T AddComponent<T>(this GameObject game, T duplicate) where T : Component
        {
            T target = game.AddComponent<T>();
            foreach (PropertyInfo x in typeof(T).GetProperties())
                if (x.CanWrite)
                    x.SetValue(target, x.GetValue(duplicate));
            return target;
        }

    }
}
