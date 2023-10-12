using UnityEngine;

public class Book : MonoBehaviour, IInteractable
{
    public static Book Instance;

    [SerializeField] Transform bookLookTransform;
    Bookmark activeBookmark;

    [SerializeField] Transform[] menus;

    [SerializeField] Transform hatButtonHolder;

    [SerializeField] Transform[] generalBookmarkPages;
    [SerializeField] Transform[] hatBookmarkPages;

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

        if (activeBookmark.index == 0)
        {
            foreach (Transform t in generalBookmarkPages) t.gameObject.SetActive(true);
            foreach (Transform t in hatBookmarkPages) t.gameObject.SetActive(false);
        }
        else
        {
            foreach (Transform t in generalBookmarkPages) t.gameObject.SetActive(false);
            foreach (Transform t in hatBookmarkPages) t.gameObject.SetActive(true);
        }
    }

    public void OpenMenu(int index)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].gameObject.SetActive(i == index);
        }
    }

    public void Interact()
    {
        Debug.Log("Interacted with book");

        StartUsing();
    }

    public void StartUsing()
    {
        //GameManager.Pause();
        GameManager.EnterBook();
        PlayerData.usingBook = true;

        Cursor.lockState = CursorLockMode.None;
    }

    public void StopUsing()
    {
        GameManager.ExitBook();
        PlayerData.usingBook = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockHat(int id)
    {
        Debug.Log($"Unlocked hat {id}");
        
        if (hatButtonHolder == null)
        {
            Debug.Log("Hat button holder / book stuff hasn't been implemented yet!");
        }
        else hatButtonHolder.GetChild(id).gameObject.SetActive(true);
    }
}
