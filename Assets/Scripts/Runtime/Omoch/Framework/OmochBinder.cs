using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable

namespace Omoch.Framework
{
    /// <summary>
    /// Logic(計算処理)とView(描画処理)にわけて開発する時に使う。
    /// LogicBaseとViewBaseを継承したクラスを同一のキーでBindする事で自動でLogicとViewが関連付けられる。
    /// MonoBehaviourを使いたい時はMonoLogicBase, MonoViewBaseを使う。
    /// </summary>
    /// <remarks>
    /// +-------+ ------Peek-------+
    /// | Logic |---ViewOrder---+  | 
    /// +-------+               |  |
    ///     ^                   V  V
    ///     |                +--------+
    ///     +----Input-------|  View  |
    ///                      +--------+
    /// Peek: ViewからLogicへアクセス可能なものをまとめたインターフェイス。Logicデータの取得用。
    /// ViewOrder: LogicからViewへアクセス可能なものをまとめたインターフェイス。Viewへの通知用。
    /// Input: ViewからLogicへの通知用。
    /// </remarks>
    public class OmochBinder : MonoBehaviour
    {
        private readonly List<IUpdatableLogic> updatableLogics = new();
        private readonly List<IUpdatableView> updatableViews = new();
        private readonly List<IFixedUpdatableLogic> fixedUpdatableLogics = new();
        private readonly List<IFixedUpdatableView> fixedUpdatableViews = new();
        private readonly Dictionary<LinkID, ILogicCore> readyLogics = new();
        private readonly Dictionary<LinkID, IViewCore> readyViews = new();
        private readonly Dictionary<ILogicCore, Action> disposeLogicHandlers = new();
        private readonly Dictionary<IViewCore, Action> disposeViewHandlers = new();
        private readonly Queue<IUpdatableLogic> removeUpdatableLogicQueue = new();
        private readonly Queue<IUpdatableView> removeUpdatableViewQueue = new();
        private readonly Queue<IFixedUpdatableLogic> removeFixedUpdatableLogicQueue = new();
        private readonly Queue<IFixedUpdatableView> removeFixedUpdatableViewQueue = new();

        [field: SerializeField]
        [field: Tooltip("LogicとViewを同時追加しないとエラーが出るモード")]
        public bool IsStrict { get; set; } = false;

#if UNITY_EDITOR
        [ReadOnly]
        [SerializeField]
        [Tooltip("未処理Logic数")]
        private int readyLogicCount;

        [ReadOnly]
        [SerializeField]
        [Tooltip("未処理View数")]
        private int readyViewCount;

