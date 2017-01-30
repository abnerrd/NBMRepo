
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

    public void AddRoomToDungeon (GameObject newRoom)
    {
        int roomCount = GetComponent<DungeonModel> ().GetRoomCount ();
        RectTransform newRect = newRoom.GetComponent<RectTransform> ();
        newRect.SetParent (mTransform, false);
        newRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, 1.0f, mTransform.rect.width);
        newRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top, 1.0f,(mTransform.rect.height  / 8 ));
        newRect.position = new Vector2 (newRect.position.x, newRect.position.y - (mTransform.rect.height/ 8) * (roomCount-1) * newRect.lossyScale.y);
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

    //  TODO aherrera : toook this out from DungeonModel.init 
    public void initialize(RectTransform MainCanvas)
    {
        RectTransform transform = GetComponent<RectTransform>();
        transform.SetParent(MainCanvas, false);
        transform.SetAsFirstSibling();
        transform.rect.size.Set(MainCanvas.rect.width+100, MainCanvas.rect.height+100);
        transform.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top, MainCanvas.rect.height * 0.25f, MainCanvas.rect.height * 0.65f);
        transform.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, MainCanvas.rect.width * 0.2f, MainCanvas.rect.width * 0.65f);
    }
}
