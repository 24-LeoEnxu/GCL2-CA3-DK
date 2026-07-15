using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;

    int choice = 1;

void Update()
    {
        switch (choice)
        {
            case 1:
                print("1 Player Game A");
                break;
            case 2:
                print("1 Player Game B");
                break;
            case 3:
                print("2 Player Game A");
                break;
            case 4:
                print("2 Player Game B");
                break;

            default:
                print("Error");
                break;
        }

        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 newPosition = transform.position;

        if (Input.GetKeyDown(KeyCode.S))
        {
            newPosition.y += -28.25f;
            transform.position = newPosition;

            choice += 1;
        }
    }
}
