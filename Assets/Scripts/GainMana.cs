using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainMana : MonoBehaviour
{
    public float mana = 0;

    // Mana charging speed (per second)
    [SerializeField] public float manaGainSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            mana += manaGainSpeed * Time.unscaledDeltaTime;
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Mana: " + mana);
            mana = 0;
        }
    }
}
