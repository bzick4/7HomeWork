using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour
{
    public void SceneReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}