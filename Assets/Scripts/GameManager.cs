using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.Scripts.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject objLogin;
    public GameObject objRegister;


    [Header("Login")]
    public InputField txtLogin;
    public InputField txtPassword;
    [Header("Register")]
    public InputField regLogin;
    public InputField regPassword;
    public InputField regPasswordRepeat;

    [Header("PlayerData")]
    public string username;
    public int level;
    public int exp;
    public int gold;
    public int rating;
    public int wins;
    public int losses;


    private void Awake()
    {
        instance = this;
        ChangeMenu(0);
    }

    public void ChangeMenu(int menu)
    {
        switch (menu)
        {
            case 0: //Login menu
                objLogin.SetActive(true);
                objRegister.SetActive(false);
                break;

            case 1: //Register menu
                objLogin.SetActive(false);
                objRegister.SetActive(true);
                break;
        }
    }

    public void Login()
    {
        DataSender.SendLogin(txtLogin.text, txtPassword.text);
        
    }

    public void Register()
    {
        if(regLogin.text == string.Empty)
        {
            Debug.Log("Insert a username");
            return;
        }
        if (regPassword.text == string.Empty)
        {
            Debug.Log("Insert a password");
            return;
        }
        if (regPasswordRepeat.text == string.Empty)
        {
            Debug.Log("Repeat your password");
            return;
        }
        if (regPassword.text != regPasswordRepeat.text)
        {
            Debug.Log("Passwords do not match");
            return;
        }
        DataSender.SendNewAccount(regLogin.text, regPassword.text);
        Debug.Log("Registration sent");
    }

    public bool Loading(int scene)
    {
        StartCoroutine(LoadScene(scene));
        return true;
    }

    public IEnumerator LoadScene(int scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
   
}
