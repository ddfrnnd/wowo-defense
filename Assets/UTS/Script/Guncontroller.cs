using System.Collections;
using UnityEngine;
using TMPro;

public class GunController : MonoBehaviour
{
    [Header("Referensi Senjata")]
    public Transform firePoint;          // Empty object di ujung laras senjata
    public GameObject bulletPrefab;      // Prefab peluru
    public GameObject muzzleFlash;       // Effect flash (optional, bisa null)

    [Header("Stats Senjata")]
    public float bulletSpeed = 30f;      // Kecepatan peluru
    public float bulletLifetime = 3f;    // Berapa detik peluru hidup sebelum destroy
    public float fireRate = 0.2f;        // Jeda antar tembakan (detik)
    public int magazineSize = 30;        // Kapasitas 1 magazin
    public float reloadTime = 2f;        // Durasi reload (detik)

    [Header("UI (optional)")]
    public TMP_Text ammoText; // Text UI ammo, bisa dikosongkan dulu

    // ── Private State ──────────────────────────────────────────────
    private int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    // ───────────────────────────────────────────────────────────────
    void Start()
    {
        currentAmmo = magazineSize;
        UpdateAmmoUI();

        // Sembunyikan muzzle flash saat start
        if (muzzleFlash != null)
            muzzleFlash.SetActive(false);
    }

    // ───────────────────────────────────────────────────────────────
    void Update()
    {
        // Jangan proses input saat reload
        if (isReloading) return;

        // Reload manual dengan tombol R
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < magazineSize)
        {
            StartCoroutine(Reload());
            return;
        }

        // Tembak saat klik kiri
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                // Ammo habis — auto reload
                StartCoroutine(Reload());
            }
        }
    }

    // ───────────────────────────────────────────────────────────────
    void Shoot()
{
    nextFireTime = Time.time + fireRate;
    currentAmmo--;
    UpdateAmmoUI();

    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

    Rigidbody rb = bullet.GetComponent<Rigidbody>();
    if (rb != null)
    {
        // Ganti firePoint.forward dengan Camera.main.transform.forward
        rb.linearVelocity = Camera.main.transform.forward * bulletSpeed;
    }

    Destroy(bullet, bulletLifetime);

    if (muzzleFlash != null)
        StartCoroutine(ShowMuzzleFlash());
}

    // ───────────────────────────────────────────────────────────────
    IEnumerator Reload()
{
    isReloading = true;

    if (ammoText != null)
    {
        ammoText.text = "Reloading...";
        ammoText.color = new Color(1f, 0.5f, 0f); // orange
    }

    yield return new WaitForSeconds(reloadTime);

    currentAmmo = magazineSize;
    isReloading = false;

    UpdateAmmoUI();
}

    // ───────────────────────────────────────────────────────────────
    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    // ───────────────────────────────────────────────────────────────
    void UpdateAmmoUI()
{
    if (ammoText != null)
    {
        ammoText.text = currentAmmo + " / " + magazineSize;

        // Warna ammo
        if (currentAmmo <= 5)
        {
            ammoText.color = Color.red;
        }
        else if (currentAmmo <= 10)
        {
            ammoText.color = Color.yellow;
        }
        else
        {
            ammoText.color = Color.black;
        }
    }
}

    // ───────────────────────────────────────────────────────────────
    // Public getter — dipakai script lain kalau perlu cek ammo
    public int GetCurrentAmmo() => currentAmmo;
    public bool IsReloading() => isReloading;
}