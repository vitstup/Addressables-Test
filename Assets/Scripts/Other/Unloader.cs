using System.Collections.Generic;
using UnityEngine;

// Простенький класс, цель которого очистить все загрузки текущей сцены после к примеру загрузки другой сцены
public class Unloader : MonoBehaviour
{
    private List<IUnloadeble> unloadebles = new();

    public void Add(IUnloadeble unloadeble)
    {
        unloadebles.Add(unloadeble);
    }

    public void Unload()
    {
        unloadebles.ForEach(e => e.Unload());
    }
}