using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

public class FBScript : MonoBehaviour {

    public GameObject Loggedin;
    public GameObject Loggedout;
    public GameObject DisplayUsername;
    public GameObject DisplayProfile;

	// Use this for initialization
	void Awake () {
        FBManager.manager.InitFB();
        FBMenus(FB.IsLoggedIn);
    }

    // Update is called once per frame
    void Update () {
	
	}

    

    public void FBLogin()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");

        FB.LogInWithReadPermissions(permissions, AuthCallBack);

    }

    void AuthCallBack(IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            if (FB.IsLoggedIn)
            {
                FBManager.manager.isloggedin = true;
                FBManager.manager.getprofile();
                Debug.Log("FB is logged in");
            }
            else
            {
                Debug.Log("FB is not logged in");
            }
            FBMenus(FB.IsLoggedIn);
        }
       
    }

    void FBMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            Loggedin.SetActive(true);
            Loggedout.SetActive(false);

            if (FBManager.manager.profilename != null)
            {
                Text Username = DisplayUsername.GetComponent<Text>();
                Username.text = "Hi, " + FBManager.manager.profilename;
            }
            else
            {
                StartCoroutine("WaitForProfileName");
            }

            if (FBManager.manager.profilepic != null)
            {
                Image profilepic = DisplayProfile.GetComponent<Image>();
                profilepic.sprite = FBManager.manager.profilepic;
            }
            else
            {
                StartCoroutine("WaitForProfilePic");
            }
        }
        else
        {
            Loggedin.SetActive(false);
            Loggedout.SetActive(true);
        }
    }

    IEnumerator WaitForProfileName()
    {
        while(FBManager.manager.profilename == null)
        {
            yield return null;
        }

        FBMenus(FBManager.manager.isloggedin);
    }

    IEnumerator WaitForProfilePic()
    {
        while (FBManager.manager.profilepic == null)
        {
            yield return null;
        }

        FBMenus(FBManager.manager.isloggedin);
    }

    public void Share()
    {
        FBManager.manager.Share();
    }

    public void Invite()
    {
        FBManager.manager.Share();
    }
    public void Challenge()
    {
        FBManager.manager.ShareWithUsers();
    }
    public void logout()
    {
        FBManager.manager.PlayerLogout();
        FBMenus(FB.IsLoggedIn);
    }
}
