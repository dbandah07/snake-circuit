using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;


public class RaceCutsceneUI : MonoBehaviour
{
    public GameObject playerWinUI;
    public GameObject aiWinUI;

    public Image errorImage;
    public Image coffeeSnakeImage;

    public ProgressBarAnti scanBarFull;
    public Image scanCompleteIMG;
    public Image gameOverIMG;
    public Image scanSiren;

    public Image CutSceneBG;



    void Start()
    {
        //cutsceneRoot.SetActive(true);
        // Player win UI hidden
        playerWinUI.SetActive(false);

        // AI win UI hidden
        aiWinUI.SetActive(false);

        errorImage.gameObject.SetActive(false);
        coffeeSnakeImage.gameObject.SetActive(false);

        scanSiren.gameObject.SetActive(false);
        scanBarFull.gameObject.SetActive(false);

        scanCompleteIMG.gameObject.SetActive(false);
        gameOverIMG.gameObject.SetActive(false);

        CutSceneBG.gameObject.SetActive(false);

    }


    public void ShowPlayerWin()
    {
        CutSceneBG.gameObject.SetActive(true);
        playerWinUI.SetActive(true);
        StartCoroutine(PlayerWinCutscene());
        //Debug.Log("CUTSCENE ROOT ACTIVE? " + cutsceneRoot.activeInHierarchy);

    }

    public void ShowAIWin()
    {
        CutSceneBG.gameObject.SetActive(true);
        aiWinUI.SetActive(true);
        StartCoroutine(AICutscene());
        //Debug.Log("CUTSCENE ROOT ACTIVE? " + cutsceneRoot.activeInHierarchy);

    }

    IEnumerator AICutscene()
    {
        // scanning again
        scanSiren.gameObject.SetActive(true);
        scanBarFull.fillBar.fillAmount = 0;
        scanBarFull.gameObject.SetActive(true);

        // load bar
        yield return scanBarFull.PlayScanBar(1.5f, 1f);

        // hide scan + bar
        scanSiren.gameObject.SetActive(false);
        scanBarFull.gameObject.SetActive(false);

        // scan complete 
        scanCompleteIMG.gameObject.SetActive(true);

        // wait
        yield return new WaitForSeconds(1.2f);

        // hide complete
        scanCompleteIMG.gameObject.SetActive(false);

        // game over
        gameOverIMG.gameObject.SetActive(true);
    }

    IEnumerator PlayerWinCutscene()
    {
        // error
        errorImage.gameObject.SetActive(true);

        // wait
        yield return new WaitForSeconds(1.2f);

        // hide error
        errorImage.gameObject.SetActive(false);

        //  snake 
        coffeeSnakeImage.gameObject.SetActive(true);

    }
}

