using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不可被删除的线
/// </summary>
public class StaticLine :Line {
    
    public override void ChangeState(LineState state)
    {
        if (state == LineState.isChoose)
            state = LineState.show;
        base.linestate = state;
        switch (state)
        {
            case LineState.ready: gameObject.GetComponent<SpriteRenderer>().color = Color.white; break;
            case LineState.show: gameObject.GetComponent<SpriteRenderer>().color = Color.green; gameObject.tag = "Line"; break;
        }
    }

}
