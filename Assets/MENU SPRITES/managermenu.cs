using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class managermenu : MonoBehaviour
{
    public Animator animatorS;
    public Animator animatorE;

    public void startbutton()
    {
        animatorS.SetTrigger("start");
        StartCoroutine(changescene());
    }

    IEnumerator changescene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("game");
    }

    public void exitbutton()
    {
        animatorE.SetTrigger("exit");
        StartCoroutine(closegame());
    }

    IEnumerator closegame()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}
