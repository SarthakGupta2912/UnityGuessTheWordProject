using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject HowToPlay_Panel, SecondHintBtn;

    [SerializeField] InputField GuessedWord_Inptfld, FinalWord_Inptfld, Hint_Inptfld, Hint_Inptfld2;

    [SerializeField] Canvas MainCanvas;

    [SerializeField] Text FirstScreen, SpawnText, HintText, HintText2, LivesText;
    Text[] SpawnText_temp;

    char[] FinalWord_char;

    int Life = 10, counter = 0;

    private void Start()
    {
        LivesText.text = "Lives: " + Life;
        StartCoroutine(_FirstScreenTimer());
    }
    public void InstantiateSpawnText()
    {
        if (FinalWord_Inptfld.text != "" && Hint_Inptfld.text != "")
        {
            LivesText.gameObject.SetActive(true);
            SpawnText.text = "";
            FinalWord_char = FinalWord_Inptfld.text.ToCharArray();
            SpawnText_temp = new Text[FinalWord_char.Length];

            float x_pos = 0;
            int x_pos2 = FinalWord_char.Length / 2;

            for (int i = 0; i < FinalWord_char.Length; i++)
            {
                if (FinalWord_char.Length % 2 != 0)
                {
                    if (i < (FinalWord_char.Length / 2))
                    {
                        x_pos = -60 * x_pos2;

                        GameObject clone = Instantiate(SpawnText.gameObject);
                        clone.transform.parent = MainCanvas.transform;
                        clone.transform.localPosition = new Vector2(x_pos, 0);

                        SpawnText_temp[i] = clone.GetComponent<Text>();
                        x_pos2 -= 1;
                    }

                    else if (i == (FinalWord_char.Length / 2))
                    {
                        x_pos = 0;

                        GameObject clone = Instantiate(SpawnText.gameObject);
                        clone.transform.parent = MainCanvas.transform;
                        clone.transform.localPosition = new Vector2(x_pos, 0);
                        SpawnText_temp[i] = clone.GetComponent<Text>();
                    }

                    else
                    {
                        x_pos += 60;

                        GameObject clone = Instantiate(SpawnText.gameObject);
                        clone.transform.parent = MainCanvas.transform;
                        clone.transform.localPosition = new Vector2(x_pos, 0);
                        SpawnText_temp[i] = clone.GetComponent<Text>();
                    }
                }

                else
                {
                    if (i < (FinalWord_char.Length / 2))
                    {
                        x_pos = (-60 * x_pos2) + 30;

                        GameObject clone = Instantiate(SpawnText.gameObject);
                        clone.transform.parent = MainCanvas.transform;
                        clone.transform.localPosition = new Vector2(x_pos, 0);

                        SpawnText_temp[i] = clone.GetComponent<Text>();
                        x_pos2 -= 1;
                    }

                    else if (i == (FinalWord_char.Length / 2))
                    {
                        x_pos = 30;

                        GameObject clone = Instantiate(SpawnText.gameObject);
                        clone.transform.parent = MainCanvas.transform;
                        clone.transform.localPosition = new Vector2(x_pos, 0);

                        SpawnText_temp[i] = clone.GetComponent<Text>();
                    }

                    else
                    {
                        x_pos += 60;

                        GameObject clone = Instantiate(SpawnText.gameObject);
                        clone.transform.parent = MainCanvas.transform;
                        clone.transform.localPosition = new Vector2(x_pos, 0);

                        SpawnText_temp[i] = clone.GetComponent<Text>();
                    }
                }
            }

            FinalWord_Inptfld.gameObject.SetActive(false);
            GuessedWord_Inptfld.gameObject.SetActive(true);
        }
        else
        {
            FinalWord_Inptfld.placeholder.GetComponent<Text>().text = "Please Enter a word!";
            Hint_Inptfld.placeholder.GetComponent<Text>().text = "Please Provide a hint!";
        }
    }

    public void CheckEnteredCharacter()
    {
        bool LifeDeducted = true;
        bool repeatWord = false;

        for (int i = 0; i < FinalWord_char.Length; i++)
        {
            if (GuessedWord_Inptfld.text.ToUpper() == SpawnText_temp[i].text.ToUpper())
            {
                repeatWord = true;
                LifeDeducted = false;
            }
        }

        if (repeatWord == false)
        {
            for (int i = 0; i < FinalWord_char.Length; i++)
            {
                if (GuessedWord_Inptfld.text.ToUpper() == FinalWord_char[i].ToString().ToUpper())
                {
                    counter++;
                    LifeDeducted = false;
                    if (i == 0)
                        SpawnText_temp[i].text = GuessedWord_Inptfld.text.ToUpper();

                    else
                        SpawnText_temp[i].text = GuessedWord_Inptfld.text.ToLower();
                }
            }
        }

        if (counter == FinalWord_char.Length)
        {
            RevealWord();
            StartCoroutine(ReloadAppTimer());
        }

        else if (LifeDeducted == true && GuessedWord_Inptfld.text != "")
            LifeDeduction();

        GuessedWord_Inptfld.text = "";
    }

    public void LifeDeduction()
    {
        Life--;
        LivesText.text = "Lives: " + Life;

        if (Life <= 0)
        {
            LivesText.text = "Game Over!";
            Destroy(GuessedWord_Inptfld.gameObject);
            RevealWord();
        }

        else if (Life == 1 && HintText2.text == "")
        {
            Destroy(SecondHintBtn);
            HintText2.gameObject.SetActive(true);
            HintText2.text = "You cannot use the second hint as you have only 1 life left!";
        }
    }
    public void PreventSpaces(InputField inputfield)
    {
        inputfield.text = inputfield.text.Replace(" ", "");
    }

    public void RevealWord()
    {
        LivesText.text = "Game Over!";
        Destroy(GuessedWord_Inptfld.gameObject);
        for (int i = 0; i < FinalWord_char.Length; i++)
        {
            SpawnText_temp[i].text = FinalWord_char[i].ToString();
        }
        StartCoroutine(ReloadAppTimer());
    }

    public void Hint()
    {
        HintText.text = Hint_Inptfld.text;

        for (int i = 0; i < FinalWord_char.Length; i++)
        {
            SpawnText_temp[i].gameObject.SetActive(false);
        }
    }

    public void SecondHint()
    {
        if (Hint_Inptfld2.text != "")
        {
            HintText2.text = Hint_Inptfld2.text;
            GuessedWord_Inptfld.gameObject.SetActive(true);
            Hint_Inptfld2.gameObject.SetActive(false);
            LivesText.gameObject.SetActive(true);

            for (int i = 0; i < FinalWord_char.Length; i++)
            {
                SpawnText_temp[i].gameObject.SetActive(true);
            }
        }

        else
        {
            Hint_Inptfld2.placeholder.GetComponent<Text>().text = "Please Provide the second hint!";
            for (int i = 0; i < FinalWord_char.Length; i++)
            {
                SpawnText_temp[i].gameObject.SetActive(false);
            }
            LivesText.gameObject.SetActive(false);
        }
    }
    public void Close()
    {
        LivesText.gameObject.SetActive(true);
        for (int i = 0; i < FinalWord_char.Length; i++)
        {
            SpawnText_temp[i].gameObject.SetActive(true);
        }
    }

    public void HidingSpawnObjects()
    {
        for (int i = 0; i < FinalWord_char.Length; i++)
        {
            SpawnText_temp[i].gameObject.SetActive(false);
        }
    }

    IEnumerator _FirstScreenTimer()
    {
        yield return new WaitForSeconds(3);
        Destroy(FirstScreen.gameObject);
        HowToPlay_Panel.SetActive(true);
    }

    IEnumerator ReloadAppTimer()
    {
        yield return new WaitForSeconds(5);
        ReloadApp();
    }
    public void ReloadApp()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}