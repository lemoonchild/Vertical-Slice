using UnityEngine;
using DG.Tweening;

public class PuzzleManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject bookshelf;        
    public int itemsRequired = 2;       

    [Header("Configuración")]
    public float dropDistance = 5f;

    private int itemsCollected = 0;

    public void ItemCollected()
    {
        itemsCollected++;
        Debug.Log($"Objetos recogidos: {itemsCollected}/{itemsRequired}");

        if (itemsCollected >= itemsRequired)
            OpenPath();
    }

    private void OpenPath()
    {
        Vector3 targetPos = bookshelf.transform.position;
        targetPos.y -= dropDistance;

        bookshelf.transform.DOMove(targetPos, 0.8f)
            .SetEase(Ease.InCubic)
            .OnComplete(() => {
                bookshelf.SetActive(false);
            });
    }
}