using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private void Update()
    {
        Vector3 destination = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        destination.Normalize();
        transform.Translate(destination * speed * Time.deltaTime);
    }
}
