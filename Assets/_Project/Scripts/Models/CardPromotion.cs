using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPromotion : MonoBehaviour
{
    public Text txtNameShop;
    public Text txtCountPromotion;
    public string address;
    public Action<string> OnButtonClicked;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(address));
    }
}