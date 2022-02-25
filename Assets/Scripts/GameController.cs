using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public BottleController FirstBottle;
    public BottleController SecondBottle;

    public BottleController CheckSecondBottle;
    public BottleController CheckFirstBottle;

    public GameObject finishPanel;
    bool finish = false;

    public static int seemColor;

    public static int level = 1;

    public bool isMove = true;

    public static float t = 0;

    //public int lvl = level;
    void Awake()
    {
        instance = GetComponent<GameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        finish = false;
        seemColor = 0;
        //level = lvl;
    }

    // Update is called once per frame
    void Update()
    {        
        //Debug.Log("seem color = " + seemColor);
        //Debug.Log("level = "+ level);

        if (finish == true || isMove == false)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            //Debug.Log(mousePos2D);
            if(hit.collider != null)
            {                            
                if (hit.collider.GetComponent<BottleController>() != null)
                {
                    if (FirstBottle == null)
                    {                     
                        FirstBottle = hit.collider.GetComponent<BottleController>();

                        CheckFirstBottle = FirstBottle;
                        CheckFirstBottle.transform.position += Vector3.up * 0.3f;
                    }
                    else
                    {
                        if (FirstBottle == hit.collider.GetComponent<BottleController>())
                        {                            
                            FirstBottle = null;
                            CheckFirstBottle.transform.position -= Vector3.up * 0.3f;
                        }
                        else
                        {
                            isMove = false;

                            SecondBottle = hit.collider.GetComponent<BottleController>();
                            FirstBottle.bottleControllerRef = SecondBottle;

                            FirstBottle.UpdateTopColorValues();
                            SecondBottle.UpdateTopColorValues();

                            CheckSecondBottle = SecondBottle;
                            //Debug.Log(CheckSecondBottle.transform.position);

                            if (SecondBottle.FillBottleCheck(FirstBottle.topColor) == true)
                            {
                                FirstBottle.StartColorTransfer();
                                FirstBottle = null;
                                SecondBottle = null;
                            }
                            else
                            {
                                FirstBottle = null;
                                SecondBottle = null; 
                            }
                        }
                    }
                }
            }
        }
    }

    public void CheckWaveAnimationSecondBottle()
    {      
        if (level == 1)
        {
            WaterWave1_1.instance.WaterWaveSecondBottle.color = CheckSecondBottle.topColor;

            WaterWave1_1.instance.transform.LeanMoveLocal(new Vector2(CheckSecondBottle.transform.position.x, CheckSecondBottle.transform.position.y - 0.3f), 0f);

            WaterWave1_1.instance.WaterWaveSecondBottle.enabled = true;

            WaterWave1_1.instance.transform.LeanMoveLocalY(-0.2f, 0.5f);
        }

        if (level == 2)
        {
            WaterWave1_1.instance.WaterWaveSecondBottle.color = CheckSecondBottle.topColor;

            if(CheckSecondBottle.numberOfColorsInBottle == 2)
                WaterWave1_1.instance.transform.LeanMoveLocal(new Vector2(CheckSecondBottle.transform.position.x, CheckSecondBottle.transform.position.y - 0.3f), 0f);
            else if (CheckSecondBottle.numberOfColorsInBottle == 3)
                WaterWave1_1.instance.transform.LeanMoveLocal(new Vector2(CheckSecondBottle.transform.position.x, CheckSecondBottle.transform.position.y + 0.5f), 0f);

            WaterWave1_1.instance.WaterWaveSecondBottle.enabled = true;

            if(CheckFirstBottle.numberOfColorsInBottle == 4)
                WaterWave1_1.instance.transform.LeanMoveLocalY(-0.2f, 1f);
            else
                WaterWave1_1.instance.transform.LeanMoveLocalY(-0.2f, 0.5f);
        }

        if (level == 3)
        {
            WaterWave1_1.instance.WaterWaveSecondBottle.color = CheckFirstBottle.topColor;

            if (CheckSecondBottle.numberOfColorsInBottle == 0)
            {
                WaterWave1_1.instance.transform.LeanMoveLocal(new Vector2(CheckSecondBottle.transform.position.x, CheckSecondBottle.transform.position.y - 1.25f), 0f);

                if(CheckFirstBottle.numberOfTopColorLayers == 1)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-2.2f, 3f);
                else if(CheckFirstBottle.numberOfTopColorLayers == 2)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-1.8f, 2.5f);
                else if (CheckFirstBottle.numberOfTopColorLayers == 3)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-2f, 3f);

                WaterWave1_1.instance.WaterWaveSecondBottle.enabled = true;
            }

            if (CheckSecondBottle.numberOfColorsInBottle == 1)
            {
                WaterWave1_1.instance.transform.LeanMoveLocal(new Vector2(CheckSecondBottle.transform.position.x, CheckSecondBottle.transform.position.y - 1f), 0f);

                if (CheckFirstBottle.numberOfTopColorLayers == 1)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-1.8f, 1.2f);
                else if (CheckFirstBottle.numberOfTopColorLayers == 2)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-0.9f, 2.5f);
                else if (CheckFirstBottle.numberOfTopColorLayers == 3)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-0.2f, 1.5f);

                WaterWave1_1.instance.WaterWaveSecondBottle.enabled = true;
            }

            if (CheckSecondBottle.numberOfColorsInBottle == 2)
            {
                WaterWave1_1.instance.WaterWaveSecondBottle.color = CheckSecondBottle.topColor;

                WaterWave1_1.instance.transform.LeanMoveLocal(new Vector2(CheckSecondBottle.transform.position.x, CheckSecondBottle.transform.position.y - 0.3f), 0f);

                if (CheckFirstBottle.numberOfTopColorLayers == 1)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-0.7f, 0.5f);
                else if (CheckFirstBottle.numberOfTopColorLayers == 2)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-0.2f, 0.5f);

                WaterWave1_1.instance.WaterWaveSecondBottle.enabled = true;
            }

            if (CheckSecondBottle.numberOfColorsInBottle == 3)
            {
                WaterWave1_1.instance.WaterWaveSecondBottle.color = CheckSecondBottle.topColor;

                WaterWave1_1.instance.transform.LeanMoveLocal(new Vector2(CheckSecondBottle.transform.position.x, CheckSecondBottle.transform.position.y + 0.5f), 0f);

                WaterWave1_1.instance.transform.LeanMoveLocalY(-0.2f, 1f);

                WaterWave1_1.instance.WaterWaveSecondBottle.enabled = true;
            }
        }

        if (level == 4)
        {
            WaterWave1_1.instance.WaterWaveSecondBottle.color = CheckFirstBottle.topColor;

            if (CheckSecondBottle.numberOfColorsInBottle == 0)
            {
                WaterWave1_1.instance.transform.LeanMoveLocal(new Vector2(CheckSecondBottle.transform.position.x, CheckSecondBottle.transform.position.y - 1.3f), 0f);

                if (CheckFirstBottle.numberOfTopColorLayers == 1)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-2.2f, 3f);
                else if (CheckFirstBottle.numberOfTopColorLayers == 2)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-1.8f, 2.5f);
                else if (CheckFirstBottle.numberOfTopColorLayers == 3)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-2f, 3f);

                WaterWave1_1.instance.WaterWaveSecondBottle.enabled = true;
            }

            if (CheckSecondBottle.numberOfColorsInBottle == 1)
            {
                WaterWave1_1.instance.transform.LeanMoveLocal(new Vector2(CheckSecondBottle.transform.position.x, CheckSecondBottle.transform.position.y - 1f), 0f);

                if (CheckFirstBottle.numberOfTopColorLayers == 1)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-1.8f, 1.2f);
                else if (CheckFirstBottle.numberOfTopColorLayers == 2)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-0.9f, 2.5f);
                else if (CheckFirstBottle.numberOfTopColorLayers == 3)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-0.2f, 1.5f);

                WaterWave1_1.instance.WaterWaveSecondBottle.enabled = true;
            }

            if (CheckSecondBottle.numberOfColorsInBottle == 2)
            {
                WaterWave1_1.instance.WaterWaveSecondBottle.color = CheckSecondBottle.topColor;

                WaterWave1_1.instance.transform.LeanMoveLocal(new Vector2(CheckSecondBottle.transform.position.x, CheckSecondBottle.transform.position.y - 0.3f), 0f);

                if (CheckFirstBottle.numberOfTopColorLayers == 1)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-0.7f, 1f);
                else if (CheckFirstBottle.numberOfTopColorLayers == 2)
                    WaterWave1_1.instance.transform.LeanMoveLocalY(-0.2f, 0.5f);

                WaterWave1_1.instance.WaterWaveSecondBottle.enabled = true;
            }

            if (CheckSecondBottle.numberOfColorsInBottle == 3)
            {
                WaterWave1_1.instance.WaterWaveSecondBottle.color = CheckSecondBottle.topColor;

                WaterWave1_1.instance.transform.LeanMoveLocal(new Vector2(CheckSecondBottle.transform.position.x, CheckSecondBottle.transform.position.y + 0.5f), 0f);

                WaterWave1_1.instance.transform.LeanMoveLocalY(-0.2f, 1f);

                WaterWave1_1.instance.WaterWaveSecondBottle.enabled = true;
            }
        }
    }

    /*public void CheckWaveAnimationFirstBottle()
    {
        WaterWave2_2.instance.WaterWaveFirstBottle.color = CheckSecondBottle.topColor;

        WaterWave2_2.instance.transform.position = CheckFirstBottle.transform.position;

        //WaterWave2_2.instance.transform.LeanMoveLocal(CheckFirstBottle.transform.position, 2f);

        WaterWave2_2.instance.WaterWaveFirstBottle.enabled = true;
    }*/

    public void CheckBottle()
    {
        seemColor++;
    }

    public void SeemColorNumber(int value)
    {
        seemColor = value;
    }

    public void LevelNumber(int value)
    {
        level = value;
    }

    void LateUpdate()
    {
        if (level == 1 && seemColor == 1)
        {
            finish = true;
            finishPanel.SetActive(true);
        }
        else if(level == 2 && seemColor == 2)
        {
            finish = true;
            finishPanel.SetActive(true);
        }
        else if (level == 3 && seemColor == 3)
        {
            finish = true;
            finishPanel.SetActive(true);
        }
        else if (level == 4 && seemColor == 3)
        {
            finish = true;
            finishPanel.SetActive(true);
        }
    }
}
