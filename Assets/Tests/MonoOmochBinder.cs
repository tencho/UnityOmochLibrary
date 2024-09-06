using System.Collections;
using System.Collections.Generic;
using Omoch.Framework;
using UnityEngine;

public class MonoOmochBinder : MonoBehaviour
{
    [SerializeField] private OmochBinder binder;

    private void Start()
    {
        binder.BindLogic<ITestPeek, ITestViewOrder>(new TestLogic(), LinkID.From("TestLinkID"));
        binder.BindView<ITestPeek, ITestViewOrder>(new TestView(), LinkID.From(2345));
    }

    private class TestLogic : LogicBase<ITestViewOrder>, ITestPeek { }
    private interface ITestPeek { }
    private class TestView : ViewBase<ITestPeek>, ITestViewOrder { }
    private interface ITestViewOrder { }
}
