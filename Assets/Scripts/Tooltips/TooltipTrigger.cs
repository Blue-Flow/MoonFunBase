using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("Optional")] public string header;
    public string content;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(nameof(ShowTooltip));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(nameof(ShowTooltip));
        TooltipSystem.Hide();
    }

    IEnumerator ShowTooltip()
    {
        yield return new WaitForSeconds(0.5f);
        TooltipSystem.Show(content, header);
    }
}
