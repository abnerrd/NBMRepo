
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DungeonView : MonoBehaviour
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

    public void AddRoom (GameObject newRoom)
    {
        int roomCount = GetComponent<DungeonModel> ().GetRoomCount ();
        RectTransform newRect = newRoom.GetComponent<RectTransform> ();
        newRect.SetParent (mTransform, false);
        newRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, 1.0f, mTransform.rect.width);
        newRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top, 1.0f,(mTransform.rect.height  / 5 ));
        newRect.position = new Vector2 (newRect.position.x, newRect.position.y - (mTransform.rect.height/ 5) * (roomCount-1) * newRect.lossyScale.y);
    }






    // Use this for initialization
    void Awake ()
    {
        mCurrentSprite = mDefaultSprite;
        mTransform = GetComponent<RectTransform> ();
    }

    // Update is called once per frame
    void Update ()
    {

    }
}
