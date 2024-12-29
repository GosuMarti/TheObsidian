using Assets.Scripts;
using TMPro;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public int damage;
    public float timeBetweenShooting;
    public float spread;
    public float range;
    public float reloadTime;
    public float timeBetweenShots;

    public int maxBullets = 6;
    public int bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft;
    int bulletsShot;

    private bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public Animator animator;

    public GameObject muzzleFlash, bulletHoleGraphic;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reloadPromptText;

    public AudioSource audioSource;
    public AudioClip gunshotSound;
    public AudioClip reloadSound;


    private void MyInput()
    {
        shooting = allowButtonHold ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < maxBullets && !reloading)
        {
            Reload();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            Shoot();
        }

        if (bulletsLeft < maxBullets && !reloading)
        {
            ShowReloadPrompt();
        }
    }

    private void Reload()
    {
        if (animator != null)
        {
            animator.SetTrigger("Reload");
        }

        reloading = true;
        PlayReloadSound();
        Invoke("ReloadFinished", reloadTime);
        HideReloadPrompt(); 
    }

    public void PlayReloadSound()
    {
        if (audioSource && reloadSound)
        {
            audioSource.PlayOneShot(reloadSound);
        }

    }

    private void ReloadFinished()
    {
        bulletsLeft = maxBullets;
        reloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Shoot()
    {
        readyToShoot = false;

        if (audioSource && gunshotSound)
        {
            audioSource.PlayOneShot(gunshotSound);
        }


        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        if (Physics.Raycast(fpsCam.transform.position,
                direction,
                out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            EnemyHealth enemyHealth = rayHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }

        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        bulletsLeft--;
        Invoke("ResetShot", timeBetweenShooting);
    }

    private void Awake()
    {
        bulletsLeft = maxBullets;
        readyToShoot = true;

        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
        }


        HideReloadPrompt();
    }


    void Update()
    {
        MyInput();
        //HandleRecoil();
        ammoText.SetText(bulletsLeft + " / " + maxBullets);
    }

    private void ShowReloadPrompt()
    {
        if (reloadPromptText != null)
        {
            reloadPromptText.gameObject.SetActive(true);
            reloadPromptText.SetText("Press R to Reload!");
        }
    }

    private void HideReloadPrompt()
    {
        if (reloadPromptText != null)
        {
            reloadPromptText.gameObject.SetActive(false);
        }
    }
}
