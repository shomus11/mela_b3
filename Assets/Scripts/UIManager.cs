using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    float baseAnimationDuration = .25f;
    public List<RectTransform> buttons;
    public RectTransform backButton;
    public ColorChanger colorChanger;
    public ObjectController objectController;

    [SerializeField] int menu = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectController.SetAnimator(false);
        objectController.Init();
        objectController.SetPosition();
        objectController.canRot = true;
        colorChanger.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectButtonMenu(int selected)
    {
        menu = selected;
        float totalAnimationDuration = 0;
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < buttons.Count; i++)
        {
            sequence.Insert(totalAnimationDuration,
                buttons[i].DOScale(Vector3.zero, baseAnimationDuration).From(Vector3.one).SetEase(Ease.OutBack))
                .OnComplete(() =>
                {
                    buttons[i].gameObject.SetActive(false);
                });
        }
        totalAnimationDuration += baseAnimationDuration;
        sequence.InsertCallback(totalAnimationDuration, () => { backButton.gameObject.SetActive(true); });
        sequence.Insert(totalAnimationDuration, backButton.DOScale(Vector3.one, baseAnimationDuration).From(Vector3.zero).SetEase(Ease.OutBack)).OnComplete(() =>
        {
            SelectMenu();
        });
    }
    public void Back()
    {
        CloseMenu();
        float totalAnimationDuration = 0;
        Sequence sequence = DOTween.Sequence();

        sequence.Insert(totalAnimationDuration,
           backButton.DOScale(Vector3.zero, baseAnimationDuration).From(Vector3.one).SetEase(Ease.OutBack))
           .OnComplete(() => { backButton.gameObject.SetActive(false); });
        totalAnimationDuration += baseAnimationDuration;

        for (int i = 0; i < buttons.Count; i++)
        {
            sequence.InsertCallback(totalAnimationDuration, () => { buttons[i].gameObject.SetActive(true); });
            sequence.Insert(totalAnimationDuration,
                buttons[i].DOScale(Vector3.one, baseAnimationDuration).From(Vector3.zero).SetEase(Ease.OutBack));
        }
        totalAnimationDuration += baseAnimationDuration;
        sequence.InsertCallback(totalAnimationDuration, () =>
        {
            objectController.SetAnimator(false);
            objectController.SetPosition();
        });
    }

    void SelectMenu()
    {
        objectController.canRot = false;
        switch (menu)
        {
            case 0:
                colorChanger.gameObject.SetActive(true);
                objectController.SetPosition();
                colorChanger.Initialize();
                break;
            case 1:
                objectController.SetAnimator(true);
                objectController.animator.Play("Pipet_Open");
                break;
            case 2:
                objectController.SetPosition();
                objectController.RotateObject(true);
                //objectController.animator.Play("Auto_Rotate");
                break;
            case 3:
                Application.Quit();
                break;
        }
    }
    void CloseMenu()
    {
        objectController.canRot = true;
        switch (menu)
        {
            case 0:
                colorChanger.gameObject.SetActive(false);
                break;
            case 1:
                objectController.animator.Play("Pipet_Closed");
                break;
            case 2:
                objectController.RotateObject(false);
                break;
        }
    }
}
