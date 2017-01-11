using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomView : MonoBehaviour
{

    public Image mDefaultSprite;
    private Image mCurrentSprite;
    private RectTransform mTransform;

    private bool mEnabled;

    public void SetEnabled (bool enabled)
    {
        mCurrentSprite.enabled = enabled;
    }

    public void SetRectPosition (Vector2 pos)
    {
        mTransform.position = pos;
    }
    public Vector2 GetRectPosition ()
    {
        return new Vector2(mTransform.position.x, mTransform.position.y);
    }

    // Use this for initialization
    void Start ()
    {
        mCurrentSprite = mDefaultSprite;
        mTransform = GetComponent<RectTransform> ();
    }

    // Update is called once per frame
    void Update ()
    {

    }
}
