using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] GameObject tooltip;
    [SerializeField] float tooltipPaddingY = 0.7f;

    public GameObject ReturnTooltip()
    {
        return tooltip;
    }

    public float ReturnPadding()
    {
        return tooltipPaddingY;
    }

}
