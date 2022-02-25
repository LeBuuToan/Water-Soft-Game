using System.Collections;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    public static BottleController instance;

    public Color[] bottleColors;
    public SpriteRenderer bottleMaskSR;

    public AnimationCurve ScaleAndRotateMultiplierCurve;
    public AnimationCurve FillAmountCurve;

    public AnimationCurve RotationSpeedMultiplier;

    public float[] fillAmounts;
    public float[] rotationValues;

    private int rotationIndex = 0;

    [Range (0, 4)]
    public int numberOfColorsInBottle = 4;

    public Color topColor;
    public int numberOfTopColorLayers = 1;

    public BottleController bottleControllerRef;
    //public bool justThisBottle = false;
    private int numberOfColorsToTransfer = 0;

    public Transform leftRotationPoint;
    public Transform rightRotationPoint;
    private Transform chosenRotationPoint;

    private float directionMultiplier = 1.0f;

    public Vector3 originalPostition;
    public Vector3 startPosition;
    public Vector3 endPostition;

    public LineRenderer lineRenderer;

    public GameObject confettiEF;

    void Awake()
    {
        instance = GetComponent<BottleController>();      
    }

    // Start is called before the first frame update
    void Start()
    {
        bottleMaskSR.material.SetFloat("_FillAmount", fillAmounts[numberOfColorsInBottle]);

        originalPostition = transform.position;

        UpdateColorsOnShader();

        UpdateTopColorValues();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyUp(KeyCode.P) && justThisBottle == true)
        {
            UpdateTopColorValues();

            if (bottleControllerRef.FillBottleCheck(topColor))
            {
                ChoseRotationPointAndDirection();

                numberOfColorsToTransfer = Mathf.Min(numberOfTopColorLayers, 4 - bottleControllerRef.numberOfColorsInBottle);

                for (int i = 0; i < numberOfColorsToTransfer; i++)
                {
                    bottleControllerRef.bottleColors[bottleControllerRef.numberOfColorsInBottle + i] = topColor;
                }
                bottleControllerRef.UpdateColorsOnShader();
            }
            CaculateRotationIndex(4 - bottleControllerRef.numberOfColorsInBottle);
            StartCoroutine(RotateBottle());
        }*/
    }

    public void StartColorTransfer()
    {
        ChoseRotationPointAndDirection();

        numberOfColorsToTransfer = Mathf.Min(numberOfTopColorLayers, 4 - bottleControllerRef.numberOfColorsInBottle);

        for (int i = 0; i < numberOfColorsToTransfer; i++)
        {
            bottleControllerRef.bottleColors[bottleControllerRef.numberOfColorsInBottle + i] = topColor;
        }
        bottleControllerRef.UpdateColorsOnShader();

        CaculateRotationIndex(4 - bottleControllerRef.numberOfColorsInBottle);

        transform.GetComponent<SpriteRenderer>().sortingOrder += 2;
        bottleMaskSR.sortingOrder += 2;

        StartCoroutine(MoveBottle());
    }

    IEnumerator MoveBottle()
    {
        startPosition = transform.position;

        if(chosenRotationPoint == leftRotationPoint)
        {
            endPostition = bottleControllerRef.rightRotationPoint.position;
        }
        else
        {
            endPostition = bottleControllerRef.leftRotationPoint.position;
        }

        float t = 0;

        while(t<1)
        {
            transform.position = Vector3.Lerp(startPosition, endPostition, t);
            t += Time.deltaTime * 2;

            yield return new WaitForEndOfFrame();            
        }

        transform.position = endPostition;

        StartCoroutine(RotateBottle());        
    }

    IEnumerator MoveBottleBack()
    {
        startPosition = transform.position;
        endPostition = originalPostition;
        
        float t = 0;

        while (t < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPostition, t);
            t += Time.deltaTime * 2;

            GameController.instance.isMove = true;

            yield return new WaitForEndOfFrame();
        }

        transform.position = endPostition;

        transform.GetComponent<SpriteRenderer>().sortingOrder -= 2;
        bottleMaskSR.sortingOrder -= 2;

        StartCoroutine(BottleIsFull());
    }

    void UpdateColorsOnShader()
    {
        bottleMaskSR.material.SetColor("_C3", bottleColors[0]);
        bottleMaskSR.material.SetColor("_C4", bottleColors[1]);
        bottleMaskSR.material.SetColor("_C2", bottleColors[2]);
        bottleMaskSR.material.SetColor("_C1", bottleColors[3]);      
    }

    public float timeToRotate = 1.0f;

    IEnumerator RotateBottle()
    {
        float t = 0;
        float lerpValue;
        float angleValue;

        float lastAngleValue = 0;

        while (t < timeToRotate)
        {
            lerpValue = t / timeToRotate;
            angleValue = Mathf.Lerp(0.0f, directionMultiplier * rotationValues[rotationIndex], lerpValue);

            //transform.eulerAngles = new Vector3(0, 0, angleValue);
             

            transform.RotateAround(chosenRotationPoint.position, Vector3.forward, lastAngleValue - angleValue);

            bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotateMultiplierCurve.Evaluate(angleValue));

            if (fillAmounts[numberOfColorsInBottle] > FillAmountCurve.Evaluate(angleValue) + 0.005f)
            {
                if(lineRenderer.enabled == false)
                {
                    //lineRenderer.startColor = topColor;
                    //lineRenderer.endColor = topColor;

                    lineRenderer.material.color = topColor;

                    lineRenderer.SetPosition(0, chosenRotationPoint.position);
                    lineRenderer.SetPosition(1, chosenRotationPoint.position - Vector3.up * 5f);
             
                    lineRenderer.enabled = true;

                    SFXManager.instance.PlaySFX(Clip.water_pour2);

                    GameController.instance.CheckWaveAnimationSecondBottle();
                }

                bottleMaskSR.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(angleValue));

                bottleControllerRef.FillUp(FillAmountCurve.Evaluate(lastAngleValue)
                    - FillAmountCurve.Evaluate(angleValue));
            }

            t += Time.deltaTime * RotationSpeedMultiplier.Evaluate(angleValue);
            lastAngleValue = angleValue;        

            yield return new WaitForEndOfFrame();
        }
        angleValue = directionMultiplier * rotationValues[rotationIndex];

        //transform.eulerAngles = new Vector3(0, 0, angleValue);

        bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotateMultiplierCurve.Evaluate(angleValue));
        bottleMaskSR.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(angleValue));

        numberOfColorsInBottle -= numberOfColorsToTransfer;

        bottleControllerRef.numberOfColorsInBottle += numberOfColorsToTransfer;

        lineRenderer.enabled = false;

        WaterWave1_1.instance.WaterWaveSecondBottle.enabled = false;       

        StartCoroutine(RotateBottleBack());

        int checkFull = 0;
        if (GameController.instance.CheckSecondBottle.numberOfColorsInBottle == 4)
        {
            for (int i = 0; i < 3; i++)
            {
                if (GameController.instance.CheckSecondBottle.bottleColors[i]
                    == GameController.instance.CheckSecondBottle.bottleColors[i + 1])
                {
                    checkFull++;
                }
            }

            if(checkFull == 3)
            {
                Instantiate(confettiEF, GameController.instance.CheckSecondBottle.transform.position, Quaternion.identity);
                SFXManager.instance.PlaySFX(Clip.ContainerFinish);
            }        
        }
    }

    IEnumerator RotateBottleBack()
    {            
        float t = 0;
        float lerpValue;
        float angleValue;

        float lastAngleValue = directionMultiplier * rotationValues[rotationIndex];

        while (t < timeToRotate)
        {
            lerpValue = t / timeToRotate;
            angleValue = Mathf.Lerp(directionMultiplier * rotationValues[rotationIndex], 0.0f, lerpValue);

            //transform.eulerAngles = new Vector3(0, 0, angleValue);

            transform.RotateAround(chosenRotationPoint.position, Vector3.forward, lastAngleValue - angleValue);

            bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotateMultiplierCurve.Evaluate(angleValue));

            lastAngleValue = angleValue;

            t += Time.deltaTime;         

            yield return new WaitForEndOfFrame();
        }
        UpdateTopColorValues();
        angleValue = 0;
        transform.eulerAngles = new Vector3(0, 0, angleValue);
        bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotateMultiplierCurve.Evaluate(angleValue));

        StartCoroutine(MoveBottleBack());       
    }

    public void UpdateTopColorValues()
    {
        if (numberOfColorsInBottle != 0)
        {
            numberOfTopColorLayers = 1;

            topColor = bottleColors[numberOfColorsInBottle - 1];

            if (numberOfColorsInBottle == 4)
            {
                if (bottleColors[3].Equals(bottleColors[2]))
                {
                    numberOfTopColorLayers = 2;
                    if (bottleColors[2].Equals(bottleColors[1]))
                    {
                        numberOfTopColorLayers = 3;
                        if (bottleColors[3].Equals(bottleColors[0]))
                        {
                            numberOfTopColorLayers = 4;
                        }
                    }
                }
            }
            else if (numberOfColorsInBottle == 3)
            {
                if (bottleColors[2].Equals(bottleColors[1]))
                {
                    numberOfTopColorLayers = 2;
                    if (bottleColors[1].Equals(bottleColors[0]))
                    {
                        numberOfTopColorLayers = 3;                       
                    }
                }
            }
            else if (numberOfColorsInBottle == 2)
            {
                if (bottleColors[1].Equals(bottleColors[0]))
                {
                    numberOfTopColorLayers = 2;
                }
            }

            rotationIndex = 3 - (numberOfColorsInBottle - numberOfTopColorLayers);
        }

    }

    public bool FillBottleCheck(Color colorToCheck)
    {
        int checkFull = 0;

        if (GameController.instance.CheckFirstBottle.numberOfColorsInBottle == 4)
        {
            for (int i = 0; i < 3; i++)
            {
                if (GameController.instance.CheckFirstBottle.bottleColors[i]
                    == GameController.instance.CheckFirstBottle.bottleColors[i + 1])
                {
                    checkFull++;
                }
            }

            if (checkFull == 3)
            {
                GameController.instance.isMove = true;
                GameController.instance.FirstBottle = null;
                GameController.instance.CheckFirstBottle.transform.position -= Vector3.up * 0.3f;
                return false;
            }
        }

        if (numberOfColorsInBottle == 0)
        {
            return true;
        }
        else
        {          
            if (numberOfColorsInBottle == 4)
            {
                GameController.instance.isMove = true;
                GameController.instance.FirstBottle = null;
                GameController.instance.CheckFirstBottle.transform.position -= Vector3.up * 0.3f;
                return false;
            }
            else
            {
                if (topColor.Equals(colorToCheck))
                {
                    return true;
                }
                else
                {
                    GameController.instance.isMove = true;
                    GameController.instance.FirstBottle = null;
                    GameController.instance.CheckFirstBottle.transform.position -= Vector3.up * 0.3f;
                    return false;
                }
            }
        }
    }

    private void CaculateRotationIndex(int numberOfEmptySpacesInSecondBottle)
    {
        rotationIndex = 3 - (numberOfColorsInBottle - Mathf.Min(numberOfEmptySpacesInSecondBottle, numberOfTopColorLayers));
    }

    private void FillUp(float fillAmountToAdd)
    {        
        bottleMaskSR.material.SetFloat("_FillAmount", bottleMaskSR.material.GetFloat("_FillAmount") + fillAmountToAdd);      
    }

    private void ChoseRotationPointAndDirection()
    {
        if (transform.position.x > bottleControllerRef.transform.position.x)
        {
            chosenRotationPoint = leftRotationPoint;
            directionMultiplier = -1.0f;
        }
        else
        {
            chosenRotationPoint = rightRotationPoint;
            directionMultiplier = 1.0f;
        }
    }

    IEnumerator BottleIsFull()
    {
        int checkFull = 0;
        if (GameController.instance.CheckSecondBottle.numberOfColorsInBottle == 4)
        {          
            for(int i = 0; i < 3; i++)
            {
                if(GameController.instance.CheckSecondBottle.bottleColors[i] 
                    == GameController.instance.CheckSecondBottle.bottleColors[i+1])
                {
                    checkFull++;
                }
            }
            if(checkFull == 3)
                GameController.instance.CheckBottle(); 
            
            yield return new WaitForEndOfFrame();
        }       
    }
}
