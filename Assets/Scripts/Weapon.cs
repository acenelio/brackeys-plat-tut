using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Weapon : MonoBehaviour {

    public float fireRate = 0; // fires per second
    public int Damage = 10;
    public LayerMask whatToHit;

    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;

    public float effectSpawnRate = 10;

    float timeToSpawnEffect = 0;
    float timeToFire = 0;
    Transform firePoint;
    Quaternion rotation;

	void Awake () {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null) {
            Debug.LogError("Firepoint not found!");
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (fireRate == 0) {
            if (Input.GetButtonDown("Fire1")) {
                Shoot();
            }
        }
        else {
            if (Input.GetButton("Fire1") && Time.time > timeToFire) {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
	}

    void Shoot() {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        Vector2 dif = (mousePosition - firePointPosition).normalized;

        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, dif, 100f, whatToHit);

        // como nao implementei o braco que gira, vou calcular a rotacao aqui
        rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg);

        //Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);
        if (hit.collider != null) {
            //Debug.DrawLine(firePointPosition, hit.point, Color.red);
            //Debug.Log("We hit " + hit.collider.name + " and did " + Damage + " damage");
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.DamageEnemy(Damage);
            }
        }

        if (Time.time >= timeToSpawnEffect) {
            Vector3 hitPos;

            if (hit.collider == null) {
                hitPos = dif * 100;
            }
            else {
                hitPos = hit.point;
            }

            Effect(hitPos);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPosition) {
        Transform trail = Instantiate(BulletTrailPrefab, firePoint.position, rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null) {
            // setPosition define os valores das posicoes da lista Positions de lineRenderer
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPosition);
        }

        Destroy(trail.gameObject, 0.04f);

        Transform clone;
        if (!GM.instance.player.GetComponent<PlatformerCharacter2D>().m_FacingRight) {
            Vector3 vet = rotation.eulerAngles;
            Quaternion rot = Quaternion.Euler(vet.x, vet.y, vet.z + 180);
            clone = Instantiate(MuzzleFlashPrefab, firePoint.position, rot) as Transform;
        }
        else {
            clone = Instantiate(MuzzleFlashPrefab, firePoint.position, rotation) as Transform;
        }
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.05f);
    }
}

