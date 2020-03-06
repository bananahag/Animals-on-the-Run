using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    public Image orangeKey, purpleKey;

    // Start is called before the first frame update
    void Start()
    {
        orangeKey.enabled = false;
        purpleKey.enabled = false;
    }

    public void ViewOrangeKey(bool view)
    {
        if (view)
            orangeKey.enabled = true;
        else
            orangeKey.enabled = false;
    }

    public void ViewPurpleKey(bool view)
    {
        if (view)
            purpleKey.enabled = true;
        else
            purpleKey.enabled = false;
    }
}
