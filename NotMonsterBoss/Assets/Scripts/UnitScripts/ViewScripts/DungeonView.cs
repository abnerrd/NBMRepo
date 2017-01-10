
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
        newRect.SetParent (GetComponent<RectTransform> ());
        newRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, 1.0f, GetComponent<RectTransform> ().rect.width);
        newRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top, 1.0f, newRect.rect.height);
        newRect.position = new Vector2 (newRect.position.x, newRect.position.y - newRect.rect.height * (roomCount-1));
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
