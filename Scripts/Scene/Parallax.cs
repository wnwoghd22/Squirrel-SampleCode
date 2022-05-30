using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float parallaxEff;

    [SerializeField] private GameObject[] front;
    [SerializeField] private GameObject[] middle;
    [SerializeField] private GameObject[] back;

    [SerializeField] private Sprite[] frontSprites;
    [SerializeField] private Sprite[] middleSprites;
    [SerializeField] private Sprite[] backSprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetSeason(int i)
    {
        --i;

        foreach(GameObject o in front)
        {
            o.GetComponent<SpriteRenderer>().sprite = frontSprites[i];
        }
        foreach (GameObject o in middle)
        {
            o.GetComponent<SpriteRenderer>().sprite = middleSprites[i];
        }
        foreach (GameObject o in back)
        {
            o.GetComponent<SpriteRenderer>().sprite = backSprites[i];
        }
    }

    public void Scroll()
    {
        ScrollLine(front, scrollSpeed);
        ScrollLine(middle, scrollSpeed * parallaxEff);
        ScrollLine(back, scrollSpeed * parallaxEff * parallaxEff);
    }
    private void ScrollLine(GameObject[] line, float speed)
    {
        Vector3 pos;

        foreach (GameObject o in line)
        {
            pos = o.transform.position;
            pos.x -= Time.deltaTime * speed;
            if (pos.x < leftBound) pos.x = rightBound;
            o.transform.position = pos;
        }
    }
}
