using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Shop
{
    public int id;
    public string razaoSocial;
    public string cnpj;
    public bool associado;
    public bool sipag ;
    public string rua ;
    public int numero ;
    public string cep ;
    public Cidade cidade ;
    public Estado estado ;
    public int percentualDesconto ;
    public int avaliacao ;
    public string categoria ;
}