using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;

public class NumerateObjects : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objects = new List<GameObject>();
    [SerializeField] private string _format = "{0}";

    [ProButton]
    public void Numerate()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].name = string.Format(_format, i + 1);
        }
    }
}
