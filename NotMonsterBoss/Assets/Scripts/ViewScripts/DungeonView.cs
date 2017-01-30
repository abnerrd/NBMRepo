
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    public void AddRoomToDungeon (GameObject newRoom, List<RoomModel> roomList)
    {
        int roomCount = GetComponent<DungeonModel> ().GetRoomCount ();
        if (roomCount == 1) {
            RectTransform newRect = newRoom.GetComponent<RectTransform> ();
            newRect.SetParent (mTransform, false);
            newRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, 0f, mTransform.rect.width);
            newRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top, 0f, (mTransform.rect.height / 4));
        } 
        else 
        {
            InsertRoomIntoDungeon (newRoom, roomList);
        }
        newRoom.GetComponent<RoomView> ().initialize ();
    }

    public void InsertRoomIntoDungeon (GameObject newRoom, List<RoomModel> roomList)
    {
        int roomCount = roomList.Count;

        for (int i = 0; i < roomCount; i++) 
        {
            GameObject roomObject = roomList[i].gameObject;

            RectTransform newRect = roomObject.GetComponent<RectTransform> ();
            newRect.SetParent (mTransform, false);
            newRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, 0f, mTransform.rect.width);
            newRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top, 0f, (mTransform.rect.height / 4));
            newRect.position = new Vector2 (newRect.position.x, 
                                            newRect.position.y + (mTransform.rect.height / 4 * i * newRect.lossyScale.y) - ((mTransform.rect.height / 4) * (roomCount-1)));
        }
    }


    //  TODO aherrera : toook this out from DungeonModel.init 
    public void initialize(RectTransform MainCanvas)
    {
        mCurrentSprite = mDefaultSprite;
        mTransform = GetComponent<RectTransform> ();

        RectTransform transform = GetComponent<RectTransform>();
        transform.SetParent(MainCanvas, false);
        transform.SetAsFirstSibling();
        transform.rect.size.Set(MainCanvas.rect.width+100, MainCanvas.rect.height+100);
        transform.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top, MainCanvas.rect.height * 0.25f, MainCanvas.rect.height * 0.55f);
        transform.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, MainCanvas.rect.width * 0.2f, MainCanvas.rect.width * 0.65f);
    }
}
