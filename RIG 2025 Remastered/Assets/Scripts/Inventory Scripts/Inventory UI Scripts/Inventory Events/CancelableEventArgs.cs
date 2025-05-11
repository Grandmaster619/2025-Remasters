using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelableEventArgs : EventArgs
{
    bool isCanceled;

    public CancelableEventArgs()
    {
        isCanceled = false;
    }

    public bool IsCanceled() { return isCanceled; }

    public void SetCanceled(bool isCanceled) { this.isCanceled = isCanceled; }
}
