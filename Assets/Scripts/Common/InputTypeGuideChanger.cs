using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 入力のタイプによってガイド画像を切り替える(保留)
public class InputTypeGuideChanger : MonoBehaviour
{
    public bool IsKeyMouse { get; private set; } = false;
    public bool IsController { get; private set; } = false;

    // Update is called once per frame
    void Update()
    {
        // キーボード/マウス
        if (Input.anyKeyDown) { IsKeyMouse = true; }

        // xboxコントローラー
        /*
        if (Input.IsJoystickPreconfigured("X-Axis")) { IsController = true; }
        if (Input.IsJoystickPreconfigured("Y-Axis")) { IsController = true; }
        if (Input.IsJoystickPreconfigured("joystick button 0")) { IsController = true; }
        if (Input.IsJoystickPreconfigured("joystick button 1")) { IsController = true; }
        */
    }
}
