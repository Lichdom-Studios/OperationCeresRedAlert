using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class QualityControl : MonoBehaviour
{
    [SerializeField] List<RenderPipelineAsset> qualityLevels;
    [SerializeField] TMP_Dropdown dropdown;
    
    void Start()
    {
        dropdown.value = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());

        if(dropdown.value != QualitySettings.GetQualityLevel())
        {
            QualitySettings.renderPipeline = qualityLevels[dropdown.value];
        }

        Debug.Log("Quality Settings Set To: " + QualitySettings.GetRenderPipelineAssetAt(dropdown.value));
    }

    public void ChangeQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = qualityLevels[value];
    }

    public void SaveQuality()
    {
        PlayerPrefs.SetInt("Quality", QualitySettings.GetQualityLevel());
    }
}
