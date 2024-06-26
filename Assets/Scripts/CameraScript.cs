using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Скрипт управління камерою
public class CameraScript : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 10f; // Скорость зума
    [SerializeField] private float minDistance = 2f; // Минимальное расстояние до объекта
    [SerializeField] private float maxDistance = 15f; // Максимальное расстояние до объекта


    private GameObject ball; //посилання на об'єкт на сцені (персонаж)
    private Vector3 offset; //зміщення камери відносно персонажу
    private Vector3 mAngles; //кути, накопичені рухом миші
    private float sensitivityH = 2f;
    private float sensitivityV = 1f;
    void Start()
    {
        ball = GameObject.Find("Ball");
        offset = this.transform.position - ball.transform.position;
        mAngles = this.transform.eulerAngles;
    }

    void Update() 
    {
        mAngles.y += Input.GetAxis("Mouse X") * sensitivityH;
        mAngles.x -= Input.GetAxis("Mouse Y") * sensitivityV;

        if (mAngles.x > 75f) mAngles.x = 75f;
        if (mAngles.x < 35f) mAngles.x = 35f;

        if (mAngles.y > 360) mAngles.y -= 360;
        if (mAngles.y < 0) mAngles.y += 360;

        // Обработка зума
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float distance = offset.magnitude;
            distance -= scrollInput * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            offset = offset.normalized * distance;
        }
    }

    void LateUpdate() //Вплив на камеру краще робити у LateUpdat
    {
        //слідування - камера рухається разом з персонажем
        //!! ігноруючи його обертання
        this.transform.position = ball.transform.position + Quaternion.Euler(0, mAngles.y, 0) * offset;
        this.transform.eulerAngles = mAngles;
    }
}
