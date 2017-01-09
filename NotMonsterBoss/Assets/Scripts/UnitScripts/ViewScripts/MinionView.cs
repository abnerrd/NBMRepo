using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MinonView : MonoBehaviour
{
    
    public Image mDefaultSprite;

    private Image mCurrentSprite;

    private bool mEnabled;

    public void SetEnabled (bool enabled)
    {
        mCurrentSprite.enabled = enabled;
    }

    // Use this for initialization
    void Start ()
    {
        mCurrentSprite = mDefaultSprite;
    }

    // Update is called once per frame
    void Update ()
    {

    }
}