        [UnityEditor.CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
        public class ReadOnlyDrawer : UnityEditor.PropertyDrawer
        {
            public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
            {
                GUI.enabled = false; // インスペクターでの編集を無効化
                UnityEditor.EditorGUI.PropertyField(position, property, label);
                GUI.enabled = true; // 次のUI要素のために再度有効化
            }
        }

        private class ReadOnlyAttribute : PropertyAttribute
        {
            // 何もしない
        }
#endif

        /// <summary>
        /// まだバインド処理が完了していないLogic
        /// </summary>
        public Dictionary<LinkID, ILogicCore> ReadyLogics => readyLogics;

        /// <summary>
        /// まだバインド処理が完了していないView
        /// </summary>
        public Dictionary<LinkID, IViewCore> ReadyViews => readyViews;

        /// <summary>
        /// LinkIDでLogicWithInputを関連付け予約する。
        /// 既に同じLinkIDでViewWithInputが関連付け予約されていたら、LogicとViewが関連付けられる。
        /// </summary>
        public void BindLogicWithInput<TPeek, TViewOrder, TInputData>
            (ILogicBaseWithInput<TViewOrder, TInputData> logic, LinkID key)
            where TViewOrder : class
            where TPeek : class
        {
            if (logic == null)
            {
                throw new Exception($"BindしようとしているLogicがNullです");
            }

            if (readyViews.ContainsKey(key))
            {
                var view = readyViews[key];
                readyViews.Remove(key);

                if (view is IViewBaseWithInput<TPeek, TInputData> viewBase)
                {
                    BindLogicAndViewWithInput(logic, viewBase);
                }
                else
                {
                    throw new Exception($"Logic({logic})とView({view})のジェネリクス型が一致しません");
                }
            }
            else
            {
                readyLogics.Add(key, logic);
            }
        }

        /// <summary>
        /// LinkIDでViewWithInputを関連付け予約する。
        /// 既に同じLinkIDでLogicWithInputが関連付け予約されていたら、LogicとViewが関連付けられる。
        /// </summary>
        public void BindViewWithInput<TPeek, TViewOrder, TInputData>
            (IViewBaseWithInput<TPeek, TInputData> view, LinkID key)
            where TPeek : class
            where TViewOrder : class
        {
            if (view == null)
            {
                throw new Exception($"BindしようとしているViewがNullです");
            }

            if (readyLogics.ContainsKey(key))
            {
                var logic = readyLogics[key];
                readyLogics.Remove(key);

                if (logic is ILogicBaseWithInput<TViewOrder, TInputData> logicBase)
                {
                    BindLogicAndViewWithInput(logicBase, view);
                }
                else
                {
                    throw new Exception($"Logic({logic})とView({view})のジェネリクス型が一致しません");
                }
            }
            else
            {
                readyViews.Add(key, view);
            }
        }

        /// <summary>
        /// LogicWithInputとViewWithInputを関連付けする。
        /// </summary>
        public void BindLogicAndViewWithInput<TViewOrder, TPeek, TInputData>
        (
            ILogicBaseWithInput<TViewOrder, TInputData> logic,
            IViewBaseWithInput<TPeek, TInputData> view
        )
            where TViewOrder : class
            where TPeek : class
        {
            InputDispatcher<TInputData> input = new();
            logic.Input = input;
            view.Input = input;

            logic.ViewOrder = view as TViewOrder ?? throw new Exception("viewがViewOrderを実装していません");
            ;
            view.Peek = logic as TPeek ?? throw new Exception("logicがPeekを実装していません");

            InitLogicAndView(logic, view);
        }

        /// <summary>
        /// LinkIDでLogicを関連付け予約する。
        /// 既に同じLinkIDでViewが関連付け予約されていたら、LogicとViewが関連付けられる。
        /// </summary>
        public void BindLogic<TPeek, TViewOrder>
            (ILogicBase<TViewOrder> logic, LinkID key)
            where TViewOrder : class
            where TPeek : class
        {
            if (logic == null)
            {
                throw new Exception($"BindしようとしているLogicがNullです");
            }

            if (readyViews.ContainsKey(key))
            {
                var view = readyViews[key];
                readyViews.Remove(key);

                if (view is IViewBase<TPeek> viewBase)
                {
                    BindLogicAndView(logic, viewBase);
                }
                else
                {
                    throw new Exception($"Logic({logic})とView({view})のジェネリクス型が一致しません");
                }
            }
            else
            {
                readyLogics.Add(key, logic);
            }
        }

        /// <summary>
        /// LinkIDでViewを関連付け予約する。
        /// 既に同じLinkIDでLogicが関連付け予約されていたら、LogicとViewが関連付けられる。
        /// </summary>
        public void BindView<TPeek, TViewOrder>
            (IViewBase<TPeek> view, LinkID key)
            where TPeek : class
            where TViewOrder : class
        {
            if (view == null)
            {
                throw new Exception($"BindしようとしているViewがNullです");
            }

            if (readyLogics.ContainsKey(key))
            {
                var logic = readyLogics[key];
                readyLogics.Remove(key);

                if (logic is ILogicBase<TViewOrder> logicBase)
                {
                    BindLogicAndView(logicBase, view);
                }
                else
                {
                    throw new Exception($"Logic({logic})とView({view})のジェネリクス型が一致しません");
                }
            }
            else
            {
                readyViews.Add(key, view);
            }
        }

        public void BindLogicAndView<TPeek, TViewOrder>
        (
            ILogicBase<TViewOrder> logic,
            IViewBase<TPeek> view
        )
            where TViewOrder : class
            where TPeek : class
        {
            logic.ViewOrder = view as TViewOrder ?? throw new Exception($"View({view})がViewOrder({typeof(TViewOrder)})を実装していません");
            view.Peek = logic as TPeek ?? throw new Exception($"Logic({logic})がPeek({typeof(TPeek)})を実装していません");

            InitLogicAndView(logic, view);
        }

        private void InitLogicAndView(ILogicCore logic, IViewCore view)
        {
            // 既にどちらか破棄されていたら両方破棄して終了
            if (logic.IsDisposed || view.IsDisposed)
            {
                if (!logic.IsDisposed)
                {
                    logic.Dispose();
                }

                if (!view.IsDisposed)
                {
                    view.Dispose();
                }

                return;
            }

            // 片方がDisposeされたら対応するもう片方も同時にDisposeする
            logic.OnDispose += DisposeLogic;
            view.OnDispose += DisposeView;
            disposeLogicHandlers.Add(logic, DisposeLogic);
            disposeViewHandlers.Add(view, DisposeView);

            logic.IsInitialized = true;
            view.IsInitialized = true;
            logic.Initialized();
            view.Initialized();
            logic.ViewInitialized();

            if (logic is IUpdatableLogic updatableLogic)
            {
                updatableLogics.Add(updatableLogic);
            }

            if (logic is IFixedUpdatableLogic fixedUpdatableLogic)
            {
                fixedUpdatableLogics.Add(fixedUpdatableLogic);
            }

            if (view is IUpdatableView updatableView)
            {
                updatableViews.Add(updatableView);
            }

            if (view is IFixedUpdatableView fixedUpdatableView)
            {
                fixedUpdatableViews.Add(fixedUpdatableView);
            }

            return;

            void DisposeLogic() => DisposeLogicHandler(logic, view);
            void DisposeView() => DisposeViewHandler(logic, view);
        }

        private void RemoveLogicAndView(ILogicCore logic, IViewCore view)
        {
            logic.OnDispose -= disposeLogicHandlers[logic];
            view.OnDispose -= disposeViewHandlers[view];
            disposeLogicHandlers.Remove(logic);
            disposeViewHandlers.Remove(view);

            // 全LogicのUpdateLogic呼び出しループ中にリストから削除するとエラーになるので、削除キューに加えておきループ後に削除する
            if (logic is IUpdatableLogic updatableLogic)
            {
                removeUpdatableLogicQueue.Enqueue(updatableLogic);
            }

            if (logic is IFixedUpdatableLogic fixedUpdatableLogic)
            {
                removeFixedUpdatableLogicQueue.Enqueue(fixedUpdatableLogic);
            }

            // 全ViewのUpdateView呼び出しループ中にリストから削除するとエラーになるので、削除キューに加えておきループ後に削除する
            if (view is IUpdatableView updatableView)
            {
                removeUpdatableViewQueue.Enqueue(updatableView);
            }

            if (view is IFixedUpdatableView fixedUpdatableView)
            {
                removeFixedUpdatableViewQueue.Enqueue(fixedUpdatableView);
            }
        }

        private void DisposeLogicHandler(ILogicCore logic, IViewCore view)
        {
            RemoveLogicAndView(logic, view);
            view.Dispose();
        }

        private void DisposeViewHandler(ILogicCore logic, IViewCore view)
        {
            RemoveLogicAndView(logic, view);
            logic.Dispose();
        }

        private void Update()
        {
            // StrictモードではLogicとViewは同時に初期化されている必要がある
            if (IsStrict)
            {
                if (readyViews.Any())
                {
                    object view = readyViews.Values.First();
                    throw new Exception($"同一フレーム内で{view}に対応するLogicが初期化されていません。");
                }

                if (readyLogics.Any())
                {
                    object logic = readyLogics.Values.First();
                    throw new Exception($"同一フレーム内で{logic}に対応するViewが初期化されていません。");
                }
            }

            foreach (IUpdatableLogic logic in updatableLogics)
            {
                logic.UpdateLogic();
            }

            foreach (IUpdatableView view in updatableViews)
            {
                view.UpdateView();
            }

            while (removeUpdatableLogicQueue.Any())
            {
                updatableLogics.Remove(removeUpdatableLogicQueue.Dequeue());
            }

            while (removeUpdatableViewQueue.Any())
            {
                updatableViews.Remove(removeUpdatableViewQueue.Dequeue());
            }

#if UNITY_EDITOR
            readyLogicCount = readyLogics.Count;
            readyViewCount = readyViews.Count;
#endif
        }

        private void FixedUpdate()
        {
            foreach (IFixedUpdatableLogic logic in fixedUpdatableLogics)
            {
                logic.FixedUpdateLogic();
            }

            foreach (IFixedUpdatableView view in fixedUpdatableViews)
            {
                view.FixedUpdateView();
            }

            while (removeFixedUpdatableLogicQueue.Any())
            {
                fixedUpdatableLogics.Remove(removeFixedUpdatableLogicQueue.Dequeue());
            }

            while (removeFixedUpdatableViewQueue.Any())
            {
                fixedUpdatableViews.Remove(removeFixedUpdatableViewQueue.Dequeue());
            }
        }
    }
}