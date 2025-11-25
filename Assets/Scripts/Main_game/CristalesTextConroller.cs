using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class CristalesTextConroller : MonoBehaviour
{
    [SerializeField] public TMP_Text textoCristales;

    public void SetCristales(int cantidad)
    {
        textoCristales.text = cantidad.ToString() + " / 5";
    }
}
