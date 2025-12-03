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

    public Button playAgainButton;

    void Start()
    {
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

        playAgainButton.gameObject.SetActive(false);
    }


    public void ShowPlayerWin()
    {
        playerWinUI.SetActive(true);
        StartCoroutine(PlayerWinCutscene());
    }

    public void ShowAIWin()
    {
        aiWinUI.SetActive(true);
        StartCoroutine(AICutscene());
    }

    IEnumerator AICutscene()
    {
        // scanning again
        scanSiren.gameObject.SetActive(true);
        scanBarFull.fillBar.fillAmount = 0;
        scanBarFull.gameObject.SetActive(true);

        // load bar
        yield return scanBarFull.PlayScanBar(1.5f, 1f);

        // wait
        yield return new WaitForSeconds(1.2f);

        // hide scan + bar
        scanSiren.gameObject.SetActive(false);
        scanBarFull.gameObject.SetActive(false);

        // scan complete 
        scanCompleteIMG.gameObject.SetActive(true);

        // wait
        yield return new WaitForSeconds(1.2f);

        // hide complete
        scanCompleteIMG.gameObject.SetActive(false);

        // game over + btn
        gameOverIMG.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(true);
    }

    IEnumerator PlayerWinCutscene()
    {
        // error
        errorImage.gameObject.SetActive(true);

        // wait
        yield return new WaitForSeconds(1.2f);

        // hide error
        errorImage.gameObject.SetActive(false);

        // show btn + snake 
        coffeeSnakeImage.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(true);
    }
}

