using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;
    public float showDamageDuration = 0.1f;
    public float powerUpDropChance = 1f;

    [Header("Set Dynamically: Enemy")]

    public Color[] originalColors;

    public Material[] materials;// All the Materials of this & its children

    public bool showingDamage = false;

    public float damageDoneTime; // Time to stop showing damage

    public bool notifiedOfDestruction = false; // Will be used later

    protected BoundsCheck bndCheck;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        materials = Utils.GetAllMaterials(gameObject);                     // b

        originalColors = new Color[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {

            originalColors[i] = materials[i].color;

        }

    }

    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }

        set
        {
            this.transform.position = value;
        }
    }

    void Update(){
        Move();

        if (showingDamage && Time.time > damageDoneTime)
        {                 // c

            UnShowDamage();

        }

        if (bndCheck != null && bndCheck.offDown)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;

        switch (otherGO.tag)
        {

            case "ProjectileHero":                                           // b

                ProjectileHero p = otherGO.GetComponent<ProjectileHero>();


                // If this Enemy is off screen, don't damage it.

                if (!bndCheck.isOnScreen)
                {                                // c

                    Destroy(otherGO);

                    break;

                }



                // Hurt this Enemy

                ShowDamage();

                // Get the damage amount from the Main WEAP_DICT.

                health -= Main.GetWeaponDefinition(p.type).damageOnHit;

                if (health <= 0)
                {                                           // d
                    if (!notifiedOfDestruction)
                    {


                        Main.S.shipDestroyed(this);


                    }


                    notifiedOfDestruction = true;

                    // Destroy this Enemy

                    Destroy(this.gameObject);

                }

                Destroy(otherGO);                                          // e

                break;



            default:

                print("Enemy hit by non-ProjectileHero: " + otherGO.name); // f

                break;
        }

    }

    void ShowDamage()
    {                                                      // e

        foreach (Material m in materials)
        {

            m.color = Color.red;

        }

        showingDamage = true;

        damageDoneTime = Time.time + showDamageDuration;

    }



    void UnShowDamage()
    {                                                    // f

        for (int i = 0; i < materials.Length; i++)
        {

            materials[i].color = originalColors[i];

        }

        showingDamage = false;

    }
}
