using System;
using System.Collections;
using NUnit.Framework;
using Omoch.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class OmochBinderTest
{
    [UnityTest]
    public IEnumerator BindSyncTest()
    {
        var go = new GameObject();
        OmochBinder binder = go.AddComponent<OmochBinder>();
        binder.IsStrict = true;
        binder.BindLogicAndView(new TestLogic(123), new TestView(123));

        Assert.AreEqual(0, binder.ReadyLogics.Count);
        Assert.AreEqual(0, binder.ReadyViews.Count);
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator NormalDisposeTest()
    {
        var go = new GameObject();
        OmochBinder binder = go.AddComponent<OmochBinder>();
        binder.IsStrict = true;
        var logic = new TestLogic(123);
        var view = new TestView(123);
        binder.BindLogicAndView(logic, view);

        Assert.AreEqual(0, binder.ReadyLogics.Count);
        Assert.AreEqual(0, binder.ReadyViews.Count);

        logic.Dispose();

        Assert.AreEqual(true, logic.IsDisposed);
        Assert.AreEqual(true, view.IsDisposed);

        yield return null;
    }

    [UnityTest]
    public IEnumerator EarlyDisposeTest()
    {
        var go = new GameObject();
        OmochBinder binder = go.AddComponent<OmochBinder>();
        binder.IsStrict = true;
        {
            var logic = new TestLogic(123);
            var view = new TestView(123);
            logic.Dispose();
            binder.BindLogicAndView(logic, view);
            Assert.AreEqual(true, logic.IsDisposed);
            Assert.AreEqual(true, view.IsDisposed);
        }
        {
            var logic = new TestLogic(456);
            var view = new TestView(456);
            view.Dispose();
            binder.BindLogicAndView(logic, view);
            Assert.AreEqual(true, logic.IsDisposed);
            Assert.AreEqual(true, view.IsDisposed);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator BindAsyncTest()
    {
        var go = new GameObject();
        OmochBinder binder = go.AddComponent<OmochBinder>();
        binder.IsStrict = false;

        var linkID1 = LinkID.From("TestLinkID1");
        var linkID2 = LinkID.From("TestLinkID2");
        binder.BindLogic<ITestPeek, ITestViewOrder>(new TestLogic(123), linkID1);
        yield return null;
        binder.BindLogic<ITestPeek, ITestViewOrder>(new TestLogic(456), linkID2);
        yield return null;
        binder.BindView<ITestPeek, ITestViewOrder>(new TestView(123), linkID1);
        yield return null;
        binder.BindView<ITestPeek, ITestViewOrder>(new TestView(456), linkID2);
        yield return null;

        Assert.AreEqual(0, binder.ReadyLogics.Count);
        Assert.AreEqual(0, binder.ReadyViews.Count);
    }

    [UnityTest]
    public IEnumerator BindDuplicatelyTest()
    {
        var go = new GameObject();
        OmochBinder binder = go.AddComponent<OmochBinder>();
        binder.IsStrict = false;
        var linkID1 = LinkID.From(1);
        var linkID2 = LinkID.From(2);
        try
        {
            binder.BindLogic<ITestPeek, ITestViewOrder>(new TestLogic(123), linkID1);
            binder.BindLogic<ITestPeek, ITestViewOrder>(new TestLogic(123), linkID1);
            throw new Exception("同じキーでBindLogicしてもエラーが発生していません");
        }
        catch (Exception)
        {
        }

        try
        {
            binder.BindView<ITestPeek, ITestViewOrder>(new TestView(123), linkID2);
            binder.BindView<ITestPeek, ITestViewOrder>(new TestView(123), linkID2);
            throw new Exception("同じキーでBindViewしてもエラーが発生していません");
        }
        catch (Exception)
        {
        }

        yield return null;
    }

    internal class TestLogic
        : LogicBase<ITestViewOrder>
        , ITestPeek
    {
        public int Seed { get; set; }
        public TestLogic(int seed)
        {
            Seed = seed;
        }

        public override void Initialized()
        {
            Seed *= 2;
        }

        public override void ViewInitialized()
        {
            ViewOrder.AssertSeed();
        }
    }

    internal interface ITestPeek
    {
        int Seed { get; }
    }

    internal class TestView
        : ViewBase<ITestPeek>
        , ITestViewOrder
    {
        public int Seed { get; set; }
        public TestView(int seed)
        {
            Seed = seed;
        }

        public override void Initialized()
        {
            Seed *= 2;
        }

        public void AssertSeed()
        {
            Assert.AreEqual(Peek.Seed, Seed);
        }
    }

    internal interface ITestViewOrder
    {
        void AssertSeed();
    }
}
