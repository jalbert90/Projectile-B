using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public KeyCode dashKey = KeyCode.Space;
    public float dashCooldown = 1f;
    public float dashDuration = .2f;

    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public Vector3 projectileOffset;

    private bool isDashing = false;
    private Vector3 dashDirection;
    private float dashTimer = 0f;
    private float dashDurationTimer = 0f;

    void Update()
    {
        //Get player moving Inputs
        float horizontalInput = Input.GetKey("a") ? -1 : Input.GetKey("d") ? 1 : 0;
        float verticalInput = Input.GetKey("w") ? 1 : Input.GetKey("s") ? -1 : 0;
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0);

        // Handle dashing
        if (isDashing)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            dashDurationTimer -= Time.deltaTime;

            if (dashDurationTimer <= 0f)
            {
                //Stop dashing after some time
                isDashing = false;
            }
        }
        else
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            //Dash input handling
            if (Input.GetKeyDown(dashKey) && dashTimer <= 0f)
            {
                isDashing = true;
                dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                dashDirection.z = 0f;
                dashDirection.Normalize();
                dashTimer = dashCooldown;
                dashDurationTimer = dashDuration;
            }
            dashTimer -= Time.deltaTime;
        }

        //Shooting inputs
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3 projectileDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        projectileDirection.z = 0;
        projectileDirection.Normalize();
        Vector3 spawnPosition = transform.position + projectileOffset;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = projectileDirection * projectileSpeed;
    }
}