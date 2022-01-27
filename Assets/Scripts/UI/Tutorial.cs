using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] Image displayImage;
    [SerializeField] List<Sprite> slides;
    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] List<string> texts;
    [SerializeField] Button previousButton, nextButton;

    int currentSlide = 0;
    void Start()
    {
        if(slides.Count > 0)
            displayImage.sprite = slides[0];
        if (texts.Count > 0)
            displayText.SetText(texts[0]);
    }

    public void PreviousSlide()
    {
        --currentSlide;
        if (currentSlide == 0)
            previousButton.interactable = false;
        else if (!previousButton.interactable)
            previousButton.interactable = true;

        displayImage.sprite = slides[currentSlide];
    }

    public void NextSlide()
    {
        ++currentSlide;
        if (currentSlide == slides.Count - 1)
            nextButton.interactable = false;
        else if (!nextButton.interactable)
            nextButton.interactable = true;

        displayImage.sprite = slides[currentSlide];
    }

    public void ChangeSlide(int value)
    {
        currentSlide = Mathf.Clamp(currentSlide + value, 0, slides.Count - 1);

        if (!previousButton.interactable && currentSlide > 0)
            previousButton.interactable = true;
        else if (previousButton.interactable && currentSlide == 0)
            previousButton.interactable = false;
        else if (!nextButton.interactable && currentSlide < slides.Count - 1)
            nextButton.interactable = true;
        else if (nextButton.interactable && currentSlide == slides.Count - 1)
            nextButton.interactable = false;

        displayImage.sprite = slides[currentSlide];
        displayText.SetText(texts[currentSlide]);
    }
}
