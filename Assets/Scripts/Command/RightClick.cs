using UnityEngine;
using System.Collections.Generic;
public class RightClick : MonoBehaviour
{
    public static RightClick instance;
    private Camera cam;
    public LayerMask layerMask;
    [SerializeField]
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created


// Start is called once before the first execution of Update after the M
    void Start()
    {
        instance = this;
        cam = Camera.main;
        layerMask = LayerMask.GetMask("Ground", "Character", "Building");
    }
    private void CommandToWalk(RaycastHit hit, List<Character> heroes)
    {
        foreach (Character h in heroes)
        {
            if (h != null)
            {
                h.WalkToPosition(hit.point);
            }
        }

        CreateVFX(hit.point, VFXManager.instance.DoubleRingMarker);
    }
    // Update is called once per frame
    void Update()
    {
        // mouse up
        if (Input.GetMouseButtonUp(1))
        {
            TryCommand(Input.mousePosition);
        }
     
    }
    private void TryCommand(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        //if we left-click something
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            switch (hit.collider.tag)
            {
                case "Ground":
                    CommandToWalk(hit, PartyManager.instance.SelectChars);
                    break;
                case  "Enemy":
                    CommandToAttack(hit, PartyManager.instance.SelectChars);
                    break;
            }
        }
    }
    private void CreateVFX(Vector3 pos, GameObject vfxPrefab)
    {
        if (vfxPrefab == null)
            return;

        Instantiate(vfxPrefab,
            pos + new Vector3(0f, 0.1f, 0f), Quaternion.identity);
    }
    private void CommandToAttack(RaycastHit hit, List<Character> heroes)
    {
        Character target = hit.collider.GetComponent<Character>();
        Debug.Log("Attack: " + target);

        foreach (Character h in heroes)
        {
            h.ToAttackCharacter(target);
        }
    }
}
