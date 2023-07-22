using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabletUIBehaviour : MonoBehaviour
{
    public List<GameObject> questions;
    public GameObject prevButton;
    public GameObject nextButton;
    public GameObject submitButton;
    public ToggleGroup toggleGroup;
    private bool submitted;
    private bool allAnswered;
//    public DropDown dropDown;

    private int questionIdx;

    private int[] responses;

    private void Awake()
    {
        questionIdx = 0;
        submitted = false;
        responses = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
        allAnswered = false;

    }

    // Start is called before the first frame update
    void Start()
    {
        // set correct camers
        gameObject.GetComponent<Canvas>().worldCamera = PlayerSingleton.Instance.GetComponentInChildren<Camera>();

        foreach (GameObject q in questions)
            q.SetActive(false);
        questions[questionIdx].SetActive(true);   

        if(questionIdx == 0)
        {
            prevButton.SetActive(false);
            submitButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getCurrentQuestionId()
    {
        return questionIdx;
    }

    public void buttonPressed(string name)
    {
        if (allAnswered)
            submitButton.SetActive(true);

        if (name == prevButton.name)
        {
            if (questionIdx > 0)
            {
                questions[questionIdx].SetActive(false);
                questionIdx -= 1;
                questions[questionIdx].SetActive(true);

                toggleGroup.SetAllTogglesOff();
                
                //if answered previously
                if (responses[questionIdx] != -1)
                {
                    var toggles = GetComponentsInChildren<Toggle>();
                    foreach (Toggle t in toggles)
                    {
                        if (t.name == responses[questionIdx].ToString())
                        {
                            t.isOn = true;
                            break;
                        }
                    }
                }

            }

            if(questionIdx == 0)
                prevButton.SetActive(false);
        }

        else if (name == nextButton.name)
        {
            // force selection of atleast 1 toggle
            if (!toggleGroup.AnyTogglesOn())
                return;

            Toggle response = toggleGroup.GetFirstActiveToggle();
            responses[questionIdx] = int.Parse(response.name);

            if (questionIdx == questions.Count - 2)
                allAnswered = true;
            
            if (questionIdx < questions.Count - 2)
            {
                
                questions[questionIdx].SetActive(false);
                questionIdx += 1;
                questions[questionIdx].SetActive(true);

                toggleGroup.SetAllTogglesOff();

                //if answered previously
                if(responses[questionIdx] != -1)
                {
                    var toggles = GetComponentsInChildren<Toggle>();
                    foreach(Toggle t in toggles)
                    {
                        if(t.name == responses[questionIdx].ToString())
                        {
                            t.isOn = true;
                            break;
                        }
                    }
                }

            }

            if (questionIdx > 0)
                prevButton.SetActive(true);
        }

        else if (name == submitButton.name)
        {
            questions[questionIdx].SetActive(false);
            questionIdx += 1;
            questions[questionIdx].SetActive(true);
            nextButton.SetActive(false);
            prevButton.SetActive(false);
            submitButton.SetActive(false);
            submitted = true;

            var sc = GameObject.Find("SceneController").GetComponent<SceneController>();
            sc.SaveTabletData(responses);
        }

    }
    public bool IsSubmitted()
    {
        return submitted;
    }
}
