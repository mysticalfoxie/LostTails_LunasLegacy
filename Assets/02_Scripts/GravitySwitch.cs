using System.Runtime.Serialization;
using UnityEngine;

public class GravitySwitch : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] private float _targetGravityScale;
    [SerializeField] private bool _allowJustOnce;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = _targetGravityScale;
        
        if (_allowJustOnce)
            gameObject.SetActive(false);
    }
}