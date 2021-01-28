using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telekinesis : MonoBehaviour
{
    public float pushForce = 10f;
    public float cursorLatchDist = 0.2f;

    Rigidbody2D lastSelectedRB;
    int currentMode;
    // Update is called once per frame
    void Update()
    {
        CheckInput();
        if (currentMode == 1|| currentMode == -1 || currentMode == 0)
        Teleken();
    }

    void CheckInput()
    {
        currentMode = -69;
        // Left + Right = Gravitate Towards mouse
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
            currentMode = 0;
        // Left = Push
        else if (Input.GetMouseButton(0))
            currentMode = 1;
        // Right = Pull
        else if (Input.GetMouseButton(1))
            currentMode = -1;
    }

    void Teleken()
    {
        // Does a raycast on whatever is under the mouse cursor
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
        // If the raycast hit a movable object, caches that
        if (hit && hit.transform.CompareTag("Movable"))
        {
            lastSelectedRB = hit.transform.GetComponent<Rigidbody2D>();
        }

        // If no object is cached
        if (!lastSelectedRB)
            return;

        // Does telekenisis based on what buttons were pressed
        if (Mathf.Abs(currentMode) == 1)
            PushPull();
        else if (currentMode == 0)
            GravitateTowardsCursor(mouseRay);
    }

    void PushPull()
    {
        // Calculates a vector from the player -> the object
        Vector2 playerToObject = lastSelectedRB.transform.position - transform.position;
        playerToObject.Normalize();

        // If the object is going in the opposite direction, stops it
        if (Vector3.Dot(playerToObject * currentMode, lastSelectedRB.velocity) < 0)
            lastSelectedRB.velocity = Vector2.zero;
        
        // Pushes or pulls the object based on the pressed button
        lastSelectedRB.AddForce(playerToObject * pushForce * Time.deltaTime * currentMode);
    }

    void GravitateTowardsCursor(Ray mouseRay)
    {
        // Calculates a vector going from the object -> the mouse cursor
        Vector2 cursorToObject = mouseRay.origin - lastSelectedRB.transform.position;
        // If the object is close enough, sticks it to the cursor
        if (cursorToObject.magnitude <= cursorLatchDist)
        {
            lastSelectedRB.transform.position = mouseRay.origin;
        }
        cursorToObject.Normalize();
        // Otherwise, moves the object towards the cursor
        lastSelectedRB.AddForce(cursorToObject * pushForce * Time.deltaTime);
    }
}
