using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.GraphicsBuffer;

public class PlayerScript : MonoBehaviour
{
    public float MoveX, MoveY, Speed;
    private Rigidbody2D RB;
    public bool isCollider;
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MoveX = Input.GetAxis("Vertical");
        RB.linearVelocity = new Vector2(MoveX * Speed, RB.linearVelocity.y);
        MoveY = Input.GetAxis("Horizontal");
        RB.linearVelocity = new Vector2(MoveY * Speed, RB.linearVelocity.x);
    }

    private void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.CompareTag("Item"))
        {
            isCollider = true;
        }
    }

    private void OnTriggerExit2D(Collider2D Target)
    {
        if (Target.gameObject.CompareTag("Item"))
        {
            isCollider = false;
        }
    }
}
