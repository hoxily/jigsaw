using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour, ILogHandler
{
    private GameObject logTemplate;
    private List<GameObject> otherLogs = new List<GameObject>();
    private Transform logPanelTransform;
    private Button logToggle;
    private Button logClear;
    private ILogHandler originalLogHandler;
    void Awake()
    {
        logTemplate = transform.Find("Panel/Scroll View/Viewport/Content/TextLogTemplate").gameObject;
        logPanelTransform = transform.Find("Panel");
        logToggle = transform.Find("ButtonToggle").GetComponent<Button>();
        logClear = transform.Find("ButtonClear").GetComponent<Button>();
        // bind events
        logToggle.onClick.AddListener(() =>
        {
            if (logPanelTransform.localPosition.x >= 0)
            {
                logPanelTransform.localPosition = new Vector3(-1280f, 0, 0);
            }
            else
            {
                logPanelTransform.localPosition = Vector3.zero;
            }
        });
        logClear.onClick.AddListener(() =>
        {
            foreach (var go in otherLogs)
            {
                Destroy(go);
            }
            otherLogs.Clear();
        });
        // hook debug.log
        originalLogHandler = Debug.logger.logHandler;
        Debug.logger.logHandler = this;
    }

    void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        originalLogHandler.LogFormat(logType, context, format, args);
        if (Application.isPlaying)
        {
            GameObject newLog = Instantiate<GameObject>(logTemplate, logTemplate.transform.parent);
            newLog.GetComponent<Text>().text = string.Format(format, args);
            otherLogs.Add(newLog);
        }
    }

    void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
    {
        originalLogHandler.LogException(exception, context);
        if (Application.isPlaying)
        {
            GameObject newLog = Instantiate<GameObject>(logTemplate, logTemplate.transform.parent);
            newLog.GetComponent<Text>().text = exception.ToString();
            otherLogs.Add(newLog);
        }
    }
}
