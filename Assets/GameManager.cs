using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button[,] buttons = new Button[4,4]; 

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            for(int j =0; j < 4; j++)
            {
                string buttonName = "Button" + i.ToString()+ j.ToString();
                buttons[i,j] = GameObject.Find(buttonName).GetComponent<Button>();
                int row = i;
                int col = j;
                //Debug.Log("Button Name:"+buttons[row,col].name+$"| At Location {row}, {col}");
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getButtonName()
    {
        string buttonUsedName = GetComponent<Button>().name;

        Debug.Log(buttonUsedName);
    }
}
