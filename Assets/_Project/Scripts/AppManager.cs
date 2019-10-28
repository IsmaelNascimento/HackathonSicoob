using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    [Header("Form New User")]
    public InputField inputFieldEmailNewUser;
    public InputField inputFieldPasswordNewUser;

    [Header("Form User Login")]
    public InputField inputFieldEmailLogin;
    public InputField inputFieldPasswordLogin;

    [Header("Panels")]
    public Dropdown typeUserLogin;
    public Dropdown typeUserRegister;
    public GameObject panelSipag;
    public GameObject panelSicard;

    public GameObject panelRegisterNewUser;
    public GameObject panelLoginUser;

    public Transform contentCardPromotion;
    public CardPromotion prefabCardPromotion;

    Firebase.Auth.FirebaseAuth auth;
    private string uriGetShops = "https://hk-sicoob.herokuapp.com/api/public/estabelecimento/getEstabelecimentosByCidade?nomeCidade=Uberlandia";


    private void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        GetShops();
    }

    public void OnButtonNewUserClicked()
    {
        auth.CreateUserWithEmailAndPasswordAsync(inputFieldEmailNewUser.text, inputFieldPasswordNewUser.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            panelRegisterNewUser.SetActive(false);

            if (typeUserRegister.value == 2)
                panelSipag.SetActive(true);
            else
                panelSicard.SetActive(true);

            panelLoginUser.SetActive(true);

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    public void OnButtonLoginUserClicked()
    {
        print("LOGIN");

        try
        {
            auth.SignInWithEmailAndPasswordAsync(inputFieldEmailLogin.text, inputFieldPasswordLogin.text).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    print("HERE 1");
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    print("HERE 2");
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                print("typeUserLogin.value:: " + typeUserLogin.value);

                if (typeUserLogin.value == 2)
                    panelSipag.SetActive(true);
                else
                    panelSicard.SetActive(true);

                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            });
        }
        catch(Exception ex)
        {
            print("ERROR:: " + ex);
        }        
    }

    private void GetShops()
    {
        StartCoroutine(GetShops_Coroutine());
    }

    private IEnumerator GetShops_Coroutine()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uriGetShops))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                // Show results as text
                Debug.Log(request.downloadHandler.text);

                var dataAsJson = request.downloadHandler.text;

                Shop[] shops = JsonHelper.FromJson<Shop>(dataAsJson);

                foreach (var shop in shops)
                {
                    var newShop = Instantiate(prefabCardPromotion, contentCardPromotion);
                    newShop.txtNameShop.text = shop.razaoSocial;
                    newShop.txtCountPromotion.text = string.Format("DESCONTO DE {0}%", shop.percentualDesconto.ToString());
                    newShop.address = string.Format("{0},{1}", shop.rua, shop.numero);
                    newShop.OnButtonClicked += OnButtonShop;
                }
            }
        }
    }

    private void OnButtonShop(string address)
    {
        Application.OpenURL("https://www.google.com/maps/dir/?api=1&destination=" + address);
    }

    public void OnButtonSearchSicoobsClicked()
    {
        Application.OpenURL("https://www.google.com/maps/search/siccob/");
    }
}