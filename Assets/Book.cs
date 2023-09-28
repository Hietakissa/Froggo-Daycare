using UnityEngine;

public class Book : MonoBehaviour, IInteractable
{
    public static Book Instance;

    [SerializeField] Transform bookLookTransform;
    Bookmark activeBookmark;

    void Awake()
    {
        Instance = this;

        PlayerData.bookLookTransform = bookLookTransform;
    }

    public void SelectBookmark(Bookmark bookmark)
    {
        if (activeBookmark != null) activeBookmark.UnSelect();
        activeBookmark = bookmark;
        Debug.Log($"Selected bookmark {activeBookmark.index}");
    }

    public void Interact()
    {
        Debug.Log("Interacted with book");

        StartUsing();
    }

    public void StartUsing()
    {
        //GameManager.Pause();
        PlayerData.usingBook = true;

        Cursor.lockState = CursorLockMode.None;
    }

    public void StopUsing()
    {
        PlayerData.usingBook = false;

        Cursor.lockState = CursorLockMode.Locked;
    }
}
