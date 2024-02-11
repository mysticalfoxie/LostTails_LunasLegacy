using UnityEngine;

public class ObjectSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _target;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _target.SetActive(true);
    }
}