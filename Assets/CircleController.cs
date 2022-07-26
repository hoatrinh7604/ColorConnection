using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleController : MonoBehaviour
{
    [SerializeField] Image color;
    [SerializeField] Button button;
    public int colorIndex;

    float distance = 10;
    bool canMove = false;

    public bool death = false;

    [SerializeField] GameObject effect;
    private Camera cam;
    private void Start()
    {
        //button.onClick.AddListener(() => Press());
        cam = GamePlayController.Instance.UICamera;
    }

    private void Update()
    {
        if(canMove)
        {
            Ray r = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 pos = r.GetPoint(distance);
            transform.position = new Vector3 (pos.x, pos.y, transform.position.z);
        }
    }

    private void OnMouseDown()
    {
        canMove = true;
    }

    private void OnMouseUp()
    {
        canMove = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Circle")
        {
            if(colorIndex == collision.gameObject.GetComponent<CircleController>().colorIndex)
            {
                if (!death)
                {
                    death = true;
                    collision.gameObject.GetComponent<CircleController>().death = true;
                    SoundController.Instance.PlayAudio(SoundController.Instance.bang, 0.1f, false);
                    GamePlayController.Instance.UpdateSliderByValue(1f);
                    SpawEffect();
                    Destroy(collision.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public void SetColorIndex(int value)
    {
        colorIndex = value;
        UpdateSprite();
    }

    public int GetColorIndex()
    {
        return colorIndex;
    }

    public void UpdateSprite()
    {
        color.color = GamePlayController.Instance.template[colorIndex];
    }

    public void Press()
    {
        GamePlayController.Instance.OnPressHandle(colorIndex);
        Destroy(this.gameObject, 0.05f);
    }

    public void RandomColor()
    {
        SetColorIndex(Random.Range(0, GamePlayController.Instance.template.Length));
    }

    public void SpawEffect()
    {
        GameObject eff = Instantiate(effect, Vector2.zero, Quaternion.identity, transform.parent);
        var main = eff.GetComponent<ParticleSystem>().main;
        main.startColor = GamePlayController.Instance.template[colorIndex];
        
        eff.transform.localPosition = Vector3.zero;
        eff.transform.localScale = Vector3.one;
        eff.transform.position = transform.position;
        Destroy(eff, 0.5f);
    }
}
