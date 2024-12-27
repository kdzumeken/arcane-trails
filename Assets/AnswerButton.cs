using UnityEngine;
using UnityEngine.UI;


public class AnswerButton : MonoBehaviour
{
    public Button correctButton;  // Assign button yang benar di Inspector
    public Button[] allButtons;   // Assign semua tombol di Inspector
    public GameObject quizPanel;  // Panel quiz yang ditampilkan
    public GameObject Door_Prefab_Opened; // Pintu yang akan dibuka
    private bool isNearDoor = false;
    private bool quizAnsweredCorrectly = false;

    private void Start()
    {
        quizPanel.SetActive(false);
        foreach (Button btn in allButtons)
        {
            btn.onClick.AddListener(() => OnButtonClick(btn));
        }
    }

    private void Update()
    {
        if (isNearDoor && Input.GetKeyDown(KeyCode.E))
        {
            if (!quizAnsweredCorrectly)
            {
                quizPanel.SetActive(true);
            }
            else
            {
               // Door_Prefab_Opened.GetComponent<DoorInteraction>().UnlockDoor();
            }
        }
    }

    void OnButtonClick(Button clickedButton)
    {
        if (clickedButton == correctButton)
        {
            clickedButton.GetComponent<Image>().color = Color.green;
            quizAnsweredCorrectly = true;
            quizPanel.SetActive(false);
        }
        else
        {
            clickedButton.GetComponent<Image>().color = Color.red;
            Invoke("CloseQuiz", 0.5f);
        }
    }

    void CloseQuiz()
    {
        quizPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wizard"))
        {
            isNearDoor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("wizard"))
        {
            isNearDoor = false;
        }
    }
}
