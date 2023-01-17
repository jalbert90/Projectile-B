using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashCooldown = 1f;
    public float dashDuration = .2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public Vector3 projectileOffset;

    private bool isDashing = false;
    private Vector3 dashDirection;
    private float dashCooldownTimer = 0f;
    private float dashDurationTimer = 0f;
    private KeyCode dashKey = KeyCode.Space;

    void Update()
    {
        // Get player movement inputs.
        float horizontalInput = Input.GetKey("a") ? -1 : Input.GetKey("d") ? 1 : 0;
        float verticalInput = Input.GetKey("w") ? 1 : Input.GetKey("s") ? -1 : 0;
        // Unit vector pointing in the direction of movement.
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0).normalized;

        // Handle dashing.
        if (isDashing)
        {
            // Move in dashDirection with magnitude dashSpeed when dashing.
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            // Decrement dashDurationTimer by the time interval between frames.
            dashDurationTimer -= Time.deltaTime;

            // Stop dashing when dashDurationTimer gets to 0.
            if (dashDurationTimer <= 0f)
            {
                isDashing = false;
            }
        }
        else
        {
            // While not dashing, move in moveDirection with magnitude moveSpeed.
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // Dash input handling.
            if (Input.GetKeyDown(dashKey) && dashCooldownTimer <= 0f)
            {
                isDashing = true;
                // Dash direction is the vector from the player to the mouse pointer.
                dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                // Set the z-component to 0 so player doesn't dash off of the visible screen.
                dashDirection.z = 0f;
                // Normalize dashDirection vector so that the velocity of dashing is as expected.
                dashDirection.Normalize();
                // Set the cooldown timer.
                dashCooldownTimer = dashCooldown;
                // Set the dash duration timer.
                dashDurationTimer = dashDuration;
            }
            // Decrement the cooldown timer by time interval between frames.
            dashCooldownTimer -= Time.deltaTime;
        }

        // Shooting inputs
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // The direction of the projectile's velocity is from the player to the mouse.
        Vector3 projectileDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        // Set the z-component to 0 so that the projectile stays in the plane.
        projectileDirection.z = 0;
        // Make the projectile's direction vector a unit vector so that the projectile's velocity is correct.
        projectileDirection.Normalize();
        // Offset the projectile from the player, if desired.
        Vector3 spawnPosition = transform.position + projectileOffset;
        // Create a clone of the projectilePrefab at the spawn position.
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        // Access the projectile object's rigidbody2D physics.
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        // Assign the velocity to the projectile object.
        rb.velocity = projectileDirection * projectileSpeed;
        // Destroy the projectile after 3 seconds.
        Destroy(projectile, 3f);
    }
}