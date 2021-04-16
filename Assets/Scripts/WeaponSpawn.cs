using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    [SerializeField] private float upLimitX;
    [SerializeField] private float bottomLimitX;

    [SerializeField] private float upLimitZ;
    [SerializeField] private float bottomLimitZ;

    [SerializeField] GameObject[] Weapons = new GameObject[3];

    [SerializeField] Animator playerAnimator;

    [SerializeField] Transform weaponHolder;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            float xPosition = Random.Range(upLimitX, bottomLimitX);
            float zPosition = Random.Range(upLimitZ, bottomLimitZ);
            Vector3 spawnPosition = new Vector3(xPosition, 0, zPosition);

            GameObject randomWeapon = Weapons[Random.Range(0, Weapons.Length)];
            Animator weaponAnimator = randomWeapon.GetComponent<Animator>();
            weaponAnimator = playerAnimator;

            GameObject weapon =  Instantiate(randomWeapon, spawnPosition, Quaternion.identity);
            weapon.transform.parent = weaponHolder;
        }
    }
}
