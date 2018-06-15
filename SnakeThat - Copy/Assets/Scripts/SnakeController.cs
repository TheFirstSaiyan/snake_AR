using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour {
	//0 ,2.5,44.4

	private const float SPEED=1f;
	private float bound=10f;
	public GameObject food;
    private int total = 1;
    public GameObject ground;
    private const float multiplier = 5;
    private List<GameObject> parts;
    private List<Vector3> positions;
	private Vector3 spawn;
	private string dir;
    private float spacing =1f;
    private Vector3 touch;
    private float XThres = 35f;
    private float ZThres = 35f;
    public TextMesh scoreText;
    public TextMesh highscoreText;
    private int highScore;

    private int score = 0;
    void Start () {
		transform.localPosition = new Vector3 (0f,0.275f,0f);
        spawn = transform.localPosition;
		dir = "none";
		food.transform.localPosition = new Vector3(Random.Range(-1*bound + 0.3f, bound - 0.3f), 0.275f, Random.Range(-1*bound + 0.3f, bound - 0.3f));
        parts = new List<GameObject>();
        positions = new List<Vector3>();
        parts.Add(transform.gameObject);
        positions.Add(transform.localPosition);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate=20;
        scoreText.text = "SCORE :" + score;
       // PlayerPrefs.SetInt("highscore", 0);
        highScore = PlayerPrefs.GetInt("highscore", 0);
        highscoreText.text = "BEST :" + highScore;

    }

     

    // Update is called once per frame
    private void Update () {
		GetDirection ();
        MoveSnake();
        CheckBounds();
        CheckGameOver();
        scoreText.text = "SCORE :" + score;
        highscoreText.text = "BEST :" + highScore;
        for (int i =positions.Count-1; i > 0; i--) // shift down body parts of snake............
            {

            
            positions[i] = positions[i -1];
          

        }

          if (positions.Count >=1) 
          {
           
             positions[0] = transform.localPosition;

        }
        for (int i = 1; i < positions.Count; i++)
        {
            GameObject c = GameObject.CreatePrimitive(PrimitiveType.Cube);
            c.transform.SetParent(ground.transform, false);
            c.gameObject.transform.localPosition = positions[i];
            c.gameObject.transform.localRotation = transform.localRotation;
            c.gameObject.transform.localScale = transform.localScale;
            
            parts.Insert(1, c);
        }

      
        for (int i = total ; i < parts.Count; i++)
            {
            GameObject k = parts[i];
               Destroy(k);
                parts.RemoveAt(i);


            }
      

    }

    private void CheckBounds()
    {
		if (positions[0].x > bound)
        {

			transform.localPosition = new Vector3(-1*bound, transform.localPosition.y, transform.localPosition.z);
			positions[0] = new Vector3(-1*bound, transform.localPosition.y, transform.localPosition.z);
        }

		if (positions[0].x < -1*bound)
        {
			transform.localPosition = new Vector3(bound, transform.localPosition.y, transform.localPosition.z);
			positions[0] = new Vector3(bound, transform.localPosition.y, transform.localPosition.z);
        }

		if (positions[0].z < -1*bound)
        {
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,bound);
			positions[0] = new Vector3(transform.localPosition.x, transform.localPosition.y, bound);
        }

		if (positions[0].z > bound)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -1*bound);
			positions[0] = new Vector3(transform.localPosition.x, transform.localPosition.y, -1*bound);
        }
        }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("food"))
        {
            GameObject c = GameObject.CreatePrimitive(PrimitiveType.Cube);
            c.transform.SetParent(ground.transform, false);
            if (dir == "LEFT")
                c.gameObject.transform.localPosition = new Vector3(transform.localPosition.x , transform.localPosition.y, transform.localPosition.z);
            if (dir == "RIGHT")
                c.gameObject.transform.localPosition = new Vector3(transform.localPosition.x , transform.localPosition.y, transform.localPosition.z);
            if (dir == "UP")
                c.gameObject.transform.localPosition = new Vector3(transform.localPosition.x , transform.localPosition.y, transform.localPosition.z );
            if (dir == "DOWN")
                c.gameObject.transform.localPosition = new Vector3(transform.localPosition.x , transform.localPosition.y, transform.localPosition.z);
            c.gameObject.transform.localRotation = transform.localRotation;
            c.gameObject.transform.localScale = transform.localScale;
            parts.Insert(1, c);
            total += 1;
            positions.Add(c.gameObject.transform.localPosition);
					food.transform.localPosition = new Vector3(Random.Range(-1*bound + 0.3f, bound - 0.3f), 0.275f, Random.Range(-1*bound + 0.3f, bound - 0.3f));
            score += 1;

            if (score >= highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("highscore", score);
            }
        }
    }
   

    private void MoveSnake()
	{
		if ( dir == "UP") 
		{
			transform.localPosition+=new Vector3(0,0, SPEED );

		} 
		else if ( dir == "DOWN") 
		{
			transform.localPosition+=new Vector3(0,0,-1*SPEED );
		}
		else if (dir== "RIGHT") 
		{
			transform.localPosition+=new Vector3(SPEED ,0,0);
		}
		else if (dir == "LEFT") 
		{
			transform.localPosition+=new Vector3(-1*SPEED ,0,0);
		}

	}

	private void GetDirection()
	{


        if (Input.GetMouseButtonDown(0))
        {

            touch = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 difference = Input.mousePosition - touch;

            if ( Mathf.Abs(difference.x)  > XThres  &&  difference.x > 0 && dir != "LEFT")
                dir = "RIGHT";
             else if (Mathf.Abs(difference.x) > XThres && difference.x < 0 && dir != "RIGHT")
                dir = "LEFT";
             else if (Mathf.Abs(difference.y) > ZThres &&difference.y < 0 && dir != "UP")
                dir = "DOWN";

             else if (Mathf.Abs(difference.y) > ZThres && difference.y > 0 && dir != "DOWN")
                dir = "UP";




        }

        if (Input.GetKeyDown (KeyCode.UpArrow) && dir != "DOWN") 
		{
			dir = "UP";
		} 
		else if (Input.GetKeyDown (KeyCode.DownArrow) && dir != "UP") 
		{
			dir = "DOWN";
		}
		else if (Input.GetKeyDown (KeyCode.LeftArrow) && dir != "RIGHT") 
		{
			dir = "LEFT";
		}
		else if (Input.GetKeyDown (KeyCode.RightArrow) && dir != "LEFT") 
		{
			dir = "RIGHT";
		}

	}

    private void CheckGameOver()
    {
        for (int i = 1; i < positions.Count; i++)
        {
            if(Vector3.Distance(transform.localPosition,positions[i]) < 0.1f)      // check for contact with body  parts
            {
                score = 0;
                for (int j = 1; j < parts.Count; j++)
                {
                    GameObject k = parts[j];
                    Destroy(k);
                   parts.RemoveAt(j);


                }
                transform.localPosition = spawn;
                positions.Clear();
                positions.Add(spawn);
                total = 1;
                break;
            }

        }

    }
}
