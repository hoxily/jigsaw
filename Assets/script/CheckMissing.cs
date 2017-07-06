using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckMissing : MonoBehaviour
{
    public float waitSeconds;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(waitSeconds);
        GameObject gameController = GameObject.Find("/GameController");
        Debug.LogError(gameController);
        if (gameController)
        {
#if UNITY_EDITOR
            System.Type type = System.Type.GetType("LevelControl", false);
#else
            // 由于 LevelControl 不在 mscorlib 内，也不在当前正执行的 assembly 内，所以会找不到。
            System.Type type = Init.loadedAssembly.GetType("LevelControl", false);
            //foreach (var t in Init.loadedAssembly.GetTypes())
            //{
            //    Debug.Log(t.FullName);
            //    AppendLog(t.FullName);
            //}
#endif
            if (type != null)
            {
                Component levelControl = gameController.GetComponent(type);
                if (levelControl)
                {
                    Debug.LogError(levelControl);
                }
                else
                {
                    Debug.LogError("Missing component in GameController GameObject!");
                }
            }
            else
            {
                Debug.LogError("Cannot find type 'LevelControl'!");
            }
        }
        else
        {
            Debug.LogError("Missing GameController GameObject!");
        }
    }
}
