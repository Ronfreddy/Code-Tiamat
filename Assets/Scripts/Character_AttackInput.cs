using UnityEngine;

public class Character_AttackInput : MonoBehaviour
{
    public GameObject weaponHolder;
    private bool canRotate = true;
    public Rigidbody2D rb;
    public float pressTime = 0.0f;
    public int currentWeaponIndex = 0;

    public GameObject[] weapons;
    public Vector2 mousePos = Vector2.zero;

    public Vector2 aimDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (GetCurrentHoldingWeapon() != null && Time.timeScale == 1)
            {
                GetCurrentHoldingWeapon().GetComponent<WeaponBase>().OnAttackKeyDown();
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (GetCurrentHoldingWeapon() != null && Time.timeScale == 1)
            {
                GetCurrentHoldingWeapon().GetComponent<WeaponBase>().OnAttackKeyUp(aimDirection);
            }
        }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (canRotate)
        {
            aimDirection = mousePos - (Vector2)transform.position;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            rb.rotation = angle;

            if (angle > 90 || angle < -90)
            {
                weaponHolder.transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                weaponHolder.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void FixedUpdate()
    {
    }

    public GameObject GetCurrentHoldingWeapon()
    {
        GameObject weapon = null;
        if (weaponHolder.transform.childCount > 0)
        {
            if (currentWeaponIndex >= weaponHolder.transform.childCount)
            {
                Debug.LogWarning("Weapon index out of range");
                return weapon;
            }
            weapon = weaponHolder.transform.GetChild(currentWeaponIndex).gameObject;
        }
        return weapon;
    }

    public void SwitchWeapon(int index)
    {
        if (index >= weapons.Length)
        {
            Debug.LogWarning("Weapon index out of range");
            return;
        }

        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // Enable the new weapon
        weapons[index].SetActive(true);
    }

    public void EnableRotation()
    {
        canRotate = true;
    }

    public void DisableRotation()
    {
        canRotate = false;
    }
}
