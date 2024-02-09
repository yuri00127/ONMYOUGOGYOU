using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���͂̃^�C�v�ɂ���ăK�C�h�摜��؂�ւ���(�ۗ�)
public class InputTypeGuideChanger : MonoBehaviour
{
    public bool IsKeyMouse { get; private set; } = false;
    public bool IsController { get; private set; } = false;

    // Update is called once per frame
    void Update()
    {
        // �L�[�{�[�h/�}�E�X
        if (Input.anyKeyDown) { IsKeyMouse = true; }

        // xbox�R���g���[���[
        /*
        if (Input.IsJoystickPreconfigured("X-Axis")) { IsController = true; }
        if (Input.IsJoystickPreconfigured("Y-Axis")) { IsController = true; }
        if (Input.IsJoystickPreconfigured("joystick button 0")) { IsController = true; }
        if (Input.IsJoystickPreconfigured("joystick button 1")) { IsController = true; }
        */
    }
}
