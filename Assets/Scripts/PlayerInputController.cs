using System;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance;

    public bool isInventoryOpen = false;

    public int currentWeaponIndex = 0;
    // Callback event for when the player changes weapons
    public event Action<int> OnWeaponChange;
    
    public GameObject[] weapons;
    public Transform weaponHolder;

    public bool canMove = true;
    public bool canRotate = true;
    public bool canInput = true;

    private Vector2 aimDirection;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Weapon
    public void LoadWeapon()
    {
        foreach (GameObject weapon in weapons)
        {
            weapon.GetComponent<WeaponBase>().LoadEquippedParts();
        }
    }
    #endregion

    #region Interact

    #endregion

    private void Update()
    {
        if (!canInput) return;
        if (Time.timeScale == 1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentWeaponIndex = 0;
                SwitchWeapon(currentWeaponIndex);
                OnWeaponChange?.Invoke(currentWeaponIndex);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentWeaponIndex = 1;
                SwitchWeapon(currentWeaponIndex);
                OnWeaponChange?.Invoke(currentWeaponIndex);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentWeaponIndex = 2;
                SwitchWeapon(currentWeaponIndex);
                OnWeaponChange?.Invoke(currentWeaponIndex);
            }
            if(Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                currentWeaponIndex = (currentWeaponIndex + 1) % 3;
                SwitchWeapon(currentWeaponIndex);
                OnWeaponChange?.Invoke(currentWeaponIndex);
            }
            else if(Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                currentWeaponIndex = (currentWeaponIndex - 1 + 3) % 3;
                SwitchWeapon(currentWeaponIndex);
                OnWeaponChange?.Invoke(currentWeaponIndex);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GetCurrentHoldingWeapon().GetComponent<WeaponBase>().OnAttackKeyDown();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                GetCurrentHoldingWeapon().GetComponent<WeaponBase>().OnAttackKeyUp(aimDirection);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.instance.ShowPauseMenu();
            }

            if (canRotate)
            {
                float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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

        // Check for inventory toggle input
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryManager.Instance.ToggleInventory();
            isInventoryOpen = !isInventoryOpen;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInventoryOpen)
            {
                InventoryManager.Instance.ToggleInventory();
                isInventoryOpen = false;
            }
        }

        // Aim direction
        aimDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    private void SwitchWeapon(int weaponIndex)
    {
        ResetMovement();
        if (weaponIndex >= weapons.Length)
        {
            Debug.LogWarning("Weapon index out of range");
            return;
        }

        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // Enable the new weapon
        weapons[weaponIndex].SetActive(true);
        UIController.Instance.ChangeWeaponSlot(weaponIndex);
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

    public void ResetMovement()
    {
        canMove = true;
        canRotate = true;
    }
}
