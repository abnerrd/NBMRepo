using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomView : MonoBehaviour
{

    public Image mDefaultSprite;
    private Image mCurrentSprite;
    private RectTransform mTransform;

    public GameObject _Icon;
    public GameObject _PartArea;

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

    public void initialize ()
    {
        mCurrentSprite = mDefaultSprite;
        mTransform = GetComponent<RectTransform> ();
        RectTransform iconRect = _Icon.GetComponent<RectTransform> ();
        RectTransform partyRect = _PartArea.GetComponent<RectTransform> ();

        iconRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Right, -iconRect.rect.width/2, iconRect.rect.width);
        partyRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, mTransform.rect.width *0.15f , mTransform.rect.width * 0.70f);
        partyRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top, mTransform.rect.height * 0.45f, mTransform.rect.height * 0.35f);
    }

    public void addAdventurer (GameObject adventurer)
    {
        RectTransform adventurerRect = adventurer.GetComponent<RectTransform> ();
        RectTransform partyRect = _PartArea.GetComponent<RectTransform> ();

        adventurerRect.SetParent (partyRect);
        adventurerRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, 0f, partyRect.rect.width / 6);
        adventurerRect.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top, 0f, partyRect.rect.height);
    }

}
