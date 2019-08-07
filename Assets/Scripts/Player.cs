using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LogicOperator logicOperator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            logicOperator.InputValue1 = !logicOperator.InputValue1;
            print($"{DateTime.Now} {logicOperator.InputValue1}, {logicOperator.InputValue2} = {logicOperator.GetOutput()}");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            logicOperator.InputValue2 = !logicOperator.InputValue2;
            print($"{DateTime.Now} {logicOperator.InputValue1}, {logicOperator.InputValue2} = {logicOperator.GetOutput()}");
        }
    }
}
